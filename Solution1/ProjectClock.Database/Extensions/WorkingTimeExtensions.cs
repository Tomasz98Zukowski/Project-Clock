using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectClock.Database.Entities;

namespace ProjectClock.Database.Extensions
{
    public static class WorkingTimeExtensions
    {
        public static void StartWork(this WorkingTime workingTime)
        {
            workingTime.StartTime = DateTime.Now;
        }

        public static bool StopWork(this WorkingTime workingTime)
        {
            var endTime = DateTime.Now;

            try
            {
                if (workingTime.StartTime > endTime)
                {
                    throw new Exception($"End time of record cannot be sooner then start time");
                    return false;
                }
                else
                {
                    workingTime.EndTime = endTime;
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
