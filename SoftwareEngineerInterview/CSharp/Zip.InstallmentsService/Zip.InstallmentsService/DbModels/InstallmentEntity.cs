using System;

namespace Zip.InstallmentsService
{
    /// <summary>
    /// Database Entity which defines all the properties for an installment.
    /// </summary>
    public class InstallmentEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for each installment.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the date that the installment payment is due.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the amount of the installment.
        /// </summary>
        public decimal Amount { get; set; }
        public PaymentPlanEntity PaymentPlan { get; set; }
        public Guid PaymentPlanId { get; set; }
    }
}
