using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    /// <summary>
    /// Applies conditional behavior according to global and bank specific configurations
    /// </summary>
    public class ConfigurationEnforcer
    {
        private readonly Validator _validator;
        private readonly IGlobalConfiguration _globalConfiguration;

        public virtual IGlobalConfiguration GlobalConfiguration
        {
            get { return _globalConfiguration; }
        }

        public ConfigurationEnforcer(IGlobalConfiguration globalConfiguration, Validator validator)
        {
            _validator = validator;
            _globalConfiguration = globalConfiguration;
        }

        public virtual void EnforceFieldLengthValidation(IVisitor handlingVisitor)
        {
            if (GlobalConfiguration.EnableFieldLengthValidation && handlingVisitor.DetectedBankConfiguration.EnableFieldLengthValidation)
            {
                _validator.ValidateFieldLengths(handlingVisitor.Result);
            }
        }
    }
}
