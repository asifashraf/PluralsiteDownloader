using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Plural
{
    public static class settings
    {
        public static string plural
        {
            get
            {
                return ConfigurationManager.AppSettings["plural"].ToString();
            }
        }

        public static decimal division_factor
        {
            get
            {
                return Convert.ToDecimal( ConfigurationManager.AppSettings["division_factor"].ToString() );
            }
        }

        public static string COURSE_DATA_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["COURSE_DATA_URL"].ToString();
            }
        }
    }
}
