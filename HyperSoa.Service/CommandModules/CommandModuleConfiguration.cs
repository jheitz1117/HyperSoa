using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules
{
    internal class CommandModuleConfiguration
    {
        public string CommandName { get; set; }
        public bool Enabled { get; set; }
        public Type CommandModuleType { get; set; }
        public IContractSerializer? ContractSerializer { get; set; }
    }
}
