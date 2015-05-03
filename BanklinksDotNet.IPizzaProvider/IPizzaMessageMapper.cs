using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using BanklinksDotNet.Exceptions;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.IPizzaProvider
{
    // ReSharper disable once InconsistentNaming
    public class IPizzaMessageMapper
    {
        private readonly MacCalculatorFactory _macCalculatorFactory;
        private readonly BasicMessageFieldFinder _basicMessageFieldFinder;

        public IPizzaMessageMapper(BasicMessageFieldFinder basicMessageFieldFinder, MacCalculatorFactory macCalculatorFactory)
        {
            _macCalculatorFactory = macCalculatorFactory;
            _basicMessageFieldFinder = basicMessageFieldFinder;
        }

        public void SetRequestMac(string encoding, IPizzaConfiguration bankConfiguration, List<BankMessageField> bankMessageFields)
        {
            string macVersion = _basicMessageFieldFinder.FindOrDefault("VK_VERSION", bankMessageFields);
            string mac = _macCalculatorFactory
                .CreateCalculator(macVersion, bankConfiguration)
                .CreateMac(encoding, bankMessageFields);

            bankMessageFields.First(messageField => messageField.FieldName == "VK_MAC").Value = mac;
        }

        private class BankResponseWrapper : IVisitable
        {
            public BankResponseWrapper(NameValueCollection responseParameters)
            {
                
            }

            public BankResponseWrapper(HttpRequest response)
            {
                
            }

            public void Accept(IVisitor visitor)
            {
                
            }
        }

        public void SetPaymentResponseProperties(VisitableNameValueCollection responseParameters,
            IPizzaConfiguration bankConfiguration,
            IPizzaPaymentResponse bankPaymentResponse)
        {
            List<BankMessageField> messageFields = bankPaymentResponse.PostParameters.ToList();

            _basicMessageFieldFinder.MapResponseParamsToMessageFields(messageFields, responseParameters);

            VerifyResponseMac(bankConfiguration, messageFields);
            bankPaymentResponse.IsAutomaticResponse = _basicMessageFieldFinder.FindOrDefault("VK_AUTO", messageFields) == "Y";

            bankPaymentResponse.Amount = _basicMessageFieldFinder.FindOrDefaultDecimal("VK_AMOUNT", bankConfiguration.DecimalFormat, messageFields);
            bankPaymentResponse.RequestStartDateTime = _basicMessageFieldFinder.FindOrDefaultDateTime("VK_T_DATETIME", bankConfiguration.DateTimeFormat, messageFields);

            string serviceCode = _basicMessageFieldFinder.FindOrDefault("VK_SERVICE", messageFields);
            bankPaymentResponse.IsPaymentSuccessful = serviceCode == "1111";

            bankPaymentResponse.Stamp = _basicMessageFieldFinder.FindOrDefault("VK_STAMP", messageFields);
            bankPaymentResponse.Currency = _basicMessageFieldFinder.FindOrDefault("VK_CURR", messageFields);
            bankPaymentResponse.Language = _basicMessageFieldFinder.FindOrDefault("VK_LANG", messageFields);
            bankPaymentResponse.PaymentMessage = _basicMessageFieldFinder.FindOrDefault("VK_MSG", messageFields);
            bankPaymentResponse.PaymentOrderNumber = _basicMessageFieldFinder.FindOrDefault("VK_T_NO", messageFields);
            bankPaymentResponse.PaymentOrderReferenceNumber = _basicMessageFieldFinder.FindOrDefault("VK_REF", messageFields);
            bankPaymentResponse.PaymentReceiverAccount = _basicMessageFieldFinder.FindOrDefault("VK_REC_ACC", messageFields);
            bankPaymentResponse.PaymentReceiverName = _basicMessageFieldFinder.FindOrDefault("VK_REC_NAME", messageFields);
            bankPaymentResponse.PaymentSenderAccount = _basicMessageFieldFinder.FindOrDefault("VK_SND_ACC", messageFields);
            bankPaymentResponse.PaymentSenderName = _basicMessageFieldFinder.FindOrDefault("VK_SND_NAME", messageFields);
            bankPaymentResponse.RequestEncoding = _basicMessageFieldFinder.FindOrDefault("VK_ENCODING", messageFields);
        }

        public void SetAuthResponseProperties(VisitableNameValueCollection responseParameters, IPizzaConfiguration bankConfiguration, IPizzaAuthResponse bankAuthResponse)
        {
            List<BankMessageField> messageFields = bankAuthResponse.PostParameters.ToList();

            _basicMessageFieldFinder.MapResponseParamsToMessageFields(messageFields, responseParameters);

            VerifyResponseMac(bankConfiguration, messageFields);

            bankAuthResponse.IdCode = _basicMessageFieldFinder.FindOrDefault("VK_USER_ID", messageFields);
            bankAuthResponse.User = _basicMessageFieldFinder.FindOrDefault("VK_USER", messageFields);
            bankAuthResponse.Nonce = _basicMessageFieldFinder.FindOrDefault("VK_NONCE", messageFields);
            bankAuthResponse.UserName = _basicMessageFieldFinder.FindOrDefault("VK_USER_NAME", messageFields);
            bankAuthResponse.Country = _basicMessageFieldFinder.FindOrDefault("VK_COUNTRY", messageFields);
            bankAuthResponse.Language = _basicMessageFieldFinder.FindOrDefault("VK_LANG", messageFields);
            bankAuthResponse.Other = _basicMessageFieldFinder.FindOrDefault("VK_OTHER", messageFields);
            // ReSharper disable once PossibleInvalidOperationException
            // Request datetime always exists for auth responses 
            bankAuthResponse.RequestDateTime = _basicMessageFieldFinder.FindOrDefaultDateTime("VK_DATETIME", bankConfiguration.DateTimeFormat, messageFields).Value;
            bankAuthResponse.RequestEncoding = _basicMessageFieldFinder.FindOrDefault("VK_ENCODING", messageFields);
            bankAuthResponse.RequestId = _basicMessageFieldFinder.FindOrDefault("VK_RID", messageFields);
            bankAuthResponse.Token = _basicMessageFieldFinder.FindOrDefault("VK_TOKEN", messageFields);
        }

        private void VerifyResponseMac(IPizzaConfiguration bankConfiguration, List<BankMessageField> bankMessageFields)
        {
            string macVersion = _basicMessageFieldFinder.FindOrDefault("VK_VERSION", bankMessageFields);
            string mac = _basicMessageFieldFinder.FindOrDefault("VK_MAC", bankMessageFields);
            string encoding = _basicMessageFieldFinder.FindOrDefault("VK_ENCODING", bankMessageFields);
            if (string.IsNullOrEmpty(encoding))
            {
                encoding = "UTF-8";
            }

            bool isMacValid = _macCalculatorFactory
                .CreateCalculator(macVersion, bankConfiguration)
                .VerifyMac(encoding, mac, bankMessageFields);

            if (!isMacValid)
            {
                throw new InvalidMacException();
            }
        }
    }
}
