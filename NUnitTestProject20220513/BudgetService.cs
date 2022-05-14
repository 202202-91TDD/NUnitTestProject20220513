#region

using System;
using System.Linq;

#endregion

namespace NUnitTestProject20220513
{
    public class BudgetService
    {
        private readonly IBudgetRepository _budgetRepository;

        public BudgetService(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            if (InvalidQueryDate(start, end))
            {
                return 0;
            }

            var budgets = _budgetRepository.GetAll();

            var current = start;
            var total = 0m;
            var period = new Period(start, end);
            foreach (var budget in budgets)
            {
                total += budget.OverlappingAmount(period);
            }

            return total;
            while (current < new DateTime(end.Year, end.Month, 1).AddMonths(1))
            {
                var budget = budgets.FirstOrDefault(x => x.YearMonth == current.ToString("yyyyMM"));

                if (budget != null)
                {
                    total += budget.OverlappingAmount(period);
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