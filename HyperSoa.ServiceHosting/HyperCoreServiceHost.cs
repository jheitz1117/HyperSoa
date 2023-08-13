using System.Reflection;
using HyperSoa.Contracts;

namespace HyperSoa.ServiceHosting
{
    public class HyperCoreServiceHost
    {
        private static Assembly thisAssembly = Assembly.GetExecutingAssembly();
        private static string thisNamespace = typeof(HyperCoreServiceHost).Namespace ?? string.Empty;

        public IHyperNodeService SingletonInstance { get; set; }

        private IHyperCoreChannel serverChannel;

        public IEnumerable<string> Endpoints
        {
            get { return HyperCoreOptions.Instance.HyperCoreServers.Select(x => x.Endpoint).ToList(); }
        }

        private CommunicationState state;
        public CommunicationState State { get { return state; } }

        //public HyperCoreServiceHost(Type type, params Uri[] baseAddresses) { }

        public HyperCoreServiceHost(IHyperNodeService singletonInstance, params Uri[] baseAddresses)
        {
            this.SingletonInstance = singletonInstance;
            if (HyperCoreOptions.Instance.HyperCoreServers.Count == 0)
            {
                throw new Exception("No servers found to start process");
            }
            if (HyperCoreOptions.Instance.HyperCoreServers.Count > 1)
            {
                throw new NotImplementedException("Hosting multiple servers from one application is currently unsupported");
            }

            //foreach (HyperCoreChannelConfig cnfg in HyperCoreOptions.Instance.HyperCoreServers) {
            HyperCoreChannelConfig cnfg = HyperCoreOptions.Instance.HyperCoreServers[0];
            Type? bindType = thisAssembly.GetType($"{thisNamespace}.{cnfg.ServiceType}", true);
            //Type? bindType = thisAssembly.GetType($"{thisNamespace}.{singletonInstance.BindingType}", true);
            serverChannel = (IHyperCoreChannel)Activator.CreateInstance(bindType, new object[] { cnfg, singletonInstance });
            //}
        }

        public virtual void Open()
        {
            serverChannel.Open();
            state = CommunicationState.Opened;
        }

        public virtual void Abort()
        {
            serverChannel.Abort();
        }

        public virtual void Close()
        {
            serverChannel.Close();
        }

        protected virtual void OnAbort()
        {
            state = CommunicationState.Closing;
        }

        protected virtual void OnClosing()
        {
            state = CommunicationState.Closing;
        }

        protected virtual void OnClosed()
        {
            state = CommunicationState.Closed;
        }

        public enum CommunicationState
        {
            // Summary:
            //     Indicates that the communication object has been instantiated and is configurable,
            //     but not yet open or ready for use.
            Created,
            // Summary:
            //     Indicates that the communication object is being transitioned from the System.ServiceModel.CommunicationState.Created
            //     state to the System.ServiceModel.CommunicationState.Opened state.
            Opening,
            // Summary:
            //     Indicates that the communication object is now open and ready to be used.
            Opened,
            // Summary:
            //     Indicates that the communication object is transitioning to the System.ServiceModel.CommunicationState.Closed
            //     state.
            Closing,
            // Summary:
            //     Indicates that the communication object has been closed and is no longer usable.
            Closed,
            // Summary:
            //     Indicates that the communication object has encountered an error or fault from
            //     which it cannot recover and from which it is no longer usable.
            Faulted
        }
    }
}