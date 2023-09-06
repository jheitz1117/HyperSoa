using HyperSoa.Contracts;

namespace HyperSoa.Client
{
    public static class CommandMetaData
    {
        public static ICommandMetaData Default()
        {
            return new CommandMetaDataImpl();
        }

        public static ICommandMetaData FromCommandRequest<T>(T request)
            where T : ICommandRequest
        {
            return new CommandMetaDataImpl(request);
        }
    }
}
