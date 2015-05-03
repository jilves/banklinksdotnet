using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using BanklinksDotNet.Exceptions;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public class BanklinkApi : IBanklinkApi
    {
        private readonly IGlobalConfiguration _config;
        private readonly HttpParameterParser _httpParameterParser;

        public BanklinkApi()
            : this(new GlobalConfiguration(), new HttpParameterParser())
        {
            
        }

        public BanklinkApi(IGlobalConfiguration config, HttpParameterParser httpParameterParser)
        {
            _config = config;
            _httpParameterParser = httpParameterParser;
        }

        public IGlobalConfiguration Configure()
        {
            return _config;
        }

        public BankRequest CreateBankRequest(AbstractRequestParams paymentRequestParams)
        {
            List<IVisitor> requestVisitors = _config.BankProviders
                .SelectMany(provider => provider.CreateTransientRequestVisitors(_config))
                .ToList();

            return (BankRequest)FindVisitorResult(requestVisitors, paymentRequestParams);
        }

        public AbstractBankResponse ParseBankResponse(NameValueCollection bankResponse)
        {
            List<IVisitor> responseVisitors = _config.BankProviders
                .SelectMany(provider => provider.CreateTransientResponseVisitors(_config))
                .ToList();

            return (AbstractBankResponse)FindVisitorResult(responseVisitors, new VisitableNameValueCollection(bankResponse));
        }

        public AbstractBankResponse ParseBankResponse(HttpRequest bankResponse, string encoding)
        {
            return ParseBankResponse(_httpParameterParser.GetRequestParameters(bankResponse, encoding));
        }

        private static object FindVisitorResult(IEnumerable<IVisitor> visitors, IVisitable visitable)
        {
            foreach (IVisitor visitor in visitors)
            {
                visitable.Accept(visitor);
                if (visitor.IsHandled)
                {
                    return visitor.Result;
                }
            }

            throw new ProviderMissingException();
        }
    }
}
