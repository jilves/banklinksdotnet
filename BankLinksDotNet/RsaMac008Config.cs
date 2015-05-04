using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public class RsaMac008Config : IRsaMacCalculatorConfig
    {
        public string BytesToString(byte[] binaryData)
        {
            return Convert.ToBase64String(binaryData);
        }

        public byte[] StringToBytes(string stringData)
        {
            return Convert.FromBase64String(stringData);
        }

        public string CreateMacInput(IEnumerable<BankMessageField> bankMessage)
        {
            string[] macDataCollection = bankMessage
                .Where(bankMessageField => bankMessageField.IsMacData)
                .OrderBy(bankMessageField => bankMessageField.OrderNr)
                .Select(bankMessageField => bankMessageField.Value ?? string.Empty)
                .ToArray();

            return string.Join(string.Empty, macDataCollection
                .Select(macData => macData.Length.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0') + macData)
                .ToArray());
        }
    }
}
