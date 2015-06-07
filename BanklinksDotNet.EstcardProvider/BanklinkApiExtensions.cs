using System.Collections.Specialized;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.EstcardProvider
{
    public static class BanklinkApiExtensions
    {
        public static BankRequest CreateEstcardPaymentRequest(this IBanklinkApi banklinkApi, EstcardPaymentRequestParams paymentRequestParams)
        {
            return banklinkApi.CreateBankRequest(paymentRequestParams);
        }

        public static EstcardPaymentResponse ParseEstcardPaymentResponse(this IBanklinkApi banklinkApi, NameValueCollection bankResponse)
        {
            return (EstcardPaymentResponse)banklinkApi.ParseBankResponse(bankResponse);
        }
    }
}
