using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BanklinksDotNet.ProviderBase;
using Moq;
using NUnit.Framework;

namespace BanklinksDotNet.UnitTests
{
    [TestFixture]
    public class GlobalConfigurationTests
    {
        private IGlobalConfiguration _globalConfiguration;

        private AbstractBankProvider _bankProvider1;
        private AbstractBankProvider _bankProvider2;
        private AbstractBankProvider _bankProvider3;

        private AbstractBankConfiguration _bankConfiguration1;
        private AbstractBankConfiguration _bankConfiguration2;

        [SetUp]
        public void SetUp()
        {
            _globalConfiguration = new GlobalConfiguration();

            _bankProvider1 = new Mock<AbstractBankProvider>().Object;
            _bankProvider2 = new Mock<AbstractBankProvider>().Object;
            _bankProvider3 = new Mock<AbstractBankProvider>().Object;

            _bankConfiguration1 = new Mock<AbstractBankConfiguration>().Object;
            _bankConfiguration2 = new Mock<AbstractBankConfiguration>().Object;
        }

        [Test]
        public void EnableFieldLengthValidation_Is_Turned_On_By_Default()
        {
            Assert.That(_globalConfiguration.EnableFieldLengthValidation, Is.True);
        }

        [Test]
        public void AddBankProvider_Gives_Positional_Priority_To_Providers_Added_Last()
        {
            // Act
            _globalConfiguration.AddBankProvider(_bankProvider1);
            _globalConfiguration.AddBankProvider(_bankProvider2);
            _globalConfiguration.AddBankProvider(_bankProvider3);

            var expectedOrder = new List<AbstractBankProvider>
            {
                _bankProvider3,
                _bankProvider2,
                _bankProvider1
            };

            Assert.That(_globalConfiguration.BankProviders, Is.EqualTo(expectedOrder));
        }

        [Test]
        public void AddBankProviders_Gives_Positional_Priority_To_Providers_Added_Last()
        {
            // Act
            _globalConfiguration.AddBankProviders(new List<AbstractBankProvider>
            {
                _bankProvider3,
                _bankProvider2,
                _bankProvider1
            });

            var expectedOrder = new List<AbstractBankProvider>
            {
                _bankProvider1,
                _bankProvider2,
                _bankProvider3
            };

            Assert.That(_globalConfiguration.BankProviders, Is.EqualTo(expectedOrder));
        }

        [Test]
        public void AddBankConfiguration_Adds_A_Bank_Configuration()
        {
            // Act
            _globalConfiguration.AddBankConfiguration(_bankConfiguration1);
            _globalConfiguration.AddBankConfiguration(_bankConfiguration2);

            var expectedBankConfigs = new List<AbstractBankConfiguration>
            {
                _bankConfiguration1,
                _bankConfiguration2
            };

            Assert.That(_globalConfiguration.BankConfigurations, Is.EquivalentTo(expectedBankConfigs));
        }

        [Test]
        public void AddBankConfigurations_Adds_Bank_Configurations()
        {
            var expectedBankConfigs = new List<AbstractBankConfiguration>
            {
                _bankConfiguration1,
                _bankConfiguration2
            };

            // Act
            _globalConfiguration.AddBankConfigurations(expectedBankConfigs);

            Assert.That(_globalConfiguration.BankConfigurations, Is.EquivalentTo(expectedBankConfigs));
        }

        [Test]
        public void AddBankConfiguration_Adds_A_Dynamically_Changing_Bank_Configuration()
        {
            // Act
            // ReSharper disable once AccessToModifiedClosure
            _globalConfiguration.AddBankConfiguration(() => _bankConfiguration1);

            Assert.That(_globalConfiguration.BankConfigurations.Single(), Is.EqualTo(_bankConfiguration1));

            _bankConfiguration1 = _bankConfiguration2;

            Assert.That(_globalConfiguration.BankConfigurations.Single(), Is.EqualTo(_bankConfiguration2));
        }

        [Test]
        public void AddBankConfigurations_Adds_Dynamically_Changing_Bank_Configurations()
        {
            // Act
            _globalConfiguration.AddBankConfigurations(new List<Func<AbstractBankConfiguration>>
            {

                // ReSharper disable AccessToModifiedClosure
                () => _bankConfiguration1,
                () => _bankConfiguration2,
                // ReSharper restore AccessToModifiedClosure
            });

            var expectedConfigurations1 = new List<AbstractBankConfiguration>
            {
                _bankConfiguration1,
                _bankConfiguration2
            };

            Assert.That(_globalConfiguration.BankConfigurations, Is.EquivalentTo(expectedConfigurations1));

            AbstractBankConfiguration bankConfiguration3 = new Mock<AbstractBankConfiguration>().Object;
            var expectedConfigurations2 = new List<AbstractBankConfiguration>
            {
                _bankConfiguration2,
                bankConfiguration3
            };

            _bankConfiguration1 = _bankConfiguration2;
            _bankConfiguration2 = bankConfiguration3;

            Assert.That(_globalConfiguration.BankConfigurations, Is.EquivalentTo(expectedConfigurations2));
        }
    }
}
