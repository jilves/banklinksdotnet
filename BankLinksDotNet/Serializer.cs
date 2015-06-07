using System.Web.Script.Serialization;

namespace BanklinksDotNet
{
    public static class Serializer
    {
        private static readonly JavaScriptSerializer JsSerializer = new JavaScriptSerializer();

        public static string ObjectToJson(object obj)
        {
            return JsSerializer.Serialize(obj);
        }
    }
}
