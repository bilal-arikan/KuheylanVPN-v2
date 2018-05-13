using System;
using System.Net;
using HtmlAgilityPack;

namespace Kuheylan
{
    public class VPN : VpnListViewItem
    {
        

        public static VPN ParseFromJson(int index,string fullText)
        {
            VPN temp = new VPN();
            temp.Index = index;
            temp = Newtonsoft.Json.JsonConvert.DeserializeObject<VPN>(fullText);

            if (temp.xl2tp == "")
                temp.Protocol = "pptp";
            else
                temp.Protocol = "l2tp";

            temp.ControlSetValues();
            temp.MouseHover += delegate { temp.BackColor = temp.isInited ? Globals.vpnInitedHover : Globals.vpnUnInitedHover; };
            temp.MouseLeave += delegate { temp.BackColor = temp.isInited ? Globals.vpnInited : Globals.vpnUnInited; };
            return temp;
        }

        public class Locat
        {
            public string @as { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string countryCode { get; set; }
            public string isp { get; set; }
            public double lat { get; set; }
            public double lon { get; set; }
            public string org { get; set; }
            public string query { get; set; }
            public string region { get; set; }
            public string regionName { get; set; }
            public string status { get; set; }
            public string timezone { get; set; }
            public string zip { get; set; }
        }

        public bool isInited = false;
        public int Index = -1;
        public string Ip;
        public string User;// { get { return xuser; }}
        public string Pass;
        public string Protocol;

        public int nochange { get; set; }
        public Locat locat { get; set; }
        public string website { get; set; }
        public string xaddr { get; set; }
        public string xl2tp { get; set; }
        public string xpasw { get; set; }
        public string xuser { get; set; }

        public void InitValues()
        {
            //--------------------------------------------
            if (nochange == 1)
            {
                User = xuser;
                Pass = xpasw;
                Ip = xaddr;
            }
            else
            {
                GetValuesFromNet();
            }
            isInited = true;
            ControlSetValues();
            //--------------------------------------------
        }

        void GetValuesFromNet()
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            /*WebClient cli = new WebClient();
            cli.Headers.Add("user-agent", Globals.useragent);
            string strResult = cli.DownloadString(website);*/


            string strResult;
            WebRequest objRequest = WebRequest.Create(website);
            WebResponse objResponse = objRequest.GetResponse();
            using (var sr = new System.IO.StreamReader(objResponse.GetResponseStream()))
            {
                strResult = sr.ReadToEnd();
                sr.Close();
            }

            doc.LoadHtml(strResult);
            /*Console.WriteLine("--> Web sayfasi indirildi");

            Console.WriteLine(website);
            Console.WriteLine(xaddr.Trim());
            Console.WriteLine(xuser.Trim());
            Console.WriteLine(xpasw.Trim());*/

            HtmlNode nodeAddr = doc.DocumentNode.SelectSingleNode(xaddr);
            HtmlNode nodeUser = doc.DocumentNode.SelectSingleNode(xuser);
            HtmlNode nodePasw = doc.DocumentNode.SelectSingleNode(xpasw);

            Ip = nodeAddr.InnerText.Trim();
            User = nodeUser.InnerText.Trim();
            Pass = nodePasw.InnerText.Trim();

            /*Console.WriteLine(Ip);
            Console.WriteLine(User);
            Console.WriteLine(Pass);
            Console.WriteLine(Protocol);
            */

        }

        public void ControlSetValues()
        {
            if (isInited)
            {
                BackColor = Globals.vpnInited;
            }
            else
            {
                BackColor = Globals.vpnUnInited;
            }
            Enabled = isInited;
            linkLabelSite.Text = website;
            //labelIndex.Text = Index+"";
            labelIp.Text = Ip;
            labelUser.Text = User;
            labelPass.Text = Pass;
            labelProt.Text = Protocol;
            labelCountry.Text = locat.country;
            labelCity.Text = locat.city;
            labelRegion.Text = locat.region;
        }
    }
}
