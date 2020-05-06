using System;

namespace Take.SmartContacts.Utils
{
    public class DateInterval
    {
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateInterval(DateTime beginDate, DateTime endDate)
        {
            BeginDate = beginDate;
            EndDate = endDate;
        }
    }
}
