using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public class RsaMac004Config : IRsaMacCalculatorConfig
    {
        public string BytesToString(byte[] binaryData)
        {
            return string.Join(string.Empty, binaryData.Select(b => b.ToString("X2")));
        }

        public byte[] StringToBytes(string stringData)
        {
            var signedBytes = new byte[stringData.Length / 2];
            for (int i = 0; i < signedBytes.Length; i++)
            {
                signedBytes[i] = byte.Parse(stringData.Substring(i * 2, 2), NumberStyles.HexNumber);
            }

            return signedBytes;
        }

        public string CreateMacInput(IEnumerable<BankMessageField> bankMessage)
        {
            string[] macDataCollection = bankMessage.Where(field => field.IsMacData)
                .OrderBy(field => field.OrderNr)
                .Select(field =>
                {
                    int maxLen = field.MaxLength;
                    string fieldValue = field.Value ?? string.Empty;

                    return IsInt(fieldValue)
                        ? fieldValue.PadLeft(maxLen, '0')
                        : fieldValue.PadRight(maxLen, ' ');
                }).ToArray();

            return string.Join(string.Empty, macDataCollection);
        }

        private bool IsInt(string value)
        {
            int intValue;
            return int.TryParse(value, out intValue);
        }
    }
}
