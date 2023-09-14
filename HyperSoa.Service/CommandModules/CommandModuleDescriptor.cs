using HyperSoa.Service.Serialization;

namespace HyperSoa.Service.CommandModules
{
    internal class CommandModuleDescriptor
    {
        public string? CommandName { get; set; }
        public bool Enabled { get; set; }
        public Type? CommandModuleType { get; set; }
        public IServiceContractSerializer? ContractSerializer { get; set; }
    }
}
