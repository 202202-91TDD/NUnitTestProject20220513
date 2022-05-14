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
                    var dailyAmount = budget.DailyAmount();
                    if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
                    {
                        total += dailyAmount * (end.Day - start.Day + 1);
                    }
                    else if (current.ToString("yyyyMM") == start.ToString("yyyyMM"))
                    {
                        total += dailyAmount *
                            (budget.Days() - start.Day + 1);
                    }
                    else if (current.ToString("yyyyMM") == end.ToString("yyyyMM"))
                    {
                        total += dailyAmount * (end.Day);
                    }
                    else
                    {
                        total += budget.Amount;
                    }
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