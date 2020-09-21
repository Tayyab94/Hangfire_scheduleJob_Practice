using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire_scheduleJob_Practice
{
    public class JobPrint : IJobPrint
    {
        public void HangfirePrintJob()
        {
            Console.WriteLine("This is Hanfire Job Print Function");
        }
    }
}
