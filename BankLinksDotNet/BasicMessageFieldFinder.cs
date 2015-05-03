using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BanklinksDotNet.ProviderBase;

namespace BanklinksDotNet
{
    public class BasicMessageFieldFinder
    {
        public string FindOrDefault(string fieldName, IEnumerable<BankMessageField> bankMessageFields)
        {
            BankMessageField messageField = bankMessageFields.FirstOrDefault(f => f.FieldName == fieldName);

            return messageField == null
                ? string.Empty
                : (messageField.Value ?? string.Empty);
        }

        public long? FindOrDefaultLong(string fieldName, IEnumerable<BankMessageField> bankMessageFields)
        {
            string value = FindOrDefault(fieldName, bankMessageFields);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return long.Parse(value);
        }

        public int? FindOrDefaultInt(string fieldName, IEnumerable<BankMessageField> bankMessageFields)
        {
            string value = FindOrDefault(fieldName, bankMessageFields);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return int.Parse(value);
        }

        public DateTime? FindOrDefaultDateTime(string fieldName, string dateTimeFormat, IEnumerable<BankMessageField> bankMessageFields)
        {
            string value = FindOrDefault(fieldName, bankMessageFields);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return DateTime.ParseExact(value, dateTimeFormat, CultureInfo.InvariantCulture);
        }

        public decimal? FindOrDefaultDecimal(string fieldName, IFormatProvider decimalFormatProvider, IEnumerable<BankMessageField> bankMessageFields)
        {
            string value = FindOrDefault(fieldName, bankMessageFields);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return decimal.Parse(value, decimalFormatProvider);
        }

        // TODO: Switch parameter positioning. Containers go to the end.
        public void MapResponseParamsToMessageFields(IEnumerable<BankMessageField> bankMessagefields, VisitableNameValueCollection visitable)
        {
            foreach (BankMessageField bankMessageField in bankMessagefields)
            {
                bankMessageField.Value = visitable[bankMessageField.FieldName];
            }
        }
    }
}
