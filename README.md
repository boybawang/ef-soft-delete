ef-soft-delete
==============
EF Soft Delete is an implementation of Entity Framework's DbContext specifically designed to perform  soft or logical deletes to entities by requiring to have a "Deleted" or "IsDeleted" column in every table.

The class also initiales the DbContext in such a way that only those columns which have their "Deleted" column set to false are returned in every DbSet.

Usage:
You must subclass DbContextBase and define your DBSets in that subclass either in Code First or Database First scenarios.
One caveat, the DbSets should be typed as IDbSet<>.
=================
namespace MyApp.Data
{
        using Juppir.EfSoftDeleteDbContext;
        public class MyAppDbContext : DbContextbase
        {
                static MyAppDbContext()
                {
                    Database.SetInitializer<DbContextBase>(null);
                }
                public MyAppDbContext(string connectionStringName)
                    : base(connectionStringName)
                {
                    Contacts = new FilteredDbSet<Contact>(this, c => c.Deleted == false, null);
                }
                
                public IDbSet<Customer> Customers { get; set; }
                
                protected override void OnModelCreating(DbModelBuilder modelBuilder)
                {
                  modelBuilder.Configurations.Add(new CustomerMap());
                }
        }
}

Also included in the solution are the modified EF Reverse Engineer Code First text templates to generate the same code above when using the EF Power Tools.
