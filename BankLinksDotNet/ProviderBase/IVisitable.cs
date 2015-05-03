namespace BanklinksDotNet.ProviderBase
{
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}
