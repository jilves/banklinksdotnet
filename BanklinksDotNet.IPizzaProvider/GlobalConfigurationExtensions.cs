namespace BanklinksDotNet.IPizzaProvider
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration AddIPizzaBankProvider(this IGlobalConfiguration configuration)
        {
            var ipizzaMessageMapper = new IPizzaMessageMapper(new BasicMessageFieldFinder(), 
                new MacCalculatorFactory(),
                new TimeProvider());;

            return configuration.AddBankProvider(new IPizzaBankProvider(ipizzaMessageMapper));
        }

        public static IGlobalConfiguration AddIPizzaBankConfiguration(this IGlobalConfiguration configuration, IPizzaConfiguration bankConfiguration)
        {
            return configuration.AddBankConfiguration(bankConfiguration);
        }

        public static IGlobalConfiguration AddIPizzaBankConfigurations(this IGlobalConfiguration configuration, params IPizzaConfiguration[] bankConfigurations)
        {
            return configuration.AddBankConfigurations(bankConfigurations);
        }
    }
}
