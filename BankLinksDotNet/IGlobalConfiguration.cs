using System.Collections.Generic;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public interface IGlobalConfiguration
    {
        IEnumerable<AbstractBankProvider> BankProviders { get; }
        IEnumerable<object> BankConfigurations { get; }

        IGlobalConfiguration AddBankProvider(AbstractBankProvider provider);
        IGlobalConfiguration AddBankProviders(IEnumerable<AbstractBankProvider> providers);
        IGlobalConfiguration AddBankConfiguration(object bankProvider);
        IGlobalConfiguration AddBankConfigurations(IEnumerable<object> bankProvider);
    }
}
