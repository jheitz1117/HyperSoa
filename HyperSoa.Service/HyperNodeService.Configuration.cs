using System.Text;
using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Service.ActivityTracking;
using HyperSoa.Service.CommandModules;
using HyperSoa.Service.CommandModules.RemoteAdmin;
using HyperSoa.Service.Configuration;
using HyperSoa.Service.EventTracking;
using HyperSoa.Service.Serialization;
using HyperSoa.Service.TaskIdProviders;

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

        private static HyperNodeService Create(IHyperNodeConfigurationProvider configProvider)
        {
            if (configProvider == null)
                throw new ArgumentNullException(nameof(configProvider));

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

            var service = new HyperNodeService(config.HyperNodeName)
            {
                EnableTaskProgressCache = config.EnableTaskProgressCache ?? DefaultTaskProgressCacheEnabled,
                EnableDiagnostics = config.EnableDiagnostics ?? DefaultDiagnosticsEnabled,
                TaskProgressCacheDuration = TimeSpan.FromMinutes(config.TaskProgressCacheDurationMinutes ?? DefaultProgressCacheDurationMinutes),
                MaxConcurrentTasks = config.MaxConcurrentTasks ?? DefaultMaxConcurrentTasks
            };

            ConfigureRemoteAdminCommands(service, config);
            ConfigureTaskProvider(service, config);
            ConfigureActivityMonitors(service, config);
            ConfigureCommandModules(service, config);
            ConfigureHyperNodeEventHandler(service, config);

            return service;
        }

        private static void ConfigureRemoteAdminCommands(HyperNodeService service, IHyperNodeConfiguration config)
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
                service.AddCommandModuleConfiguration(remoteAdminCommandConfig);
            }
        }

        private static void ConfigureTaskProvider(HyperNodeService service, IHyperNodeConfiguration config)
        {
            ITaskIdProvider? taskIdProvider = null;

            // Set our task id provider if applicable, but if we have any problems creating the instance or casting to ITaskIdProvider, we deliberately want to fail out and make them fix the configuration
            if (!string.IsNullOrWhiteSpace(config.TaskIdProviderType))
            {
                taskIdProvider = (ITaskIdProvider?)Activator.CreateInstance(Type.GetType(config.TaskIdProviderType, true)!);

                if (taskIdProvider == null)
                    throw new HyperNodeConfigurationException($"Unable to create {nameof(ITaskIdProvider)} from type '{config.TaskIdProviderType}'.");

                taskIdProvider.Initialize();
            }

            service.TaskIdProvider = taskIdProvider ?? DefaultTaskIdProvider;
        }

        private static void ConfigureActivityMonitors(HyperNodeService service, IHyperNodeConfiguration config)
        {
            // Consider a null collection equivalent to an empty one
            if (config.ActivityMonitors == null)
                return;

            // Instantiate our activity monitors
            foreach (var monitorConfig in config.ActivityMonitors)
            {
                // If we have any problems creating the instance or casting to HyperNodeServiceActivityMonitor, we deliberately want to fail out and make them fix the config
                var monitor = (HyperNodeServiceActivityMonitor?)Activator.CreateInstance(Type.GetType(monitorConfig.MonitorType!, true)!);
                if (monitor != null)
                {
                    monitor.Name = monitorConfig.MonitorName;
                    monitor.Enabled = monitorConfig.Enabled;

                    monitor.Initialize();

                    if (service._customActivityMonitors.Any(m => m.Name == monitorConfig.MonitorName))
                    {
                        throw new DuplicateActivityMonitorException(
                            $"An activity monitor already exists with the {nameof(monitorConfig.MonitorName)} '{monitorConfig.MonitorName}'."
                        );
                    }

                    service._customActivityMonitors.Add(monitor);
                }
            }
        }

        private static void ConfigureCommandModules(HyperNodeService service, IHyperNodeConfiguration config)
        {
            // Consider a null collection equivalent to an empty one
            if (config.CommandModules == null)
                return;

            Type collectionContractSerializerType = null;

            // First, see if we have a serializer type defined at the collection level
            if (!string.IsNullOrWhiteSpace(config.CommandModules.ContractSerializerType))
                collectionContractSerializerType = Type.GetType(config.CommandModules.ContractSerializerType, true);

            foreach (var commandModuleConfig in config.CommandModules)
            {
                var commandModuleType = Type.GetType(commandModuleConfig.CommandModuleType, true);
                if (commandModuleType.GetInterfaces().Contains(typeof(ICommandModule)) ||
                    commandModuleType.GetInterfaces().Contains(typeof(IAwaitableCommandModule)))
                {
                    Type commandContractSerializerType = null;

                    // Now check to see if we have a serializer type defined at the command level
                    if (!string.IsNullOrWhiteSpace(commandModuleConfig.ContractSerializerType))
                        commandContractSerializerType = Type.GetType(commandModuleConfig.ContractSerializerType, true);

                    // Our final configuration allows command-level serializer types to take precedence, if available. Otherwise, the collection-level types are used.
                    var configContractSerializerType = commandContractSerializerType ?? collectionContractSerializerType;

                    IContractSerializer configContractSerializer = null;

                    // Attempt construction of config-level serializer types
                    if (configContractSerializerType != null)
                        configContractSerializer = (IContractSerializer)Activator.CreateInstance(configContractSerializerType);

                    // Finally, construct our command module configuration
                    var commandConfig = new CommandModuleConfiguration
                    {
                        CommandName = commandModuleConfig.CommandName,
                        Enabled = commandModuleConfig.Enabled,
                        CommandModuleType = commandModuleType,
                        ContractSerializer = configContractSerializer ?? DefaultContractSerializer
                    };

                    service.AddCommandModuleConfiguration(commandConfig);
                }
            }
        }

        private static void ConfigureHyperNodeEventHandler(HyperNodeService service, IHyperNodeConfiguration config)
        {
            IHyperNodeEventHandler eventHandler = null;

            // Set our event handler if applicable, but if we have any problems creating the instance or casting to HyperNodeEventHandlerBase, we deliberately want to fail out and make them fix the configuration
            if (!string.IsNullOrWhiteSpace(config.HyperNodeEventHandlerType))
            {
                eventHandler = (IHyperNodeEventHandler)Activator.CreateInstance(Type.GetType(config.HyperNodeEventHandlerType, true));
                eventHandler.Initialize();
            }

            service.EventHandler = eventHandler ?? DefaultEventHandler;
        }
        
        #endregion Configuration
    }
}
