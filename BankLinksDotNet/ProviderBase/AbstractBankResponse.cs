using System.Collections.Generic;

namespace BanklinksDotNet.ProviderBase
{
    public abstract class AbstractBankResponse : IBankMessage
    {
        public IEnumerable<BankMessageField> PostParameters { get; private set; }

        protected AbstractBankResponse(IEnumerable<BankMessageField> postParameters)
        {
            PostParameters = postParameters;
        }
    }
}
