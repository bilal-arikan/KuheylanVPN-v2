using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using Microsoft.Win32;
using System.Net;
using System.IO;

namespace Kuheylan
{
    public partial class FormKayit : Form
    {
        String adres = "http:// Your_Server_Address /";
        public FormKayit()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs x)
        {
            richTextBox1.Text = "";
            string urunAnahtari;
            try
            {
                if (textBox1.Text.Length < 3 || textBox2.Text.Length < 3)
                {
                    richTextBox1.Text = "Lütfen Geçerli Bir İsim ve Soyisim girin !!!";
                    return;
                }

                RegistryKey registry = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
                urunAnahtari = registry.GetValue("ProductId").ToString();
                urunAnahtari = urunAnahtari.Replace("-", "");
                registry.Close();

                Console.WriteLine(System.Windows.Forms.SystemInformation.UserName);
                Console.WriteLine(urunAnahtari);

                WebRequest req = WebRequest.Create(adres + "kayit.php?" +
                    "kull=" + System.Windows.Forms.SystemInformation.UserName + "&" +
                    "key=" + urunAnahtari + "&" +
                    "isim=" + textBox1.Text + "&" +
                    "soyisim=" + textBox2.Text);
                req.Method = "GET";
                req.Timeout = 15000;

                string buffer = "";
                using (var response = req.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    buffer = reader.ReadToEnd();
                }
                Console.WriteLine("Kayit Cevap = ", buffer);

                if (buffer.Trim() == "1")
                {
                    richTextBox1.Text = "Kaydınız Başarıyla Alınmıştır. Kullanım Süreniz Dolana Kadar Kısıtlama Olmadan Kullanabilirsiniz";
                    Application.Restart();
                }
                else if (buffer.Trim() == "0")
                {
                    richTextBox1.Text = "Kaydınız Daha Önce Alınmış ! Lütfen onaylnamasını bekleyin yada program yapımcısı ile görüşün";
                }
                else if (buffer.Trim() == "2")
                {
                    richTextBox1.Text = "Sunucu Hatası !";
                }
                else
                {
                    richTextBox1.Text = "Lütfen İnternet Bağlantınızı Kontrol Edin !";
                }
            }
            catch
            {
                richTextBox1.Text = "Lütfen İnternet Bağlantınızı Kontrol Edin !";
            }
        }
        //---------------------------------------------------------------------------------------
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void FormKayit_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void buttonX_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //---------------------------------------------------------------------------------------
    }
}
