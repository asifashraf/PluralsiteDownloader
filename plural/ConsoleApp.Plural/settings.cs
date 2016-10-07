using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Setting
{
    /*G:\_app_downloader\
    \config\
    \db\
    \tools\
    \tools\ffmpeg\*/

    public SettingCommon Common { get; set; }

    public SettingPlural Plural { get; set; }
    
    public Setting()
    {
        var machine = Environment.MachineName;

        var dir = Environment.CurrentDirectory;

        var configName = string.Format("\\\\{0}\\{1}", machine, dir);

        var rootDir = ConfigurationManager.AppSettings[configName].ToString();

        //Set values
        this.SettingDirectory = rootDir;

        var fileCommon = string.Format(@"{0}config\common.json", this.SettingDirectory);

        var jsonCommon = File.ReadAllText(fileCommon);

        this.Common = JsonConvert.DeserializeObject<SettingCommon>(jsonCommon);

        var filePlural = string.Format(@"{0}config\ConsoleApp.Plural.json", this.SettingDirectory); 

        var jsonPlural = File.ReadAllText(filePlural);

        this.Plural = JsonConvert.DeserializeObject<SettingPlural>(jsonPlural);

    }

    public string SettingDirectory { get; set; }


}

class SettingPlural
{
    public string PluralTextFile { get; set; }

    public decimal DivisionFactor { get; set; }

    public string CourseDataUrl { get; set; }
    public string DownloadingHost { get; set; }
}