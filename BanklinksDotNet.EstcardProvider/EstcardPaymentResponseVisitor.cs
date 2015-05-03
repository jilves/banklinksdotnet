using System.Collections.Generic;
using System.Linq;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.EstcardProvider
{
    public class EstcardPaymentResponseVisitor : AbstractResponseVisitor<EstcardPaymentResponse, VisitableNameValueCollection, EstcardConfiguration>
    {
        private readonly EstcardMessageMapper _bankMessageMapper;

        public EstcardPaymentResponseVisitor(IGlobalConfiguration globalConfiguration, EstcardMessageMapper bankMessageMapper)
            : base(globalConfiguration)
        {
            _bankMessageMapper = bankMessageMapper;
        }

        protected override bool CanHandle(VisitableNameValueCollection visitable)
        {
            string version = visitable["ver"];
            string merchantId = visitable["id"];
            string ecuno = visitable["ecuno"];

            return version != null
                && (version == "004" || version == "4")
                && merchantId != null
                && ecuno != null;
        }

        protected override EstcardConfiguration FindBankConfiguration(VisitableNameValueCollection visitable, IEnumerable<EstcardConfiguration> bankConfigurations)
        {
            return bankConfigurations.First();
        }

        protected override EstcardPaymentResponse ParseResult(VisitableNameValueCollection visitable, EstcardConfiguration bankConfiguration)
        {
            var fields = new List<BankMessageField>
            {
                new BankMessageField { OrderNr = 1, FieldName = "ver", MaxLength = 3 },
                new BankMessageField { OrderNr = 2, FieldName = "id", MaxLength = 10 },
                new BankMessageField { OrderNr = 3, FieldName = "ecuno", MaxLength = 12 },
                new BankMessageField { OrderNr = 4, FieldName = "receipt_no", MaxLength = 6 }, 
                new BankMessageField { OrderNr = 5, FieldName = "eamount", MaxLength = 12 },
                new BankMessageField { OrderNr = 6, FieldName = "cur", MaxLength = 3 }, 
                new BankMessageField { OrderNr = 7, FieldName = "respcode", MaxLength = 3 },
                new BankMessageField { OrderNr = 8, FieldName = "datetime", MaxLength = 14 },
                new BankMessageField { OrderNr = 9, FieldName = "msgdata", MaxLength = 40 },
                new BankMessageField { OrderNr = 10, FieldName = "actiontext", MaxLength = 40 },
                new BankMessageField { OrderNr = null, FieldName = "charEncoding", MaxLength = int.MaxValue },
                new BankMessageField { OrderNr = null, FieldName = "mac", MaxLength = int.MaxValue },
            };

            var bankRequest = new EstcardPaymentResponse(fields);

            _bankMessageMapper.SetPaymentResponseProperties(visitable, bankConfiguration, bankRequest);

            return bankRequest;
        }
    }
}
