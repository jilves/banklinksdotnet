using System.Collections.Generic;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public interface IRsaMacCalculatorConfig
    {
        string BinaryToString(byte[] binaryData);
        byte[] StringToBinary(string stringData);
        string CreateMacInput(IEnumerable<BankMessageField> bankMessage);
    }
}
