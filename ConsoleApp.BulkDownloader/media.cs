using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.BulkDownloader
{
    public static class media
    {


        /*var duration = media.getDuration(@"G:\movies_best\Catch Me If You Can (2002) [1080p]\Catch.Me.If.You.Can.2002.1080p.BluRay.x264.YIFY.mp4");

         Console.WriteLine(duration);*/



        public static Info getInfo(String filePath)
        {
            var ffprobe = String.Format("{0}\\ffmpeg\\ffprobe.exe", Environment.CurrentDirectory);

            var output = command.run(ffprobe,
                String.Format("-v quiet -print_format json -show_format -show_streams \"{0}\"", filePath));
            var info = JsonConvert.DeserializeObject<Info>(output);
            return info;
        }

        public static TimeSpan getDuration(String filePath)
        {
            var info = getInfo(filePath);
            var span = new TimeSpan(0, 0, Convert.ToInt32(Math.Round(Convert.ToDouble(info.format.duration))));
            return span;
        }
    }
}
