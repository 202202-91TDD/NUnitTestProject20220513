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
                    var daysInMonth = budget.Days();
                    // var daysInMonth = DaysInMonth(current);
                    if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
                    {
                        total += budget.Amount / daysInMonth * (end.Day - start.Day + 1);
                    }
                    else if (current.ToString("yyyyMM") == start.ToString("yyyyMM"))
                    {
                        total += budget.Amount /
                            daysInMonth *
                            (daysInMonth - start.Day + 1);
                    }
                    else if (current.ToString("yyyyMM") == end.ToString("yyyyMM"))
                    {
                        total += budget.Amount / daysInMonth * (end.Day);
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

        private int DaysInMonth(DateTime start)
        {
            return DateTime.DaysInMonth(start.Year, start.Month);
        }

        private bool InvalidQueryDate(DateTime start, DateTime end)
        {
            return end < start;
        }
    }
}