using System.Data.Entity;

namespace REA.Models.Context
{
    public class REAContext : DbContext
    {
        public REAContext() : base("DbConnection")
        { }

        public DbSet<Deal> Deals { get; set; }
        public DbSet<EmployeesPhoto> EmployeesPhotos { get; set; }
        public DbSet<RealtyPhoto> RealtyPhotos { get; set; }
        public DbSet<TypeOfDeal> TypesOfDeals { get; set; }
        public DbSet<TypeOfRealty> TypesOfRealty { get; set; }
        public DbSet<Realty> Realty { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
