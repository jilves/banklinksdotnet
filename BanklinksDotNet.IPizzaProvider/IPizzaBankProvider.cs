using System.Collections.Generic;
using BanklinksDotNet.ProviderBase;


namespace BanklinksDotNet.IPizzaProvider
{
    // ReSharper disable once InconsistentNaming
    public class IPizzaBankProvider : AbstractBankProvider
    {
        private readonly IPizzaMessageMapper _bankMessageMapper;

        public IPizzaBankProvider(IPizzaMessageMapper bankMessageMapper)
        {
            _bankMessageMapper = bankMessageMapper;
        }

        public override IEnumerable<IVisitor> CreateTransientRequestVisitors(IGlobalConfiguration globalConfiguration)
        {
            yield return new IPizzaRequest1011Visitor(globalConfiguration, _bankMessageMapper);
            yield return new IPizzaRequest1012Visitor(globalConfiguration, _bankMessageMapper);
            yield return new IPizzaRequest4011Visitor(globalConfiguration, _bankMessageMapper);
            yield return new IPizzaRequest4012Visitor(globalConfiguration, _bankMessageMapper);
        }

        public override IEnumerable<IVisitor> CreateTransientResponseVisitors(IGlobalConfiguration globalConfiguration)
        {
            yield return new IPizzaResponse1111Visitor(globalConfiguration, _bankMessageMapper);
            yield return new IPizzaResponse1911Visitor(globalConfiguration, _bankMessageMapper);
            yield return new IPizzaResponse3012Visitor(globalConfiguration, _bankMessageMapper);
            yield return new IPizzaResponse3013Visitor(globalConfiguration, _bankMessageMapper);
        }
    }
}
