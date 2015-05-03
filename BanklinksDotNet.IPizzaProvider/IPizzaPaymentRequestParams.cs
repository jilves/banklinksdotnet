using System;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.IPizzaProvider
{
    // ReSharper disable once InconsistentNaming
    public class IPizzaPaymentRequestParams : AbstractRequestParams
    {
        /// <summary>
        /// Required (VK_STAMP).
        /// </summary>
        public string Stamp { get; set; }
        /// <summary>
        /// Required (VK_AMOUNT).
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Optional (VK_REF).
        /// </summary>
        public string PaymentReferenceNumber { get; set; }
        /// <summary>
        /// Required (VK_MSG).
        /// </summary>
        public string PaymentMessage { get; set; }
        /// <summary>
        /// Required (VK_RETURN).
        /// </summary>
        public string SuccessReturnUrl { get; set; }
        /// <summary>
        /// Required (VK_CANCEL).
        /// </summary>
        public string ErrorReturnUrl { get; set; }
        /// <summary>
        /// Optional (VK_ACC).
        /// </summary>
        public string RecipientAccountNumber { get; set; }
        /// <summary>
        /// Optional (VK_NAME).
        /// </summary>
        public string RecipientName { get; set; }
        /// <summary>
        /// Optional (VK_LANG). Defaults to EST (Estonian).
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// Optional (VK_CURR). Defaults to "EUR".
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// Optional (VK_DATETIME). Will be set automatically with local computer timezone, if left empty.
        /// </summary>
        public DateTime RequestStartDateTime { get; set; }
        /// <summary>
        /// Optional (VK_ENCODING). Defaults to UTF-8.
        /// </summary>
        public string RequestEncoding { get; set; }

        public IPizzaPaymentRequestParams()
        {
            Currency = "EUR";
            Language = "EST";
            RequestEncoding = "UTF-8";
            RequestStartDateTime = DateTime.Now;
        }
    }
}
