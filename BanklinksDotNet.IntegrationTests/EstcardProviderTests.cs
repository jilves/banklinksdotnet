using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using BanklinksDotNet.EstcardProvider;
using BanklinksDotNet.ProviderBase;
using NUnit.Framework;

namespace BanklinksDotNet.IntegrationTests
{
    [TestFixture]
    public class EstcardProviderTests
    {
        private BanklinkApi _banklinkApi;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _banklinkApi = new BanklinkApi();
            _banklinkApi.Configure()
                .AddEstcardProvider()
                .AddEstcardConfiguration(new EstcardConfiguration
                {
                    MerchantId = "uid100049",
                    ServiceUrl = "http://localhost:8080/banklink/ec",
                    BanksPublicCertificate = () => new X509Certificate2("Certs/Estcard/bank_cert.pem"),
                    MerchantsPrivateKey = () => new X509Certificate2("Certs/Estcard/merchant.pfx"),
                });
        }

        [Test]
        public void CreateEstcardPaymentRequest_Verify_Payment_Request_Parameters()
        {
            BankRequest bankRequest = _banklinkApi.CreateEstcardPaymentRequest(new EstcardPaymentRequestParams
            {
                AmountInCents = 1336,
                ReturnUrl = "http://localhost:8080/project/mJltvgDF1boyOPpL?payment_action=success",
                TransactionDateTime = DateTime.ParseExact("20140217154349", "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                Language = "en",
                RequestEncoding = "utf-8",
                TransactionNr = 1392644629
            });

            Assert.AreEqual("http://localhost:8080/banklink/ec", bankRequest.RequestUrl);

            bankRequest.AssertFieldValueEqualTo("action", "gaf");
            bankRequest.AssertFieldValueEqualTo("ver", "004");
            bankRequest.AssertFieldValueEqualTo("id", "uid100049");
            bankRequest.AssertFieldValueEqualTo("ecuno", "1392644629");
            bankRequest.AssertFieldValueEqualTo("eamount", "1336");
            bankRequest.AssertFieldValueEqualTo("cur", "EUR");
            bankRequest.AssertFieldValueEqualTo("datetime", "20140217154349");
            bankRequest.AssertFieldValueEqualTo("feedBackUrl", "http://localhost:8080/project/mJltvgDF1boyOPpL?payment_action=success");
            bankRequest.AssertFieldValueEqualTo("delivery", "S");
            bankRequest.AssertFieldValueEqualTo("charEncoding", "utf-8");
            bankRequest.AssertFieldValueEqualTo("mac", "6E519E71A0403F1A528A4433D89CDEC413B8018A5471E98E0224AD9135C4AA9E3AF6C4635E06CD855E9CEAA7672FB14CA25E542DFEC83840C2D35D078CF7C02F55CEF1ED73F462DD404E141E38E3877ECA287E751469EBD1C20F4E35E77F4751F99688421A5E909D6CBD7208D4F1EAA8BC7F598ACFFAC11D8B7343B0FED592906E37D160AB4A5C3E263459D1FAFD849E242FC2BDF7F9450AA030D1DF1A1A836E64FBA3B2607C030DC4F4976249ED195D578A3DB16FB65D4C158F64A4215F4C150EA6B51DC32164DDE29B61D0FA0F711B731D11B3FB37AAF8D423D871C1122F0AAF11781F4CC078C39EEDE5DD56C14C4F651D7F347C3FF7405C5EAD074A4EB51A");
        }

        [Test]
        public void CreateEstcardPaymentRequest_Verify_Succesful_Payment_Response_Parameters()
        {
            EstcardPaymentResponse bankResponse = _banklinkApi.ParseEstcardPaymentResponse(new NameValueCollection
            {
                { "action", "afb" },
                { "ver", "4" },
                { "id", "uid100049" },

                { "ecuno", "1392644629" },
                { "receipt_no", "10001" },
                { "eamount", "1336" },

                { "cur", "EUR" },
                { "respcode", "000" },
                { "datetime", "20140217154349" },

                { "msgdata", "Tõõger Leõpäöld" },
                { "actiontext", "OK, tehing autoriseeritud" },

                { "charEncoding", "utf-8" },

                { "mac", "7C4CC2A28C4BDD0C861557BD52C1B5F1C597534BC14B07F621207A295B7E4D6D36FA9FFE140AE711AFEE68D2DFE371E31A38DF6AB83CB383DB131E0B4B846A6D001696F5EBFE13FECB83B1A2CF3B8335CB0EACF5E842D9DED2F4DCE3C4EFBDBD1B5FCE397C8BC2B829DEFB04E7179F04BE61E2773F9B1447C517CECA396C823BD8DEC4718D6CE2C32DD56D8C294E5084C6A87A1F69B17E1419BB2361B7AB66E5910B30A1243C8F7DF531857F793B4ABF2B11FA4C6D16950D17FCA54161FCFF6920E366EB2A9B37CBB64F5E3C3C6C4549FED3ED3D365C57F307C3DB75E2147BA5E81FD87E0EDF48ECE49A33F834C3A5FA1DC71154FA0A877916165053BE96876E" },
                { "auto", "N" },
            });


            Assert.That(bankResponse.TransactionNr, Is.EqualTo(1392644629));
            Assert.That(bankResponse.ReceiptNr, Is.EqualTo(10001));
            Assert.That(bankResponse.AmountInCents, Is.EqualTo(1336));
            Assert.That(bankResponse.Currency, Is.EqualTo("EUR"));
            Assert.That(bankResponse.IsPaymentSuccessful, Is.True);
            Assert.That(bankResponse.TransactionDateTime,
                Is.EqualTo(DateTime.ParseExact("20140217154349", "yyyyMMddHHmmss", CultureInfo.InvariantCulture)));
            Assert.That(bankResponse.MsgData, Is.EqualTo("Tõõger Leõpäöld"));
            Assert.That(bankResponse.ActionText, Is.EqualTo("OK, tehing autoriseeritud"));
        }
    }
}
