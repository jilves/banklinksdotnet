using System.Linq;
using BanklinksDotNet.Exceptions;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public class Validator
    {
        public virtual void ValidateFieldLengths(IBankMessage result)
        {
            foreach (BankMessageField bankMessageField in result.PostParameters.Where(field => field.Value != null))
            {
                if (bankMessageField.Value.Length > bankMessageField.MaxLength)
                {
                    throw new FieldLengthOutOfRangeException(bankMessageField, result);
                }
            }
        }
    }
}
