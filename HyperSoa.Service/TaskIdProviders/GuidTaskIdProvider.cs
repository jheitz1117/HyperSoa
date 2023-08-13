namespace HyperSoa.Service.TaskIdProviders
{
    internal sealed class GuidTaskIdProvider : TaskIdProviderBase
    {
        public override string CreateTaskId(IReadOnlyHyperNodeMessageInfo message)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
