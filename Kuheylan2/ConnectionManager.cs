using System;
using System.Collections.Generic;

using DotRas;
using Microsoft.Win32;
using System.Collections;
using DLL;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using System.Threading;

namespace Kuheylan
{

    class ConnectionManager
    {
        public delegate void ConnectionStartE(VPN hangiVpn);
        public event ConnectionStartE ConnectionStart;
        public delegate void DisconnectionStartE();
        public event DisconnectionStartE DisconnectionStart;
        public delegate void ConnectionCompleteE(bool success);
        public event ConnectionCompleteE ConnectionComplete;
        public delegate void DisConnectionCompleteE(bool success);
        public event DisConnectionCompleteE DisconnectionComplete;
        public delegate void StateChangedE(RasConnectionState state,int sIndex);
        public event StateChangedE StateChanged;
        public delegate void AVpnInfoTakenE(int i,VPN vpn);
        public event AVpnInfoTakenE AVpnInfoTaken;

        public bool isConnected = false;
        public bool isBusy = false;
        public bool portAlreadyOpen = false;
        public List<VPN> Vpnlist = new List<VPN>();
        RasDialer rasDialer = new RasDialer();
        RasConnectionWatcher watcher = new RasConnectionWatcher();
        RasConnectionWatcher watcherUnpredict = new RasConnectionWatcher();
        RasPhoneBook phoneBook = new RasPhoneBook();
        RasHandle handle;
        Thread threadInitAllVpns;
        Thread threadConnectionStart;
        Thread threadDisconnectionStart;
        Thread threadLoadAllJson;


        public ConnectionManager()
        {
            this.rasDialer.StateChanged += RasDialer_StateChanged;
            this.watcher.Connected += Watcher_Connected;
            this.watcher.Disconnected += Watcher_Disconnected;
            this.watcherUnpredict.Disconnected += Unpredict_Disconnected;

            watcher.EnableRaisingEvents = true;
#pragma warning disable CS0618 // 'RasPhoneBook.Open(bool)' is obsolete: 'This method will be removed in a future version, please use the Open(string) overload to open the phone book.'
            phoneBook.Open(true);
#pragma warning restore CS0618 // 'RasPhoneBook.Open(bool)' is obsolete: 'This method will be removed in a future version, please use the Open(string) overload to open the phone book.'
        }

        private void Unpredict_Disconnected(object sender, RasConnectionEventArgs e)
        {
            isConnected = false;
            isBusy = false;
            portAlreadyOpen = false;
            if (DisconnectionComplete != null)
                DisconnectionComplete(true);
        }

