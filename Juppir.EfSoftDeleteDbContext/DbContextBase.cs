using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juppir.EfSoftDeleteDbContext
{
    public class DbContextBase : DbContext
    {
        static DbContextBase()
        {
            Database.SetInitializer<DbContextBase>(null);
        }
        public DbContextBase(string connectionStringName)
            : base(connectionStringName)
        {

        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted);
            foreach (var entry in entries)
            {
                SoftDelete(entry);
            }

            return base.SaveChanges();
        }

        private void SoftDelete(DbEntityEntry entry)
        {
            var deletedProperty = entry.Property("Deleted");
            if (deletedProperty == null)
                deletedProperty = entry.Property("IsDeleted");
            if (deletedProperty != null)
            {
                deletedProperty.CurrentValue = true;
                entry.State = EntityState.Modified;
            }
        }

    }
}
