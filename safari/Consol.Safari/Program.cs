using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Consol.Safari
{
    class Program
    {

        public static Setting setting = Setting.Load(SettingClientType.NonWeb);

        static List<string> items = new List<string>();
        static System.Timers.Timer timer = new System.Timers.Timer();
        static void Main(string[] args)
        {
            Console.WriteLine("Started");

            var lines = getUrls("safari.txt");

            items = lines.ToList();

            Console.WriteLine("Parsed file");

            timer.Interval = 5000;

            timer.Elapsed += timer_Elapsed;

            timer.Start();

            Console.ReadKey();
        }

        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("timer elapsed: {0}", DateTime.Now.ToLongTimeString());

            timer.Stop();

            if (items.Count > 0)
            {
                var current = items[0];
                Console.WriteLine("Processing course: {0}", current);
                Process.Start(current);
                items = items.Where(item => item.ToLower() != current.ToLower()).ToList();
                if (items.Count > 0)
                {
                    var hours = 3;
                    var minutes = 0;
                    var seconds = 0;

                    var span = new TimeSpan(hours, minutes, seconds);

                    var nextInterval = span.TotalMilliseconds;

                    timer.Interval = nextInterval;

                    timer.Start();

                    Console.WriteLine("Next process will be: {0}; in: {1} time", items[0], span.ToString());
                }
            }
        }

        static List<String> getUrls(string fileName)
        {
            var dir = String.Format("{0}\\{1}", Environment.CurrentDirectory, fileName);
            var lines = File.ReadAllLines(dir);
            List<string> http = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].ToLower().Trim();
                if (line.StartsWith("http") && line.Contains("://"))
                {
                    http.Add(lines[i].Trim());
                }
            }
            return http;
        }
    }
}
