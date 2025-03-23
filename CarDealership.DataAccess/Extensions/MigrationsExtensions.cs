using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarDealership.DataAccess.Extensions
{
    public static class MigrationsExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<CarDealershipDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
