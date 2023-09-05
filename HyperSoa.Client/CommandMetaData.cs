namespace HyperSoa.Client
{
    public static class CommandMetaData
    {
        public static ICommandMetaData Empty()
        {
            return new CommandMetaDataImpl();
        }
    }
}
