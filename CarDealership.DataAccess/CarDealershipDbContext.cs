using CarDealership.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.DataAccess
{
    public class CarDealershipDbContext : DbContext
    {
        public CarDealershipDbContext(DbContextOptions<CarDealershipDbContext> options) : base(options)
        {
        }

        public DbSet<AutoModelEntity> AutoModels { get; set; }
    }
}
