using System.Collections.Generic;
using System.Linq;

namespace BanklinksDotNet.ProviderBase
{
    public abstract class AbstractVisitor<TResult, TVisitable, TBankConfiguration> : IVisitor
        where TResult : IBankMessage
        where TVisitable : class, IVisitable
        where TBankConfiguration : AbstractBankConfiguration
    {
        protected IGlobalConfiguration GlobalConfiguration;

        protected AbstractVisitor(IGlobalConfiguration globalConfiguration)
        {
            GlobalConfiguration = globalConfiguration;
        }

        /// <summary>
        /// Set, when IsHandled = true.
        /// </summary>
        public IBankMessage Result { get; private set; }

        /// <summary>
        /// Set, when IsHandled = true.
        /// </summary>
        public AbstractBankConfiguration DetectedBankConfiguration { get; private set; }

        /// <summary>
        /// Set to true, when all fields are validated, parsed and stored into the 'Result' property.
        /// </summary>
        public bool IsHandled { get; private set; }

        public void Visit(IVisitable visitable)
        {
            var concreteVisitable = visitable as TVisitable;
            if (concreteVisitable == null || !CanHandle(concreteVisitable))
            {
                return;
            }

            IEnumerable<TBankConfiguration> bankProviderConfigurations = GlobalConfiguration
                .BankConfigurations
                .OfType<TBankConfiguration>();

            TBankConfiguration bankConfiguration = FindBankConfiguration(concreteVisitable, bankProviderConfigurations);

            DetectedBankConfiguration = bankConfiguration;
            Result = ParseResult(concreteVisitable, bankConfiguration);

            IsHandled = true;
        }

        protected abstract bool CanHandle(TVisitable visitable);
        protected abstract TBankConfiguration FindBankConfiguration(TVisitable visitable, IEnumerable<TBankConfiguration> bankConfigurations);
        protected abstract TResult ParseResult(TVisitable visitable, TBankConfiguration bankConfiguration);
    }
}
