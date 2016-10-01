using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Plural
{
    public class Business
    {
        List<string> items;
        System.Timers.Timer timer;

        public Business()
        {
            timer = new System.Timers.Timer();
            items = new List<string>();
        }

        public void Start()
        {
            Console.WriteLine("Started");

            var plural = getUrls("plural.txt");

            for (int i = 0; i < plural.Count; i++)
            {
                plural[i] = plural[i].Replace(
                    "https://app.pluralsight.com/library/courses/", String.Empty);
            }

            items = plural.ToList();

            Console.WriteLine("Parsed file");

            timer.Interval = 5000;

            timer.Elapsed += timer_Elapsed;

            timer.Start();

            Console.ReadKey();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("timer elapsed: {0}", DateTime.Now.ToLongTimeString());

            timer.Stop();

            if (items.Count > 0)
            {
                Course course;
                var name = items[0];
                Console.WriteLine("Processing course: {0}", name);
                using (var webClient = new WebClient())
                {
                    try
                    {
                        var json = webClient.DownloadString(string.Format(Program.setting.Plural.CourseDataUrl, name));
                        course = JsonConvert.DeserializeObject<Course>(json);

                        var url = String.Format("{0}/Home/Index?course={1}", Program.setting.Plural.PluralTextFile, name);

                        Console.WriteLine("Course duration found: {0}; starting process", course.Duration);

                        Process.Start(url);

                        items = items.Where(item => item.ToLower() != name.ToLower()).ToList();
                        if (items.Count > 0)
                        {
                            var segments = course.Duration.Split(new char[] { ':' });
                            var hours = int.Parse(segments[0]);
                            var minutes = int.Parse(segments[1]);
                            var seconds = int.Parse(segments[2]);

                            var span = new TimeSpan(hours, minutes, seconds);

                            var nextInterval = Convert.ToDouble(Math.Floor(
                                Convert.ToDecimal(span.TotalMilliseconds) / Program.setting.Plural.DivisionFactor));

                            timer.Interval = nextInterval;

                            timer.Start();

                            var nextSpan = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(timer.Interval));

                            Console.WriteLine("Next process will be: {0}; in: {1} time", items[0], nextSpan.ToString());
                        }
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine("Error in connecting to pluralsite, We will retry in 1 minute");

                        timer.Interval = 60000;

                        timer.Elapsed += timer_Elapsed;

                        timer.Start();

                    }
                }
            }
        }

        List<String> getUrls(string fileName)
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
