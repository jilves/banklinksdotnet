using System.Collections.Generic;
using System.Linq;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.IPizzaProvider
{
    // ReSharper disable once InconsistentNaming
    public class IPizzaRequest1011Visitor : AbstractRequestVisitor<BankRequest, IPizzaPaymentRequestParams, IPizzaConfiguration>
    {
        private readonly IPizzaMessageMapper _bankMessageMapper;

        public IPizzaRequest1011Visitor(IGlobalConfiguration globalConfiguration, IPizzaMessageMapper bankMessageMapper) : base(globalConfiguration)
        {
            _bankMessageMapper = bankMessageMapper;
        }

        protected override bool CanHandle(IPizzaPaymentRequestParams requestParams)
        {
            return !string.IsNullOrEmpty(requestParams.RecipientName)
                && !string.IsNullOrEmpty(requestParams.RecipientAccountNumber);
        }

        protected override IPizzaConfiguration FindBankConfiguration(IPizzaPaymentRequestParams visitable, IEnumerable<IPizzaConfiguration> bankConfigurations)
        {
            return bankConfigurations.First(config => config.BankId == visitable.BankId);
        }

        protected override BankRequest ParseResult(IPizzaPaymentRequestParams visitable, IPizzaConfiguration bankConfiguration)
        {
            var fields = new List<BankMessageField>
            {
                new BankMessageField { OrderNr = 1, FieldName = "VK_SERVICE", MaxLength = 4, Value = "1011" },
                new BankMessageField { OrderNr = 2, FieldName = "VK_VERSION", MaxLength = 3, Value = "008" },
                new BankMessageField { OrderNr = 3, FieldName = "VK_SND_ID", MaxLength = 15, Value = bankConfiguration.MerchantId },
                new BankMessageField { OrderNr = 4, FieldName = "VK_STAMP", MaxLength = 20, Value = visitable.Stamp },
                new BankMessageField { OrderNr = 5, FieldName = "VK_AMOUNT", MaxLength = 12, Value = visitable.Amount.ToString(bankConfiguration.DecimalFormat) },
                new BankMessageField { OrderNr = 6, FieldName = "VK_CURR", MaxLength = 3, Value = visitable.Currency },
                new BankMessageField { OrderNr = 7, FieldName = "VK_ACC", MaxLength = 34, Value = visitable.RecipientAccountNumber },
                new BankMessageField { OrderNr = 8, FieldName = "VK_NAME", MaxLength = 70, Value = visitable.RecipientName },
                new BankMessageField { OrderNr = 9, FieldName = "VK_REF", MaxLength = 35, Value = visitable.PaymentReferenceNumber },
                new BankMessageField { OrderNr = 10, FieldName = "VK_MSG", MaxLength = 95, Value = visitable.PaymentMessage },
                new BankMessageField { OrderNr = 11, FieldName = "VK_RETURN", MaxLength = 255, Value = visitable.SuccessReturnUrl },
                new BankMessageField { OrderNr = 12, FieldName = "VK_CANCEL", MaxLength = 255, Value = visitable.ErrorReturnUrl },
                new BankMessageField { OrderNr = 13, FieldName = "VK_DATETIME", MaxLength = 24, Value = visitable.RequestStartDateTime.ToString(bankConfiguration.DateTimeFormat) },
                new BankMessageField { OrderNr = null, FieldName = "VK_MAC", MaxLength = 700 },
                new BankMessageField { OrderNr = null, FieldName = "VK_ENCODING", MaxLength = 12, Value = visitable.RequestEncoding },
                new BankMessageField { OrderNr = null, FieldName = "VK_LANG", MaxLength = 3, Value = visitable.Language },
            };

            _bankMessageMapper.SetRequestMac(visitable.RequestEncoding, bankConfiguration, fields);

            return new BankRequest(bankConfiguration.PaymentServiceUrl, fields);
        }
    }
}
