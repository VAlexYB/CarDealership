using CarDealership.Application.Auth;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace CarDealership.Infrastructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection ConfigureSupportingServices(this IServiceCollection services)
        {
            services.AddScoped<CleanUpJob<AutoConfigurationEntity>>();
            services.AddScoped<CleanUpJob<AutoModelEntity>>();
            services.AddScoped<CleanUpJob<BodyTypeEntity>>();
            services.AddScoped<CleanUpJob<BrandEntity>>();
            services.AddScoped<CleanUpJob<CarEntity>>();
            services.AddScoped<CleanUpJob<ColorEntity>>();
            services.AddScoped<CleanUpJob<CountryEntity>>();
            services.AddScoped<CleanUpJob<DealEntity>>();
            services.AddScoped<CleanUpJob<DriveTypeEntity>>();
            services.AddScoped<CleanUpJob<EngineEntity>>();
            services.AddScoped<CleanUpJob<EngineTypeEntity>>();
            services.AddScoped<CleanUpJob<EquipmentEntity>>();
            services.AddScoped<CleanUpJob<EquipmentFeatureEntity>>();
            services.AddScoped<CleanUpJob<FeatureEntity>>();
            services.AddScoped<CleanUpJob<OrderEntity>>();
            services.AddScoped<CleanUpJob<TransmissionTypeEntity>>();

            services.AddSingleton<IJobFactory, ScopedJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton(provider =>
            {
                var factory = provider.GetRequiredService<ISchedulerFactory>();
                var scheduler = factory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetRequiredService<IJobFactory>();
                return scheduler;
            });

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<AutoConfigurationEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_AutoConfiguration", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<AutoModelEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_AutoModel", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<BodyTypeEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_BodyType", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<BrandEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_Brand", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<CarEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_Car", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<ColorEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_Color", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                        //.WithIntervalInSeconds(20))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<CountryEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_Country", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<DealEntity>), 
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_Deal", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<DriveTypeEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_DriveType", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<EngineEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_Engine", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<EngineTypeEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_EngineType", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<EquipmentEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_Equipment", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<EquipmentFeatureEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_EquipmentFeature", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<FeatureEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_Feature", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<OrderEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_Order", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CleanUpJob<TransmissionTypeEntity>),
                trigger: TriggerBuilder.Create()
                    .WithIdentity("CleanUpJobTrigger_TransmissionType", "Group1")
                    .StartNow()
                    .WithSchedule(CalendarIntervalScheduleBuilder
                        .Create()
                        .WithIntervalInYears(5))
                    .Build()
            ));

            services.AddHostedService<QuartzHostedService>();


            services.AddScoped<IPasswordVerifier, PasswordVerifier>();
            services.AddScoped<JwtProvider>();

            return services;
        }
    }
}
