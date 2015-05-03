using System.Collections.Generic;

namespace BanklinksDotNet.Exceptions
{
    public class BankConfigurationMissingException : BanklinksDotNetException
    {
        public IEnumerable<string> AllBankIds { get; private set; }
        public string BankId { get; private set; }

        public BankConfigurationMissingException(string bankId, IEnumerable<string> allBankIds)
        {
            BankId = bankId;
            AllBankIds = allBankIds;
        }

        public override string Message
        {
            get
            {
                return string.Format("Configuration not found for bankId: '{0}'. Registered bank configurations: '{1}'",
                BankId,
                string.Join(", ", AllBankIds));
            }
        }
    }
}
