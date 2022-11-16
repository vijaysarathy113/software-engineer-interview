using System;
using System.Collections.Generic;

namespace Zip.InstallmentsService
{
    /// <summary>
    /// Database Entity which defines all the properties for a purchase installment plan.
    /// </summary>
    public class PaymentPlanEntity
    {
        public Guid Id { get; set; }

		public decimal PurchaseAmount { get; set; }

        public ICollection<InstallmentEntity> Installments { get; set; }
    }
}
