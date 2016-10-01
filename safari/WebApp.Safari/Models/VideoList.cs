using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Safari.Models
{
    public class VideoList
    {
        public VideoList()
        {
            this.Items = new List<VideoInfo>();
            this.ServerInfo = new ServerInfo();
        }

        public string CourseName { get; set; }

        public List<VideoInfo> Items { get; set; }

        public ServerInfo ServerInfo { get; set; }
    }


    public class ServerInfo
    {
        public string Root { get; set; }
        public string Folder { get; set; }
        public string FolderName { get; set; }
    }
}