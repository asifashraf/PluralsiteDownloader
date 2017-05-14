using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Setting
{
    /*G:\_app_downloader\
    \config\
    \db\
    \tools\
    \tools\ffmpeg\*/

    public SettingCommon Common { get; set; }

    public SettingPlural Plural { get; set; }

    public SettingSafari Safari { get; set; }

    public static Setting Load(SettingClientType type)
    {
        var machine = Environment.MachineName;

        var dir = Environment.CurrentDirectory;

        var configName = string.Format("\\\\{0}\\{1}\\", machine, dir);

        if (type == SettingClientType.Web)
        {
            var webRoot = System.Web.HttpContext.Current.Request.Url.Host 
                + ":" + System.Web.HttpContext.Current.Request.Url.Port;
            configName = string.Format("\\\\{0}\\{1}", machine, webRoot);
        }   

        var rootDir = ConfigurationManager.AppSettings[configName].ToString();
            
        

        //Set values
        SettingDirectory = rootDir;

        var fileCommon = string.Format(@"{0}config\common.json", SettingDirectory);

        var jsonCommon = File.ReadAllText(fileCommon);

        Setting sett = JsonConvert.DeserializeObject<Setting>(jsonCommon);

        return sett;
        
    }

    public static string SettingDirectory = "";
    
}

public class SettingCommon
{
    public string FFMPegDirectory { get; set; }
    public string DbDirectory { get; set; }
}


public class SettingPlural
{
    public string PluralTextFile { get; set; }

    public decimal DivisionFactor { get; set; }

    public string CourseDataUrl { get; set; }
    public string DownloadingHost { get; set; }
    public string PluralDownloadDirectory { get; set; }
}

public class SettingSafari
{
    public string SafariDownloadDirectory { get; set; }
}

public enum SettingClientType
{
    Web,
    NonWeb
}