namespace BanklinksDotNet.ProviderBase
{
    public abstract class AbstractRequestParams : IVisitable
    {
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
