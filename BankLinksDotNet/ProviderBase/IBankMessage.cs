using System.Collections.Generic;

namespace BanklinksDotNet.ProviderBase
{
    public interface IBankMessage
    {
        IEnumerable<BankMessageField> PostParameters { get; } 
    }
}
