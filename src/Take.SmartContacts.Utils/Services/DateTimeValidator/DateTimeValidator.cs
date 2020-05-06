using System;
using System.Collections.Generic;
using System.Linq;
using Take.SmartContacts.Utils.Extensions;

namespace Take.SmartContacts.Utils
{
    public class DateTimeValidator : IDateTimeValidator
    {
        public bool IsInInterval(IEnumerable<DayOfWeek> daysOfWeeks, TimeSpan beginHour, TimeSpan endHour, DateTime checkDate)
        {
            return daysOfWeeks.Contains(checkDate.DayOfWeek) && checkDate.TimeOfDay >= beginHour && checkDate.TimeOfDay < endHour;
        }

        public bool IsInInterval(IEnumerable<DayOfWeek> daysOfWeeks, TimeSpan beginHour, TimeSpan endHour)
        {
            return IsInInterval(daysOfWeeks, beginHour, endHour, DateTime.Now);
        }

        public bool IsInInterval(DateTime startDate, DateTime endDate, DateTime checkDate)
        {
            return checkDate >= startDate && checkDate < endDate;
        }

        public bool IsInInterval(DateTime startDate, DateTime endDate)
        {
            return IsInInterval(startDate, endDate, DateTime.Now);
        }

        public bool IsInInterval(IEnumerable<DaysOfWeekDateInterval> dateInterval, DateTime checkDate)
        {
            return dateInterval.Any(d => IsInInterval(d.DaysOfWeeks, d.BeginHour, d.EndHour, checkDate));
        }

        public bool IsInInterval(IEnumerable<DaysOfWeekDateInterval> dateInterval)
        {
            return IsInInterval(dateInterval, DateTime.Now);
        }

        public bool IsInInterval(IEnumerable<DateInterval> dateInterval, DateTime checkDate)
        {
            return dateInterval.Any(d => IsInInterval(d.BeginDate, d.EndDate, checkDate));
        }

        public bool IsInInterval(IEnumerable<DateInterval> dateInterval)
        {
            return IsInInterval(dateInterval, DateTime.Now);
        }

        public bool IsHoliday(DateTime checkDate)
        {
            return checkDate.IsBrazilianFixedHoliday() || checkDate.IsBrazilianMobileHoliday();
        }
    }
}
