namespace HyperSoa.ServiceHosting
{
    public interface IHyperNodeServiceHost
    {
        public IEnumerable<IHyperNodeChannel> GetChannels();
    }
}
