using System;
using System.Globalization;
using Xunit;

namespace Take.SmartContacts.Utils.Tests
{
    public class DateTimeValidatorTests
    {
        private readonly IDateTimeValidator _dateValidator;

        public DateTimeValidatorTests()
        {
            _dateValidator = new DateTimeValidator();
        }

        [Fact]
        public void ValidDate_IsBetween_ReturnCorrect()
        {
            DayOfWeek[] daysOfWeeks = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
            var beginHour = new TimeSpan(8, 0, 0);
            var endHour = new TimeSpan(18, 0, 0);

            var isBetween = _dateValidator.IsInInterval(daysOfWeeks, beginHour, endHour, new DateTime(2019, 6, 27, 14, 30, 0));
            Assert.True(isBetween);
        }

        [Fact]
        public void ValidDate_IsBetween_ReturnsIncorrect()
        {
            DayOfWeek[] daysOfWeeks = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
            var beginHour = new TimeSpan(20, 0, 0);
            var endHour = new TimeSpan(06, 0, 0);

            var isBetween = _dateValidator.IsInInterval(daysOfWeeks, beginHour, endHour, new DateTime(2019, 6, 27, 14, 30, 0));
            Assert.False(isBetween);
        }

        [Theory]
        [InlineData("01/01/2019")]
        [InlineData("04/03/2019")]
        [InlineData("05/03/2019")]
        [InlineData("19/04/2019")]
        [InlineData("21/04/2019")]
        [InlineData("01/05/2019")]
        [InlineData("20/06/2019")]
        [InlineData("07/09/2019")]
        [InlineData("12/10/2019")]
        [InlineData("02/11/2019")]
        [InlineData("15/11/2019")]
        [InlineData("25/12/2019")]
        public void HolidayDate_IsBetweenAndWorkDay_ReturnsIncorrect(string date)
        {
            var dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var isHoliday = _dateValidator.IsHoliday(dateTime);
            Assert.True(isHoliday);
        }
    }
}
