using CarDealership.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Threading.Channels;

namespace CarDealership.DataAccess.Jobs
{
    public class CleanUpJob<T> : IJob where T : BaseEntity
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public CleanUpJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CarDealershipDbContext>();

                var thresholdDate = DateTime.UtcNow.AddYears(-5);

                var entitiesToDelete = dbContext.Set<T>()
                    .Where(e => e.IsDeleted && e.DeletedDate <= thresholdDate)
                    .ToList();

                if (entitiesToDelete != null && entitiesToDelete.Any())
                {
                    dbContext.Set<T>().RemoveRange(entitiesToDelete);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
