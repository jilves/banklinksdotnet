using System.Collections.Generic;
using System.Linq;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.IPizzaProvider
{
    // ReSharper disable once InconsistentNaming
    public class IPizzaRequest4012Visitor : AbstractRequestVisitor<BankRequest, IPizzaAuthRequestParams, IPizzaConfiguration>
    {
        private readonly IPizzaMessageMapper _bankMessageMapper;

        public IPizzaRequest4012Visitor(IGlobalConfiguration globalConfiguration, IPizzaMessageMapper bankMessageMapper)
            : base(globalConfiguration)
        {
            _bankMessageMapper = bankMessageMapper;
        }

        protected override bool CanHandle(IPizzaAuthRequestParams requestParams)
        {
            return string.IsNullOrEmpty(requestParams.ExpectedReturnCode);
        }

        protected override IPizzaConfiguration FindBankConfiguration(IPizzaAuthRequestParams visitable, IEnumerable<IPizzaConfiguration> bankConfigurations)
        {
            return bankConfigurations.First(config => config.BankId == visitable.BankId);
        }

        protected override BankRequest ParseResult(IPizzaAuthRequestParams visitable, IPizzaConfiguration bankConfiguration)
        {
            var fields = new List<BankMessageField>
            {
                new BankMessageField { OrderNr = 1, FieldName = "VK_SERVICE", MaxLength = 4, Value = "4012" },
                new BankMessageField { OrderNr = 2, FieldName = "VK_VERSION", MaxLength = 3, Value = "008" },
                new BankMessageField { OrderNr = 3, FieldName = "VK_SND_ID", MaxLength = 15, Value = bankConfiguration.MerchantId },
                new BankMessageField { OrderNr = 4, FieldName = "VK_REC_ID", MaxLength = 15, Value = bankConfiguration.BankId },
                new BankMessageField { OrderNr = 5, FieldName = "VK_NONCE", MaxLength = 50, Value = visitable.Nonce },
                new BankMessageField { OrderNr = 6, FieldName = "VK_RETURN", MaxLength = 255, Value = visitable.ReturnUrl },
                new BankMessageField { OrderNr = 7, FieldName = "VK_DATETIME", MaxLength = 24, Value = visitable.RequestStartDateTime.ToString(bankConfiguration.DateTimeFormat) },
                new BankMessageField { OrderNr = 8, FieldName = "VK_RID", MaxLength = 30, Value = visitable.RequestId },
                new BankMessageField { OrderNr = null, FieldName = "VK_MAC", MaxLength = 700 },
                new BankMessageField { OrderNr = null, FieldName = "VK_ENCODING", MaxLength = 12, Value = visitable.RequestEncoding },
                new BankMessageField { OrderNr = null, FieldName = "VK_LANG", MaxLength = 3, Value = visitable.Language },
            };

            _bankMessageMapper.SetRequestMac(visitable.RequestEncoding, bankConfiguration, fields);

            return new BankRequest(bankConfiguration.AuthServiceUrl, fields); ;
        }
    }
}
