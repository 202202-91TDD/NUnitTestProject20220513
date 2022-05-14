﻿#region

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
            DateTime overlappingEnd = new[] { End, budget.LastDay() }.Min();
            DateTime overlappingStart = new[] { Start, budget.FirstDay() }.Max();
            if (Start.ToString("yyyyMM") == End.ToString("yyyyMM"))
            {
                // overlappingEnd = End;
                // overlappingStart = Start;
            }
            else if (budget.YearMonth == Start.ToString("yyyyMM"))
            {
                // overlappingEnd = budget.LastDay();
                // overlappingStart = Start;
            }
            else if (budget.YearMonth == End.ToString("yyyyMM"))
            {
                // overlappingEnd = End;
                // overlappingStart = budget.FirstDay();
            }
            else
            {
                // overlappingEnd = budget.LastDay();
                // overlappingStart = budget.FirstDay();
            }

            var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            return overlappingDays;
        }
    }
}