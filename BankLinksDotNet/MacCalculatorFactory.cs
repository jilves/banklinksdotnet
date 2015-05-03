using System;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public class MacCalculatorFactory
    {
        public virtual IMacCalculator CreateCalculator(string macVersion, IPkiBankConfiguration bankConfig)
        {
            if (macVersion == "008")
            {
                return new RsaMacCalculator(new RsaMac008Config(),  bankConfig);
            }

            if (macVersion == "004" || macVersion == "4")
            {
                return new RsaMacCalculator(new RsaMac004Config(), bankConfig);
            }

            throw new NotSupportedException("MacVersion: " + macVersion);
        }
    }
}