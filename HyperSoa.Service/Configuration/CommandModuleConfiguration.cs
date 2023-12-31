﻿namespace HyperSoa.Service.Configuration
{
    public class CommandModuleConfiguration : ICommandModuleConfiguration
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public bool Enabled { get; set; }
        public string? ContractSerializerType { get; set; }

        string? ICommandModuleConfiguration.CommandName => Name;
        string? ICommandModuleConfiguration.CommandModuleType => Type;
    }
}
