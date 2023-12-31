﻿using HyperSoa.Contracts;
using HyperSoa.Contracts.Extensions;
using HyperSoa.Service.CommandModules;

namespace HostingTest.Modules.CommandModules
{
    public class NoContractCommandModule: ICommandModule
    {
        public ICommandResponse Execute(ICommandExecutionContext context)
        {
            return MessageProcessStatusFlags.Success.ToEmptyCommandResponse();
        }
    }
}
