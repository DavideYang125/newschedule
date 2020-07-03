using Microsoft.Extensions.DependencyInjection;
using NewSchedule.Job;

namespace NewSchedule.Base
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJob(this IServiceCollection services)
        {
            //services.AddSingleton(new JobSchedule(
            //   jobType: typeof(HelloWorldJob),
            //   cronExpression: "0/5 * * * * ?")); // run every 5 seconds


            services.AddSingleton(new JobSchedule(
              jobType: typeof(CyptoFearGreedIndexJob),
              cronExpression: "6 0/5 * * * ?")); // run every 5min               
        }
    }
}
