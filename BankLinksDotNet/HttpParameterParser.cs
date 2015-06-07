using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;

namespace BanklinksDotNet
{
    public class HttpParameterParser
    {
        private static byte[] GetRequestBytes(HttpRequest request)
        {
            request.InputStream.Position = 0;
            var bytes = request.BinaryRead((int)request.InputStream.Length);
            request.InputStream.Position = 0; 

            return bytes;
        }

        public virtual NameValueCollection GetRequestParameters(HttpRequest request, string encodingString)
        {
            Encoding encoding = Encoding.GetEncoding(encodingString);

            string queryString = request.HttpMethod == WebRequestMethods.Http.Post
                ? encoding.GetString(GetRequestBytes(request))
                : new Uri(request.Url, request.RawUrl).Query;

            return HttpUtility.ParseQueryString(queryString, encoding);
        }
    }
}
