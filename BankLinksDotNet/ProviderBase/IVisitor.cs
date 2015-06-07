namespace BanklinksDotNet.ProviderBase
{
    public interface IVisitor
    {
        IBankMessage Result { get; }
        AbstractBankConfiguration DetectedBankConfiguration { get; }

        bool IsHandled { get; }
        void Visit(IVisitable visitable);
    }
}
