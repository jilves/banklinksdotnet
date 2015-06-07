using System.Collections.Generic;
using System.Collections.Specialized;
using BanklinksDotNet.Exceptions;
using BanklinksDotNet.ProviderBase;
using Moq;
using NUnit.Framework;

namespace BanklinksDotNet.UnitTests
{
    [TestFixture]
    public class BanklinkApiTests
    {
        private IBanklinkApi _banklinkApi;
        private GlobalConfiguration _globalConfig;
        private Mock<ConfigurationEnforcer> _configEnforcerMock;
        private Mock<IVisitor> _handlingVisitorMock;

        [SetUp]
        public void SetUp()
        {
            _globalConfig = new GlobalConfiguration();

            var handlingVisitor = Mock.Of<IVisitor>(v => v.IsHandled == true);
            _handlingVisitorMock = Mock.Get(handlingVisitor);

            var visitorData = new List<IVisitor>
            {
                handlingVisitor,
                Mock.Of<IVisitor>(v => v.IsHandled == false)
            };

            var bankProvider = Mock.Of<AbstractBankProvider>(provider =>
                provider.CreateTransientResponseVisitors(It.IsAny<IGlobalConfiguration>()) == visitorData &&
                provider.CreateTransientRequestVisitors(It.IsAny<IGlobalConfiguration>()) == visitorData);

            _configEnforcerMock = new Mock<ConfigurationEnforcer>(new object[]{ null, null });
            _configEnforcerMock
                .SetupGet(mock => mock.GlobalConfiguration)
                .Returns(_globalConfig);

            _banklinkApi = new BanklinkApi(_configEnforcerMock.Object, new HttpParameterParser());
            _banklinkApi
                .Configure()
                .AddBankProvider(bankProvider);
        }

        [Test]
        public void ParseBankResponse_Applies_Field_Length_Validation_To_Handling_Visitor()
        {
            // Act
            _banklinkApi.ParseBankResponse(new NameValueCollection());

            // Assert
            _configEnforcerMock.Verify(m => m.EnforceFieldLengthValidation(_handlingVisitorMock.Object), Times.Once());
        }

        [Test]
        public void CreateBankRequest_Applies_Field_Length_Validation_To_Handling_Visitor()
        {
            // Act
            _banklinkApi.CreateBankRequest(Mock.Of<AbstractRequestParams>());

            // Assert
            _configEnforcerMock.Verify(m => m.EnforceFieldLengthValidation(_handlingVisitorMock.Object), Times.Once());
        }


        [Test]
        [ExpectedException(typeof(ProviderMissingException))]
        public void ParseBankResponse_Throws_When_No_Suitable_Bank_Response_Visitors()
        {
            _handlingVisitorMock.SetupGet(mock => mock.IsHandled).Returns(false);
            
            // Act
            _banklinkApi.ParseBankResponse(new NameValueCollection());
        }

        [Test]
        [ExpectedException(typeof(ProviderMissingException))]
        public void CreateBankRequest_Throws_When_No_Suitable_Bank_Response_Visitors()
        {
            _handlingVisitorMock.SetupGet(mock => mock.IsHandled).Returns(false);

            // Act
            _banklinkApi.CreateBankRequest(Mock.Of<AbstractRequestParams>());
        }
    }
}
