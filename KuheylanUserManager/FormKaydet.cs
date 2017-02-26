using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.IO;
using System.Xml;

namespace UserManager
{
    public partial class FormKaydet : Form
    {
        private delegate void IconDegistirE();
        private event IconDegistirE Listele;
        private delegate void IconDegistir2E();
        private event IconDegistir2E Degistir;
        private delegate void IconDegistir3E();
        private event IconDegistir3E Ekle;
        private delegate void IconDegistir4E();
        private event IconDegistir4E Sil;
        struct KISI
        {
            public string ID;
            public string Kullanici;
            public string Urunanahtari;
            public string Durum;
            public string Isim;
            public string Soyisim;
            public DateTime Sonkullanma;
        }
        List<KISI> liste = new List<KISI>();
        String adres = "http:// Your_Server_Address /";

        public FormKaydet()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this.Listele += delegate {
                string temp = MysqlListeyiAl();
                if (temp != null)
                    ListeyeYerlestir(temp);
                };
            this.Degistir += delegate
            {
                MysqlDegistir(listBox1.SelectedIndex);
            };
            this.Ekle += delegate
            {
                MysqlEkle(listBox1.SelectedIndex);
            };
            this.Sil += delegate
            {
                MysqlSil(listBox1.SelectedIndex);
            };

