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
                    int overlappingDays;
                    if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
                    {
                        overlappingDays = (end.Day - start.Day + 1);
                        // total += budget.DailyAmount() * overlappingDays;
                    }
                    else if (current.ToString("yyyyMM") == start.ToString("yyyyMM"))
                    {
                        overlappingDays = (budget.Days() - start.Day + 1);
                        // total += budget.DailyAmount() * overlappingDays;
                    }
                    else if (current.ToString("yyyyMM") == end.ToString("yyyyMM"))
                    {
                        overlappingDays = (end.Day);
                        // total += budget.DailyAmount() * overlappingDays;
                    }
                    else
                    {
                        overlappingDays = budget.Days();
                        // total += budget.DailyAmount() * overlappingDays;
                    }

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