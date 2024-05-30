using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;

namespace ShalehPrpject.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<NewPrice>  Prices { get; set; }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<Booking> Bookings { get; set; }

 


    }
}
