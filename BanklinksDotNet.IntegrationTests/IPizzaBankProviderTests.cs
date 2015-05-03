using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BanklinksDotNet.IPizzaProvider;
using BanklinksDotNet.ProviderBase;
using NUnit.Framework;

namespace BanklinksDotNet.IntegrationTests
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class IPizzaBankProviderTests
    {
        private BanklinkApi _banklinkApi;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _banklinkApi = new BanklinkApi();
            _banklinkApi.Configure()
                .AddIPizzaBankProvider()
                .AddIPizzaBankConfiguration(new IPizzaConfiguration
                {
                    BankId = "GENIPIZZA",
                    MerchantId = "uid100010",
                    PaymentServiceUrl = "http://localhost:8080/banklink/ipizzapayment",
                    AuthServiceUrl = "http://localhost:8080/banklink/ipizzaauth",
                    BanksPublicCertificate = () => new X509Certificate2("Certs/IPizza/bank_cert.pem"),
                    MerchantsPrivateKey = () => new X509Certificate2("Certs/IPizza/merchant.pfx"),
                });
        }

        [Test]
        public void CreateIPizzaPaymentRequest_Verify_1011_Request_Parameters()
        {
            BankRequest bankRequest = _banklinkApi.CreateIPizzaPaymentRequest(new IPizzaPaymentRequestParams
            {
                ErrorReturnUrl = "http://localhost:8080/project/6rGPnXJ7cvstStKx?payment_action=cancel",
                Amount = 150M,
                PaymentMessage = "Torso Tiger",
                SuccessReturnUrl = "http://localhost:8080/project/6rGPnXJ7cvstStKx?payment_action=success",
                RecipientAccountNumber = "EE871600161234567892",
                RecipientName = "ÕIE MÄGER",
                Stamp = "12345",
                Currency = "EUR",
                PaymentReferenceNumber = "1234561",
                Language = "EST",
                RequestStartDateTime = DateTime.ParseExact("2015-04-04T22:58:16+0300", "yyyy-MM-ddTHH:mm:sszz00", CultureInfo.InvariantCulture),
                RequestEncoding = "UTF-8",
                BankId = "GENIPIZZA"
            });

            Assert.That(bankRequest.RequestUrl, Is.EqualTo("http://localhost:8080/banklink/ipizzapayment"));

            bankRequest.AssertFieldValueEqualTo("VK_SERVICE", "1011");
            bankRequest.AssertFieldValueEqualTo("VK_VERSION", "008");
            bankRequest.AssertFieldValueEqualTo("VK_SND_ID", "uid100010");
            bankRequest.AssertFieldValueEqualTo("VK_STAMP", "12345");
            bankRequest.AssertFieldValueEqualTo("VK_AMOUNT", "150");
            bankRequest.AssertFieldValueEqualTo("VK_CURR", "EUR");
            bankRequest.AssertFieldValueEqualTo("VK_ACC", "EE871600161234567892");
            bankRequest.AssertFieldValueEqualTo("VK_NAME", "ÕIE MÄGER");
            bankRequest.AssertFieldValueEqualTo("VK_REF", "1234561");
            bankRequest.AssertFieldValueEqualTo("VK_MSG", "Torso Tiger");
            bankRequest.AssertFieldValueEqualTo("VK_RETURN", "http://localhost:8080/project/6rGPnXJ7cvstStKx?payment_action=success");
            bankRequest.AssertFieldValueEqualTo("VK_CANCEL", "http://localhost:8080/project/6rGPnXJ7cvstStKx?payment_action=cancel");
            bankRequest.AssertFieldValueEqualTo("VK_DATETIME", "2015-04-04T22:58:16+0300");
            bankRequest.AssertFieldValueEqualTo("VK_MAC", "N2aCtj20ynyMqirBV77bd/8JpG7yCH6G1W44yKFI7fHdnzgd0zUXZgkhNKOuZB+se0lmj5m+rzzvpkhuPjig3MI0gTE7SLcUAerck8QKr02dd+EQv14OKrw3wZSuuwPUZcH/PvySctKT3kOSO6FFI8ymdww7/CC1xdSAWAOes2wbv68Dtwhy4qQ1pc+jSqFNmNjwgK56BP1D2JBhikATUJxM4b0/GUA0utqkekekqrinElaQRQ8ddN8Jij7t+KPQme1w5xGU48DR4mI+A6nHKAVm+UinbEkRIuoc0sWba9vBvNBSdIAMYv1a3BVpg956Kx0uo1ZTc7veDDi7tFCb9w==");
            bankRequest.AssertFieldValueEqualTo("VK_ENCODING", "UTF-8");
            bankRequest.AssertFieldValueEqualTo("VK_LANG", "EST");
        }

        [Test]
        public void CreateIPizzaPaymentRequest_Verify_1012_Request_Parameters()
        {
            BankRequest bankRequest = _banklinkApi.CreateIPizzaPaymentRequest(new IPizzaPaymentRequestParams
            {
                ErrorReturnUrl = "http://localhost:30535/Home/AcceptPayment",
                Amount = 10.15M,
                PaymentMessage = "Shut up and take my $$$!",
                SuccessReturnUrl = "http://localhost:30535/Home/AcceptPayment",
                Stamp = "74525",
                Currency = "EUR",
                PaymentReferenceNumber = "",
                Language = "EST",
                RequestStartDateTime = DateTime.ParseExact("2015-04-04T23:17:34+0300", "yyyy-MM-ddTHH:mm:sszz00", CultureInfo.InvariantCulture),
                RequestEncoding = "UTF-8",
                BankId = "GENIPIZZA"
            });

            Assert.That(bankRequest.RequestUrl, Is.EqualTo("http://localhost:8080/banklink/ipizzapayment"));

            bankRequest.AssertFieldValueEqualTo("VK_SERVICE", "1012");
            bankRequest.AssertFieldValueEqualTo("VK_VERSION", "008");
            bankRequest.AssertFieldValueEqualTo("VK_SND_ID", "uid100010");
            bankRequest.AssertFieldValueEqualTo("VK_STAMP", "74525");
            bankRequest.AssertFieldValueEqualTo("VK_AMOUNT", "10.15");
            bankRequest.AssertFieldValueEqualTo("VK_CURR", "EUR");
            bankRequest.AssertFieldValueEqualTo("VK_REF", "");
            bankRequest.AssertFieldValueEqualTo("VK_MSG", "Shut up and take my $$$!");
            bankRequest.AssertFieldValueEqualTo("VK_RETURN", "http://localhost:30535/Home/AcceptPayment");
            bankRequest.AssertFieldValueEqualTo("VK_CANCEL", "http://localhost:30535/Home/AcceptPayment");
            bankRequest.AssertFieldValueEqualTo("VK_DATETIME", "2015-04-04T23:17:34+0300");
            bankRequest.AssertFieldValueEqualTo("VK_MAC", "L1R5dsif+0Bba2NMZSN0f8uvyyLvtGd6JDIdOQ4/tK5bwLMHss5cZgFmKrUC8pXlmOtMYXrd+sh3WdqTo5W2K1N/Vwq6iPM14J56ZlJ24hM9iRS8KeCIiWkXgAnWpRtrXvvZWnLYkTdZrTiwZajml6hbkxiQY0UPSkQ/CNU3Re1mSjNW2ys0AJJ7RuoGmXbY5htubEcEiKsHFC76q1Ije+OrKlVJPCSLMbfAPe8Wc7LEP5OTowJ/O6MxocFE0A4U5ks5bSc3QO2XRZaIy6FkCIjZABhABICBmyuabfsWaHYAuuN7OjIvfYaTTryaAhY1gfWpLs1z5ForwOmGT5bG4A==");
            bankRequest.AssertFieldValueEqualTo("VK_ENCODING", "UTF-8");
            bankRequest.AssertFieldValueEqualTo("VK_LANG", "EST");
        }

        [Test]
        public void ParseIPizzaPaymentResponse_Verify_1111_Response_Parameters()
        {
            IPizzaPaymentResponse paymentResponse = _banklinkApi.ParseIPizzaPaymentResponse(new NameValueCollection
            {
                { "VK_SERVICE", "1111" },
                { "VK_VERSION", "008" },
                { "VK_SND_ID", "GENIPIZZA" },

                { "VK_REC_ID", "uid100010" },
                { "VK_STAMP", "74525" },
                { "VK_T_NO", "10010" },

                { "VK_AMOUNT", "10.15" },
                { "VK_CURR", "EUR" },
                { "VK_REC_ACC", "" },

                { "VK_REC_NAME", "" },
                { "VK_SND_ACC", "EE871600161234567892" },
                { "VK_SND_NAME", "Tõõger Leõpäöld" },

                { "VK_REF", "" },
                { "VK_MSG", "Shut up and take my $$$!" },
                { "VK_T_DATETIME", "2015-04-04T23:17:35+0300" },

                { "VK_ENCODING", "UTF-8" },
                { "VK_LANG", "EST" },
                { "VK_MAC", "U99SSoDkqZXN6zE5ht8ORLKs4xghXwGcTbi5QU97qF/7sg8T2yN/HIBNuIi/ww0oQr0ZcCrseDYGc58QE+UMejNBnc1cH2TYxmWGBPyAoAfUoGyHtgJqdPtXf5oa22OxhDk0hhMCyMEzbLClsYZ3L3nUIc3LI6Lf/JFOLQuRYm20NErOx6oXtn03bfLYBV4iQGFn91HwD3z4w8VDHJmBC+KlVHLTmSCTXzB1KDIXT2iE6eLe/6cUT0laUtU/EB5a03Eds6TM/17iwdk1wIA9m/JCSvsE4VAacwiOdOv4oua0DQhVTMNevVA5Sx/IA0BiENM7m6FnTs++XKOkqboU5Q==" },
                
                { "VK_AUTO", "N" },
            });

            Assert.That(paymentResponse.BankId, Is.EqualTo("GENIPIZZA"));
            Assert.That(paymentResponse.Amount, Is.EqualTo(10.15M));
            Assert.That(paymentResponse.Currency, Is.EqualTo("EUR"));

            Assert.That(paymentResponse.IsAutomaticResponse, Is.False);
            Assert.That(paymentResponse.IsPaymentSuccessful, Is.True);

            Assert.That(paymentResponse.PaymentMessage, Is.EqualTo("Shut up and take my $$$!"));
            Assert.That(paymentResponse.PaymentOrderNumber, Is.EqualTo("10010"));
            Assert.That(paymentResponse.PaymentOrderReferenceNumber, Is.Empty);

            Assert.That(paymentResponse.PaymentReceiverAccount, Is.Empty);
            Assert.That(paymentResponse.PaymentReceiverName, Is.Empty);
            Assert.That(paymentResponse.PaymentSenderAccount, Is.EqualTo("EE871600161234567892"));

            Assert.That(paymentResponse.PaymentSenderName, Is.EqualTo("Tõõger Leõpäöld"));
            Assert.That(paymentResponse.RequestEncoding, Is.EqualTo("UTF-8"));
            Assert.That(paymentResponse.RequestStartDateTime,
                Is.EqualTo(DateTime.ParseExact("2015-04-04T23:17:35+0300", "yyyy-MM-ddTHH:mm:sszz00", CultureInfo.InvariantCulture)));

            Assert.That(paymentResponse.Language, Is.EqualTo("EST"));
            Assert.That(paymentResponse.Stamp, Is.EqualTo("74525"));
        }

        [Test]
        public void ParseIPizzaPaymentResponse_Verify_1911_Response_Parameters()
        {
            IPizzaPaymentResponse paymentResponse = _banklinkApi.ParseIPizzaPaymentResponse(new NameValueCollection
            {
                { "VK_SERVICE", "1911" },
                { "VK_VERSION", "008" },
                { "VK_SND_ID", "GENIPIZZA" },

                { "VK_REC_ID", "uid100010" },
                { "VK_STAMP", "541260" },
                { "VK_REF", "" },

                { "VK_MSG", "Shut up and take my $$$!" },
                { "VK_ENCODING", "UTF-8" },
                { "VK_LANG", "EST" },

                { "VK_MAC", "HfELX7F/t1+Uhwj9zxfHbCmUcjpHse7Gz1bN/QnW3OKvyBkZUvTdJnpQU2sZuvZ1TdIfXjRxpyJQ2J2JoeE/MZEtFD0xNnOGHAXc1Z8N8+nnExSGam0MLPnHywMYm5c64K9NQJ3Bplch8c3hvfp5+DnOuFLZcaEDi7tBVY0+IX3AfMSpn62FIfJKAUkPiCIgJUFvlLnFFx/84Msyi8NlsfgQzTof7BCLSf8iGl1TiAgXh8AXy0OGVgOKc0F4iWnvxPPE5kX2egQY0ysmNMzTkahEypAfH4DSwYfpI4wQww2iFA0jhjdMQUaItfHuU2Y0EkSmdyu2YKFSPn93lDN83A==" },
                { "VK_AUTO", "N" },
            });

            Assert.That(paymentResponse.BankId, Is.EqualTo("GENIPIZZA"));
            Assert.That(paymentResponse.Amount, Is.Null);
            Assert.That(paymentResponse.Currency, Is.Empty);

            Assert.That(paymentResponse.IsAutomaticResponse, Is.False);
            Assert.That(paymentResponse.IsPaymentSuccessful, Is.False);

            Assert.That(paymentResponse.PaymentMessage, Is.EqualTo("Shut up and take my $$$!"));
            Assert.That(paymentResponse.PaymentOrderNumber, Is.Empty);
            Assert.That(paymentResponse.PaymentOrderReferenceNumber, Is.Empty);

            Assert.That(paymentResponse.PaymentReceiverAccount, Is.Empty);
            Assert.That(paymentResponse.PaymentReceiverName, Is.Empty);
            Assert.That(paymentResponse.PaymentSenderAccount, Is.Empty);

            Assert.That(paymentResponse.PaymentSenderName, Is.Empty);
            Assert.That(paymentResponse.RequestEncoding, Is.EqualTo("UTF-8"));
            Assert.That(paymentResponse.RequestStartDateTime, Is.Null);

            Assert.That(paymentResponse.Language, Is.EqualTo("EST"));
            Assert.That(paymentResponse.Stamp, Is.EqualTo("541260"));
        }

        [Test]
        public void CreateIPizzaAuthRequest_Verify_4011_Request_Parameters()
        {
            BankRequest bankRequest = _banklinkApi.CreateIPizzaAuthRequest(new IPizzaAuthRequestParams
            {
                Language = "EST",
                RequestStartDateTime = DateTime.ParseExact("2015-04-04T22:23:30+0300", "yyyy-MM-ddTHH:mm:sszz00", CultureInfo.InvariantCulture),
                RequestEncoding = "utf-8",
                BankId = "GENIPIZZA",
                ExpectedReturnCode = "3012",
                RequestId = "1428175410690",
                ReturnUrl = "http://localhost:8080/project/6rGPnXJ7cvstStKx?auth_action=success"
            });

            Assert.AreEqual("http://localhost:8080/banklink/ipizzaauth", bankRequest.RequestUrl);

            bankRequest.AssertFieldValueEqualTo("VK_SERVICE", "4011");
            bankRequest.AssertFieldValueEqualTo("VK_VERSION", "008");
            bankRequest.AssertFieldValueEqualTo("VK_SND_ID", "uid100010");
            bankRequest.AssertFieldValueEqualTo("VK_REPLY", "3012");
            bankRequest.AssertFieldValueEqualTo("VK_RETURN", "http://localhost:8080/project/6rGPnXJ7cvstStKx?auth_action=success");
            bankRequest.AssertFieldValueEqualTo("VK_DATETIME", "2015-04-04T22:23:30+0300");
            bankRequest.AssertFieldValueEqualTo("VK_RID", "1428175410690");
            bankRequest.AssertFieldValueEqualTo("VK_MAC", "SglHiOfR/FWRSnImNGBqmt139gmKxWeQQoSd3NoLJ5oSqSykT704oTZgZteJn8tDRSwhQEO5d7x1CX3wCn2Huzau6wZx/7k1nqH4s11TF9X6tw5DNHpgCId4li9RGH/v7j2cUBz5835ZZZ6gPNFZ6NCQdCeYD1Qr15Mq4wduotsAKznD/TWna5geQgfSbvPKrRT2yKCFdO+k9WujHD/OqqIQ5HNJy+q2k/8+5zb+stHr84FnoR/V2+prKhCM+yyJR3W85t7RiUQa+DgooZ2lITJj1SeNVxxilNcclkuwe7Jm/+CXrb2IEF1bXYkTO4W4dPDq4u7ofhJjXvc7c2EThQ==");
            bankRequest.AssertFieldValueEqualTo("VK_ENCODING", "utf-8");
            bankRequest.AssertFieldValueEqualTo("VK_LANG", "EST");
        }

        [Test]
        public void CreateIPizzaAuthRequest_Verify_4012_Request_Parameters()
        {
            BankRequest bankRequest = _banklinkApi.CreateIPizzaAuthRequest(new IPizzaAuthRequestParams
            {
                Language = "EST",
                RequestStartDateTime = DateTime.ParseExact("2015-04-04T22:23:30+0300", "yyyy-MM-ddTHH:mm:sszz00", CultureInfo.InvariantCulture),
                RequestEncoding = "utf-8",
                BankId = "GENIPIZZA",
                RequestId = "1428175410690",
                ReturnUrl = "http://localhost:30535/Home/AcceptAuth",
                Nonce = "9a3d6bd2-36d4-49b0-ae44-680c0281f39f"
            });

            Assert.That(bankRequest.RequestUrl, Is.EqualTo("http://localhost:8080/banklink/ipizzaauth"));

            bankRequest.AssertFieldValueEqualTo("VK_SERVICE", "4012");
            bankRequest.AssertFieldValueEqualTo("VK_VERSION", "008");
            bankRequest.AssertFieldValueEqualTo("VK_SND_ID", "uid100010");
            bankRequest.AssertFieldValueEqualTo("VK_REC_ID", "GENIPIZZA");
            bankRequest.AssertFieldValueEqualTo("VK_NONCE", "9a3d6bd2-36d4-49b0-ae44-680c0281f39f");
            bankRequest.AssertFieldValueEqualTo("VK_RETURN", "http://localhost:30535/Home/AcceptAuth");
            bankRequest.AssertFieldValueEqualTo("VK_DATETIME", "2015-04-04T22:23:30+0300");
            bankRequest.AssertFieldValueEqualTo("VK_RID", "1428175410690");
            bankRequest.AssertFieldValueEqualTo("VK_MAC", "H0r2Xoa0vzvylLxsWAphevA15rZpQ2buzzZlf/VFexaYKmXzZHWZnHCa7Pwf3VZ8ZoScFwVzlQAW5LJLlElPsliWaiovKZrVs4WCXTkP7Cd4jqRfDh4YwXCL+fISn/D9xTi/J3v17vKek7BbTVicRUGqf0ynh+DjgAVezaoPbfZ53ppaLESoqK3RF7O+HqphOxDwM8vO8g8+QiTs7dxJzru7SJlPCmLO1idc9TCoY+e80+fHCo2+zzz2cZu2iM48C6Ahk0tZ06Sw83f/UOhOxNJL6Ar0K9FgNCGBSN+NVQ0SPLg+0ubZ2DA16QAUjYdwYQSYhA+KF8B2ElN74eYvDw==");
            bankRequest.AssertFieldValueEqualTo("VK_ENCODING", "utf-8");
            bankRequest.AssertFieldValueEqualTo("VK_LANG", "EST");
        }

        [Test]
        public void ParseIPizzaAuthResponse_Verify_3012_Response_Parameters()
        {
            IPizzaAuthResponse authResponse = _banklinkApi.ParseIPizzaAuthResponse(new NameValueCollection
            {
                { "VK_SERVICE", "3012" },
                { "VK_VERSION", "008" },
                { "VK_DATETIME", "2015-04-05T08:30:31+0300" },

                { "VK_SND_ID", "GENIPIZZA" },
                { "VK_REC_ID", "uid100010" },
                { "VK_USER_NAME", "Tõõger Leõpäöld" },

                { "VK_USER_ID", "37602294565" },
                { "VK_COUNTRY", "EE" },
                { "VK_OTHER", "" },

                { "VK_TOKEN", "5" },
                { "VK_RID", "1428211826915" },

                { "VK_MAC", "ERyUeE71UFwdEUqbKzOeByxDhHDFLPw5EDh7nXBHsMpA/JlRS/DBeviBXBYx7w72Zu7VYoc8yC4fgYJ935OJrAnBVnBwUIVg2hArrDE+/7piyon6ivZ9hNaSuWwgen1cyWZ8ObvtDcTnoRuBK/HEQjugjetzZO7Gdkdj/D/0fA+KQ8rZxojYQsXQ/HPbv9DVPNK9NPgG/Ir3UAdChkr5HYXREc4S+J0FQZafuJqJGvSDfOoe8Bc5uXl2VP12h30SCQeHuqLj//q19I1lXqIPHOxakjl00uJR8UBM41g/ZhH/2Lkbzm63KXhmavn862WoAEDpE4WaaHPpp07kIHyD+g==" },
                { "VK_ENCODING", "utf-8" },
                { "VK_LANG", "EST" },
            });


            Assert.That(authResponse.BankId, Is.EqualTo("GENIPIZZA"));
            Assert.That(authResponse.Country, Is.EqualTo("EE"));
            Assert.That(authResponse.IdCode, Is.EqualTo("37602294565"));
            Assert.That(authResponse.Language, Is.EqualTo("EST"));
            Assert.That(authResponse.Nonce, Is.Empty);
            Assert.That(authResponse.Other, Is.Empty);
            Assert.That(authResponse.RequestDateTime, 
                Is.EqualTo(DateTime.ParseExact("2015-04-05T08:30:31+0300", "yyyy-MM-ddTHH:mm:sszz00", CultureInfo.InvariantCulture)));
            Assert.That(authResponse.RequestEncoding, Is.EqualTo("utf-8"));
            Assert.That(authResponse.RequestId, Is.EqualTo("1428211826915"));
            Assert.That(authResponse.Token, Is.EqualTo("5"));
            Assert.That(authResponse.UserName, Is.EqualTo("Tõõger Leõpäöld"));
            Assert.That(authResponse.User, Is.Empty);
        }

        [Test]
        public void ParseIPizzaAuthResponse_Verify_3013_Response_Parameters()
        {
            IPizzaAuthResponse authResponse = _banklinkApi.ParseIPizzaAuthResponse(new NameValueCollection
            {
                { "VK_SERVICE", "3013" },
                { "VK_VERSION", "008" },
                { "VK_DATETIME", "2015-04-05T04:25:27+0300" },

                { "VK_SND_ID", "GENIPIZZA" },
                { "VK_REC_ID", "uid100010" },
                { "VK_NONCE", "9a3d6bd2-36d4-49b0-ae44-680c0281f39f" },

                { "VK_USER_NAME", "Tõõger Leõpäöld" },
                { "VK_USER_ID", "37602294565" },
                { "VK_COUNTRY", "EE" },

                { "VK_OTHER", "" },
                { "VK_TOKEN", "5" },
                { "VK_RID", "1428175410690" },

                { "VK_MAC", "Bj2GyCfHQPGygk4iiAyr5e7UvoV2I27p7EcxH6XbsSGob3TeGNjoQG0lCUh0I3hiFxw5h5Z4uV7d9d+YGfTZqiord1VSxI0F1gDJFgNg6DWrwtoMlQNtEWHjsXz5dzg/DwHIYVLgbM3ttOopyiOsp6xUS7Jx95V6ga5/T0KFhxBGHIXLfM4mWiC38MY/nW3W8vztdfjb1WTWWTG9n25wIrM2utznNAeUgqikaFi4Brvf3xVdoV0WuRuBJEBx82jZrih36VENF3njQqGxWzbn9lziDsAJK3cAM02o3g+sMfErD6ccUX+pTjehBOO1hDSx4QOkrG28ra96otDyAobhpA==" },
                { "VK_ENCODING", "utf-8" },
                { "VK_LANG", "EST" },
            });

            Assert.That(authResponse.BankId, Is.EqualTo("GENIPIZZA"));
            Assert.That(authResponse.Country, Is.EqualTo("EE"));
            Assert.That(authResponse.IdCode, Is.EqualTo("37602294565"));
            Assert.That(authResponse.Language, Is.EqualTo("EST"));
            Assert.That(authResponse.Nonce, Is.EqualTo("9a3d6bd2-36d4-49b0-ae44-680c0281f39f"));
            Assert.That(authResponse.Other, Is.Empty);
            Assert.That(authResponse.RequestDateTime,
                Is.EqualTo(DateTime.ParseExact("2015-04-05T04:25:27+0300", "yyyy-MM-ddTHH:mm:sszz00", CultureInfo.InvariantCulture)));
            Assert.That(authResponse.RequestEncoding, Is.EqualTo("utf-8"));
            Assert.That(authResponse.RequestId, Is.EqualTo("1428175410690"));
            Assert.That(authResponse.Token, Is.EqualTo("5"));
            Assert.That(authResponse.UserName, Is.EqualTo("Tõõger Leõpäöld"));
            Assert.That(authResponse.User, Is.Empty);
        }

        [Test]
        public void ParseIPizzaAuthResponse_Verifies_Mac_With_Utf8_Encoding_When_Bank_Does_Not_Return_VK_ENCODING_Parameter()
        {
            IPizzaAuthResponse authResponse = _banklinkApi.ParseIPizzaAuthResponse(new NameValueCollection
            {
                { "VK_SERVICE", "3013" },
                { "VK_VERSION", "008" },
                { "VK_DATETIME", "2015-04-05T04:25:27+0300" },

                { "VK_SND_ID", "GENIPIZZA" },
                { "VK_REC_ID", "uid100010" },
                { "VK_NONCE", "9a3d6bd2-36d4-49b0-ae44-680c0281f39f" },

                { "VK_USER_NAME", "Tõõger Leõpäöld" },
                { "VK_USER_ID", "37602294565" },
                { "VK_COUNTRY", "EE" },

                { "VK_OTHER", "" },
                { "VK_TOKEN", "5" },
                { "VK_RID", "1428175410690" },

                { "VK_MAC", "Bj2GyCfHQPGygk4iiAyr5e7UvoV2I27p7EcxH6XbsSGob3TeGNjoQG0lCUh0I3hiFxw5h5Z4uV7d9d+YGfTZqiord1VSxI0F1gDJFgNg6DWrwtoMlQNtEWHjsXz5dzg/DwHIYVLgbM3ttOopyiOsp6xUS7Jx95V6ga5/T0KFhxBGHIXLfM4mWiC38MY/nW3W8vztdfjb1WTWWTG9n25wIrM2utznNAeUgqikaFi4Brvf3xVdoV0WuRuBJEBx82jZrih36VENF3njQqGxWzbn9lziDsAJK3cAM02o3g+sMfErD6ccUX+pTjehBOO1hDSx4QOkrG28ra96otDyAobhpA==" },
                // { "VK_ENCODING", "" },
                { "VK_LANG", "EST" },
            });

            Assert.That(authResponse.BankId, Is.EqualTo("GENIPIZZA"));
            Assert.That(authResponse.Country, Is.EqualTo("EE"));
            Assert.That(authResponse.IdCode, Is.EqualTo("37602294565"));
            Assert.That(authResponse.Language, Is.EqualTo("EST"));
            Assert.That(authResponse.Nonce, Is.EqualTo("9a3d6bd2-36d4-49b0-ae44-680c0281f39f"));
            Assert.That(authResponse.Other, Is.Empty);
            Assert.That(authResponse.RequestDateTime,
                Is.EqualTo(DateTime.ParseExact("2015-04-05T04:25:27+0300", "yyyy-MM-ddTHH:mm:sszz00", CultureInfo.InvariantCulture)));

            // Bank didn't send encoding back
            Assert.That(authResponse.RequestEncoding, Is.Empty);
            Assert.That(authResponse.RequestId, Is.EqualTo("1428175410690"));
            Assert.That(authResponse.Token, Is.EqualTo("5"));
            Assert.That(authResponse.UserName, Is.EqualTo("Tõõger Leõpäöld"));
            Assert.That(authResponse.User, Is.Empty);
        }
    }
}
