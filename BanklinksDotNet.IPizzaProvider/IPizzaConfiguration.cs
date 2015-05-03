using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.IPizzaProvider
{
    // ReSharper disable once InconsistentNaming
    public class IPizzaConfiguration : IPkiBankConfiguration
    {
        public string PaymentServiceUrl { get; set; }
        public string AuthServiceUrl { get; set; }
        /// <summary>
        /// Required. Equals to VK_REC_ID in bank responses or VK_SND_ID in requests. 
        /// </summary>
        public string MerchantId { get; set; }
        /// <summary>
        /// Required. Equals to VK_SND_ID in bank responses or VK_REC_ID in requests.
        /// </summary>
        public string BankId { get; set; }
        /// <summary>
        /// Required.
        /// </summary>
        public Func<X509Certificate2> BanksPublicCertificate { get; set; }
        /// <summary>
        /// Required.
        /// </summary>
        public Func<X509Certificate2> MerchantsPrivateKey { get; set; }
        /// <summary>
        /// Optional. Defaults to yyyy-MM-ddTHH:mm:sszz00.
        /// </summary>
        public string DateTimeFormat { get; set; }
        /// <summary>
        /// Optional. Defaults to . separator.
        /// </summary>
        public NumberFormatInfo DecimalFormat { get; set; }

        public IPizzaConfiguration()
        {
            // TODO: This will fail for half hour time zones. Find a proper solution.
            DateTimeFormat = "yyyy-MM-ddTHH:mm:sszz00";
            DecimalFormat = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };
        }
    }
}