        private void Watcher_Disconnected(object sender, RasConnectionEventArgs e)
        {
            isConnected = false;
            isBusy = false;
            portAlreadyOpen = false;
            if (DisconnectionComplete != null)
                DisconnectionComplete(true);
        }
        private void Watcher_Connected(object sender, RasConnectionEventArgs e)
        {
            isConnected = true;
            isBusy = false;
            if (ConnectionComplete != null)
                ConnectionComplete(true);
        }
        private void RasDialer_StateChanged(object sender, StateChangedEventArgs e)
        {
            PLog("#"+e.State.ToString(), false);

            if (StateChanged == null) return;
            int sNo = -1;

            // Olmasi Gereken
            switch (e.State)
            {
                // Tekrar Port AÇılma Hatasına çözüm
                case RasConnectionState.OpenPort:
                    sNo = 1;
                    if (!portAlreadyOpen)
                        portAlreadyOpen = true;
                    else
                    {
                        PLog("SERIOUS ERROR tekrar port acıldı");
                        Disconnect();
                    }
                    break;
                case RasConnectionState.PortOpened:
                    sNo = 2;
                    break;
                case RasConnectionState.ConnectDevice:
                    sNo = 3;
                    break;
                case RasConnectionState.DeviceConnected:
                    sNo = 4;
                    break;
                case RasConnectionState.AllDevicesConnected:
                    sNo = 5;
                    break;
                case RasConnectionState.Authenticate:
                    sNo = 6;
                    break;
                case RasConnectionState.AuthNotify:
                    sNo = 7;
                    break;
                case RasConnectionState.AuthProject:
                    sNo = 8;
                    break;
                case RasConnectionState.Projected:
                    sNo = 9;
                    break;
                case RasConnectionState.Authenticated:
                    sNo = 10;
                    break;
                case RasConnectionState.ApplySettings:
                    sNo = 11;
                    break;
                case RasConnectionState.Connected:
                    sNo = 12;
                    break;
                default:
                    sNo = -1;
                    break;
            }

            // Beklenmeyen
            switch (e.State)
            {
                case RasConnectionState.AuthAck:
                    break;
                case RasConnectionState.AuthCallback:
                    break;
                case RasConnectionState.AuthChangePassword:
                    break;
                case RasConnectionState.AuthLinkSpeed:
                    break;
                case RasConnectionState.AuthRetry:
                    break;
                case RasConnectionState.CallbackComplete:
                    break;
                case RasConnectionState.CallbackSetByCaller:
                    break;
                case RasConnectionState.Disconnected:
                    break;
                case RasConnectionState.Interactive:
                    break;
                case RasConnectionState.InvokeEapUI:
                    break;
                case RasConnectionState.LogOnNetwork:
                    break;
                case RasConnectionState.PasswordExpired:
                    break;
                case RasConnectionState.PostCallbackAuthentication:
                    break;
                case RasConnectionState.PrepareForCallback:
                    break;
                case RasConnectionState.RetryAuthentication:
                    break;
                case RasConnectionState.StartAuthentication:
                    break;
                case RasConnectionState.SubEntryConnected:
                    break;
                case RasConnectionState.SubEntryDisconnected:
                    break;
                case RasConnectionState.WaitForCallback:
                    break;
                case RasConnectionState.WaitForModemReset:
                    break;
            }

            StateChanged(e.State,sNo);

        }
        //---------------------------------------------------------------------------------------
        public void Connect(VPN v){Connect(Vpnlist.IndexOf(v));}
        public void Connect(int i)
        {
            if (ConnectionStart != null)
                ConnectionStart(Vpnlist[i]);

            if (threadConnectionStart != null)
                threadConnectionStart.Abort();

            isBusy = true;
            threadConnectionStart = new Thread(new ThreadStart(delegate
            {
                if (!Vpnlist[i].isInited)
                {
                    Vpnlist[i].InitValues();
                }

                if (CreateDevice(i) == 1)
                {
                    if (ConnectDevice(i) == 1)
                    {
                        watcherUnpredict.EnableRaisingEvents = true;
                        return;
                    }
                }
                // Kod Buraya gelirse bağlantı hatası
                if (ConnectionComplete != null)
                    ConnectionComplete(false);
            }));
            threadConnectionStart.Start();

        }
        public void Disconnect()
        {
            if(DisconnectionStart != null)
                DisconnectionStart();

            if (threadDisconnectionStart != null)
                threadDisconnectionStart.Abort();

            isBusy = true;
            threadDisconnectionStart = new Thread(new ThreadStart(delegate
            {
                watcherUnpredict.EnableRaisingEvents = false;
                if (DisconnectDevice() == 1)
                {
                    if (RemoveDevice() == 1)
                    {
                        return;
                    }
                }

                if (DisconnectionComplete != null)
                    DisconnectionComplete(false);
            }));
            threadDisconnectionStart.Start();
        }
        //---------------------------------------------------------------------------------------
        int CreateDevice(int i)
        {
            PLog(phoneBook.Path,false);
            if (RasEntry.Exists(Globals.vpnIsim, phoneBook.Path))
            {
                PLog("Önceki Vpn Siliniyor...");
                DisconnectDevice();
                RemoveDevice();
            }

            RasEntry entry;
            if (Vpnlist[i].Protocol == "pptp")
            {
                entry = RasEntry.CreateVpnEntry(
                                Globals.vpnIsim,
                                Vpnlist[i].Ip,
                                RasVpnStrategy.PptpFirst,
#pragma warning disable CS0618 // 'RasDevice.GetDeviceByName(string, RasDeviceType)' is obsolete: 'This method will be removed in a future version, please use the GetDevices method to find the devices.'
                                RasDevice.GetDeviceByName(Vpnlist[i].Protocol, RasDeviceType.Vpn));
#pragma warning restore CS0618 // 'RasDevice.GetDeviceByName(string, RasDeviceType)' is obsolete: 'This method will be removed in a future version, please use the GetDevices method to find the devices.'
                this.phoneBook.Entries.Add(entry);
            }
            else
            {
                entry = RasEntry.CreateVpnEntry(
                                Globals.vpnIsim,
                                Vpnlist[i].Ip,
                                RasVpnStrategy.L2tpFirst,
#pragma warning disable CS0618 // 'RasDevice.GetDeviceByName(string, RasDeviceType)' is obsolete: 'This method will be removed in a future version, please use the GetDevices method to find the devices.'
                                RasDevice.GetDeviceByName(Vpnlist[i].Protocol, RasDeviceType.Vpn));
#pragma warning restore CS0618 // 'RasDevice.GetDeviceByName(string, RasDeviceType)' is obsolete: 'This method will be removed in a future version, please use the GetDevices method to find the devices.'
                entry.Options.UsePreSharedKey = true;
                this.phoneBook.Entries.Add(entry);

                foreach (RasEntry e in this.phoneBook.Entries)
                {
                    if (e.PhoneNumber == Vpnlist[i].Ip)
                        e.UpdateCredentials(RasPreSharedKey.Client, Vpnlist[i].xl2tp);

                }
            }
            

            PLog(this.phoneBook.Entries.Count + ". # IP:" + this.phoneBook.Entries[this.phoneBook.Entries.Count-1].PhoneNumber);
            PLog("Vpn Created: " + Vpnlist[i].Protocol);
            return 1;
        }
        //---------------------------------------------------------------------------------------
        int ConnectDevice(int i)
        {
            this.rasDialer.EntryName = Globals.vpnIsim;
            this.rasDialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User); //yada alluser
            this.rasDialer.Credentials = new System.Net.NetworkCredential(Vpnlist[i].User, Vpnlist[i].Pass);
            this.handle = this.rasDialer.DialAsync();
            while (rasDialer.IsBusy)
            {
                System.Threading.Thread.Sleep(200);
            }

#pragma warning disable CS0618 // 'RasConnection.GetActiveConnectionByName(string, string)' is obsolete: 'This method will be removed in a future version, please use the GetActiveConnections method to find the connection.'
            if (RasConnection.GetActiveConnectionByName(Globals.vpnIsim, phoneBook.Path) == null)
#pragma warning restore CS0618 // 'RasConnection.GetActiveConnectionByName(string, string)' is obsolete: 'This method will be removed in a future version, please use the GetActiveConnections method to find the connection.'
            {
                PLog("Sunucu Kaynaklı Bağlantı Hatası");
                return 901;
            }
            PLog("Baglanti kuruldu");

