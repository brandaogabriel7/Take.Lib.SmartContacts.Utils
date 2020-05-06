using System;
using System.Collections.Generic;

namespace Take.SmartContacts.Utils
{
    public interface IDateTimeValidator
    {
        bool IsInInterval(IEnumerable<DayOfWeek> daysOfWeeks, TimeSpan beginHour, TimeSpan endHour, DateTime checkDate);

        bool IsInInterval(IEnumerable<DayOfWeek> daysOfWeeks, TimeSpan beginHour, TimeSpan endHour);

        bool IsInInterval(DateTime startDate, DateTime endDate, DateTime checkDate);

        bool IsInInterval(DateTime startDate, DateTime endDate);

        bool IsInInterval(IEnumerable<DaysOfWeekDateInterval> dateInterval, DateTime checkDate);

        bool IsInInterval(IEnumerable<DaysOfWeekDateInterval> dateInterval);

        bool IsInInterval(IEnumerable<DateInterval> dateInterval, DateTime checkDate);

        bool IsInInterval(IEnumerable<DateInterval> dateInterval);

        bool IsHoliday(DateTime checkDate);
    }
}
