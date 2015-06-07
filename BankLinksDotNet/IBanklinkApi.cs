using System.Collections.Specialized;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    /// <summary>
    /// The IBanklinkApi interface provides a generic api for sending and receiving payment and authentication messages.
    /// It is up to providers to provide wrapper methods with appropriate signatures using extension methods.
    /// </summary>
    public interface IBanklinkApi
    {
        IGlobalConfiguration Configure();
        
        BankRequest CreateBankRequest(AbstractRequestParams paymentRequestParams);

        AbstractBankResponse ParseBankResponse(NameValueCollection bankResponse);
    }
}