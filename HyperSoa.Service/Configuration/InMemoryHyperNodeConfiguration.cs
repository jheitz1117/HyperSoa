namespace HyperSoa.Service.Configuration
{
    internal class InMemoryHyperNodeConfiguration : IHyperNodeConfiguration
    {
        private readonly EmptyActivityMonitorConfigurationCollection _activityMonitors = new();
        private readonly EmptyCommandModuleConfigurationCollection _commandModules = new();
        private readonly FullAdminSystemCommandModuleConfigurationCollection _systemCommands = new();

        public string HyperNodeName => "InMemoryNode";
        public bool? EnableTaskProgressCache => true;
        public bool? EnableDiagnostics => true;
        public int? TaskProgressCacheDurationMinutes => null;
        public int? MaxConcurrentTasks => null;
        public string? TaskIdProviderType => null;
        public string HyperNodeEventHandlerType => null;


        public IActivityMonitorConfigurationCollection? ActivityMonitors => _activityMonitors;
        public ISystemCommandConfigurationCollection SystemCommands => _systemCommands;
        public ICommandModuleConfigurationCollection CommandModules => _commandModules;
    }
}
