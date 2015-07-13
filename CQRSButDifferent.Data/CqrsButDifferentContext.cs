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
    {
    }
}
