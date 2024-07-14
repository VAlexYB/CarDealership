using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace CarDealership.Infrastructure.JobSheduling
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _schedules;
        private IScheduler _scheduler;

        public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, IEnumerable<JobSchedule> schedules)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _schedules = schedules;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler();
            _scheduler.JobFactory = _jobFactory;

            foreach (var schedule in _schedules)
            {
                var job = CreateJob(schedule);
                await _scheduler.ScheduleJob(job, schedule.Trigger, cancellationToken);
            }

            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler?.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJob(JobSchedule schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }
    }
}
