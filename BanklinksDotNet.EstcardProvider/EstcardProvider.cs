using System.Collections.Generic;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.EstcardProvider
{
    public class EstcardProvider : AbstractBankProvider
    {
        private readonly EstcardMessageMapper _bankMessageMapper;

        public EstcardProvider(EstcardMessageMapper bankMessageMapper)
        {
            _bankMessageMapper = bankMessageMapper;
        }

        public override IEnumerable<IVisitor> CreateTransientRequestVisitors(IGlobalConfiguration globalConfiguration)
        {
            yield return new EstcardPaymentRequestVisitor(globalConfiguration, _bankMessageMapper);
        }

        public override IEnumerable<IVisitor> CreateTransientResponseVisitors(IGlobalConfiguration globalConfiguration)
        {
            yield return new EstcardPaymentResponseVisitor(globalConfiguration, _bankMessageMapper);
        }
    }
}
