using AnimalMarketDal.Dal;
using System;
using System.Linq;

namespace ConsoleApp.Plural
{
    class Program
    {
        public static Setting setting = new Setting();
        static void Main(string[] args)
        {
            var downloader = new Business();
            downloader.Start();


            Console.ReadKey();
        }
    }
}
