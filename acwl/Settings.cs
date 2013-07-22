using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace acwl
{
    public class Settings
    {
        public ushort Port
        {
            get
            {
                return Convert.ToUInt16(ConfigurationManager.AppSettings["Port"]);
            }
        }
        public string ProgramPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ProgramPath"];
            }
        }
    }
}
