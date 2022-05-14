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
            var period = new Period(start, end);

            return _budgetRepository.GetAll()
                                    .Sum(budget => budget.OverlappingAmount(period));
        }
    }
}