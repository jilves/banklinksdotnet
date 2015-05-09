using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BanklinksDotNet.Exceptions;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.EstcardProvider
{
    public class EstcardMessageMapper
    {
        private readonly BasicMessageFieldFinder _basicMessageFieldFinder;
        private readonly MacCalculatorFactory _macCalculatorFactory;

        public EstcardMessageMapper(BasicMessageFieldFinder basicMessageFieldFinder, MacCalculatorFactory macCalculatorFactory)
        {
            _basicMessageFieldFinder = basicMessageFieldFinder;
            _macCalculatorFactory = macCalculatorFactory;
        }

        public void SetRequestMac(string encoding, EstcardConfiguration bankConfiguration, List<BankMessageField> bankMessageFields)
        {
            string macVersion = _basicMessageFieldFinder.FindOrDefault("ver", bankMessageFields);
            string mac = _macCalculatorFactory
                .CreateCalculator(macVersion, bankConfiguration)
                .CreateMac(encoding, bankMessageFields);

            bankMessageFields.First(messageField => messageField.FieldName == "mac").Value = mac;
        }

        public void SetPaymentResponseProperties(VisitableNameValueCollection responseParameters,
            EstcardConfiguration bankConfiguration,
            EstcardPaymentResponse bankPaymentResponse)
        {
            List<BankMessageField> postParameters = bankPaymentResponse.PostParameters.ToList();
            _basicMessageFieldFinder.MapResponseParamsToMessageFields(responseParameters, postParameters);

            VerifyResponseMac(bankConfiguration, postParameters);

            bankPaymentResponse.AmountInCents = _basicMessageFieldFinder.FindOrDefaultLong("eamount", postParameters).Value;
            bankPaymentResponse.Currency = _basicMessageFieldFinder.FindOrDefault("cur", postParameters);
            bankPaymentResponse.MsgData = _basicMessageFieldFinder.FindOrDefault("msgdata", postParameters);
            bankPaymentResponse.ActionText = _basicMessageFieldFinder.FindOrDefault("actiontext", postParameters);
            bankPaymentResponse.ReceiptNr = _basicMessageFieldFinder.FindOrDefaultInt("receipt_no", postParameters).Value;

            // TODO: configurable datetime format
            string transactionDateTimeString = _basicMessageFieldFinder.FindOrDefault("datetime", postParameters);
            bankPaymentResponse.TransactionDateTime = DateTime.ParseExact(transactionDateTimeString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            bankPaymentResponse.TransactionNr = _basicMessageFieldFinder.FindOrDefaultLong("ecuno", postParameters).Value;
            string respCode = _basicMessageFieldFinder.FindOrDefault("respcode", postParameters);
            bankPaymentResponse.RespCode = respCode;
            bankPaymentResponse.IsPaymentSuccessful = respCode == "000";
        }

        private void VerifyResponseMac(EstcardConfiguration bankConfiguration, List<BankMessageField> bankMessageFields)
        {
            string macVersion = _basicMessageFieldFinder.FindOrDefault("ver", bankMessageFields);

            // TODO: Estcard documentation does not mention sending back the charEncoding parameter,
            // yet their test service does. 
            // Needs clarification if it's safe to expect that parameter in the response.
            // http://www.estcard.ee/publicweb/files/ecomdevel/e-comDocEST.html

            string encoding = _basicMessageFieldFinder.FindOrDefault("charEncoding", bankMessageFields);
            string mac = _basicMessageFieldFinder.FindOrDefault("mac", bankMessageFields);

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
