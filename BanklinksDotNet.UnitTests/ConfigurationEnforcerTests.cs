using BanklinksDotNet.ProviderBase;
using Moq;
using NUnit.Framework;

namespace BanklinksDotNet.UnitTests
{
    [TestFixture]
    public class ConfigurationEnforcerTests
    {
        private Mock<IGlobalConfiguration> _globalConfigMock;
        private Mock<Validator> _validatorMock;
        private ConfigurationEnforcer _configEnforcer;
        private Mock<AbstractBankConfiguration> _bankConfigMock;

        [SetUp]
        public void SetUp()
        {
            _globalConfigMock = new Mock<IGlobalConfiguration>();
            _bankConfigMock = new Mock<AbstractBankConfiguration>();
            _validatorMock = new Mock<Validator>();

            _configEnforcer = new ConfigurationEnforcer(_globalConfigMock.Object, _validatorMock.Object);
        }

        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public void EnforceFieldLengthValidation_Applies_Validation_Only_When_Configuration_Allows_It(
            bool globalConfigAllows, bool bankConfigAllows, bool expectedValidationCalled)
        {
            _globalConfigMock.SetupGet(mock => mock.EnableFieldLengthValidation).Returns(globalConfigAllows);
            _bankConfigMock.SetupGet(mock => mock.EnableFieldLengthValidation).Returns(bankConfigAllows);

            // Act
            _configEnforcer.EnforceFieldLengthValidation(Mock.Of<IVisitor>(mock => mock.DetectedBankConfiguration == _bankConfigMock.Object));

            // Assert
            _validatorMock.Verify(mock => mock.ValidateFieldLengths(It.IsAny<IBankMessage>()), 
                expectedValidationCalled
                    ? Times.Once()
                    : Times.Never());
        }
    }
}
