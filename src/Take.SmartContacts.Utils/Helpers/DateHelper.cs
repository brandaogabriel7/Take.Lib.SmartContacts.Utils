using System;

namespace Take.SmartContacts.Utils.Helpers
{
    internal static class DateHelper
    {
        internal static DateTime GetEasterDate(int year)
        {
            var a = year % 19;
            var b = year / 100;
            var c = (b - (b / 4) - ((8 * b + 13) / 25) + (19 * a) + 15) % 30;
            var d = c - (c / 28) * (1 - (c / 28) * (29 / (c + 1)) * ((21 - a) / 11));
            var e = d - ((year + (year / 4) + d + 2 - b + (b / 4)) % 7);

            var month = 3 + ((e + 40) / 44);
            var day = e + 28 - (31 * (month / 4));

            return new DateTime(year, month, day);
        }

        internal static (DateTime StartDate, DateTime EndDate) GetCarnivalPeriod(int year)
        {
            var carnivalWednesday = GetEasterDate(year).AddDays(-46);
            var carnivalMonday = carnivalWednesday.AddDays(-2);

            return (carnivalMonday, carnivalWednesday);
        }

        internal static DateTime GetCrucifixionDate(int year)
        {
            return GetEasterDate(year).AddDays(-2);
        }

        internal static DateTime GetCorpusChristiDate(int year)
        {
            return GetEasterDate(year).AddDays(60);
        }
    }
}
