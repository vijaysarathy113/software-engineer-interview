namespace InstallmentCalculatorApi.Models
{
    public class InstallmentResponse
    {
        /// <summary>
        /// Gets or sets the amount of the installment.
        /// </summary>
        public decimal PurchaseAmount { get; set; }
        /// <summary>
        /// Gets or sets the date that the installment payment is due.
        /// </summary>
        public DateTime DueDate { get; set; }
    }
}
