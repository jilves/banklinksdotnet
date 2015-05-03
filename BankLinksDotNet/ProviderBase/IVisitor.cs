namespace BanklinksDotNet.ProviderBase
{
    public interface IVisitor
    {
        IBankMessage Result { get; }
        bool IsHandled { get; }
        void Visit(IVisitable visitable);
    }
}
