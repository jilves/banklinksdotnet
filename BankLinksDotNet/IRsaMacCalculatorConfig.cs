using System.Collections.Generic;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public interface IRsaMacCalculatorConfig
    {
        string BytesToString(byte[] binaryData);
        byte[] StringToBytes(string stringData);
        string CreateMacInput(IEnumerable<BankMessageField> bankMessage);
    }
}
