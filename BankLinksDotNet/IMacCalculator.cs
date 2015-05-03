using System.Collections.Generic;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public interface IMacCalculator
    {
        bool VerifyMac(string encoding, string mac, IEnumerable<BankMessageField> bankMessage);
        string CreateMac(string encoding, IEnumerable<BankMessageField> bankMessage);
    }
}
