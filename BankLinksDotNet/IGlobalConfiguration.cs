using System;
using System.Collections.Generic;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public interface IGlobalConfiguration
    {
        IEnumerable<AbstractBankProvider> BankProviders { get; }
        IEnumerable<AbstractBankConfiguration> BankConfigurations { get; }

        bool EnableFieldLengthValidation { get; set; }

        IGlobalConfiguration AddBankProvider(AbstractBankProvider provider);
        IGlobalConfiguration AddBankProviders(IEnumerable<AbstractBankProvider> providers);

        IGlobalConfiguration AddBankConfiguration(AbstractBankConfiguration bankConfiguration);
        IGlobalConfiguration AddBankConfigurations(IEnumerable<AbstractBankConfiguration> bankConfigurations);

        IGlobalConfiguration AddBankConfiguration(Func<AbstractBankConfiguration> bankProviderFunc);
        IGlobalConfiguration AddBankConfigurations(IEnumerable<Func<AbstractBankConfiguration>> bankProviderFuncs);
    }
}
