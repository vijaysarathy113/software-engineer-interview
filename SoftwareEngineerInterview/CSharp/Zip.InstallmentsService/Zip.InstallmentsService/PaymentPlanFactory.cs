using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PremiumCalculator.Models.DbModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zip.InstallmentsService
{
    /// <summary>
    /// This class is responsible for building the PaymentPlans according to the Zip product definition.
    /// </summary>
    public class PaymentPlanFactory
    {
        private const int numberOfInstallments = 4;
        private const int installmentDurationInDays = 14;
        private readonly InstallmentCalculatorDbContext _installmentCalculatorDbContext;

        public PaymentPlanFactory(InstallmentCalculatorDbContext installmentCalculatorDbContext)
        {
            _installmentCalculatorDbContext = installmentCalculatorDbContext;
        }
        /// <summary>
        /// Builds the PaymentPlans instance.
        /// </summary>
        /// <param name="purchaseAmount">The total amount for the purchase that the customer is making.</param>
        /// <returns>The PaymentPlans created with all properties set.</returns>
        public PaymentPlan CreatePaymentPlan(decimal purchaseAmount)
        {
            PaymentPlanEntity paymentPlan;
            try
            {
                if (numberOfInstallments > 0)
                {
                    if (purchaseAmount <= 0)
                        throw new ArgumentException();
                    //Check if Purchase plan already exist in db
                    paymentPlan = _installmentCalculatorDbContext.PaymentPlans.Include(p => p.Installments)
                                    .FirstOrDefault(p => p.PurchaseAmount == purchaseAmount);
                    if (paymentPlan != null)
                    {
                        return MaptoPaymentPlan(paymentPlan);
                    }
                    //Create New PurchasePlan
                    else
                    {
                        paymentPlan = new PaymentPlanEntity();
                        paymentPlan.Id = Guid.NewGuid();
                        paymentPlan.PurchaseAmount = purchaseAmount;
                        //Calculat Purchase Installment value
                        decimal purchaseInstallment = Math.Round(purchaseAmount / numberOfInstallments, 2, MidpointRounding.ToPositiveInfinity);
                        DateTime installmentDate = DateTime.Today;
                        //Create First Installment
                        paymentPlan.Installments = new List<InstallmentEntity>();
                        paymentPlan.Installments.Add(CreateInstallment(purchaseInstallment, installmentDate));
                        //Create Other Installments
                        for (int i = 1; i < numberOfInstallments; i++)
                        {
                            installmentDate = installmentDate.Add(TimeSpan.FromDays(installmentDurationInDays));
                            paymentPlan.Installments.Add(CreateInstallment(purchaseInstallment, installmentDate));
                        }
                        //Add to Database
                        _installmentCalculatorDbContext.PaymentPlans.Add(paymentPlan);
                        _installmentCalculatorDbContext.SaveChanges();
                        return MaptoPaymentPlan(paymentPlan);
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        private static PaymentPlan MaptoPaymentPlan(PaymentPlanEntity paymentPlan)
        {
            return new PaymentPlan
            {
                Id = paymentPlan.Id,
                PurchaseAmount = paymentPlan.PurchaseAmount,
                Installments = paymentPlan.Installments.Select(i => new Installment
                {
                    Id = i.Id,
                    DueDate = i.DueDate,
                    Amount = i.Amount
                }).ToArray()
            };
        }

        private InstallmentEntity CreateInstallment(decimal purchaseInstallment, DateTime installmentDate) => new InstallmentEntity
        {
            Id = Guid.NewGuid(),
            DueDate = installmentDate,
            Amount = purchaseInstallment
        };

    }
}