            return 1;
        }
        //---------------------------------------------------------------------------------------
        int RemoveDevice()
        {
            if (RasEntry.Exists(Globals.vpnIsim, phoneBook.Path))
                this.phoneBook.Entries.Remove(Globals.vpnIsim);
            PLog("Vpn Silindi");
            return 1;
        }
        //---------------------------------------------------------------------------------------
        int DisconnectDevice()
        {
            if (this.rasDialer.IsBusy)
            {
                // The connection attempt has not been completed, cancel the attempt.
                this.rasDialer.DialAsyncCancel();
                PLog("Bağlanma İptal Edildi");
                //ConnectionError(73, false);
            }
            else
            {
                // The connection attempt has completed, attempt to find the connection in the active connections.
#pragma warning disable CS0618 // 'RasConnection.GetActiveConnectionByName(string, string)' is obsolete: 'This method will be removed in a future version, please use the GetActiveConnections method to find the connection.'
                RasConnection connection = RasConnection.GetActiveConnectionByName(Globals.vpnIsim, phoneBook.Path);
#pragma warning restore CS0618 // 'RasConnection.GetActiveConnectionByName(string, string)' is obsolete: 'This method will be removed in a future version, please use the GetActiveConnections method to find the connection.'
                if (connection != null)
                {
                    // The connection has been found, disconnect it.
                    connection.HangUp();
                    PLog("Bağlantı kapatıldı");
                }
            }
            portAlreadyOpen = false;
            return 1;
        }
        //---------------------------------------------------------------------------------------
        public void LoadAllJson()
        {
            if (threadLoadAllJson != null)
                threadLoadAllJson.Abort();

            threadLoadAllJson = new Thread(new ThreadStart(delegate
            {
                StreamReader read = new StreamReader(Globals.jsonFileName);
                Vpnlist.Clear();
                int i = 0;
                while (!read.EndOfStream)
                {
                    string line = read.ReadLine();
                    VPN v = VPN.ParseFromJson(i, line);
                    v.Click += delegate
                    {
                        Connect(v);
                    };
                    Vpnlist.Add(v);
                    if (AVpnInfoTaken != null)
                        AVpnInfoTaken.Invoke(v.Index, v);
                    i++;
                }
                PLog(" VPN Mevcut: " + Vpnlist.Count);
                read.Close();
            }));
            threadLoadAllJson.Start();
        }
        //---------------------------------------------------------------------------------------
        public void InitAllVpns()
        {
            if (threadInitAllVpns != null)
                threadInitAllVpns.Abort();

            threadInitAllVpns = new Thread(new ThreadStart(delegate
            {
                while (threadLoadAllJson.IsAlive)
                    Thread.Sleep(50);

                int i = 0;
                foreach (VPN v in Vpnlist)
                {
                    try
                    {

                        v.InitValues();
                        Console.WriteLine(i + "#" +v.isInited + "#"+v.Protocol);

                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(i + " "+ex.Data + ex.Message);
                    }
                    if (AVpnInfoTaken != null)
                        AVpnInfoTaken(i, v);
                    i++;
                }
                Console.WriteLine("Taking Info DONE");

            }));
            threadInitAllVpns.Start();
        }
        public void InitAllVpnsAbort()
        {
            if (threadInitAllVpns != null)
                threadInitAllVpns.Abort();
            PLog("InitAllVpns Aborted.");
        }
        //---------------------------------------------------------------------------------------
        public void Close()
        {
            threadInitAllVpns.Abort();
            threadLoadAllJson.Abort();
            Disconnect();
        }
        //---------------------------------------------------------------------------------------
        public void PLog(string text,bool arrow = true)
        {

            if(arrow)
                Console.WriteLine("---> " + text);
            else
                Console.WriteLine(text);
        }
    }
}
