namespace BanklinksDotNet.ProviderBase
{
    public abstract class AbstractRequestVisitor<TResult, TVisitable, TBankConfiguration> : AbstractVisitor<TResult, TVisitable, TBankConfiguration>
        where TResult : BankRequest
        where TVisitable : AbstractRequestParams
        where TBankConfiguration : class
    {

        protected AbstractRequestVisitor(IGlobalConfiguration globalConfiguration) : base(globalConfiguration)
        {
        }
    }
}
