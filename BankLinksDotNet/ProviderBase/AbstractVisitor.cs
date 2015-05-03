using System.Collections.Generic;
using System.Linq;
using BanklinksDotNet.Exceptions;

namespace BanklinksDotNet.ProviderBase
{
    public abstract class AbstractVisitor<TResult, TVisitable, TBankConfiguration> : IVisitor
        where TResult : IBankMessage
        where TVisitable : class, IVisitable
        where TBankConfiguration : class
    {
        protected IGlobalConfiguration GlobalConfiguration;

        protected AbstractVisitor(IGlobalConfiguration globalConfiguration)
        {
            GlobalConfiguration = globalConfiguration;
        }

        public IBankMessage Result { get; private set; }

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
                .Where(bankConfig => bankConfig is TBankConfiguration)
                .Cast<TBankConfiguration>();

            TBankConfiguration bankConfiguration = FindBankConfiguration(concreteVisitable, bankProviderConfigurations);

            Result = ParseResult(concreteVisitable, bankConfiguration);

            ValidateFieldLengths(Result);

            IsHandled = true;
        }

        // TODO: Bring validation logic to BanklinkApi and simplify AbstractVisitor for readability?
        private void ValidateFieldLengths(IBankMessage result)
        {
            foreach (BankMessageField bankMessageField in Result.PostParameters.Where(field => field.Value != null))
            {
                if (bankMessageField.Value.Length > bankMessageField.MaxLength)
                {
                    throw new FieldLengthOutOfRangeException(bankMessageField, result);
                }
            }
        }

        protected abstract bool CanHandle(TVisitable visitable);
        protected abstract TBankConfiguration FindBankConfiguration(TVisitable visitable, IEnumerable<TBankConfiguration> bankConfigurations);
        protected abstract TResult ParseResult(TVisitable visitable, TBankConfiguration bankConfiguration);
    }
}
