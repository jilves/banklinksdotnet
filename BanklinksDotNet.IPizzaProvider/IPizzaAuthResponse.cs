using System;
using System.Collections.Generic;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.IPizzaProvider
{
    // ReSharper disable once InconsistentNaming
    public class IPizzaAuthResponse : AbstractBankResponse
    {
        public IPizzaAuthResponse(string bankId, IEnumerable<BankMessageField> postParameters)
            : base(postParameters)
        {
            BankId = bankId;
        }

        public DateTime RequestDateTime { get; internal set; }

        // TODO: Must have test coverage
        /// <summary>
        /// Returns true, if RequestDateTime falls into +- 5 minute range
        /// </summary>
        public bool IsRequestDateTimeValid
        {
            get
            {
                DateTime now = DateTime.Now.ToUniversalTime();
                DateTime requestDateTime = RequestDateTime.ToUniversalTime();

                TimeSpan difference = now - requestDateTime;

                return difference.Minutes < 5;
            }
        }

        public string BankId { get; internal set; }
        public string RequestId { get; internal set; }
        public string Nonce { get; internal set; }
        public string UserName { get; internal set; }
        public string IdCode { get; internal set; }
        public string Country { get; internal set; }
        public string Other { get; internal set; }
        public string Token { get; internal set; }
        public string RequestEncoding { get; internal set; }
        public string Language { get; internal set; }
        public string User { get; internal set; }
    }
}
