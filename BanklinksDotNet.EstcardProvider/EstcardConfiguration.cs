using System;
using System.Security.Cryptography.X509Certificates;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.EstcardProvider
{
    public class EstcardConfiguration : AbstractBankConfiguration, IPkiBankConfiguration
    {
        public Func<X509Certificate2> BanksPublicCertificate { get; set; }
        public Func<X509Certificate2> MerchantsPrivateKey { get; set; }
        public string MerchantId { get; set; }
        public string ServiceUrl { get; set; }
    }
}