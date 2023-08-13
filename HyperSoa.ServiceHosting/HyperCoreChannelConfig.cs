namespace HyperSoa.ServiceHosting
{
    public class HyperCoreChannelConfig
    {
        public string Name { get; set; } = String.Empty;
        public string Endpoint { get; set; } = String.Empty;
        public HyperCoreOptions.MsgSerializationType MessageFormat { get; set; } = HyperCoreOptions.MsgSerializationType.proto;
        public HyperCoreOptions.ServiceBindingType ServiceType { get; set; } = HyperCoreOptions.ServiceBindingType.HyperCoreHttpChannel;
    }
}
