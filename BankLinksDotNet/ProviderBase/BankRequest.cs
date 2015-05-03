using System.Collections.Generic;

namespace BanklinksDotNet.ProviderBase
{
    public class BankRequest : IBankMessage
    {
        public IEnumerable<BankMessageField> PostParameters { get; private set; }
        public string RequestUrl { get; private set; }

        public BankRequest(string requestUrl, IEnumerable<BankMessageField> postParameters)
        {
            RequestUrl = requestUrl;
            PostParameters = postParameters;
        }
    }
}
