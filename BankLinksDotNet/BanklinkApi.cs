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
        private readonly ConfigurationEnforcer _configEnforcer;
        private readonly HttpParameterParser _httpParameterParser;

        public BanklinkApi()
            : this(new ConfigurationEnforcer(new GlobalConfiguration(), new Validator()), new HttpParameterParser())
        {
            
        }

        public BanklinkApi(ConfigurationEnforcer configEnforcer, HttpParameterParser httpParameterParser)
        {
            _configEnforcer = configEnforcer;
            _httpParameterParser = httpParameterParser;
        }

        public IGlobalConfiguration Configure()
        {
            return _configEnforcer.GlobalConfiguration;
        }

        public BankRequest CreateBankRequest(AbstractRequestParams paymentRequestParams)
        {
            List<IVisitor> requestVisitors = Configure().BankProviders
                .SelectMany(provider => provider.CreateTransientRequestVisitors(Configure()))
                .ToList();

            return (BankRequest)FindVisitorResult(requestVisitors, paymentRequestParams);
        }

        public AbstractBankResponse ParseBankResponse(NameValueCollection bankResponse)
        {
            List<IVisitor> responseVisitors = Configure().BankProviders
                .SelectMany(provider => provider.CreateTransientResponseVisitors(Configure()))
                .ToList();

            return (AbstractBankResponse)FindVisitorResult(responseVisitors, new VisitableNameValueCollection(bankResponse));
        }

        public AbstractBankResponse ParseBankResponse(HttpRequest bankResponse, string encoding)
        {
            return ParseBankResponse(_httpParameterParser.GetRequestParameters(bankResponse, encoding));
        }

        private IBankMessage FindVisitorResult(IEnumerable<IVisitor> visitors, IVisitable visitable)
        {
            foreach (IVisitor visitor in visitors)
            {
                visitable.Accept(visitor);
                if (!visitor.IsHandled)
                {
                    continue;
                }

                _configEnforcer.EnforceFieldLengthValidation(visitor);

                return visitor.Result;
            }

            throw new ProviderMissingException();
        }
    }
}
