using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kuheylan
{
    class VPN
    {
        public VPN()
        {
            Ip = "";
            User = "";
            Pass = "";
            Protocol = "";
        }

        public string Ip;
        public string User;
        public string Pass;
        public string Protocol;

        public void SetUserPass(string user,string pass){
            User = user;
            Pass = pass;
        }
    }
}
