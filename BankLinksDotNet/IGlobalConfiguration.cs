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
        IGlobalConfiguration AddBankConfiguration(AbstractBankConfiguration bankProvider);
        IGlobalConfiguration AddBankConfigurations(IEnumerable<AbstractBankConfiguration> bankProvider);
    }
}
