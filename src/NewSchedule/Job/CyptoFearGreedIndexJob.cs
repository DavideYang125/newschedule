using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NewSchedule.ToolKit;
using Newtonsoft.Json;
using Quartz;

namespace NewSchedule.Job
{
    [DisallowConcurrentExecution]
    public class CyptoFearGreedIndexJob : IJob
    {
        private readonly ILogger<CyptoFearGreedIndexJob> _logger;
        public CyptoFearGreedIndexJob(ILogger<CyptoFearGreedIndexJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("开始检测");
            var baseDir = Directory.GetCurrentDirectory();
            var url = "https://api.alternative.me/fng/";
            var logPath = Path.Combine(baseDir, "log.txt");// @"F:\Project\schedule\config\log.txt";
            var apm = DateTime.Now.ToString("tt", CultureInfo.InvariantCulture);
            var timeStr = DateTime.Now.ToString("yyyy年MM月dd日hh时 ") + apm;
            var timeSign = DateTime.Now.ToString("yyyy年MM月dd日 ") + apm;
            File.AppendAllText(logPath, "" + Environment.NewLine);
            var logInfos = File.ReadAllLines(logPath, Encoding.UTF8).ToList();
            if (logInfos.Contains(timeSign))
            {
                _logger.LogInformation ("数据已存在");
                return Task.CompletedTask;
            }
            //if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour <= 13) return Task.CompletedTask;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var result = client.GetStringAsync(url).Result.ToString();

                    dynamic obj = JsonConvert.DeserializeObject(result);
                    var value = obj.data[0].value.ToString();
                    var index = Convert.ToInt32(value);

                    var pre = "";
                    if (index > 70 || index < 40)
                        pre = "警报：";
                    if (index > 80 || index < 20) pre = "一级警报";

                    var mailMsg = pre + timeStr + $"，当前Cypto Index是【{index}】";

                    try
                    {
                        MailTool mt = new MailTool();
                        mt.Send(mailMsg);

                        _logger.LogInformation(mailMsg);
                        _logger.LogInformation("发送邮件成功");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        return Task.CompletedTask;
                    }

                    //var logWriter = new System.IO.StreamWriter(logPath);

                    File.AppendAllText(logPath, timeSign + Environment.NewLine);
                    File.AppendAllText(logPath, mailMsg + Environment.NewLine);

                    _logger.LogError("写入成功");

                    //logWriter.WriteLine(timeSign);
                    //logWriter.WriteLine(mailMsg);
                    //logWriter.Dispose();

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
                return Task.CompletedTask;
            }
        }
    }
}
