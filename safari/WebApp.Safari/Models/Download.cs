using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Safari.Models
{
    public class Download
    {
        public string Url { get; set; }
        public string Page { get; set; }
        public bool Downloaded { get; set; }
    }
}
