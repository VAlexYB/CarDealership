using Quartz;
using Quartz.Spi;
using Microsoft.Extensions.DependencyInjection;

namespace CarDealership.Infrastructure
{
    public class ScopedJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ScopedJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            }
            
        }

        public void ReturnJob(IJob job)
        {
            
        }
    }
}
