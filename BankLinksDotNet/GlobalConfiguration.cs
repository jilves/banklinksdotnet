using System;
using System.Collections.Generic;
using System.Linq;
using BanklinksDotNet.ProviderBase;


namespace BanklinksDotNet
{
    public class GlobalConfiguration : IGlobalConfiguration
    {
        private readonly List<Func<AbstractBankConfiguration>> _bankConfigurations;
        private readonly List<AbstractBankProvider> _bankProviders;

        // TODO: ReadOnly test
        public IEnumerable<AbstractBankProvider> BankProviders { get { return _bankProviders.AsReadOnly(); } }
        public IEnumerable<AbstractBankConfiguration> BankConfigurations
        {
            get
            {
                return _bankConfigurations.Select(configFunc => configFunc());
            }
        }

        public bool EnableFieldLengthValidation { get; set; }

        public GlobalConfiguration()
        {
            _bankConfigurations = new List<Func<AbstractBankConfiguration>>();

            _bankProviders = new List<AbstractBankProvider>();

            EnableFieldLengthValidation = true;
        }

        public IGlobalConfiguration AddBankProvider(AbstractBankProvider provider)
        {
            _bankProviders.Insert(0, provider);

            return this;
        }

        public IGlobalConfiguration AddBankProviders(IEnumerable<AbstractBankProvider> providers)
        {
            foreach (AbstractBankProvider provider in providers)
            {
                AddBankProvider(provider);
            }

            return this;
        }

        public IGlobalConfiguration AddBankConfiguration(AbstractBankConfiguration bankConfiguration)
        {
            _bankConfigurations.Add(() => bankConfiguration);

            return this;
        }

        public IGlobalConfiguration AddBankConfigurations(IEnumerable<AbstractBankConfiguration> bankConfigurations)
        {
           _bankConfigurations.AddRange(bankConfigurations.Select((bankConfig) => new Func<AbstractBankConfiguration>(() => bankConfig)));

            return this;
        }

        public IGlobalConfiguration AddBankConfiguration(Func<AbstractBankConfiguration> bankProviderFunc)
        {
            _bankConfigurations.Add(bankProviderFunc);

            return this;
        }

        public IGlobalConfiguration AddBankConfigurations(IEnumerable<Func<AbstractBankConfiguration>> bankProviderFuncs)
        {
            _bankConfigurations.AddRange(bankProviderFuncs);

            return this;
        }
    }
}
