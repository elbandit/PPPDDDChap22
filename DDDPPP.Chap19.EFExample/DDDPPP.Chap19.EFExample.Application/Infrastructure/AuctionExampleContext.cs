using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DDDPPP.Chap19.EFExample.Application.Infrastructure.DataModel;
using DDDPPP.Chap19.EFExample.Application.Infrastructure.Mapping;

namespace DDDPPP.Chap19.EFExample.Application.Infrastructure
{
    public partial class AuctionExampleContext : DbContext
    {
        static AuctionExampleContext()
        {
            Database.SetInitializer<AuctionExampleContext>(null);
        }

        public AuctionExampleContext()
            : base("Name=AuctionExampleContext")
        {
        }

        public DbSet<AuctionDTO> Auctions { get; set; }
        public DbSet<BidHistoryDTO> BidHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AuctionMap());
            modelBuilder.Configurations.Add(new BidHistoryMap());           
        }

        public void Clear()
        {
            var context = ((IObjectContextAdapter)this).ObjectContext;

            var addedObjects = context
                             .ObjectStateManager
                             .GetObjectStateEntries(EntityState.Added);

            foreach (var objectStateEntry in addedObjects)
            {
                context.Detach(objectStateEntry.Entity);
            }

            var modifiedObjects = context
                             .ObjectStateManager
                             .GetObjectStateEntries(EntityState.Modified);

            foreach (var objectStateEntry in modifiedObjects)
            {
                context.Detach(objectStateEntry.Entity);
            }
        }
    }
}
