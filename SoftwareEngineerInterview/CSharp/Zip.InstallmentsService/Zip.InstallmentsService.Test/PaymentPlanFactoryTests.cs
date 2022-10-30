using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Zip.InstallmentsService.Test
{
    public class PaymentPlanFactoryTests
    {
        private PaymentPlanFactory paymentPlanFactory;

        public PaymentPlanFactoryTests()
        {
            // Arrange
            paymentPlanFactory = new PaymentPlanFactory();
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

            // Assert
            Assert.Equal(new DateTime(2022,10,30), paymentPlan.Installments[0].DueDate);
            Assert.Equal(new DateTime(2022, 11, 13), paymentPlan.Installments[1].DueDate);
            Assert.Equal(new DateTime(2022, 11, 27), paymentPlan.Installments[2].DueDate);
            Assert.Equal(new DateTime(2022, 12, 11), paymentPlan.Installments[3].DueDate);
        }
        [Fact]
        public void WhenCreatePaymentPlanWithInValidOrderAmount_ShouldReturnArgumentException()
        {
            // Act
            Action act = () => paymentPlanFactory.CreatePaymentPlan(-55);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }
    }
}
