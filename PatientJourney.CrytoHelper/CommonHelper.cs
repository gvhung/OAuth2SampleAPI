using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientJourney.CrytoHelper
{
    public static class CommonHelper
    {
        public static string UserName(string user511Id)
        {
            string username = user511Id;
            username = (username.Split('\\').Length > 1) ? username.Split('\\')[1] : username;
            if (username == "")
                username = Environment.UserName.ToString();
            //username = "ALAGAKX";                   
            return username;
        }
    }
}
