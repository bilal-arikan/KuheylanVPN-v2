using System.Drawing;

namespace Kuheylan
{
    public static class Globals
    {
        public static string vpnIsim = "KÜHEYLAN-2";
        public static string jsonFileName = "VpnList.json";
        public static string useragent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
        
        public enum State
        {
            NewlyOpen,
            InfoGathering,
            InfoGatheringSuccess,
            InfoGatheringEnded,
            Connecting,
            Connected,
            Disconnecting,
            Disconnected,
            ConnError,
            Closing
        }


        public static Color vpnInited = Color.DarkOrange;
        public static Color vpnInitedHover = Color.SandyBrown;
        public static Color vpnUnInited = SystemColors.WindowFrame;
        public static Color vpnUnInitedHover = Color.Red;

    }
}
