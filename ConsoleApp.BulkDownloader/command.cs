using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
namespace ConsoleApp.BulkDownloader
{
    public static class command
    {
        //var output = command.run(ffprobe,
          //      String.Format("-v quiet -print_format json -show_format -show_streams \"{0}\"", filePath));


        public static string run(string filePath, string args)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = filePath;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            if (!proc.Start())
            {
                Console.WriteLine("Error starting");
                return string.Empty;
            }

            StreamReader reader = proc.StandardOutput;
            string line;
            StringBuilder sb = new StringBuilder();
            while ((line = reader.ReadLine()) != null)
            {
                sb.AppendLine(line);
            }
            proc.Close();
            return sb.ToString();
        }
    }
}
