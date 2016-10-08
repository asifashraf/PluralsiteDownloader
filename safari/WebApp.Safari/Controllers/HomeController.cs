using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Safari.Models;
using System.IO;
using System.Net;
using System.Threading.Tasks;
namespace WebApp.Safari.Controllers
{
    public class HomeController : Controller
    {
        public JsonResult Info(VideoList model) 
        {
            removeDuplicateUrls(model);

            shortenNames(model);

            createCourseDirectory(model);

            checkFileStatus(model);

            allowCors();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private void shortenNames(VideoList model)
        {
            model.CourseName = limitName(model.CourseName, 80);
            foreach (var item in model.Items)
            {
                item.Title = limitName(item.Title, 60);
            }
        }

        private string limitName(string name, int limit)
        {
            if (name.Length > limit)
            {
                name = name.Substring(0, limit) + "(more)";
            }
            
            return name;
        }

        private void removeDuplicateUrls(VideoList model)
        {
            var urls = model.Items.Select(item => item.Url).Distinct().ToList();
            var list = new List<VideoInfo>();
            for (int i = 0; i < urls.Count(); i++)
            {
                var first = model.Items.Where(item => item.Url == urls[i]).First();
                list.Add(first);
            }
            model.Items = list.ToList();
        }

        public JsonResult IsDownloaded(VideoInfo model)
        {
            var busy = HttpContext.Cache["busy_" + model.CourseTitle];
            var working = false;
            if (busy != null)
            {
                if (busy.ToString() == "1")
                {
                    working = true;
                }
            }
            allowCors();
            var fileInfo = new FileInfo(model.FilePath);
            if (!fileInfo.Exists)
            {
                model.Done = false;
                model.Length = 0;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            
            model.Done = fileInfo.Exists && fileInfo.Length > 100 && !working;
            model.Length = fileInfo.Length;
            HttpContext.Cache[model.Url] = model;

            

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public  JsonResult Download(Download model)
        {
            var info = HttpContext.Cache[model.Page] as VideoInfo;            
            if (info != null)
            {
                var webClient = new WebClient();
                HttpContext.Cache["busy_" + info.CourseTitle] = "1";
                bool alreadyDownloading = false;
                try
                {
                    webClient.DownloadFile(new Uri(model.Url), info.FilePath);
                }
                catch(Exception error)
                {
                    if (error.InnerException.Message.Contains("The process cannot access the file"))
                    {
                        alreadyDownloading = true;
                    }
                }
                finally
                {
                    if (!alreadyDownloading)
                    {
                        HttpContext.Cache["busy_" + info.CourseTitle] = "0";
                    }
                }               
                
                model.Downloaded = true;
            }
            else
            {
                model.Downloaded = false;
            }

            allowCors();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Download_(Download model)
        {
            var info = HttpContext.Cache[model.Page] as VideoInfo;  
            // 3- download the video and report progress back.
            int receivedBytes = 0;
            int totalBytes = 0;
            var videoFileName = info.FileName;
            var client = new WebClient();

            using (var stream = await client.OpenReadTaskAsync(model.Url))
            {
                byte[] buffer = new byte[8192];
                totalBytes = Int32.Parse(client.ResponseHeaders[HttpResponseHeader.ContentLength]);
                using (var fileStream = System.IO.File.OpenWrite(info.FilePath))
                {
                    for (; ; )
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            await Task.Yield();
                            break;
                        }

                        receivedBytes += bytesRead;
                        fileStream.Write(buffer, 0, bytesRead);
                    }
                }
            }

            // 4- save the video file.

            return Json(new 
            {
                Id = info.FileName,
                BytesReceived = receivedBytes,
                FileName = videoFileName,
                TotalBytes = totalBytes,
                IsDownloading = false
            });
        }

        public void createCourseDirectory(VideoList model)
        {                      
            var rootDir = new DirectoryInfo(settings.DownloadFolder);
            var courseDirName = cleanName(model.CourseName);
            var courseDirPath = MvcApplication.settings.Safari.SafariDownloadDirectory;
            var courseDir = new DirectoryInfo( courseDirPath );
            #region Create directories
            if (!rootDir.Exists)
            {
                rootDir.Create();
            }

            if (!courseDir.Exists)
            {
                courseDir.Create();
            }
            #endregion

            //set server info
            model.ServerInfo.Root = settings.DownloadFolder;
            model.ServerInfo.Folder = courseDirPath;
            model.ServerInfo.FolderName = courseDirName;
        }

        public void checkFileStatus(VideoList model)
        {
            var countFiles = model.Items.Count;
            for (int i = 0; i < countFiles; i++)
            {
                var item = model.Items[i];
                var fileNumber = threeDigits(i);
                item.FileName = string.Format("{0}__{1}.mp4", fileNumber, cleanName(item.Title));
                var filePath = string.Format("{0}{1}", model.ServerInfo.Folder, item.FileName);
                item.FilePath = filePath;
                var fileInfo = new FileInfo(filePath);
                item.Done = fileInfo.Exists;

                HttpContext.Cache[item.Url] = item;

            }
        }

        string cleanName(string name)
        {
            var chars = new string[] { ":","/", @"\" , "?", "'", "\"",
            "#", "%", "&", "*", "+", ",", "@", "!" , "^", ";", "{", "}" };

            for (int i = 0; i < chars.Count(); i++)
            {
                name = name.Replace(chars[i], "-");
            }

            return name;
        }

        string threeDigits(int num)
        {
            if (num.ToString().Length == 1)
                return "00" + num.ToString();

            if (num.ToString().Length == 2)
                return "0" + num.ToString();

            return num.ToString();
        }

        void allowCors()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        #region Crap
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #endregion
    }
}

