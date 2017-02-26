using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;
using System.Net;
using System.IO;
using DLL;
using System.Runtime.InteropServices;

namespace Kuheylan
{
    public partial class FormLock : Form
    {
        DllYukleyici dll = new DllYukleyici();
        Form1 mainForm = new Form1();
        String adres = "http://178.62.149.121/kuheylan/";

        public delegate void KayitKontrolE();
        public event KayitKontrolE Kontrol;
        //---------------------------------------------------------------------------------------
        public FormLock( string ileti, Color color)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.Kontrol += FormLock_Kontrol;
            this.Kontrol.BeginInvoke(new AsyncCallback(delegate { }), null);
            mainForm.FormClosing += delegate
            {
                mainForm.buttonX_Click(null, null);
            };

            ChangeColor(color);
            textBox1.Text = ileti;
        }
        void FormLock_Kontrol()
        {
            KontrolSonuc( KayitKontrol() );
        }
        //---------------------------------------------------------------------------------------
        string KayitKontrol()
        {
            try
            {
                string urunAnahtari;
                RegistryKey registry = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
                urunAnahtari = registry.GetValue("ProductId").ToString();
                urunAnahtari = urunAnahtari.Replace("-", "");
                registry.Close();

                Console.Write(System.Windows.Forms.SystemInformation.UserName);
                Console.WriteLine("---" + urunAnahtari);

                WebRequest req = WebRequest.Create(adres + "onay.php?" +
                    "kull=" + System.Windows.Forms.SystemInformation.UserName + "&" +
                    "key=" + urunAnahtari);
                req.Method = "GET";
                req.Timeout = 10000;

                string buffer;
                using (var response = req.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    buffer = reader.ReadToEnd();
                }

                Console.WriteLine(buffer);
                return buffer;
            }
            catch (WebException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return "902";
            }
            catch (TimeoutException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return "907";
            }
            catch (UnauthorizedAccessException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return "908";
            }
            catch (NotImplementedException w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return "909";
            }
            catch (Exception w)
            {
                Console.WriteLine(w.Message + "\n" + w.StackTrace);
                return "902";
            }
        }
        string KontrolSonuc(string cevap)
        {
            string ilkHarf;
            if (cevap.Length > 1)
                ilkHarf = cevap.Remove(1);
            else
                ilkHarf = cevap;

            pictureBox1.Dispose();

            if (ilkHarf == "1")
            {
                this.Hide();

                Console.WriteLine("--> KalanSalise = " + cevap.Remove(0, 1));
                int gun = Convert.ToInt32(cevap.Remove(0, 1)) / (60 * 60 * 24);
                mainForm.label1.Text = gun + " Gun Kaldi.";

                mainForm.ShowDialog();
            }
            else if (ilkHarf == "0")
            {
                ChangeColor(Color.Red);
                textBox1.Text = "Bilgisyarınız Sistemde Kayıtlı Değil. Lütfen Kaydolun yada Program Yapımcısı ile Görüşün";
                button3.Enabled = true;
            }
            else if (ilkHarf == "2")
            {
                ChangeColor(Color.Black);
                textBox1.Text = "Sunucu Kaynaklı Hata !";
            }
            else if (ilkHarf == "3")
            {
                ChangeColor(Color.Olive);
                textBox1.Text ="! Kaydınız Henüz Onaylanmamıştır. Hatırlatma için Program Yapımcısı ile Görüşebilirsiniz!";
            }
            else if (ilkHarf == "4")
            {
                ChangeColor(Color.DarkBlue);
                textBox1.Text = "Kullanım Süreniz Dolmuştur ! Lütfen Program Yapımcısı ile Görüşün";
            }
            else if (ilkHarf == "9")
            {
                if (cevap == "902" || cevap == "909" || cevap == "907")
                {
                    ChangeColor(Color.Indigo);
                    textBox1.Text = "Lütfen İnternet Bağlantınızı Kontrol Edin !";
                }
                else if (cevap == "908")
                {
                    ChangeColor(Color.DarkRed);
                    textBox1.Text = "Erişim Reddedildi (Yönetici olarak çalıştırın)";
                }
                else
                {
                    ChangeColor(Color.Black);
                    textBox1.Text = "Bilinmeyen HATA";
                }
            }else if (ilkHarf == null)
            {
                ChangeColor(Color.DarkRed);
                textBox1.Text = "Lütfen İnternet Bağlantınızı Kontrol Edin ! Sürekli Bu Hatayı Alıyorsanız Bilgisyaranızı Yeniden Başaltın";
            }
            else
            {
                ChangeColor(Color.DarkGoldenrod);
                textBox1.Text = "Lütfen İnternet Bağlantınızı Kontrol Edin !";
            }
            
            return "1";
        }
        //---------------------------------------------------------------------------------------
        void ChangeColor(System.Drawing.Color color)
        {
            button1.ForeColor = color;
            button1.FlatAppearance.BorderColor = color;
            button2.ForeColor = color;
            button2.FlatAppearance.BorderColor = color;
            button3.ForeColor = color;
            button3.FlatAppearance.BorderColor = color;
            textBox1.ForeColor = color;
            linkLabel1.LinkColor = color;
        }
        //---------------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            (new FormAbout(Color.GreenYellow)).ShowDialog();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            (new FormKayit()).ShowDialog();
        }
        //---------------------------------------------------------------------------------------
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void FormLock_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        //---------------------------------------------------------------------------------------
        private void FormLock_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        //---------------------------------------------------------------------------------------

    }
}
