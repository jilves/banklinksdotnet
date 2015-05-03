using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.Exceptions
{
    public class FieldLengthOutOfRangeException : BanklinksDotNetException
    {
        public BankMessageField InvalidField { get; private set; }
        public IBankMessage BankMessage { get; private set; }

        public FieldLengthOutOfRangeException(BankMessageField invalidField, IBankMessage bankMessage)
        {
            InvalidField = invalidField;
            BankMessage = bankMessage;
        }

        public override string Message
        {
            get
            {
                return string.Format("'{0}' field's value '{1}' exceeds the maximum allowed length {2}.",
                    InvalidField.FieldName,
                    InvalidField.Value,
                    InvalidField.MaxLength);
            }
        }
    }
}
