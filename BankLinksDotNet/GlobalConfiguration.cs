using System.Collections.Generic;
using BanklinksDotNet.ProviderBase;


namespace BanklinksDotNet
{
    public class GlobalConfiguration : IGlobalConfiguration
    {
        private readonly List<AbstractBankConfiguration> _bankConfigurations;
        private readonly List<AbstractBankProvider> _bankProviders;

        public IEnumerable<AbstractBankProvider> BankProviders { get { return _bankProviders.AsReadOnly(); } }
        public IEnumerable<AbstractBankConfiguration> BankConfigurations { get { return _bankConfigurations.AsReadOnly(); } }
        public bool EnableFieldLengthValidation { get; set; }

        public GlobalConfiguration()
        {
            _bankConfigurations = new List<AbstractBankConfiguration>();
            _bankProviders = new List<AbstractBankProvider>();

            // TODO: Unit test
            EnableFieldLengthValidation = true;
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

        public IGlobalConfiguration AddBankConfiguration(AbstractBankConfiguration bankConfiguration)
        {
            _bankConfigurations.Add(bankConfiguration);

            return this;
        }

        public IGlobalConfiguration AddBankConfigurations(IEnumerable<AbstractBankConfiguration> bankConfigurations)
        {
            _bankConfigurations.AddRange(bankConfigurations);

            return this;
        }
    }
}
