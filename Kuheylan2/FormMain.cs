using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;

using DotRas;
using Microsoft.Win32;
using System.Collections;
using DLL;
using System.Net;
using System.Net.NetworkInformation;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Kuheylan
{
    [TestFixture]
    public partial class FormMain : Form
    {
        ConnectionManager Manager;
#pragma warning disable CS0169 // The field 'FormMain.managerCurrentState' is never used
        Globals.State managerCurrentState;
#pragma warning restore CS0169 // The field 'FormMain.managerCurrentState' is never used
        List<Icon> iconlar = new List<Icon>();
        Thread threadIconChange;
        bool iconDon;

        public FormMain()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            InitButtonEvents();

            threadIconChange = new Thread(new ThreadStart(IconYukle));
            iconDon = false;
            threadIconChange.Start();

            notifyIcon1.Text = Globals.vpnIsim;
            Manager = new ConnectionManager();
            Manager.AVpnInfoTaken += aVpnInfoTaken;
            Manager.ConnectionStart += ConnectionStart;
            Manager.DisconnectionStart += DisconnectionStart;
            Manager.StateChanged += StateChanged;
            Manager.ConnectionComplete += ConnectionComplete;
            Manager.DisconnectionComplete += DisconnectionComplete;
            Manager.LoadAllJson();
            Manager.InitAllVpns();
            SetUI(Globals.State.InfoGathering);
        }


        [Test]
        void InitButtonEvents()
        {
            disconnectButton.Click += delegate
            {
                Manager.Disconnect();
            };
            buttonLoadJson.Click += delegate
            {
                Manager.InitAllVpns();
                SetUI(Globals.State.InfoGathering);
            };
            flowLayoutPanel1.MouseHover += delegate
            {
                flowLayoutPanel1.Focus();
            };
            Closing += delegate
            {
                SetUI(Globals.State.Closing);
                Application.Exit();
            };
        }

        [Test]
        private void ConnectionStart(VPN hangiVpn)
        {
            SetUI(Globals.State.Connecting);
            Manager.InitAllVpnsAbort();
        }

        private void DisconnectionStart()
        {
            SetUI(Globals.State.Disconnecting);
        }

        private void ConnectionComplete(bool state)
        {
            if (state)
                SetUI(Globals.State.Connected);
            else
                SetUI(Globals.State.ConnError);
        }

        private void DisconnectionComplete(bool success)
        {
            SetUI(Globals.State.Disconnected);
        }

        private void aVpnInfoTaken(int i, VPN vpn)
        {
            // GUI Threadında işlem yaptırıyor
            this.Invoke((MethodInvoker)delegate
            {
                if (flowLayoutPanel1.Controls.Contains(vpn))
                {
                    progressBar1.Value = (int)((i + 1) * (100.0 / Manager.Vpnlist.Count));
                    if (i == Manager.Vpnlist.Count - 1)
                    {
                        SetUI(Globals.State.InfoGatheringSuccess);
                    }
                }
                else
                {
                    vpn.Size = new Size(flowLayoutPanel1.Size.Width - flowLayoutPanel1.Margin.Left * 2 - 20, vpn.Size.Height); //-5 ScroolBar Genişliği
                    flowLayoutPanel1.Controls.Add(vpn);
                }
            });
        }
        //---------------------------------------------------------------------------------------
        void StateChanged(RasConnectionState state,int sNo)
        {
            labelInfo.Text = state.ToString();
            progressBar1.Value = (int)((sNo) * (100.0 / 12));
        }
        //---------------------------------------------------------------------------------------
        void SetUI(Globals.State state)
        {
            switch (state)
            {
                case Globals.State.NewlyOpen:
                    flowLayoutPanel1.Enabled = true;
                    disconnectButton.Enabled = true;
                    textBoxInfo.Text = global::Kuheylan.Properties.Resources.welcome;
                    break;
                case Globals.State.InfoGathering:
                    iconDon = true;
                    buttonLoadJson.Enabled = false;
                    textBoxInfo.Text = global::Kuheylan.Properties.Resources.String1;
                    break;
                case Globals.State.InfoGatheringSuccess:
                    iconDon = false;
                    buttonLoadJson.Enabled = true;
                    progressBar1.Value = 100;
                    textBoxInfo.Text = global::Kuheylan.Properties.Resources.String2;
                    break;
                case Globals.State.Connecting:
                    iconDon = true;
                    buttonLoadJson.Enabled = true;
                    flowLayoutPanel1.Enabled = false;
                    disconnectButton.Enabled = true;
                    textBoxInfo.Text = global::Kuheylan.Properties.Resources.String5;
                    break;
                case Globals.State.Connected:
                    iconDon = false;
                    buttonLoadJson.Enabled = true;
                    flowLayoutPanel1.Enabled = true;
                    disconnectButton.Enabled = true;
                    textBoxInfo.Text = global::Kuheylan.Properties.Resources.String9;
                    break;
                case Globals.State.Disconnecting:
                    iconDon = true;
                    disconnectButton.Enabled = false;
                    textBoxInfo.Text = global::Kuheylan.Properties.Resources.String16;
                    break;
                case Globals.State.Disconnected:
                    iconDon = false;
                    buttonLoadJson.Enabled = true;
                    flowLayoutPanel1.Enabled = true;
                    disconnectButton.Enabled = false;
                    progressBar1.Value = 0;
                    textBoxInfo.Text = global::Kuheylan.Properties.Resources.String17;
                    break;
                case Globals.State.ConnError:
                    iconDon = false;
                    flowLayoutPanel1.Enabled = true;
                    disconnectButton.Enabled = false;
                    textBoxInfo.Text = global::Kuheylan.Properties.Resources.String10;
                    //MessageBox.Show("Bağlanılamadı !!!");
                    break;
                case Globals.State.Closing:
                    Manager.Close();
                    iconDon = false;
                    threadIconChange.Abort();
                    notifyIcon1.Visible = false;
                    notifyIcon1.Dispose();
                    break;
            }
        }
        //---------------------------------------------------------------------------------------
        public void buttonX_Click(object sender, EventArgs e)
        {
            SetUI(Globals.State.Closing);
            Close();
            Application.Exit();
        }
        private void buttonS_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void buttonAbout_Click(object sender, EventArgs e)
        {
            (new FormAbout()).ShowDialog();
        }
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
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
        void IconYukle()
        {
            try
            {
                int iconBoyut = 15;
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_1, iconBoyut, iconBoyut));
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_2, iconBoyut, iconBoyut));
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_3, iconBoyut, iconBoyut));
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_4, iconBoyut, iconBoyut));
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_3, iconBoyut, iconBoyut));
                iconlar.Add(new Icon(Kuheylan.Properties.Resources.B_2, iconBoyut, iconBoyut));

                IconDegistir();

                Console.WriteLine("--> Iconlar Yuklendi");
            }
            catch (Exception e)
            {
                Console.WriteLine("--> Iconlar Yuklenemedi:" + e.Message + " " + e.StackTrace);
            }

        }

        int i;
        void IconDegistir()
        {
            while (true)
            {
                i = 0;
                while (iconDon)
                {
                    notifyIcon1.Icon = iconlar[i];
                    i++;
                    if (i > iconlar.Count - 1)
                        i = 0;
                    System.Threading.Thread.Sleep(100);
                }
                notifyIcon1.Icon = iconlar[0];
                System.Threading.Thread.Sleep(200);
            }
            
        }

    }
    //---------------------------------------------------------------------------------------

    //---------------------------------------------------------------------------------------

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
