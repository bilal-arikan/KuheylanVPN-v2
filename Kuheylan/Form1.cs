using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DotRas;
using Microsoft.Win32;
using System.Collections;
using DLL;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using HtmlAgilityPack;

namespace Kuheylan
{
    public partial class Form1 : Form
    {

        string vpnIsim = "---KÜHEYLAN---";
        String adres = "http:// Your_Server_Address /";
        RasHandle handle;
        VPN[] vpnler = new VPN[12];
        Color c1 = Color.Chartreuse; //connected
        Color c2 = Color.DarkOrange; //disconnected
        List<Icon> iconlar = new List<Icon>();
        int i;
        bool iconDon;

        private delegate void ConnectionStartE(int hangiVpn);
        private event ConnectionStartE ConnectionStart;
        private delegate void DisconnectionStartE();
        private event DisconnectionStartE DisconnectionStart;
        private delegate void IconYukleE();
        private event IconYukleE IconYukle;
        private delegate void IconDegistirE();
        private event IconDegistirE IconDegistir;

        public Form1()
        {
            InitializeComponent();
            this.ConnectionStart += Form1_ConnectionStart;
            this.DisconnectionStart += Form1_DisconnectionStart;
            this.IconYukle += Form1_IconYukle;
            this.IconDegistir += Form1_IconDegistir;

            this.IconYukle.BeginInvoke(new AsyncCallback(delegate {}), null);
            iconDon = false;

            notifyIcon1.Text = vpnIsim;
            watcher1.EnableRaisingEvents = true;
            rasPhoneBook1.Open(true);

            RenkDegistir(c2,false);
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            Console.WriteLine("### PROGRAM BASLADI ###");
            notifyIcon1.Visible = true;

            //this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            //this.Top = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - this.Height;
        }
        //---------------------------------------------------------------------------------------        
        void Form1_ConnectionStart(int hangiVpn)
        {
            iconDon = true;
            this.IconDegistir.BeginInvoke(new AsyncCallback(delegate { }), null);
            BilgilendirmeDegistir(30, false); //Bilgi
            if (VpnListesiniYenile(adres + "vpnler.php") == 1)
            {
                //BilgilendirmeDegistir(40, false); //Bilgi
                disconnectButton.Enabled = true;
                if (Create(vpnler[hangiVpn]) == 1)
                {
                    BilgilendirmeDegistir(50, false); //Bilgi
                    if (Connect(vpnler[hangiVpn]) == 1)
                    {
                        watcherBeklenmedik.EnableRaisingEvents = true;
                        BilgilendirmeDegistir(59, true); //Bilgi
                        return;
                    }
                    else
                    {
                        Console.WriteLine(">>> Baglanma Hatası");
                    }
                }
                else
                {
                    Console.WriteLine(">>> Vpn Olusturma Hatası");
                }
            }
            else
            {
                Console.WriteLine(">>> Liste Alma Hatası");
            }
            ConnectionError(90, true);
        }
        void Form1_DisconnectionStart()
        {
            iconDon = true;
            this.IconDegistir.BeginInvoke(new AsyncCallback(delegate { }), null);
            watcherBeklenmedik.EnableRaisingEvents = false;
            BilgilendirmeDegistir(70, false); //Bilgi

            Disconnect();
            int cevap1 = Remove();
            if (cevap1 == 1)
            {
                BilgilendirmeDegistir(71, true); //Bilgi
            }
            else
            {
                GorunumDegistir(true);
            }
        }
        private void rasDialer1_StateChanged(object sender, StateChangedEventArgs e)
        {
            switch (e.State)
            {
                case RasConnectionState.OpenPort:
                    progressBar1.Value = 20;
                    break;
                case RasConnectionState.PortOpened:
                    progressBar1.Value = 30;
                    break;
                case RasConnectionState.ConnectDevice:
                    progressBar1.Value = 40;
                    break;
                case RasConnectionState.DeviceConnected:
                    progressBar1.Value = 50;
                    break;
                case RasConnectionState.AllDevicesConnected:
                    progressBar1.Value = 55;
                    break;
                case RasConnectionState.Authenticate:
                    progressBar1.Value = 60;
                    break;
                case RasConnectionState.AuthProject:
                    progressBar1.Value = 65;
                    break;
                case RasConnectionState.Projected:
                    progressBar1.Value = 70;
                    break;
                case RasConnectionState.Authenticated:
                    progressBar1.Value = 80;
                    break;
                case RasConnectionState.ApplySettings:
                    progressBar1.Value = 90;
                    break;
            }
            Console.WriteLine("###:" + e.State.ToString());
        }
        //---------------------------------------------------------------------------------------
        int Create(VPN vpn)
        {
            try
            {
                Console.WriteLine(rasPhoneBook1.Path);
                if (RasEntry.Exists(vpnIsim, rasPhoneBook1.Path))
                {
                    Console.WriteLine("--> Önceki Vpn Siliniyor...");
                    Disconnect();
                    Remove();
                }
                RasEntry entry = RasEntry.CreateVpnEntry(
                    vpnIsim, 
                    vpn.Ip, 
                    RasVpnStrategy.PptpFirst,
                    RasDevice.GetDeviceByName(vpn.Protocol, RasDeviceType.Vpn));
                this.rasPhoneBook1.Entries.Add(entry);
                Console.WriteLine("--> Vpn Olusturuldu");
                return 1;
            }
            #region HATALAR
            catch(ArgumentNullException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 904;
            }
            catch (ArgumentException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 905;
            }
            catch (NullReferenceException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 901;
            }
            #endregion
        }
        int Connect(VPN vpn)
        {
            try
            {
                this.rasDialer1.EntryName = vpnIsim;
                this.rasDialer1.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User); //yada alluser

                this.rasDialer1.Credentials = new System.Net.NetworkCredential(vpn.User, vpn.Pass);
                this.handle = this.rasDialer1.DialAsync();
                while (rasDialer1.IsBusy)
                {
                    System.Threading.Thread.Sleep(100);
                }
                Console.WriteLine("--> Baglanti kuruldu");

                if (RasConnection.GetActiveConnectionByName(vpnIsim, rasPhoneBook1.Path) == null)
                {
                    Console.WriteLine("--> Sunucu Kaynaklı Bağlantı Hatası");
                    return 901;
                }
                return 1;
            }
            #region HATALAR
            catch (InvalidOperationException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 903;
            }
            catch (ArgumentNullException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 904;
            }
            catch (ArgumentException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 905;
            }
            catch (NullReferenceException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 901;
            }
            #endregion
        }
        int Remove()
        {
            try
            {
                if (RasEntry.Exists(vpnIsim, rasPhoneBook1.Path))
                    this.rasPhoneBook1.Entries.Remove(vpnIsim);
                Console.WriteLine("--> Vpn Silindi");
                return 1;
            }
            #region HATALAR
            catch (FileNotFoundException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 906;
            }
            catch (ArgumentNullException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 904;
            }
            catch (ArgumentException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 905;
            }
            catch (NullReferenceException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 901;
            }
            #endregion
        }
        int Disconnect()
        {
            try
            {
                //rasPhoneBook1.Open(); 
                if (this.rasDialer1.IsBusy)
                {
                    // The connection attempt has not been completed, cancel the attempt.
                    this.rasDialer1.DialAsyncCancel();
                    Console.WriteLine("--> Bağlanma İptal Edildi");
                    ConnectionError(73,false);
                }
                else
                {
                    // The connection attempt has completed, attempt to find the connection in the active connections.
                    RasConnection connection = RasConnection.GetActiveConnectionByName(vpnIsim, rasPhoneBook1.Path);
                    if (connection != null)
                    {
                        // The connection has been found, disconnect it.
                        connection.HangUp();
                        Console.WriteLine("--> Bağlantı kapatıldı");
                    }
                }
                return 1;
            }
            #region HATALAR
            catch (FileNotFoundException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 906;
            }
            catch (ArgumentNullException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 904;
            }
            catch (ArgumentException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 905;
            }
            catch (NullReferenceException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 901;
            }
            #endregion 
        }
        int VpnListesiniYenile(string url)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml((new WebClient()).DownloadString(url));
                Console.WriteLine("\n--> Web sayfasi indirildi"/* + respString.ToString()*/);

