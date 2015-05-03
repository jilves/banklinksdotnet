namespace BanklinksDotNet.ProviderBase
{
    public abstract class AbstractResponseVisitor<TResult, TVisitable, TBankConfiguration> : AbstractVisitor<TResult, TVisitable, TBankConfiguration>
        where TResult : AbstractBankResponse
        where TVisitable : VisitableNameValueCollection
        where TBankConfiguration : class
    {
        protected AbstractResponseVisitor(IGlobalConfiguration globalConfiguration) : base(globalConfiguration)
        {
        }
    }
}
