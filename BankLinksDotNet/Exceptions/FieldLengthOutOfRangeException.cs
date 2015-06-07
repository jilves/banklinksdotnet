using System.Linq;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet.Exceptions
{
    /// <summary>
    /// Thrown when one of the parameters sent to or received from a bank exceeds the allowed max length.
    /// </summary>
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
                return string.Format("'{0}' field's value '{1}' exceeds the maximum allowed length {2}. Post parameters: {3}",
                    InvalidField.FieldName,
                    InvalidField.Value,
                    InvalidField.MaxLength,
                    Serializer.ObjectToJson(BankMessage.PostParameters.ToArray()));
            }
        }
    }
}
