namespace HyperSoa.ServiceHosting
{
    public interface IHyperNodeChannel
    {
        public IEnumerable<string>? Endpoints { get; }
        public void Open();
        public void Abort();
        public void Close();
    }
}
