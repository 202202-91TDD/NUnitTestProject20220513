#region

using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace NUnitTestProject20220513
{
    public class BudgetTest
    {
        private BudgetService _budgetService;
        private IBudgetRepository _repo;

        [SetUp]
        public void SetUp()
        {
            _repo = Substitute.For<IBudgetRepository>();
            _budgetService = new BudgetService(_repo);
        }

        [Test]
        public void TestBudgetQuery_InvalidDateTime()
        {
            var start = new DateTime(2022, 5, 10);
            var end = new DateTime(2022, 5, 1);
            GivenBudgets();
            TotalAmountShouldBe(start, end, 0);
        }

        [Test]
        public void TestBudgetQuery_MultiMonth()
        {
            GivenBudgets(
                new Budget() { YearMonth = "202204", Amount = 180 },
                new Budget() { YearMonth = "202205", Amount = 310 },
                new Budget() { YearMonth = "202206", Amount = 420 });

            var start = new DateTime(2022, 4, 30);
            var end = new DateTime(2022, 6, 10);

            TotalAmountShouldBe(start, end, 456);
        }

        [Test]
        public void TestBudgetQuery_NoBudget_ReturnZero()
        {
            GivenBudgets(
                new Budget() { YearMonth = "202204", Amount = 180 },
                new Budget() { YearMonth = "202205", Amount = 310 },
                new Budget() { YearMonth = "202206", Amount = 420 });

            var start = new DateTime(2021, 4, 30);
            var end = new DateTime(2021, 6, 10);

            TotalAmountShouldBe(start, end, 0);
        }

        [Test]
        public void TestBudgetQuery_SingleDay()
        {
            GivenBudgets(
                new Budget() { YearMonth = "202204", Amount = 180 },
                new Budget() { YearMonth = "202205", Amount = 310 });

            var start = new DateTime(2022, 5, 10);
            var end = new DateTime(2022, 5, 10);

            TotalAmountShouldBe(start, end, 10);
        }

        [Test]
        public void TestBudgetQuery_SingleMonth()
        {
            GivenBudgets(
                new Budget() { YearMonth = "202205", Amount = 310 }
            );

            var start = new DateTime(2022, 5, 1);
            var end = new DateTime(2022, 5, 10);

            TotalAmountShouldBe(start, end, 100);
        }

        private void GivenBudgets(params Budget[] budgets)
        {
            _repo.GetAll()
                 .Returns(budgets.ToList());
        }

        private void TotalAmountShouldBe(DateTime start, DateTime end, int expected)
        {
            Assert.AreEqual(expected, _budgetService.Query(start, end));
        }
    }
}