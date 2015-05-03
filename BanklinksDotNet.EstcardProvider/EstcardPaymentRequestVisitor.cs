using System.Collections.Generic;
using System.Linq;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.EstcardProvider
{
    public class EstcardPaymentRequestVisitor : AbstractRequestVisitor<BankRequest, EstcardPaymentRequestParams, EstcardConfiguration>
    {
        private readonly EstcardMessageMapper _bankMessageMapper;

        public EstcardPaymentRequestVisitor(IGlobalConfiguration globalConfiguration, EstcardMessageMapper bankMessageMapper)
            : base(globalConfiguration)
        {
            _bankMessageMapper = bankMessageMapper;
        }

        protected override bool CanHandle(EstcardPaymentRequestParams visitable)
        {
            // If this method is called then EstcardPaymentRequestParams is passed in as a parameter.
            // This is the only visitor that can handle that params class.

            return true;
        }

        protected override EstcardConfiguration FindBankConfiguration(EstcardPaymentRequestParams visitable, IEnumerable<EstcardConfiguration> bankConfigurations)
        {
            return bankConfigurations.First();
        }

        protected override BankRequest ParseResult(EstcardPaymentRequestParams visitable, EstcardConfiguration bankConfiguration)
        {
            var fields = new List<BankMessageField>
            {
                new BankMessageField { OrderNr = 1, FieldName = "ver", MaxLength = 3, Value = visitable.Version },
                new BankMessageField { OrderNr = 2, FieldName = "id", MaxLength = 10, Value = bankConfiguration.MerchantId },
                new BankMessageField { OrderNr = 3, FieldName = "ecuno", MaxLength = 12, Value = visitable.TransactionNr.ToString() },
                new BankMessageField { OrderNr = 4, FieldName = "eamount", MaxLength = 12, Value = visitable.AmountInCents.ToString() }, 
                new BankMessageField { OrderNr = 5, FieldName = "cur", MaxLength = 3, Value = visitable.Currency },
                new BankMessageField { OrderNr = 6, FieldName = "datetime", MaxLength = 14, Value = visitable.TransactionDateTime.ToString("yyyyMMddHHmmss") }, // TODO: Configurable?
                new BankMessageField { OrderNr = 7, FieldName = "feedBackUrl", MaxLength = 128, Value = visitable.ReturnUrl },
                new BankMessageField { OrderNr = 8, FieldName = "delivery", MaxLength = 1, Value = visitable.Delivery },
                new BankMessageField { OrderNr = null, FieldName = "lang", MaxLength = 2, Value = visitable.Language },
                new BankMessageField { OrderNr = null, FieldName = "action", MaxLength = 3, Value = visitable.ActionName },
                // Max length is not specified in spec.
                new BankMessageField { OrderNr = null, FieldName = "charEncoding", MaxLength = int.MaxValue, Value = visitable.RequestEncoding },
                new BankMessageField { OrderNr = null, FieldName = "mac", MaxLength = int.MaxValue },
            };

            _bankMessageMapper.SetRequestMac(visitable.RequestEncoding, bankConfiguration, fields);

            return new BankRequest(bankConfiguration.ServiceUrl, fields);
        }
    }
}
