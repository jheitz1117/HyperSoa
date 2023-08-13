using Microsoft.Extensions.Configuration;

namespace HyperSoa.ServiceHosting
{
    public class HyperCoreOptions
    {
        public enum MsgSerializationType { json, proto }
        public MsgSerializationType MessageSerializationType { get; }

        public enum ServiceBindingType { HyperCoreTcpChannel, HyperCoreHttpChannel }
        public ServiceBindingType BindingType { get; }


        public List<HyperCoreChannelConfig> HyperCoreClients { get; set; } = new List<HyperCoreChannelConfig>();
        public List<HyperCoreChannelConfig> HyperCoreServers { get; set; } = new List<HyperCoreChannelConfig>();

        public HyperCoreOptions() { }
        public HyperCoreOptions(List<HyperCoreChannelConfig>? lstClients, List<HyperCoreChannelConfig>? lstServers)
        {
            if (lstClients != null)
                HyperCoreClients.AddRange(lstClients);
            if (lstServers != null)
                HyperCoreServers.AddRange(lstServers);
        }


        private static HyperCoreOptions? instance;
        public static HyperCoreOptions Instance
        {
            get
            {
                try
                {
                    if (instance == null)
                    {
                        instance = GetCurrentSettings();
                    }
                }
                catch
                {
                    instance = new HyperCoreOptions();
                }
                return instance;
            }
        }

        public static HyperCoreOptions GetCurrentSettings()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            return new HyperCoreOptions(
                            configuration.GetSection(nameof(HyperCoreClients)).Get<List<HyperCoreChannelConfig>>(),
                            configuration.GetSection(nameof(HyperCoreServers)).Get<List<HyperCoreChannelConfig>>()
            );
        }
    }
}
