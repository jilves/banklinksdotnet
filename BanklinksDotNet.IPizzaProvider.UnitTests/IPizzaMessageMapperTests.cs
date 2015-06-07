using System;
using System.Collections.Generic;
using System.Globalization;
using BanklinksDotNet.ProviderBase;
using Moq;
using NUnit.Framework;

namespace BanklinksDotNet.IPizzaProvider.UnitTests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    public class IPizzaMessageMapperTests
    {
        private Mock<IMacCalculator> _macCalculatorMock;
        private IPizzaMessageMapper _messageMapper;
        private VisitableNameValueCollection _testData;
        private IPizzaAuthResponse _authResponse;
        private Mock<TimeProvider> _timeProviderMock;
        private IPizzaConfiguration _ipizzaConfiguration;

        [SetUp]
        public void SetUp()
        {
            _macCalculatorMock = new Mock<IMacCalculator>();
            _macCalculatorMock.Setup(macCalc => macCalc.VerifyMac(It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<IEnumerable<BankMessageField>>()))
                    .Returns(true);

            var macCalculatorFactoryMock = new Mock<MacCalculatorFactory>();
            macCalculatorFactoryMock
                .Setup(factory => factory.CreateCalculator(It.IsAny<string>(), It.IsAny<IPkiBankConfiguration>()))
                .Returns(_macCalculatorMock.Object);

            _timeProviderMock = new Mock<TimeProvider>();

            _messageMapper = new IPizzaMessageMapper(new BasicMessageFieldFinder(), macCalculatorFactoryMock.Object, _timeProviderMock.Object);

            _testData = new VisitableNameValueCollection
            {
                {"VK_DATETIME", string.Empty},
            };

            _authResponse = new IPizzaAuthResponse(string.Empty, new List<BankMessageField>
            {
                new BankMessageField
                {
                    FieldName = "VK_DATETIME",
                    MaxLength = int.MaxValue,
                }
            });

            _ipizzaConfiguration = new IPizzaConfiguration();
        }

        [Test]

        // Note that if the response generation datetime value is greater than the current time, then that means the response
        // "was generated from the future". 
        // According to the specification this is valid, if the difference in total minutes doesn't exceed 5.
        // http://pangaliit.ee/images/files/Pangalingi_tehniline_spetsifikatsioon_2014_FINAL.pdf

        [TestCase("2015-04-04T16:05:00+0300", "2015-04-04T16:00:00+0300", true, TestName = "generation date = current time +5 minutes")]
        [TestCase("2015-04-04T15:55:00+0300", "2015-04-04T16:00:00+0300", true, TestName = "generation date = current time -5 minutes")]
        [TestCase("2015-04-04T15:56:46+0300", "2015-04-04T16:00:00+0300", true, TestName = "generation date = current time - 3 minutes and 14 seconds")]
        [TestCase("2015-04-04T16:05:01+0300", "2015-04-04T16:00:00+0300", false, TestName = "generation date = current time +5 minutes and 1 second")]
        [TestCase("2015-04-04T15:54:59+0300", "2015-04-04T16:00:00+0300", false, TestName = "generation date = current time -5 minutes and 1 second")]

        // Same differences between times as the test cases above, but with different time zones.
        [TestCase("2015-04-04T18:05:00+0400", "2015-04-04T16:00:00+0200", true, TestName = "Different time zones. generation date = current time +5 minutes")]
        [TestCase("2015-04-04T16:55:00+0200", "2015-04-04T16:00:00+0100", true, TestName = "Different time zones. generation date = current time -5 minutes")]
        [TestCase("2015-04-04T15:56:46+0300", "2015-04-04T17:00:00+0400", true, TestName = "Different time zones. generation date = current time - 3 minutes and 14 seconds")]
        [TestCase("2015-04-04T16:05:01+0500", "2015-04-04T16:00:00+0500", false, TestName = "Different time zones. generation date = current time +5 minutes and 1 second")]
        [TestCase("2015-04-04T19:54:59+0100", "2015-04-04T16:00:00+0500", false, TestName = "Different time zones. generation date = current time -5 minutes and 1 second")]
        public void SetAuthResponseProperties_Sets_IsRequestDateTimeValid_True_If_Request_Generation_DateTime_Is_In_5_Minute_Range
            (string authResponseGenerationDateTime, string currentDateTime, bool expectedIsResponseValid)
        {
            _testData["VK_DATETIME"] = authResponseGenerationDateTime;

            DateTime systemDateTime = DateTime.ParseExact(currentDateTime, _ipizzaConfiguration.DateTimeFormat, CultureInfo.InvariantCulture);
            _timeProviderMock
                .SetupGet(mock => mock.Now)
                .Returns(systemDateTime);

            _messageMapper.SetAuthResponseProperties(_testData, _ipizzaConfiguration, _authResponse);

            Assert.That(_authResponse.IsRequestDateTimeValid, Is.EqualTo(expectedIsResponseValid));
        }
    }
}
