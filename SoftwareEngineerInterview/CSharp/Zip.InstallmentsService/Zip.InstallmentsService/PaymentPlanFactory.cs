using Microsoft.Extensions.Configuration;
using System;


namespace Zip.InstallmentsService
{
    /// <summary>
    /// This class is responsible for building the PaymentPlan according to the Zip product definition.
    /// </summary>
    public class PaymentPlanFactory
    {
        private const int numberOfInstallments = 4;
        private const int installmentDurationInDays = 14;
        /// <summary>
        /// Builds the PaymentPlan instance.
        /// </summary>
        /// <param name="purchaseAmount">The total amount for the purchase that the customer is making.</param>
        /// <returns>The PaymentPlan created with all properties set.</returns>
        public PaymentPlan CreatePaymentPlan(decimal purchaseAmount)
        {
            PaymentPlan paymentPlan = new PaymentPlan();            
            if (numberOfInstallments > 0)
            {
                try
                {
                    if (purchaseAmount <= 0)
                        throw new ArgumentException();
                    //Assign PurchasePlan
                    paymentPlan.Id = Guid.NewGuid();
                    paymentPlan.PurchaseAmount = purchaseAmount;    
                    //Assign Installments
                    paymentPlan.Installments = new Installment[numberOfInstallments];
                    decimal purchaseInstallment = Math.Round(purchaseAmount / numberOfInstallments, 2, MidpointRounding.ToPositiveInfinity);
                    DateTime installmentDate = DateTime.Today;
                    //AssignFirst Installment
                    paymentPlan.Installments[0] = CreateInstallment(purchaseInstallment, installmentDate);
                    //AssignFirst Other Installments
                    for (int i = 1; i < numberOfInstallments; i++)
                    {
                        installmentDate = installmentDate.Add(TimeSpan.FromDays(installmentDurationInDays));
                        paymentPlan.Installments[i] = CreateInstallment(purchaseInstallment, installmentDate);
                    }
                }
                catch
                {
                    throw;
                }
            }
            return paymentPlan;
        }

        private Installment CreateInstallment(decimal purchaseInstallment, DateTime installmentDate) => new Installment
        {
            Id = Guid.NewGuid(),
            DueDate = installmentDate,
            Amount = purchaseInstallment
        };

    }
}