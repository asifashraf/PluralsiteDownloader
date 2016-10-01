using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace WebApp.Safari.Models
{
    public static class settings
    {
        public static string DownloadFolder { get { return ConfigurationManager.AppSettings["Download.Folder"]; } }
    }
}