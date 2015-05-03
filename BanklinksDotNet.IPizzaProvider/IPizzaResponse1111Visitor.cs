using System.Collections.Generic;
using System.Linq;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.IPizzaProvider
{

    // ReSharper disable once InconsistentNaming
    public class IPizzaResponse1111Visitor : AbstractResponseVisitor<IPizzaPaymentResponse, VisitableNameValueCollection, IPizzaConfiguration>
    {
        private readonly IPizzaMessageMapper _bankMessageMapper;

        public IPizzaResponse1111Visitor(IGlobalConfiguration globalConfiguration, IPizzaMessageMapper bankMessageMapper)
            : base(globalConfiguration)
        {
            _bankMessageMapper = bankMessageMapper;
        }

        protected override bool CanHandle(VisitableNameValueCollection requestParams)
        {
            string serviceCode = requestParams["VK_SERVICE"];
            return serviceCode != null
                   && serviceCode == "1111";
        }

        protected override IPizzaConfiguration FindBankConfiguration(VisitableNameValueCollection visitable, IEnumerable<IPizzaConfiguration> bankConfigurations)
        {
            return bankConfigurations.First(config => config.BankId == visitable["VK_SND_ID"]);
        }

        protected override IPizzaPaymentResponse ParseResult(VisitableNameValueCollection visitable, IPizzaConfiguration bankConfiguration)
        {
            var bankPaymentResponse = new List<BankMessageField>
            {
                new BankMessageField { OrderNr = 1, FieldName = "VK_SERVICE", MaxLength = 4 },
                new BankMessageField { OrderNr = 2, FieldName = "VK_VERSION", MaxLength = 3 },
                new BankMessageField { OrderNr = 3, FieldName = "VK_SND_ID", MaxLength = 15 },
                new BankMessageField { OrderNr = 4, FieldName = "VK_REC_ID", MaxLength = 15, },
                new BankMessageField { OrderNr = 5, FieldName = "VK_STAMP", MaxLength = 20 },
                new BankMessageField { OrderNr = 6, FieldName = "VK_T_NO", MaxLength = 20 },
                new BankMessageField { OrderNr = 7, FieldName = "VK_AMOUNT", MaxLength = 12 },
                new BankMessageField { OrderNr = 8, FieldName = "VK_CURR", MaxLength = 3 },
                new BankMessageField { OrderNr = 9, FieldName = "VK_REC_ACC", MaxLength = 34 },
                new BankMessageField { OrderNr = 10, FieldName = "VK_REC_NAME", MaxLength = 70 },
                new BankMessageField { OrderNr = 11, FieldName = "VK_SND_ACC", MaxLength = 34 },
                new BankMessageField { OrderNr = 12, FieldName = "VK_SND_NAME", MaxLength = 70 },
                new BankMessageField { OrderNr = 13, FieldName = "VK_REF", MaxLength = 35 },
                new BankMessageField { OrderNr = 14, FieldName = "VK_MSG", MaxLength = 95 },
                new BankMessageField { OrderNr = 15, FieldName = "VK_T_DATETIME", MaxLength = 24 },
                new BankMessageField { OrderNr = null, FieldName = "VK_MAC", MaxLength = 700 },
                new BankMessageField { OrderNr = null, FieldName = "VK_ENCODING", MaxLength = 12 },
                new BankMessageField { OrderNr = null, FieldName = "VK_LANG", MaxLength = 3 },
                new BankMessageField { OrderNr = null, FieldName = "VK_AUTO", MaxLength = 1 },
            };

            var ipizzaPaymentResponse = new IPizzaPaymentResponse(bankConfiguration.BankId, bankPaymentResponse);

            _bankMessageMapper.SetPaymentResponseProperties(visitable, bankConfiguration, ipizzaPaymentResponse);

            return ipizzaPaymentResponse;
        }
    }
}