namespace HyperSoa.Service.Host
{
    public interface IHyperNodeServiceHost
    {
        public IEnumerable<IHyperNodeChannel> GetChannels();
    }
}
