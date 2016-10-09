using System;

namespace ConsoleApp.Plural
{
    class Program
    {
        public static Setting setting = Setting.Load(SettingClientType.NonWeb);
        static void Main(string[] args)
        {
            var downloader = new Business();
            downloader.Start();


            Console.ReadKey();
        }
    }
}
