using System;

namespace Take.SmartContacts.Utils
{
    public class DaysOfWeekDateInterval
    {
        public DayOfWeek[] DaysOfWeeks { get; set; }

        public TimeSpan BeginHour { get; set; }

        public TimeSpan EndHour { get; set; }

        public DaysOfWeekDateInterval(DayOfWeek[] daysOfWeeks, TimeSpan beginHour, TimeSpan endHour)
        {
            DaysOfWeeks = daysOfWeeks;
            BeginHour = beginHour;
            EndHour = endHour;
        }
    }
}
