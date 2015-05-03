using System;
using System.Collections.Generic;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.EstcardProvider
{
    public class EstcardPaymentResponse : AbstractBankResponse
    {
        public EstcardPaymentResponse(IEnumerable<BankMessageField> postParameters) : base(postParameters)
        {
        }

        /// <summary>
        /// ecuno
        /// </summary>
        public long TransactionNr { get; internal set; }
        /// <summary>
        /// receipt_no
        /// </summary>
        public int ReceiptNr { get; internal set; }
        /// <summary>
        /// eamount
        /// </summary>
        public long AmountInCents { get; internal set; }
        /// <summary>
        /// cur
        /// </summary>
        public string Currency { get; internal set; }
        /// <summary>
        /// respcode '000' == OK
        /// </summary>
        public bool IsPaymentSuccessful { get; internal set; }
        /// <summary>
        /// datetime
        /// </summary>
        public DateTime TransactionDateTime { get; internal set; }
        /// <summary>
        /// msgdata
        /// </summary>
        public string MsgData { get; internal set; }
        public string ActionText { get; internal set; }
        public string RespCode { get; internal set; }
    }
}