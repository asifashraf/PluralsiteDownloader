using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Consol.Safari
{
    public static class settings
    {
       
        public static string safari
        {
            get
            {
                return ConfigurationManager.AppSettings["safari"].ToString();
            }
        }


    }
}
