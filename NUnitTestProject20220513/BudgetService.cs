#region

using System;
using System.Linq;

#endregion

namespace NUnitTestProject20220513
{
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
                    DateTime overlappingEnd;
                    DateTime overlappingStart;
                    if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
                    {
                        overlappingEnd = end;
                        overlappingStart = start;
                    }
                    else if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        overlappingEnd = budget.LastDay();
                        overlappingStart = start;
                    }
                    else if (budget.YearMonth == end.ToString("yyyyMM"))
                    {
                        overlappingEnd = end;
                        overlappingStart = budget.FirstDay();
                    }
                    else
                    {
                        overlappingEnd = budget.LastDay();
                        overlappingStart = budget.FirstDay();
                    }

                    var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;

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