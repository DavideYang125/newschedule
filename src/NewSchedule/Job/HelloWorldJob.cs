using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace NewSchedule.Job
{
    [DisallowConcurrentExecution]
    public class HelloWorldJob : IJob
    {
        private readonly ILogger<HelloWorldJob> _logger;
        public HelloWorldJob(ILogger<HelloWorldJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")} Hello world!");
        
            //try
            //{
            //    File.AppendAllText(logPath, DateTime.Now.ToShortDateString() + Environment.NewLine);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogInformation(ex.ToString());
            //}
            ////var logPath = @"F:\Project\schedule\config\log.txt";
          
            //var con = File.ReadAllText(logPath);
            //_logger.LogInformation("content:" + con);
            return Task.CompletedTask;
        }
    }
}
