#region

using System;
using System.Linq;

#endregion

namespace NUnitTestProject20220513
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        private DateTime End { get; set; }
        private DateTime Start { get; set; }

        public int OverlappingDays(Budget budget)
        {
            var firstDay = budget.FirstDay();
            var lastDay = budget.LastDay();
            var overlappingEnd = new[] { End, lastDay }.Min();
            var overlappingStart = new[] { Start, firstDay }.Max();

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}