using Quartz;

namespace CarDealership.Infrastructure.JobSheduling
{
    public class JobSchedule
    {
        public Type JobType { get; set; }
        public ITrigger Trigger { get; set; }
        public JobSchedule(Type jobType, ITrigger trigger)
        {
            JobType = jobType;
            Trigger = trigger;
        }
    }
}