            buttonListele.Enabled = false;
            this.Listele.BeginInvoke(new AsyncCallback(delegate {
                buttonListele.Enabled = true;
                buttonListele.Image = null;
                buttonEkle.Enabled = true;
                buttonSil.Enabled = true;
                buttonDegistir.Enabled = true;
            }), null);
            
        }
        private void FormKaydet_Load(object sender, EventArgs e)
        {
            if ((new FormGiris()).ShowDialog() == DialogResult.OK)
            {
                return;
            }
            Application.Exit();
        }
        private void buttonEkle_Click(object sender, EventArgs e)
        {
            buttonEkle.Enabled = false;
            BilgileriListeyeYerlestir(listBox1.SelectedIndex);
            this.Ekle.BeginInvoke(new AsyncCallback(delegate
            {
                pictureBox1.Image = null;
                buttonEkle.Enabled = true;
                buttonListele_Click(null, null);
            }), null);
        }
        private void buttonSil_Click(object sender, EventArgs e)
        {
            buttonSil.Enabled = false;
            BilgileriListeyeYerlestir(listBox1.SelectedIndex);
            this.Sil.BeginInvoke(new AsyncCallback(delegate
            {
                pictureBox1.Image = null;
                buttonSil.Enabled = true;
                buttonListele_Click(null, null);
            }), null);
        }
        private void buttonDegistir_Click(object sender, EventArgs e)
        {
            buttonDegistir.Enabled = false;
            BilgileriListeyeYerlestir(listBox1.SelectedIndex);
            this.Degistir.BeginInvoke(new AsyncCallback(delegate
            {
                pictureBox1.Image = null;
                buttonDegistir.Enabled = true;
                buttonListele_Click(null, null);
            }), null);
        }
        private void buttonListele_Click(object sender, EventArgs e)
        {
            buttonListele.Enabled = false;
            pictureBox1.Image = UserManager.Properties.Resources.ezgif_save;
            this.Listele.BeginInvoke(new AsyncCallback(delegate
            {
                pictureBox1.Image = null;
                buttonListele.Enabled = true;
            }), null);
        }
        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;
            if (listBox1.Items.Count < 1)
                return;
            int id = listBox1.SelectedIndex;
            groupBox1.Text = liste[id].ID;
            textBox1.Text = liste[id].Isim;
            textBox2.Text = liste[id].Soyisim;
            textBox3.Text = liste[id].Kullanici;
            numericUpDown1.Text = liste[id].Durum;
            textBox5.Text = liste[id].Urunanahtari;
            dateTimePicker1.Value = liste[id].Sonkullanma;
        }
        string MysqlListeyiAl()
        {
            liste.Clear();
            listBox1.Items.Clear();

            try
            {
                WebRequest req = WebRequest.Create(adres + "duzenle.php?islem=listele");
                req.Method = "GET";
                req.Timeout = 10000;

                string buffer = "";
                using (var response = req.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    buffer = reader.ReadToEnd();
                }

                if (string.IsNullOrEmpty(buffer))
                    return null;
                else {
                    return buffer;
                }
            }
            catch (System.Exception ex)
            {
                richTextBox1.Text = ex.Message + ex.StackTrace;
                return null;
            }
        }
        void ListeyeYerlestir(string veri)
        {
            if (veri == null)
            {
                richTextBox1.Text = "Alınan bilgi Boş";
                return;
            }
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(veri);

                if (xml != null)
                {
                    int i = 0;
                    foreach (XmlNode satir in xml.SelectNodes("//tr"))
                    {
                        KISI tempKisi = new KISI();
                        var parcaVeriler = satir.SelectNodes("//td");
                        tempKisi.ID = parcaVeriler[0 + i*7].InnerText;
                        tempKisi.Kullanici = parcaVeriler[1 + i*7].InnerText;
                        tempKisi.Urunanahtari = parcaVeriler[2 + i * 7].InnerText;
                        tempKisi.Durum = parcaVeriler[3 + i*7].InnerText;
                        tempKisi.Isim = parcaVeriler[4 + i*7].InnerText;
                        tempKisi.Soyisim = parcaVeriler[5 + i*7].InnerText;
                        tempKisi.Sonkullanma = DateTime.ParseExact(parcaVeriler[6 + i*7].InnerText.Remove(16) ,"yyyy-MM-dd HH:ss",null);
                        
                        liste.Add(tempKisi);
                        listBox1.Items.Add(tempKisi.ID + ":" + tempKisi.Isim + " " + tempKisi.Soyisim);
                        i++;
                    }
                }
                richTextBox1.Text = "";
            }
            catch (System.Exception ex)
            {
                richTextBox1.Text = ex.Message + ex.StackTrace;            	
            }
        }
        void MysqlDegistir(int index)
        {
            try
            {
                WebRequest req = WebRequest.Create(adres + "duzenle.php?islem=degistir" +
                    "&id="+ liste[index].ID +
                     "&kullanici=" + liste[index].Kullanici +
                     "&urunanahtari=" + liste[index].Urunanahtari +
                     "&durum=" + liste[index].Durum +
                     "&isim=" + liste[index].Isim +
                     "&soyisim=" + liste[index].Soyisim +
                     "&sonkullanma=" + liste[index].Sonkullanma.ToString("yyyy-MM-dd HH:ss") + ":00"
                );

                req.Method = "GET";
                req.Timeout = 10000;

                string buffer = "";
                using (var response = req.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    buffer = reader.ReadToEnd();
                }

                if (buffer == "1")
                {
                    richTextBox1.Text = "Başarıyla Deiğiştirildi";
                } 
                else
                {
                    richTextBox1.Text = "Değiştirilemedi" + buffer;
                }
            }
            catch (System.Exception ex)
            {
                richTextBox1.Text = ex.Message + ex.StackTrace;
            }
        }
        void MysqlEkle(int index)
        {
            try
            {
                WebRequest req = WebRequest.Create(adres + "duzenle.php?islem=ekle" +
                     "&id=" + liste[index].ID +
                     "&kullanici=" + liste[index].Kullanici +
                     "&urunanahtari=" + liste[index].Urunanahtari +
                     "&durum=" + liste[index].Durum +
                     "&isim=" + liste[index].Isim +
                     "&soyisim=" + liste[index].Soyisim +
                     "&sonkullanma=" + liste[index].Sonkullanma.ToString("yyyy-MM-dd HH:ss") + ":00"
                );
                req.Method = "GET";
                req.Timeout = 10000;

                string buffer = "";
                using (var response = req.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    buffer = reader.ReadToEnd();
                }

                if (buffer == "1")
                {
                    richTextBox1.Text = "Başarıyla Ekle";
                }
                else
                {
                    richTextBox1.Text = "Eklenemedi" + buffer;
                }
            }
            catch (System.Exception ex)
            {
                richTextBox1.Text = ex.Message + ex.StackTrace;
            }
        }
        void MysqlSil(int index)
        {
            try
            {
                WebRequest req = WebRequest.Create(adres + "duzenle.php?islem=sil" +
                     "&id=" + liste[index].ID 
                );
                req.Method = "GET";
                req.Timeout = 10000;

                string buffer = "";
                using (var response = req.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    buffer = reader.ReadToEnd();
                }

                if (buffer == "1")
                {
                    richTextBox1.Text = "Başarıyla Silindi";
                }
                else
                {
                    richTextBox1.Text = "Silinemedi" + buffer;
                }
            }
            catch (System.Exception ex)
            {
                richTextBox1.Text = ex.Message + ex.StackTrace;
            }
        }
        void BilgileriListeyeYerlestir(int index)
        {
            if(index < 0)
                return;

            /*Değiştirilemedi0You have an error in your SQL syntax;
             * check the manual that corresponds to your MySQL 
             * server version for the right syntax to use near 'soyisim='ari', 
             * sonkullanma='2014-06-14 00:00:00'  WHERE ID=15' at line 1*/

            KISI temp = new KISI();
            temp.ID = groupBox1.Text;
            temp.Kullanici = textBox3.Text;
            temp.Urunanahtari = textBox5.Text;
            temp.Durum = numericUpDown1.Value.ToString();
            temp.Isim = textBox1.Text;
            temp.Soyisim = textBox2.Text;
            temp.Sonkullanma = dateTimePicker1.Value;

            liste.RemoveAt(index);
            liste.Insert(index,temp);

        }
    }
}
