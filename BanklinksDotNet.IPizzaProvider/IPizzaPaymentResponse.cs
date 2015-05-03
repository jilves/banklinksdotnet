using System;
using BanklinksDotNet.ProviderBase;
using System.Collections.Generic;

namespace BanklinksDotNet.IPizzaProvider
{
    // ReSharper disable once InconsistentNaming
    public class IPizzaPaymentResponse : AbstractBankResponse
    {
        public IPizzaPaymentResponse(string bankId, IEnumerable<BankMessageField> postParameters)
            : base(postParameters)
        {
            BankId = bankId;
        }

        
        public string BankId { get; internal set; }
        public bool IsPaymentSuccessful { get; internal set; }
        public string Stamp { get; internal set; }
        public decimal? Amount { get; internal set; }
        public string Currency { get; internal set; }
        public bool IsAutomaticResponse { get; internal set; }
        public string Language { get; internal set; }
        public string PaymentMessage { get; internal set; }
        /// <summary>
        /// "VK_T_NO"
        /// </summary>
        public string PaymentOrderNumber { get; internal set; }

        public string PaymentOrderReferenceNumber { get; internal set; }
        public string PaymentReceiverAccount { get; internal set; }
        public string PaymentReceiverName { get; internal set; }
        public string PaymentSenderAccount { get; internal set; }
        public string PaymentSenderName { get; internal set; }
        public string RequestEncoding { get; internal set; }
        public DateTime? RequestStartDateTime { get; internal set; }
    }
}
