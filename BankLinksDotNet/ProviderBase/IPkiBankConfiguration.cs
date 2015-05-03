using System;
using System.Security.Cryptography.X509Certificates;

namespace BanklinksDotNet.ProviderBase
{
    public interface IPkiBankConfiguration
    {
        Func<X509Certificate2> BanksPublicCertificate { get; set; }
        Func<X509Certificate2> MerchantsPrivateKey { get; set; }
    }
}
