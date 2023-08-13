namespace HyperSoa.ServiceHosting
{
    public interface IHyperCoreChannel
    {
        public void Open();
        public void Abort();
        public void Close();
    }
}
