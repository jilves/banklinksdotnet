using System.Collections.Specialized;
using System.Web;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.IPizzaProvider
{
    public static class BanklinkApiExtensions
    {
        public static BankRequest CreateIPizzaPaymentRequest(this IBanklinkApi banklinkApi, IPizzaPaymentRequestParams paymentRequestParams)
        {
            return banklinkApi.CreateBankRequest(paymentRequestParams);
        }

        public static BankRequest CreateIPizzaAuthRequest(this IBanklinkApi banklinkApi, IPizzaAuthRequestParams authRequestParams)
        {
            return banklinkApi.CreateBankRequest(authRequestParams);
        }

        public static IPizzaPaymentResponse ParseIPizzaPaymentResponse(this IBanklinkApi banklinkApi, NameValueCollection bankResponse)
        {
            return (IPizzaPaymentResponse)banklinkApi.ParseBankResponse(bankResponse);
        }

        public static IPizzaAuthResponse ParseIPizzaAuthResponse(this IBanklinkApi banklinkApi, NameValueCollection bankResponse)
        {
            return (IPizzaAuthResponse)banklinkApi.ParseBankResponse(bankResponse);
        }
    }
}
