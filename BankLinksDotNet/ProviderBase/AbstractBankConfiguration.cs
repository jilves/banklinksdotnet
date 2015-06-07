namespace BanklinksDotNet.ProviderBase
{
    public abstract class AbstractBankConfiguration
    {
        public virtual bool EnableFieldLengthValidation { get; set; }

        protected AbstractBankConfiguration()
        {
            // TODO: Unit test
            EnableFieldLengthValidation = true;
        }
    }
}
