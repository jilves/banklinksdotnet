using System;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.IPizzaProvider
{
    // ReSharper disable once InconsistentNaming
    public class IPizzaAuthRequestParams : AbstractRequestParams
    {
        /// <summary>
        /// Optional (VK_LANG). Defaults to EST (Estonian).
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// Optional (VK_DATETIME). Will be set automatically with local computer timezone, if left empty.
        /// </summary>
        public DateTime RequestStartDateTime { get; set; }
        /// <summary>
        /// Optional (VK_ENCODING). Defaults to UTF-8.
        /// </summary>
        public string RequestEncoding { get; set; }
        /// <summary>
        /// Nonce is used to ensure that old communications cannot be reused in replay attacks.
        /// </summary>
        public string Nonce { get; set; }
        public string ReturnUrl { get; set; }
        /// <summary>
        /// Session/Request id
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// 3012 for 4011 request, empty for 4012 request
        /// </summary>
        public string ExpectedReturnCode { get; set; }

        public IPizzaAuthRequestParams()
        {
            Language = "EST";
            RequestEncoding = "UTF-8";
            RequestStartDateTime = DateTime.Now;
        }
    }
}
