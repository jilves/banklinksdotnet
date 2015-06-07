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
            return Serializer.ObjectToJson(this);
        }
    }
}
