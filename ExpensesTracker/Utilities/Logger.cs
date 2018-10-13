using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Utilities
{
    public class Logger
    {
        private string path;

        public Logger(string p)
        {
            path = p;
        }

        public void WriteEvent(string[] msg)
        {
            string eventFilePath = path + @"Event logs\";
            string textPath = string.Empty;
            DateTime now = DateTime.Now;

            if (!Directory.Exists(eventFilePath))
            {
                Directory.CreateDirectory(eventFilePath);
            }

            textPath = eventFilePath + $"Expenses Tracker event log {now.ToString("ddMMyyyy")}.txt";

            using (StreamWriter sw = new StreamWriter(textPath, true))
            {
                sw.WriteLine($"Timestamp: {now.ToString("ddMMyyy hh:mm:ss")}");

                string message = string.Empty;
                foreach (string m in msg)
                {
                    message += $"{{{m}}}";
                }
                sw.WriteLine(message);
                sw.WriteLine("------------------------------------------------------------------");
                sw.WriteLine();
                sw.WriteLine();
                sw.Flush();
                sw.Close();
            }

        }

        public void WriteError(string[] msg)
        {
            string errorFilePath = path + @"Error logs\";
            string textPath = string.Empty;
            DateTime now = DateTime.Now;

            if (!Directory.Exists(errorFilePath))
            {
                Directory.CreateDirectory(errorFilePath);
            }

            textPath = errorFilePath + $"Expenses Tracker error log {now.ToString("ddMMyyyy")}.txt";

            using (StreamWriter sw = new StreamWriter(textPath, true))
            {
                sw.WriteLine($"Timestamp: {now.ToString("ddMMyyy hh:mm:ss")}");
                string message = string.Empty;
                foreach (string m in msg)
                {
                    message += $"{{{m}}}";
                }
                sw.WriteLine(message);
                sw.WriteLine("------------------------------------------------------------------");
                sw.WriteLine();
                sw.Flush();
                sw.Close();
            }
        }
    }
}
