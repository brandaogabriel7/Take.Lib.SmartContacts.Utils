using System;
using System.Linq;
using Take.SmartContacts.Utils.Helpers;

namespace Take.SmartContacts.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly string[] _brazilianFixedHolidays =
        {
            "01/01", // Ano novo
            "21/04", // Tiradentes
            "01/05", // Dia do trabalhador
            "07/09", // Independência do Brasil
            "12/10", // Padroeira do Brasil
            "02/11", // Finados
            "15/11", // Proclamação da Republica
            "24/12", // Natal
            "25/12", // Natal
            "31/12"  // Ano novo
        };

        public static bool IsBrazilianFixedHoliday(this DateTime date)
        {
            return _brazilianFixedHolidays.Contains(date.ToString("dd/MM"));
        }

        public static bool IsBrazilianMobileHoliday(this DateTime date)
        {
            var dayOfYear = date.DayOfYear;

            var easterDate = DateHelper.GetEasterDate(date.Year);
            if (dayOfYear == easterDate.DayOfYear)
                return true;

            var carnivalPeriod = DateHelper.GetCarnivalPeriod(date.Year);
            if (dayOfYear >= carnivalPeriod.StartDate.DayOfYear && dayOfYear <= carnivalPeriod.EndDate.DayOfYear)
                return true;

            var crucifixionDate = DateHelper.GetCrucifixionDate(date.Year);
            if (dayOfYear == crucifixionDate.DayOfYear)
                return true;

            var corpusChristiDate = DateHelper.GetCorpusChristiDate(date.Year);
            if (dayOfYear == corpusChristiDate.DayOfYear)
                return true;

            return false;
        }
    }
}
