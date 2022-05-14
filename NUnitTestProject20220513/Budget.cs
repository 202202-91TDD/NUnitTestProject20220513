#region

using System;

#endregion

namespace NUnitTestProject20220513
{
    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }

        public decimal OverlappingAmount(Period period)
        {
            return DailyAmount() * period.OverlappingDays(CreatePeriod());
        }

        private Period CreatePeriod()
        {
            return new Period(FirstDay(), LastDay());
        }

        private decimal DailyAmount()
        {
            return Amount / (decimal)Days();
        }

        private int Days()
        {
            var firstDay = FirstDay();
            return DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
        }

        private DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }

        private DateTime LastDay()
        {
            return DateTime.ParseExact(YearMonth + Days(), "yyyyMMdd", null);
        }
    }
}