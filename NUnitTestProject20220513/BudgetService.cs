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

        public DateTime End { get; private set; }
        public DateTime Start { get; private set; }
    }

    public class BudgetService
    {
        private readonly IBudgetRepository budgetRepository;

        public BudgetService(IBudgetRepository _budgetRepository)
        {
            budgetRepository = _budgetRepository;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            if (InvalidQueryDate(start, end))
            {
                return 0;
            }

            var budgets = budgetRepository.GetAll();

            var current = start;
            var total = 0m;
            while (current < new DateTime(end.Year, end.Month, 1).AddMonths(1))
            {
                var budget = budgets.FirstOrDefault(x => x.YearMonth == current.ToString("yyyyMM"));

                if (budget != null)
                {
                    var overlappingDays = OverlappingDays(new Period(start, end), budget);

                    total += budget.DailyAmount() * overlappingDays;
                }

                current = current.AddMonths(1);
            }

            return total;
        }

        private static int OverlappingDays(Period period, Budget budget)
        {
            DateTime overlappingEnd;
            DateTime overlappingStart;
            if (period.Start.ToString("yyyyMM") == period.End.ToString("yyyyMM"))
            {
                overlappingEnd = period.End;
                overlappingStart = period.Start;
            }
            else if (budget.YearMonth == period.Start.ToString("yyyyMM"))
            {
                overlappingEnd = budget.LastDay();
                overlappingStart = period.Start;
            }
            else if (budget.YearMonth == period.End.ToString("yyyyMM"))
            {
                overlappingEnd = period.End;
                overlappingStart = budget.FirstDay();
            }
            else
            {
                overlappingEnd = budget.LastDay();
                overlappingStart = budget.FirstDay();
            }

            var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            return overlappingDays;
        }

        private bool InvalidQueryDate(DateTime start, DateTime end)
        {
            return end < start;
        }
    }
}