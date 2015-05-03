using System.Collections.Generic;
using BanklinksDotNet.ProviderBase;


namespace BanklinksDotNet
{
    public class GlobalConfiguration : IGlobalConfiguration
    {
        private readonly List<object> _bankConfigurations;
        private readonly List<AbstractBankProvider> _bankProviders;

        public IEnumerable<AbstractBankProvider> BankProviders { get { return _bankProviders.AsReadOnly(); } }
        public IEnumerable<object> BankConfigurations { get { return _bankConfigurations.AsReadOnly(); } }

        public GlobalConfiguration()
        {
            _bankConfigurations = new List<object>();
            _bankProviders = new List<AbstractBankProvider>();
        }

        public IGlobalConfiguration AddBankProvider(AbstractBankProvider provider)
        {
            _bankProviders.Insert(0, provider);

            return this;
        }

        public IGlobalConfiguration AddBankProviders(IEnumerable<AbstractBankProvider> providers)
        {
            _bankProviders.InsertRange(0, providers);

            return this;
        }

        public IGlobalConfiguration AddBankConfiguration(object bankConfiguration)
        {
            _bankConfigurations.Add(bankConfiguration);

            return this;
        }

        public IGlobalConfiguration AddBankConfigurations(IEnumerable<object> bankConfigurations)
        {
            _bankConfigurations.AddRange(bankConfigurations);

            return this;
        }
    }
}
