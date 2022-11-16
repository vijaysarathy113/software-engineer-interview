using Microsoft.EntityFrameworkCore;
using Zip.InstallmentsService;

namespace PremiumCalculator.Models.DbModels
{
    public class InstallmentCalculatorDbContext : DbContext
    {
        public InstallmentCalculatorDbContext(DbContextOptions<InstallmentCalculatorDbContext> options)
            : base(options)
        {

        }

        public DbSet<PaymentPlanEntity> PaymentPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentPlanEntity>(model =>
            {
                model.HasKey(p => p.Id);
                model.HasMany(p => p.Installments).WithOne(i => i.PaymentPlan).HasForeignKey(p => p.PaymentPlanId);
                model.HasIndex(p => p.PurchaseAmount);
            });
        }
    }
}
