using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AnimalMarketDal.DomainModel;

namespace AnimalMarketDal.Dal
{

    //sqlite mysql workbench export tool
    //https://github.com/tatsushid/mysql-wb-exportsqlite

    public class AnimalContext : DbContext
    {
        public AnimalContext() 
        {
            // Turn off the Migrations, (NOT a code first Db)
            //Database.SetInitializer<AnimalContext>(null);           

        }

        //public DbSet<AnimalType> AnimalTypes { get; set; }
       // public DbSet<EventData> EventDataValues { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Database does not pluralize table names
           // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}