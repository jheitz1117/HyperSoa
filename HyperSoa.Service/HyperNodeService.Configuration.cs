using System.Text;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.ActivityTracking;
using HyperSoa.Service.CommandModules;
using HyperSoa.Service.CommandModules.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.EventTracking;
using HyperSoa.Service.Serialization;
using HyperSoa.Service.TaskIdProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace HyperSoa.Service
{
    public partial class HyperNodeService
    {
        #region Defaults

        private static readonly IHyperNodeEventHandler DefaultEventHandler = new HyperNodeEventHandlerBase();
        private static readonly ITaskIdProvider DefaultTaskIdProvider = new GuidTaskIdProvider();
        internal const bool DefaultTaskProgressCacheEnabled = false;
        internal const bool DefaultDiagnosticsEnabled = false;
        internal const int DefaultProgressCacheDurationMinutes = 60;
        internal const int DefaultMaxConcurrentTasks = -1;

        #endregion Defaults

        #region Configuration

        /// <summary>
        /// Initializes an instance of the <see cref="HyperNodeService"/> class with the specified name.
        /// </summary>
        /// <param name="configProvider">DI dependency for the config</param>
        /// <param name="serviceProvider">DI dependency for the <see cref="IServiceProvider"/></param>
        public HyperNodeService(IHyperNodeConfigurationProvider configProvider, IServiceProvider serviceProvider)
        {
            if (configProvider == null)
                throw new ArgumentNullException(nameof(configProvider));
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            IHyperNodeConfiguration config;

            try
            {
                config = configProvider.GetConfiguration();
            }
            catch (Exception ex)
            {
                throw new HyperNodeConfigurationException(
                    $"An exception was thrown while attempting to retrieve the configuration for this {typeof (HyperNodeService).FullName} using {configProvider.GetType().FullName}. See inner exception for details.",
                    ex
                );
            }

            // Validate our configuration
            var builder = new StringBuilder();
            new HyperNodeConfigurationValidator(
                (_, args) => builder.AppendLine(args.Message)
            ).ValidateConfiguration(config);

            // Check for validation errors before proceeding to create and configure our HyperNodeService instance
            if (builder.Length > 0)
            { throw new HyperNodeConfigurationException(builder.ToString()); }

            HyperNodeName = config.HyperNodeName;

            ServiceProvider = serviceProvider;
            Logger = serviceProvider.GetService<ILogger<HyperNodeService>>() ?? NullLogger<HyperNodeService>.Instance;
            EnableTaskProgressCache = config.EnableTaskProgressCache ?? DefaultTaskProgressCacheEnabled;
            EnableDiagnostics = config.EnableDiagnostics ?? DefaultDiagnosticsEnabled;
            TaskProgressCacheDuration = TimeSpan.FromMinutes(config.TaskProgressCacheDurationMinutes ?? DefaultProgressCacheDurationMinutes);
            MaxConcurrentTasks = config.MaxConcurrentTasks ?? DefaultMaxConcurrentTasks;

            ConfigureRemoteAdminCommands(config);
            ConfigureTaskProvider(config, serviceProvider);
            ConfigureActivityMonitors(config, serviceProvider);
            ConfigureCommandModules(config, serviceProvider);
            ConfigureHyperNodeEventHandler(config, serviceProvider);
        }

        private void ConfigureRemoteAdminCommands(IHyperNodeConfiguration config)
        {
            // Grab our user-defined default for remote admin commands being enabled or disabled
            bool? userDefinedRemoteAdminCommandsEnabledDefault = null;
            var remoteAdminCommandsCollection = config.RemoteAdminCommands;
            if (remoteAdminCommandsCollection != null)
                userDefinedRemoteAdminCommandsEnabledDefault = remoteAdminCommandsCollection.Enabled;

            // If the user didn't configure the remote admin commands, they will be on by default (so that we can get task statuses and such)
            var actualDefaultEnabled = userDefinedRemoteAdminCommandsEnabledDefault ?? true;

            // Make all commands enabled or disabled according to the user-defined default, or the HyperNode's default if the user did not define a default
            var remoteAdminCommandConfigs = new List<CommandModuleConfiguration>
            {
                new()
                {
                    CommandName = RemoteAdminCommandName.GetCachedTaskProgressInfo,
                    Enabled = actualDefaultEnabled,
                    CommandModuleType = typeof(GetCachedTaskProgressInfoCommand)
                },
                new()
                {
                    CommandName = RemoteAdminCommandName.GetNodeStatus,
                    Enabled = actualDefaultEnabled,
                    CommandModuleType = typeof(GetNodeStatusCommand)
                },
                new()
                {
                    CommandName = RemoteAdminCommandName.Echo,
                    Enabled = actualDefaultEnabled,
                    CommandModuleType = typeof(EchoCommand)
                },
                new()
                {
                    CommandName = RemoteAdminCommandName.EnableCommand,
                    Enabled = actualDefaultEnabled,
                    CommandModuleType = typeof(EnableCommandModuleCommand)
                },
                new()
                {
                    CommandName = RemoteAdminCommandName.EnableActivityMonitor,
                    Enabled = actualDefaultEnabled,
                    CommandModuleType = typeof(EnableActivityMonitorCommand)
                },
                new()
                {
                    CommandName = RemoteAdminCommandName.RenameActivityMonitor,
                    Enabled = actualDefaultEnabled,
                    CommandModuleType = typeof(RenameActivityMonitorCommand)
                },
                new()
                {
                    CommandName = RemoteAdminCommandName.EnableTaskProgressCache,
                    Enabled = actualDefaultEnabled,
                    CommandModuleType = typeof(EnableTaskProgressCacheCommand)
                },
                new()
                {
                    CommandName = RemoteAdminCommandName.EnableDiagnostics,
                    Enabled = actualDefaultEnabled,
                    CommandModuleType = typeof(EnableDiagnosticsCommand)
                },
                new()
                {
                    CommandName = RemoteAdminCommandName.CancelTask,
                    Enabled = actualDefaultEnabled,
                    CommandModuleType = typeof(CancelTaskCommand)
                },
                new()
                {
                    CommandName = RemoteAdminCommandName.SetTaskProgressCacheDuration,
                    Enabled = actualDefaultEnabled,
                    CommandModuleType = typeof(SetTaskProgressCacheDurationCommand)
                }
            };

            foreach (var remoteAdminCommandConfig in remoteAdminCommandConfigs)
            {
                // Allow each remote admin command to be enabled or disabled individually. This takes precedence over any defaults defined previously
                if (config.RemoteAdminCommands != null && config.RemoteAdminCommands.ContainsCommandName(remoteAdminCommandConfig.CommandName))
                {
                    var userConfig = config.RemoteAdminCommands.GetByCommandName(remoteAdminCommandConfig.CommandName);
                    if (userConfig != null)
                        remoteAdminCommandConfig.Enabled = userConfig.Enabled;
                }

                // Finally, try to add this remote admin command to our collection
                AddCommandModuleConfiguration(remoteAdminCommandConfig);
            }
        }

        private void ConfigureTaskProvider(IHyperNodeConfiguration config, IServiceProvider? serviceProvider)
        {
            ITaskIdProvider? taskIdProvider = null;

            if (!string.IsNullOrWhiteSpace(config.TaskIdProviderType))
            {
                // Set our task id provider if applicable, but if we have any problems creating the instance or casting to ITaskIdProvider, we deliberately want to fail out and make them fix the configuration
                var taskIdProviderType = Type.GetType(config.TaskIdProviderType, true) ?? throw new HyperNodeConfigurationException($"Unable to determine {nameof(ITaskIdProvider)} type from '{config.TaskIdProviderType}'.");

                if (serviceProvider != null)
                    taskIdProvider = (ITaskIdProvider)ActivatorUtilities.CreateInstance(serviceProvider, taskIdProviderType);
                else
                    taskIdProvider = (ITaskIdProvider?)Activator.CreateInstance(taskIdProviderType);

                if (taskIdProvider == null)
                    throw new HyperNodeConfigurationException($"Unable to create {nameof(ITaskIdProvider)} from type '{config.TaskIdProviderType}'.");

                taskIdProvider.Initialize();
            }

            TaskIdProvider = taskIdProvider ?? DefaultTaskIdProvider;
        }

        private void ConfigureActivityMonitors(IHyperNodeConfiguration config, IServiceProvider? serviceProvider)
        {
            // Consider a null collection equivalent to an empty one
            if (config.ActivityMonitors == null)
                return;

            // Instantiate our activity monitors
            foreach (var monitorConfig in config.ActivityMonitors)
            {
                HyperNodeServiceActivityMonitor? monitor;

                // If we have any problems creating the instance or casting to HyperNodeServiceActivityMonitor, we deliberately want to fail out and make them fix the config
                var monitorType = Type.GetType(monitorConfig.MonitorType!, true) ?? throw new HyperNodeConfigurationException($"Unable to determine activity monitor type from '{monitorConfig.MonitorType}'.");
                if (serviceProvider != null)
                    monitor = (HyperNodeServiceActivityMonitor)ActivatorUtilities.CreateInstance(serviceProvider, monitorType);
                else
                    monitor = (HyperNodeServiceActivityMonitor?)Activator.CreateInstance(monitorType);

                if (monitor != null)
                {
                    monitor.Name = string.IsNullOrWhiteSpace(monitorConfig.MonitorName)
                        ? Guid.NewGuid().ToString()
                        : monitorConfig.MonitorName;
                    monitor.Enabled = monitorConfig.Enabled;

                    monitor.Initialize();

                    if (_customActivityMonitors.Any(m => m.Name == monitorConfig.MonitorName))
                    {
                        throw new DuplicateActivityMonitorException(
                            $"An activity monitor already exists with the {nameof(monitorConfig.MonitorName)} '{monitorConfig.MonitorName}'."
                        );
                    }

                    _customActivityMonitors.Add(monitor);
                }
            }
        }

        private void ConfigureCommandModules(IHyperNodeConfiguration config, IServiceProvider? serviceProvider)
        {
            // Consider a null collection equivalent to an empty one
            if (config.CommandModules == null)
                return;

            Type? collectionContractSerializerType = null;

            // First, see if we have a serializer type defined at the collection level
            if (!string.IsNullOrWhiteSpace(config.CommandModules.ContractSerializerType))
                collectionContractSerializerType = Type.GetType(config.CommandModules.ContractSerializerType, true);

            foreach (var commandModuleConfig in config.CommandModules)
            {
                var commandModuleType = Type.GetType(
                    commandModuleConfig.CommandModuleType,
                    true
                ) ?? throw new HyperNodeConfigurationException($"Unable to determine command type from '{commandModuleConfig.CommandModuleType}'.");

                if (commandModuleType.GetInterfaces().Contains(typeof(ICommandModule)) ||
                    commandModuleType.GetInterfaces().Contains(typeof(IAwaitableCommandModule)))
                {
                    Type? commandContractSerializerType = null;

                    // Now check to see if we have a serializer type defined at the command level
                    if (!string.IsNullOrWhiteSpace(commandModuleConfig.ContractSerializerType))
                        commandContractSerializerType = Type.GetType(commandModuleConfig.ContractSerializerType, true);

                    // Our final configuration allows command-level serializer types to take precedence, if available. Otherwise, the collection-level types are used.
                    var configContractSerializerType = commandContractSerializerType ?? collectionContractSerializerType;

                    IServiceContractSerializer? configContractSerializer = null;

                    // Attempt construction of config-level serializer types
                    if (configContractSerializerType != null)
                    {
                        if (serviceProvider != null)
                            configContractSerializer = (IServiceContractSerializer)ActivatorUtilities.CreateInstance(serviceProvider, configContractSerializerType);
                        else
                            configContractSerializer = (IServiceContractSerializer?)Activator.CreateInstance(configContractSerializerType);
                    }

                    // Finally, construct our command module configuration
                    var commandConfig = new CommandModuleConfiguration
                    {
                        CommandName = commandModuleConfig.CommandName,
                        Enabled = commandModuleConfig.Enabled,
                        CommandModuleType = commandModuleType,
                        ContractSerializer = configContractSerializer ?? DefaultContractSerializer
                    };

                    AddCommandModuleConfiguration(commandConfig);
                }
            }
        }

        private void ConfigureHyperNodeEventHandler(IHyperNodeConfiguration config, IServiceProvider? serviceProvider)
        {
            IHyperNodeEventHandler? eventHandler = null;

            if (!string.IsNullOrWhiteSpace(config.HyperNodeEventHandlerType))
            {
                // Set our event handler if applicable, but if we have any problems creating the instance or casting to HyperNodeEventHandlerBase, we deliberately want to fail out and make them fix the configuration
                var eventHandlerType = Type.GetType(config.HyperNodeEventHandlerType, true) ?? throw new HyperNodeConfigurationException($"Unable to determine {nameof(IHyperNodeEventHandler)} type from '{config.HyperNodeEventHandlerType}'.");
                
                if (serviceProvider != null)
                    eventHandler = (IHyperNodeEventHandler)ActivatorUtilities.CreateInstance(serviceProvider, eventHandlerType);
                else
                    eventHandler = (IHyperNodeEventHandler?)Activator.CreateInstance(eventHandlerType);

                if (eventHandler == null)
                    throw new HyperNodeConfigurationException($"Unable to create {nameof(IHyperNodeEventHandler)} from type '{config.TaskIdProviderType}'.");

                eventHandler.Initialize();
            }

            EventHandler = eventHandler ?? DefaultEventHandler;
        }
        
        #endregion Configuration

        #region Programmatic Configuration

        /// <summary>
        /// Adds the specified <see cref="Type"/> as an enabled command module with the specified command name. Command modules
        /// added using this method do not have <see cref="IServiceContractSerializer"/> or <see cref="IServiceContractSerializer"/>
        /// implementations defined.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="commandModuleType">The <see cref="Type"/> of the command module.</param>
        public void AddCommandModuleConfiguration(string commandName, Type commandModuleType)
        {
            AddCommandModuleConfiguration(commandName, commandModuleType, true, null);
        }

        /// <summary>
        /// Adds the specified <see cref="Type"/> as a command module with the specified command name and configuration options.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="commandModuleType">The <see cref="Type"/> of the command module.</param>
        /// <param name="enabled">Indicates whether the command will be enabled immediately.</param>
        /// <param name="contractSerializer">The <see cref="IServiceContractSerializer"/> implementation to use to serialize and deserialize request and response objects. This parameter can be null.</param>
        public void AddCommandModuleConfiguration(string commandName, Type commandModuleType, bool enabled, IServiceContractSerializer? contractSerializer)
        {
            AddCommandModuleConfiguration(
                new CommandModuleConfiguration
                {
                    CommandName = commandName,
                    CommandModuleType = commandModuleType,
                    Enabled = enabled,
                    ContractSerializer = contractSerializer
                }
            );
        }

        #endregion Programmatic Configuration
    }
}
