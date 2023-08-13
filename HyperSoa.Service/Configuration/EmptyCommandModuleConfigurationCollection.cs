using System.Collections;

namespace HyperSoa.Service.Configuration
{
    internal class EmptyCommandModuleConfigurationCollection : ICommandModuleConfigurationCollection
    {
        private readonly IEnumerable<ICommandModuleConfiguration> _empty = Array.Empty<ICommandModuleConfiguration>();

        public IEnumerator<ICommandModuleConfiguration> GetEnumerator()
        {
            return _empty.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string ContractSerializerType => null;
    }
}
