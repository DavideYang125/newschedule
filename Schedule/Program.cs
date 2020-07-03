using System;
using Schedule.ScheduleCollection;
using Schedule.ToolKit;

namespace Schedule
{
    class Program
    {
        static void Main(string[] args)
        {
          
            var arg = args[0];
            switch (arg)
            {
                case "cyptoIndex":
                    CyptoFearGreedIndexSc.Run();
                    break;
            }
        }
    }
}
