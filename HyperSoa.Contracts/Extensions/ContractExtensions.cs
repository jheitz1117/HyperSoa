namespace HyperSoa.Contracts.Extensions
{
    public static class ContractExtensions
    {
        public static EmptyCommandResponse ToEmptyCommandResponse(this MessageProcessStatusFlags statusFlags)
        {
            return new EmptyCommandResponse
            {
                ProcessStatusFlags = statusFlags
            };
        }
    }
}
