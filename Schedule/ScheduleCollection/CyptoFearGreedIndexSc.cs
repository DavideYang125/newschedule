using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Schedule.ToolKit;

namespace Schedule.ScheduleCollection
{
    public class CyptoFearGreedIndexSc : BaseSchedule
    {
        public static void Run()
        {
            var url = "https://api.alternative.me/fng/";
            var logPath = @"F:\Project\schedule\config\log.txt";
            var apm = DateTime.Now.ToString("tt", CultureInfo.InvariantCulture);
            var timeStr = DateTime.Now.ToString("yyyy年MM月dd日hh时 ") + apm;
            var timeSign = DateTime.Now.ToString("yyyy年MM月dd日 ") + apm;
            var logInfos = File.ReadAllLines(logPath, Encoding.UTF8).ToList();
            if (logInfos.Contains(timeSign)) return;
            if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour <= 13) return;

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

                    MailTool mt = new MailTool();
                    mt.Send(mailMsg);

                    //var logWriter = new System.IO.StreamWriter(logPath);

                    File.AppendAllText(logPath, timeSign+ Environment.NewLine);
                    File.AppendAllText(logPath, mailMsg+ Environment.NewLine);

                    //logWriter.WriteLine(timeSign);
                    //logWriter.WriteLine(mailMsg);
                    //logWriter.Dispose();

                }
                catch (Exception ex)
                {

                }

            }
        }
    }
}
