using System.Collections.Generic;
using System.Linq;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.IPizzaProvider
{
    // ReSharper disable once InconsistentNaming
    public class IPizzaResponse3012Visitor : AbstractResponseVisitor<AbstractBankResponse, VisitableNameValueCollection, IPizzaConfiguration>
    {
        private readonly IPizzaMessageMapper _bankMessageMapper;

        public IPizzaResponse3012Visitor(IGlobalConfiguration globalConfiguration, IPizzaMessageMapper bankMessageMapper)
            : base(globalConfiguration)
        {
            _bankMessageMapper = bankMessageMapper;
        }

        protected override bool CanHandle(VisitableNameValueCollection requestParams)
        {
            string serviceCode = requestParams["VK_SERVICE"];
            return serviceCode != null
                   && serviceCode == "3012";
        }

        protected override IPizzaConfiguration FindBankConfiguration(VisitableNameValueCollection visitable, IEnumerable<IPizzaConfiguration> bankConfigurations)
        {
            return bankConfigurations.First(config => config.BankId == visitable["VK_SND_ID"]);
        }

        protected override AbstractBankResponse ParseResult(VisitableNameValueCollection visitable, IPizzaConfiguration bankConfiguration)
        {
            var bankAuthResponse = new List<BankMessageField>
            {
                new BankMessageField { OrderNr = 1, FieldName = "VK_SERVICE", MaxLength = 4 },
                new BankMessageField { OrderNr = 2, FieldName = "VK_VERSION", MaxLength = 3 },
                new BankMessageField { OrderNr = 3, FieldName = "VK_USER", MaxLength = 16 },
                new BankMessageField { OrderNr = 4, FieldName = "VK_DATETIME", MaxLength = 24 },
                new BankMessageField { OrderNr = 5, FieldName = "VK_SND_ID", MaxLength = 15 },
                new BankMessageField { OrderNr = 6, FieldName = "VK_REC_ID", MaxLength = 15, },
                new BankMessageField { OrderNr = 7, FieldName = "VK_USER_NAME", MaxLength = 140 },
                new BankMessageField { OrderNr = 8, FieldName = "VK_USER_ID", MaxLength = 20 },
                new BankMessageField { OrderNr = 9, FieldName = "VK_COUNTRY", MaxLength = 2 },
                new BankMessageField { OrderNr = 10, FieldName = "VK_OTHER", MaxLength = 150 },
                new BankMessageField { OrderNr = 11, FieldName = "VK_TOKEN", MaxLength = 2 },
                new BankMessageField { OrderNr = 12, FieldName = "VK_RID", MaxLength = 30 },
                new BankMessageField { OrderNr = null, FieldName = "VK_MAC", MaxLength = 700 },
                new BankMessageField { OrderNr = null, FieldName = "VK_ENCODING", MaxLength = 12 },
                new BankMessageField { OrderNr = null, FieldName = "VK_LANG", MaxLength = 3 },
            };

            var ipizzaAuthResponse = new IPizzaAuthResponse(bankConfiguration.BankId, bankAuthResponse);

            _bankMessageMapper.SetAuthResponseProperties(visitable, bankConfiguration, ipizzaAuthResponse);

            return ipizzaAuthResponse;
        }
    }
}
