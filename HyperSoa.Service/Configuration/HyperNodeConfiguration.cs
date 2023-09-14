namespace HyperSoa.Service.Configuration
{
    public class HyperNodeConfiguration : IHyperNodeConfiguration
    {
        public const string ConfigurationSectionName = "HyperSoa:ServiceConfig";

        public string? InstanceName { get; set; }
        public bool? EnableTaskProgressCache { get; set; }
        public bool? EnableDiagnostics { get; set; }
        public int? TaskProgressCacheDuration { get; set; }
        public int? MaxConcurrentTasks { get; set; }
        public string? TaskIdProviderType { get; set; }
        public string? HyperNodeEventHandlerType { get; set; }

        public ActivityMonitorConfigurationCollection? ActivityMonitors { get; set; }
        public RemoteAdminConfigurationCollection? RemoteAdmin { get; set; }
        public CommandModuleConfigurationCollection? CommandConfig { get; set; }

        int? IHyperNodeConfiguration.TaskProgressCacheDurationMinutes => TaskProgressCacheDuration;
        IActivityMonitorConfigurationCollection? IHyperNodeConfiguration.ActivityMonitors => ActivityMonitors;
        IRemoteAdminConfigurationCollection? IHyperNodeConfiguration.RemoteAdminCommands => RemoteAdmin;
        ICommandModuleConfigurationCollection? IHyperNodeConfiguration.CommandModules => CommandConfig;
    }
}
