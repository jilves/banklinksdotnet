using System.Collections.Generic;

namespace BanklinksDotNet.IPizzaProvider
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration AddIPizzaBankProvider(this IGlobalConfiguration configuration)
        {
            return configuration.AddBankProvider(new IPizzaBankProvider(new IPizzaMessageMapper(new BasicMessageFieldFinder(), new MacCalculatorFactory())));
        }

        public static IGlobalConfiguration AddIPizzaBankConfiguration(this IGlobalConfiguration configuration, IPizzaConfiguration bankConfiguration)
        {
            return configuration.AddBankConfiguration(bankConfiguration);
        }

        public static IGlobalConfiguration AddIPizzaBankConfigurations(this IGlobalConfiguration configuration, IEnumerable<IPizzaConfiguration> bankConfigurations)
        {
            return configuration.AddBankConfigurations(bankConfigurations);
        }
    }
}
