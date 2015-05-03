using System;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.EstcardProvider
{
    public class EstcardPaymentRequestParams : AbstractRequestParams
    {
        /// <summary>
        /// Required (ecuno). Format: YYYYMM + Random integer in range of 100000-999999.
        /// </summary>
        public long TransactionNr { get; set; }
        /// <summary>
        /// Required (feedBackUrl).
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// Required (eamount).
        /// </summary>
        public long AmountInCents { get; set; }
        /// <summary>
        /// Optional (ver). Defaults to 004.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Optional (cur). Defaults to "EUR". Accepts currencies in ISO-4217 format.
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// Optional (datetime). Defaults to current time in local timezone.
        /// </summary>
        public DateTime TransactionDateTime { get; set; }
        /// <summary>
        /// Optional (delivery). Defaults to "S".
        /// </summary>
        public string Delivery { get; set; }
        /// <summary>
        /// Optional (lang). Accepts language codes in ISO 639-1 format. Defaults to "et".
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// Optional (action). Defaults to "gaf".
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// Optional (charEncoding). Defaults to "UTF-8".
        /// </summary>
        public string RequestEncoding { get; set; }

        public EstcardPaymentRequestParams()
        {
            Currency = "EUR";
            TransactionDateTime = DateTime.Now;
            Version = "004";
            Delivery = "S";
            ActionName = "gaf";
            RequestEncoding = "UTF-8";
        }
    }
}