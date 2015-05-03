namespace BanklinksDotNet.ProviderBase
{
    public abstract class AbstractRequestParams : IVisitable
    {
        public virtual string BankId { get; set; }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
