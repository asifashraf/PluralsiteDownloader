using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Safari.Models
{
    public class VideoInfo
    {
        public VideoInfo()
        {
            this._span = new TimeSpan(0, 0, 0);
        }

        public string Title { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Length { get; set; }
        public string Time { get; set; }
        public string CourseTitle { get; set; }
        private TimeSpan _span;
        public TimeSpan Span
        {
            get
            {
                if (_span.TotalMilliseconds == 0)
                {
                    var timeSegments = this.Time.Split(new char[] { ':' });
                    var hour = int.Parse(timeSegments[0]);
                    var minute = int.Parse(timeSegments[1]);
                    var second = int.Parse(timeSegments[2]);
                    _span = new TimeSpan(hour, minute, second);
                }

                return _span;
            }
        }

        public string Url { get; set; }

        public bool Done { get; set; }
    }
}
