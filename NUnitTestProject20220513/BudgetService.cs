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

        public int OverlappingDays(Budget budget)
        {
            DateTime overlappingEnd;
            DateTime overlappingStart;
            if (Start.ToString("yyyyMM") == End.ToString("yyyyMM"))
            {
                overlappingEnd = End;
                overlappingStart = Start;
            }
            else if (budget.YearMonth == Start.ToString("yyyyMM"))
            {
                overlappingEnd = budget.LastDay();
                overlappingStart = Start;
            }
            else if (budget.YearMonth == End.ToString("yyyyMM"))
            {
                overlappingEnd = End;
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
                    var overlappingDays = new Period(start, end).OverlappingDays(budget);

                    total += budget.DailyAmount() * overlappingDays;
                }

                current = current.AddMonths(1);
            }

            return total;
        }

        private bool InvalidQueryDate(DateTime start, DateTime end)
        {
            return end < start;
        }
    }
}