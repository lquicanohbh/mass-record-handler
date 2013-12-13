using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Models
{
    public class BaseConfiguration
    {
        public string SystemCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public BaseConfiguration()
        {
            SystemCode = "LIVE";
            UserName = "ODBC";
            Password = "hotwire2011";
        }
    }
}