                HtmlNodeCollection collection = doc.DocumentNode.SelectNodes("//tbody");
                if (collection != null)
                {
                    int colN;
                    foreach (HtmlNode table in collection) //birden fazla tablo icin
                    {
                        Console.WriteLine("--> Tablo Bulundu: " + table.Id);
                        colN = 0;
                        foreach (HtmlNode row in table.SelectNodes("tr"))
                        {
                            HtmlNodeCollection cell = row.SelectNodes("td");
                            vpnler[colN] = new VPN();
                            vpnler[colN].Ip = cell[0].InnerText.Trim();
                            vpnler[colN].Protocol = cell[1].InnerText.Trim();
                            vpnler[colN].User = cell[2].InnerText.Trim();
                            vpnler[colN].Pass = cell[3].InnerText.Trim();

                            Console.WriteLine(vpnler[colN].Ip + "-" + vpnler[colN].Protocol + "-" + vpnler[colN].User + "-" + vpnler[colN].Pass);

                            colN++;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("--> Liste ALINAMADI !!!");
                    return 0;
                }
                Console.WriteLine("--> Liste ALINDI.");
                return 1;
            }
            #region HATALAR
            catch (TimeoutException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 907;
            }
            catch (WebException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return 902;
            }
            #endregion
        }
        //---------------------------------------------------------------------------------------
        private void connectButton_Click(object sender, EventArgs e)
        {
                int hangisi = 0;
                if (radioButton1.Checked == true)
                    hangisi = 0;
                else if (radioButton2.Checked == true)
                    hangisi = 1;
                else if(radioButton3.Checked == true)
                    hangisi = 2;
                else if(radioButton4.Checked == true)
                    hangisi = 3;
                else if (radioButton5.Checked == true)
                    hangisi = 4;
                else if(radioButton6.Checked == true)
                    hangisi = 5;
                else if (radioButton5.Checked == true)
                    hangisi = 6;
                else if (radioButton5.Checked == true)
                    hangisi = 7;
                else if (radioButton5.Checked == true)
                    hangisi = 8;
                else if (radioButton5.Checked == true)
                    hangisi = 9;
                else if (radioButton5.Checked == true)
                    hangisi = 10;
                connectButton.Enabled = false;
                panel1.Enabled = false;
                this.ConnectionStart.BeginInvoke(hangisi, new AsyncCallback(delegate{}),null);
                //this.ConnectionStart.Invoke(hangisi);
                
        }
        public void disconnectButton_Click(object sender, EventArgs e)
        {
            disconnectButton.Enabled = false;
            this.DisconnectionStart.BeginInvoke( new AsyncCallback(delegate { }), null);

        }
        //---------------------------------------------------------------------------------------
        private void watcher1_Connected(object sender, RasConnectionEventArgs e)
        {
            RenkDegistir(c1, true);
            GorunumDegistir(true);
            iconDon = false;
            notifyIcon1.Icon = iconlar[0];
        }
        private void watcher1_Disconnected(object sender, RasConnectionEventArgs e)
        {
            RenkDegistir(c2, false);
            GorunumDegistir(false);
            iconDon = false;
            notifyIcon1.Icon = iconlar[0];
        }
        private void watcherBeklenmedik_Disconnected(object sender, RasConnectionEventArgs e)
        {
            BilgilendirmeDegistir(72, true);
            connectButton_Click(null, null);
            BilgilendirmeDegistir(53, true);
        }
        //---------------------------------------------------------------------------------------
        void RenkDegistir(Color renk,bool connected)
        {
            if (connected)
            {
                connectButton.BackColor = SystemColors.Control;
                disconnectButton.BackColor = renk;
            }
            else
            {
                connectButton.BackColor = renk;
                disconnectButton.BackColor = SystemColors.Control;
            }
            
            progressBar1.ForeColor = renk;
            panelP.BackColor = renk;
            textBox1.ForeColor = renk;
            radioButton1.FlatAppearance.CheckedBackColor = renk;
            radioButton2.FlatAppearance.CheckedBackColor = renk;
            radioButton3.FlatAppearance.CheckedBackColor = renk;
            radioButton4.FlatAppearance.CheckedBackColor = renk;
            radioButton5.FlatAppearance.CheckedBackColor = renk;
            radioButton6.FlatAppearance.CheckedBackColor = renk;
            radioButton7.FlatAppearance.CheckedBackColor = renk;
            radioButton8.FlatAppearance.CheckedBackColor = renk;
            radioButton9.FlatAppearance.CheckedBackColor = renk;
            radioButton10.FlatAppearance.CheckedBackColor = renk;
            button2.BackColor = renk;
            button3.BackColor = renk;
            buttonX.BackColor = renk;
            label1.ForeColor = renk;
        }
        void GorunumDegistir(bool connected)
        {
            if (connected)
            {
                disconnectButton.Enabled = true;
                connectButton.Enabled = false;
                panel1.Enabled = false;
                baglanToolStripMenuItem.Enabled = false;
                baglatiyikapatToolStripMenuItem.Enabled = true;
                progressBar1.Value = 100;
            }
            else
            {
                disconnectButton.Enabled = false;
                connectButton.Enabled = true;
                panel1.Enabled = true;
                baglanToolStripMenuItem.Enabled = true;
                baglatiyikapatToolStripMenuItem.Enabled = false;
                progressBar1.Value = 0;
            }
            iconDon = false;
            notifyIcon1.Icon = iconlar[0];
        }
        void BilgilendirmeDegistir(int durum, bool showBaloon)
        {
            string metin = "";
            switch (durum)
            {
                case 0:
                    metin = "";
                    break;
                case 30:
                    metin = "BİLGİLER ...ALINIYOR...";
                    break;
                case 31:
                    metin = "BİLGİLER ALINDI";
                    break;
                case 40:
                    metin = "VPN ...OLUŞTURULUYOR...";
                    break;
                case 41:
                    metin = "VPN OLUŞTURULDU";
                    break;
                case 50:
                    metin = "BAĞLANTI .KURULUYOR.";
                    break;
                case 51:
                    metin = "PAROLA .DOĞRULANIYOR.";
                    break;
                case 52:
                    metin = "PAROLA DOĞRULANDI";
                    break;
                case 53:
                    metin = "TEKRAR .BAĞLANIYOR.";
                    break;
                case 59:
                    metin = "--BAĞLANDI--";
                    break;
                case 90:
                    metin = "BAĞLANILAMADI !!!";
                    break;
                case 91:
                    metin = "BAĞLANILAMADI (Vpn)!!!";
                    break;
                case 900:
                    metin = "İNTERNET BAĞLANTINIZI KONTROL EDİN !!!";
                    break;
                case 907:
                    metin = "BAĞLANTI ZAMAN AŞIMI";
                    break;
                case 908:
                    metin = "ERİŞİM REDDEİLDİ (Yönetici olarak başlatın)";
                    break;
                case 60:
                    metin = "VPN ...KALDIRILIYOR...";
                    break;
                case 61:
                    metin = "VPN KALDIRILDI";
                    break;
                case 70:
                    metin = "BAĞLANTI ..KESİLİYOR..";
                    break;
                case 71:
                    metin = "-BAĞLANTI- -KESİLDİ-";
                    break;
                case 72:
                    metin = "BAĞLANTI Beklenmedik bir şekilde KESİLDİ";
                    break;
                case 73:
                    metin = "BAĞLANMA İPTAL EDİLDİ";
                    break;
                default:
                    metin = "Bilinmeyen Komut";
                    break;
            }
            notifyIcon1.BalloonTipText = metin;
            if(showBaloon){
                //notifyIcon1.ShowBalloonTip(2);
                notifyIcon1.ShowBalloonTip(2, notifyIcon1.Text, metin, ToolTipIcon.None);
            }
            textBox1.Text = metin;
        }
        void ConnectionError(int kod, bool showBaloon)
        {
            watcherBeklenmedik.EnableRaisingEvents = false;
            RenkDegistir(c2, false);
            GorunumDegistir(false);
            BilgilendirmeDegistir(kod, showBaloon);
        }
        //---------------------------------------------------------------------------------------
        public  void buttonX_Click(object sender, EventArgs e)
        {
            iconDon = false;
            
            this.DisconnectionStart.Invoke();
            notifyIcon1.Dispose();
            
            Application.Exit();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            (new FormAbout(Color.GreenYellow)).ShowDialog();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            /*this.Hide();
            this.ShowInTaskbar = false;*/
            this.WindowState = FormWindowState.Minimized;
        }
        private void kapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonX_Click(null, null);
        }
        private void hakkindaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new FormAbout(Color.GreenYellow)).ShowDialog();
        }
        private void arayuzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*this.Show();
            this.ShowInTaskbar = true;*/
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
        private void baglanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectButton_Click(new Object(), new EventArgs());
        }
        private void baglatiyikapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disconnectButton_Click(new Object(), new EventArgs());
        }
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                /*this.Hide();
                this.ShowInTaskbar = false;*/
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                /*this.Show();
                this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - this.Width;
                this.Top = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - this.Height;
                this.ShowInTaskbar = true;*/
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }
        //---------------------------------------------------------------------------------------
        #region EKRANI KAYDIR
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion
        //---------------------------------------------------------------------------------------
        void Form1_IconYukle()
        {
            try
            {
                Icon[] iconlarTemp = new Icon[6];
                int iconBoyut = 15;
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_1, iconBoyut, iconBoyut));
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_2, iconBoyut, iconBoyut));
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_3, iconBoyut, iconBoyut));
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_4, iconBoyut, iconBoyut));
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_3, iconBoyut, iconBoyut));
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_2, iconBoyut, iconBoyut));

                Console.WriteLine("--> Iconlar Yuklendi");
                //iconlar = iconlarTemp;
            }
            catch(Exception e)
            {
                Console.WriteLine("--> Iconlar Yuklenemedi:" + e.Message +" "+e.StackTrace);
            }
            
        }
        void Form1_IconDegistir()
        {
            i = 0;
            while (iconDon)
            {
                notifyIcon1.Icon = iconlar[i];
                i++;
                if (i > 5)
                    i = 0;
                System.Threading.Thread.Sleep(100);
            }
        }
        //---------------------------------------------------------------------------------------

        //---------------------------------------------------------------------------------------
    }
}/*
public bool InternetKontrol()
        {
            try
            {
                System.Net.Sockets.TcpClient kontrol = new System.Net.Sockets.TcpClient(
                    "www.google.com.tr", 80);
                kontrol.Close();
                Console.WriteLine("Internet Var");
                return true;
            }
            catch
            {
                Console.WriteLine("Internet Yok");
                return false;
            }
        }
*/
/*
bool CreateVPN()
      {
          try
          {

              return true;
          }
          catch (Exception e)
          {
              return false;
          }
      }
*/
