namespace BanklinksDotNet.EstcardProvider
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration AddEstcardProvider(this IGlobalConfiguration configuration)
        {
            return configuration.AddBankProvider(new EstcardProvider(new EstcardMessageMapper(new BasicMessageFieldFinder(), new MacCalculatorFactory())));
        }

        public static IGlobalConfiguration AddEstcardConfiguration(this IGlobalConfiguration configuration, EstcardConfiguration bankConfiguration)
        {
            // TODO: Validate to avoid > 1 EstcardConfigurations?

            return configuration.AddBankConfiguration(bankConfiguration);
        }
    }
}
