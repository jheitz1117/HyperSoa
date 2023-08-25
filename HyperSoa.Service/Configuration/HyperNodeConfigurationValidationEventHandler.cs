using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.ActivityTracking;
using HyperSoa.Service.CommandModules;
using HyperSoa.Service.EventTracking;
using HyperSoa.Service.Serialization;
using HyperSoa.Service.TaskIdProviders;

namespace HyperSoa.Service.Configuration
{
    /// <summary>
    /// Represents the callback method that will handle configuration validation events and the <see cref="HyperNodeConfigurationValidationEventArgs"/>.
    /// </summary>
    public delegate void HyperNodeConfigurationValidationEventHandler(object sender, HyperNodeConfigurationValidationEventArgs e);

    /// <summary>
    /// Validates implementations of <see cref="IHyperNodeConfiguration"/>.
    /// </summary>
    public sealed class HyperNodeConfigurationValidator
    {
        private readonly HyperNodeConfigurationValidationEventHandler _validationEventHandler;

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperNodeConfigurationValidator"/> class.
        /// </summary>
        /// <param name="validationEventHandler">Callback for validation errors. If this is null, an exception is thrown if any validation errors occur.</param>
        public HyperNodeConfigurationValidator(HyperNodeConfigurationValidationEventHandler? validationEventHandler)
        {
            _validationEventHandler = validationEventHandler ?? DefaultValidationEventHandler;
        }

