using System.Collections.Generic;

namespace BanklinksDotNet.ProviderBase
{
    public abstract class AbstractBankProvider
    {
        public abstract IEnumerable<IVisitor> CreateTransientRequestVisitors(IGlobalConfiguration globalConfiguration);
        public abstract IEnumerable<IVisitor> CreateTransientResponseVisitors(IGlobalConfiguration globalConfiguration);
    }
}
