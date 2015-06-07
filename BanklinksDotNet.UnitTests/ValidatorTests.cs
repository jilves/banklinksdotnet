using System.Collections.Generic;
using System.Linq;
using BanklinksDotNet.Exceptions;
using BanklinksDotNet.ProviderBase;
using Moq;
using NUnit.Framework;

namespace BanklinksDotNet.UnitTests
{
    public class ValidatorTests
    {
        private Validator _validator;
        private IBankMessage _bankMessage;
        private List<BankMessageField> _parameters;

        [SetUp]
        public void SetUp()
        {
            _validator = new Validator();

            _parameters = new List<BankMessageField>
            {
                new BankMessageField { FieldName = "NAME", MaxLength = 1, Value = "X" },
                new BankMessageField { FieldName = "LANG", MaxLength = 3, Value = "ENG" },
                new BankMessageField { FieldName = "SERVICE_CODE", MaxLength = 4, Value = "1011" },
            };

            _bankMessage = Mock.Of<IBankMessage>(bankMsg => bankMsg.PostParameters == _parameters);
        }

        [Test]
        public void ValidateFieldLengths_Does_Not_Throw_When_Field_Lengths_Are_Below_Limit()
        {
            Assert.DoesNotThrow(() => _validator.ValidateFieldLengths(_bankMessage));
        }

        [Test]
        public void ValidateFieldLengths_Does_Not_Throw_When_Field_Value_Is_Null()
        {
            _parameters.First().Value = null;
            Assert.DoesNotThrow(() => _validator.ValidateFieldLengths(_bankMessage));
        }

        [Test]
        public void ValidateFieldLengths_Throws_When_Field_Length_Is_Too_Long()
        {
            BankMessageField fieldWithInvalidLength = _parameters.First(p => p.FieldName == "SERVICE_CODE");
            fieldWithInvalidLength.MaxLength = 3;

            Assert.Throws<FieldLengthOutOfRangeException>(() =>
            {
                try
                {
                    // Act
                    _validator.ValidateFieldLengths(_bankMessage);
                }
                catch (FieldLengthOutOfRangeException e)
                {
                    // Assert
                    Assert.That(e.BankMessage, Is.EqualTo(_bankMessage));
                    Assert.That(e.InvalidField, Is.EqualTo(fieldWithInvalidLength));
                    throw;
                }
            });
        }
    }
}