        /// <summary>
        /// Validates the specified <see cref="IHyperNodeConfiguration"/> object.
        /// </summary>
        /// <param name="config"><see cref="IHyperNodeConfiguration"/> object to validate.</param>
        public void ValidateConfiguration(IHyperNodeConfiguration config)
        {
            if (config == null)
            { throw new ArgumentNullException(nameof(config)); }

            var configClassName = config.GetType().FullName;
            if (string.IsNullOrWhiteSpace(config.InstanceName))
            {
                RaiseValidationEvent(
                    new HyperNodeConfigurationException($"The {nameof(config.InstanceName)} property is required for {configClassName}.")
                );
            }

            // TaskIdProviderType is not required, but if it is specified, it must implement the correct interface
            if (!string.IsNullOrWhiteSpace(config.TaskIdProviderType))
                ValidateTypeImplementsInterface(config.TaskIdProviderType, typeof(ITaskIdProvider));

            // HyperNodeEventHandlerType is not required, but if it is specified, it must implement the correct interface
            if (!string.IsNullOrWhiteSpace(config.HyperNodeEventHandlerType))
                ValidateTypeImplementsInterface(config.HyperNodeEventHandlerType, typeof(IHyperNodeEventHandler));

            if (config.ActivityMonitors != null)
            {
                foreach (var activityMonitor in config.ActivityMonitors)
                {
                    ValidateConfiguration(activityMonitor);
                }
            }

            if (config.RemoteAdminCommands != null)
            {
                foreach (var remoteAdminCommand in config.RemoteAdminCommands)
                {
                    ValidateConfiguration(remoteAdminCommand);
                }
            }

            if (config.CommandModules != null)
            {
                // ContractSerializerType property is not required at the collection level, but if it is specified, it must implement the correct interface
                if (!string.IsNullOrWhiteSpace(config.CommandModules.ContractSerializerType))
                    ValidateTypeImplementsInterface(config.CommandModules.ContractSerializerType, typeof(IServiceContractSerializer));

                foreach (var commandModule in config.CommandModules)
                {
                    ValidateConfiguration(commandModule);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void ValidateConfiguration(IActivityMonitorConfiguration config)
        {
            var configClassName = config.GetType().FullName;

            if (string.IsNullOrWhiteSpace(config.MonitorName))
            {
                RaiseValidationEvent(
                    new HyperNodeConfigurationException(
                        $"The MonitorName property is required for {configClassName}."
                    )
                );
            }

            if (string.IsNullOrWhiteSpace(config.MonitorType))
            {
                RaiseValidationEvent(
                    new HyperNodeConfigurationException(
                        $"The MonitorType property is required for {configClassName}."
                    )
                );
            }
            else
            {
                ValidateTypeHasBaseType(config.MonitorType, typeof(HyperNodeServiceActivityMonitor));
            }
        }

        private void ValidateConfiguration(IRemoteAdminCommandConfiguration config)
        {
            var configClassName = config.GetType().FullName;

            if (string.IsNullOrWhiteSpace(config.CommandName))
            {
                RaiseValidationEvent(
                    new HyperNodeConfigurationException(
                        $"The CommandName property is required for {configClassName}."
                    )
                );
            }
            else
            {
                if (!RemoteAdminCommandName.IsRemoteAdminCommand(config.CommandName))
                {
                    RaiseValidationEvent(
                        new HyperNodeConfigurationException(
                            string.Format(
                                "The value '{0}' is not a valid remote admin command name. The following is a list of all recognized remote admin command names:{1}{1}{2}",
                                config.CommandName,
                                Environment.NewLine,
                                string.Join(
                                    Environment.NewLine,
                                    RemoteAdminCommandName.GetAll()
                                )
                            )
                        )
                    );
                }
            }
        }

        private void ValidateConfiguration(ICommandModuleConfiguration config)
        {
            var configClassName = config.GetType().FullName;

            if (string.IsNullOrWhiteSpace(config.CommandName))
            {
                RaiseValidationEvent(
                    new HyperNodeConfigurationException(
                        $"The CommandName property is required for {configClassName}."
                    )
                );
            }

            if (string.IsNullOrWhiteSpace(config.CommandModuleType))
            {
                RaiseValidationEvent(
                    new HyperNodeConfigurationException(
                        $"The CommandModuleType property is required for {configClassName}."
                    )
                );
            }
            else
            {
                ValidateTypeImplementsAnyInterface(
                    config.CommandModuleType,
                    typeof(ICommandModule),
                    typeof(IAwaitableCommandModule)
                );
            }

            // ContractSerializerType property is not required, but if it is specified, it must implement the correct interface
            if (!string.IsNullOrWhiteSpace(config.ContractSerializerType))
                ValidateTypeImplementsInterface(config.ContractSerializerType, typeof(IServiceContractSerializer));
        }

        private void ValidateTypeImplementsInterface(string targetTypeString, Type requiredInterface)
        {
            if (requiredInterface == null)
                throw new ArgumentNullException(nameof(requiredInterface));

            if (!requiredInterface.IsInterface)
                throw new ArgumentException("Type must be an interface.", nameof(requiredInterface));

            ValidateTypeString(targetTypeString, out var targetType);

            if (targetType != null && !targetType.GetInterfaces().Contains(requiredInterface))
            {
                RaiseValidationEvent(
                    new HyperNodeConfigurationException(
                        $"The type '{targetType.FullName}' must implement {requiredInterface.FullName}."
                    )
                );
            }
        }

        private void ValidateTypeImplementsAnyInterface(string targetTypeString, params Type[] acceptedInterfaces)
        {
            if (acceptedInterfaces == null)
                throw new ArgumentNullException(nameof(acceptedInterfaces));

            if (!acceptedInterfaces.All(t => t.IsInterface))
                throw new ArgumentException("Every Type must be an interface.", nameof(acceptedInterfaces));

            ValidateTypeString(targetTypeString, out var targetType);

            if (targetType != null && !targetType.GetInterfaces().Intersect(acceptedInterfaces).Any())
            {
                RaiseValidationEvent(
                    new HyperNodeConfigurationException(
                        $"The type '{targetType.FullName}' must implement one of the following interfaces: {string.Join(", ", acceptedInterfaces.Select(t => t.FullName))}."
                    )
                );
            }
        }

        private void ValidateTypeHasBaseType(string targetTypeString, Type requiredBaseType)
        {
            if (requiredBaseType == null)
                throw new ArgumentNullException(nameof(requiredBaseType));

            if (!requiredBaseType.IsClass)
                throw new ArgumentException("Type must be a class; that is, not a value type or interface.", nameof(requiredBaseType));

            ValidateTypeString(targetTypeString, out var targetType);

            if (targetType != null && !targetType.IsSubclassOf(requiredBaseType))
            {
                RaiseValidationEvent(
                    new HyperNodeConfigurationException(
                        $"The type '{targetType.FullName}' must inherit {requiredBaseType.FullName}."
                    )
                );
            }
        }

        private void ValidateTypeString(string targetTypeString, out Type? targetType)
        {
            try
            {
                targetType = Type.GetType(targetTypeString, true);
            }
            catch (Exception ex)
            {
                targetType = null;

                RaiseValidationEvent(
                    new HyperNodeConfigurationException(
                        $"The string '{targetTypeString}' could not be parsed into a {typeof (Type).FullName} object. The string must be an assembly qualified type name. See inner exception for details.",
                        ex
                    )
                );
            }
        }

        private void RaiseValidationEvent(HyperNodeConfigurationException ex)
        {
            _validationEventHandler(
                this,
                new HyperNodeConfigurationValidationEventArgs
                {
                    Exception = ex,
                    Message = ex.Message
                }
            );
        }

        private static void DefaultValidationEventHandler(object sender, HyperNodeConfigurationValidationEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(
                    nameof(args),
                    $"An error occurred while validating an instance of {typeof (IHyperNodeConfiguration).FullName}, but no event arguments were supplied."
                );
            }

            throw args.Exception ?? new HyperNodeConfigurationException(
                args.Message ?? $"An error occurred while validating an instance of {typeof (IHyperNodeConfiguration).FullName}, but no error message was specified."
            );
        }

        #endregion Private Methods
    }
}
