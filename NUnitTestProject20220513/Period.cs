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

        public int OverlappingDays(Period another)
        {
            if (IsInvalid() || HasNoOverlapping(another))
            {
                return 0;
            }

            var overlappingEnd = new[] { End, another.End }.Min();
            var overlappingStart = new[] { Start, another.Start }.Max();

            return (overlappingEnd - overlappingStart).Days + 1;
        }

        private bool HasNoOverlapping(Period another)
        {
            return another.End < Start || another.Start > End;
        }

        private bool IsInvalid()
        {
            return End < Start;
        }
    }
}