using System.Linq;
using BanklinksDotNet.ProviderBase;
using NUnit.Framework;

namespace BanklinksDotNet.IntegrationTests
{
    public static class BankRequestExtensions
    {
        public static void AssertFieldValueEqualTo(this BankRequest bankRequest, string fieldName, string expectedFieldValue)
        {
            BankMessageField bankMessageField = bankRequest
                .PostParameters
                .First(field => field.FieldName == fieldName);

            Assert.That(bankMessageField.Value, Is.EqualTo(expectedFieldValue));
        }
    }
}
