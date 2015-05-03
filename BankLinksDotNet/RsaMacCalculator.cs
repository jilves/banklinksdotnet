using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public class RsaMacCalculator : IMacCalculator
    {
        private readonly IPkiBankConfiguration _bankConfig;
        private readonly IRsaMacCalculatorConfig _calculatorConfig;

        public RsaMacCalculator(IRsaMacCalculatorConfig calculatorConfig, IPkiBankConfiguration bankConfig)
        {
            _bankConfig = bankConfig;
            _calculatorConfig = calculatorConfig;
        }

        public bool VerifyMac(string encoding, string mac, IEnumerable<BankMessageField> bankMessage)
        {
            string macInput = _calculatorConfig.CreateMacInput(bankMessage);
            X509Certificate2 certificate = _bankConfig.BanksPublicCertificate();

            byte[] signatureBytes = _calculatorConfig.StringToBinary(mac);
            byte[] macInputBytes = Encoding.GetEncoding(encoding).GetBytes(macInput);

            using (var rsaProvider = (RSACryptoServiceProvider)certificate.PublicKey.Key)
            {
                return rsaProvider.VerifyData(macInputBytes, "SHA1", signatureBytes);
            }
        }

        public string CreateMac(string encoding, IEnumerable<BankMessageField> bankMessage)
        {
            string macInput = _calculatorConfig.CreateMacInput(bankMessage);

            X509Certificate2 certificate = _bankConfig.MerchantsPrivateKey();
            byte[] bytes = Encoding.GetEncoding(encoding).GetBytes(macInput);
            using (var rsaProvider = (RSACryptoServiceProvider)certificate.PrivateKey)
            {
                return _calculatorConfig.BinaryToString(rsaProvider.SignData(bytes, "SHA1"));
            }
        }
    }
}
