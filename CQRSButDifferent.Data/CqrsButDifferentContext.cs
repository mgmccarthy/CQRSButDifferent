using System.Data.Entity;

namespace CQRSButDifferent.Data
{
    public class CqrsButDifferentContext : DbContext
    {
        public CqrsButDifferentContext()
        {
            Database.SetInitializer(new CqrsButDifferentContextInitializer());
        }

        public DbSet<ProductQuantity> ProductQuantity { get; set; }
    }

    public class CqrsButDifferentContextInitializer : DropCreateDatabaseIfModelChanges<CqrsButDifferentContext>
    //public class CqrsButDifferentContextInitializer : DropCreateDatabaseAlways<CqrsButDifferentContext>
    {
        //forget seeding, let's resupply our vendor to start
        //protected override void Seed(CqrsButDifferentContext context)
        //{
        //    for (var i = 1; i <= 1000; i++)
        //    {
        //        context.ProductQuantity.Add(new ProductQuantity {});
        //    }
        //}
    }
}
