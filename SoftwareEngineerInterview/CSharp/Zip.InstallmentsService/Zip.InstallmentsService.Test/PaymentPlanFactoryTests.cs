using Microsoft.EntityFrameworkCore;
using PremiumCalculator.Models.DbModels;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Zip.InstallmentsService.Test
{
    public class PaymentPlanFactoryTests
    {
        private const int numberOfInstallments = 4;
        private const int installmentDurationInDays = 14;
        private PaymentPlanFactory paymentPlanFactory;

        public PaymentPlanFactoryTests()
        {
            // Arrange            
            paymentPlanFactory = new PaymentPlanFactory(GetDatabaseContext());
        }
        [Fact]
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnValidPaymentPlan()
        {
            // Arrange
            //var paymentPlanFactory = new PaymentPlanFactory();

            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(123.45M);

            // Assert
            paymentPlan.ShouldNotBeNull();
        }       

        [Fact]
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnCorrectNumberofInstallments()
        {
            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(123.45M);

            // Assert
            Assert.Equal(4, paymentPlan.Installments.Count());
        }
        [Fact]
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnCorrectInstallmentDuration()
        {
            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(123.45M);

            // Assert
            Assert.Equal(14, (paymentPlan.Installments[1].DueDate - paymentPlan.Installments[0].DueDate).TotalDays);
        }
        [Fact]
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnCorrectInstallmentAmount()
        {
            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(123.45M);

            // Assert
            Assert.Equal(30.87M, paymentPlan.Installments[0].Amount);
        }
        [Fact]
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnCorrectInstallmentDates()
        {
            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(123.45M);
            DateTime installmentDate = DateTime.Today;
            
            // Assert
            Assert.Equal(DateTime.Today, paymentPlan.Installments[0].DueDate);
            Assert.Equal(DateTime.Today.Add(TimeSpan.FromDays(installmentDurationInDays)), paymentPlan.Installments[1].DueDate);
            Assert.Equal(DateTime.Today.Add(TimeSpan.FromDays(installmentDurationInDays*2)), paymentPlan.Installments[2].DueDate);
            Assert.Equal(DateTime.Today.Add(TimeSpan.FromDays(installmentDurationInDays*3)), paymentPlan.Installments[3].DueDate);

        }
        [Fact]
        public void WhenCreatePaymentPlanWithInValidOrderAmount_ShouldReturnArgumentException()
        {
            // Act
            Action act = () => paymentPlanFactory.CreatePaymentPlan(-55);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }


        private InstallmentCalculatorDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<InstallmentCalculatorDbContext>()
                .UseInMemoryDatabase(databaseName: "InstallmentCalculatorDbTest")
                .Options;
            var installmentCalculatorDbContext = new InstallmentCalculatorDbContext(options);
            //installmentCalculatorDbContext.Database.EnsureCreated();            
            return installmentCalculatorDbContext;
        }
    }
}
