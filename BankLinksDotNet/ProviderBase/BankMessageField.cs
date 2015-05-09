using System.Web.Script.Serialization;

namespace BanklinksDotNet.ProviderBase
{
    public class BankMessageField
    {
        public int? OrderNr { get; set; }
        public string FieldName { get; set; }
        public int MaxLength { get; set; }
        public string Value { get; set; }
        public virtual bool IsMacData { get { return OrderNr.HasValue; } }

        public override string ToString()
        {
            // TODO: Give all BanklinksDotNet dto objects json serialized ToString()-s?
            return new JavaScriptSerializer().Serialize(this);
        }
    }
}
