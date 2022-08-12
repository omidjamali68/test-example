using System;

namespace TestExample.Infrastructure
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }

    public class UtcDateTimeService : IDateTimeService
    {
        public DateTime Today => DateTime.Now.Date.ToUniversalTime();

        public DateTime Now => RefineDate(DateTime.Now.ToUniversalTime());

        public DateTime RefineDate(DateTime dateTime)
        {
            var timeTicks = dateTime.TimeOfDay.Ticks;
            var extraDigit = timeTicks % 10;

            return dateTime.Subtract(TimeSpan.FromTicks(extraDigit));
        }
    }
}