﻿using HastaneOtomasyonu.Class_Lib;
using HastaneOtomasyonu.ClassLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HastaneOtomasyonu
{
    public partial class FormDoktor : Form
    {
        public FormDoktor()
        {
            InitializeComponent();
        }

        List<Kisi> aramalar = new List<Kisi>();
        List<Hemsire> BransliHemsireler = new List<Hemsire>();
        Hemsire tut = new Hemsire();
        bool silKontrol = false;
  

        private void btnDoktorKaydet_Click(object sender, EventArgs e)
        {
            Doktor doktor = new Doktor();

            doktor.Ad = txtDoktorAd.Text;
            doktor.Soyad = txtDoktorSoyad.Text;
            doktor.Email = txtDoktorEmail.Text;
            doktor.Telefon = txtDoktorTelefon.Text;
            doktor.TCKN = txtDoktorTCKN.Text;
            doktor.Maas = txtDoktorMaas.Text;
            doktor.HemsireSec = cmbDoktorHemsire.SelectedItem as Hemsire;

            BransliHemsireler.Remove(cmbDoktorHemsire.SelectedItem as Hemsire);



            //BransliHemsireler.AddRange((this.MdiParent as FormGiris).hemsireler.ToArray());


            if (memoryStream.Length > 0)
            {
                doktor.Fotograf = memoryStream.ToArray();
            }

            memoryStream = new MemoryStream();

            DoktorBranslari doktorBrans = (DoktorBranslari)Enum.Parse(typeof(DoktorBranslari), cmbDoktorBrans.SelectedItem.ToString());
            //brans doktora eklendi.
            doktor.DoktorBrans = doktorBrans;



            (this.MdiParent as FormGiris).doktorlar.Add(doktor);
            FormuTemizle();
            lstDoktorlar.Items.AddRange(((this.MdiParent as FormGiris).doktorlar).ToArray());
            btnDoktorGuncelle.Enabled = false;
        }

        public void FormuTemizle()
        {

            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    if (control.Name == "txtDoktorAra")
                    {
                        continue;
                    }
                    control.Text = string.Empty;
                }
                else if (control is ListBox lst)
                {
                    lst.Items.Clear();
                }
                else if (control is PictureBox pbox)
                {
                    if (control.Name == "DoktorAramaResim")
                    {
                        continue;
                    }
                    pbox.Image = null;
                }
                else if (control is ComboBox cb)
                {

                    cb.Text = string.Empty; ;
                }
            }
        }

        private void lstDoktorlar_SelectedIndexChanged(object sender, EventArgs e)
        {

            tut = (lstDoktorlar.SelectedItem as Doktor).HemsireSec;
            cmbDoktorHemsire.Items.Clear();
            pbDoktor.Image = null;

            if (lstDoktorlar.SelectedItem == null) return;

            Doktor secilikisi = lstDoktorlar.SelectedItem as Doktor;
            txtDoktorAd.Text = secilikisi.Ad;
            txtDoktorSoyad.Text = secilikisi.Soyad;
            txtDoktorEmail.Text = secilikisi.Email;
            txtDoktorTelefon.Text = secilikisi.Telefon;
            txtDoktorTCKN.Text = secilikisi.TCKN;
            txtDoktorMaas.Text = secilikisi.Maas;
            cmbDoktorBrans.Text = secilikisi.DoktorBrans.ToString();
            cmbDoktorHemsire.Text = secilikisi.HemsireSec.ToString();

            if (secilikisi.Fotograf != null && secilikisi.Fotograf.Length > 0)
            {
                pbDoktor.Image = new Bitmap(new MemoryStream(secilikisi.Fotograf));
            }


            //foreach (Hemsire item in BransliHemsireler)
            //{
            //    if (item.HemsireBrans == secilikisi.DoktorBrans)
            //    {
            //        cmbDoktorHemsire.Items.Add(item);
            //    }
            //}


            btnDoktorKaydet.Enabled = false;
            btnDoktorGuncelle.Enabled = true;
        }

        private void FormDoktor_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnDoktorGuncelle.Enabled = false;
            lstDoktorlar.Items.AddRange((this.MdiParent as FormGiris).doktorlar.ToArray());
            cmbDoktorBrans.Items.AddRange(Enum.GetNames(typeof(DoktorBranslari)));
            BransliHemsireler.AddRange((this.MdiParent as FormGiris).hemsireler.ToArray());

        }

        private void içeriAktarToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            dosyaAc.Title = "Bir JSON dosyası seçiniz";
            dosyaAc.Filter = "(JSON Dosyası) | *.json";
            dosyaAc.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dosyaAc.FileName = "Doktorlar.json"; // string.Empty;
            if (dosyaAc.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream dosya = File.OpenRead(dosyaAc.FileName);
                    StreamReader reader = new StreamReader(dosya);
                    string dosyaIcerigi = reader.ReadToEnd();
                    reader.Close();
                    dosya.Close();
                    (this.MdiParent as FormGiris).doktorlar = JsonConvert.DeserializeObject<List<Doktor>>(dosyaIcerigi);
                    //Kisiler = JsonConvert.DeserializeObject(dosyaIcerigi) as List > Kisi >;
                    //Kisiler = (list<Kisi>)JsonConvert.DeserializeObject(dosyaIcerigi);

                    MessageBox.Show($"{(this.MdiParent as FormGiris).doktorlar.Count} kişi başarıyala aktarıldı");
                    lstDoktorlar.Items.Clear();
                    lstDoktorlar.Items.AddRange((this.MdiParent as FormGiris).doktorlar.ToArray());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("bir hata oluştu " + ex.Message);
                }
            }
        }

        private void dışarıAktarToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            dosyaKaydet.Title = "Bir JSON dosyası seçiniz";
            dosyaKaydet.Filter = "(JSON Dosyası) | *.json";
            dosyaKaydet.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dosyaKaydet.FileName = "Doktorlar.json"; // string.Empty;
            if (dosyaKaydet.ShowDialog() == DialogResult.OK)
            {
                FileStream file = File.Open(dosyaKaydet.FileName, FileMode.Create);
                StreamWriter writer = new StreamWriter(file);
                writer.Write(JsonConvert.SerializeObject((this.MdiParent as FormGiris).doktorlar));
                writer.Close();
                writer.Dispose();
            }

        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (lstDoktorlar.SelectedItem == null) return;

            
            Doktor seciliKisi = (Doktor)lstDoktorlar.SelectedItem;
            (this.MdiParent as FormGiris).doktorlar.Remove(seciliKisi);
            FormuTemizle();
            lstDoktorlar.Items.AddRange((this.MdiParent as FormGiris).doktorlar.ToArray());
            silKontrol = true;
        }

        private void txtDoktorAra_KeyUp(object sender, KeyEventArgs e)
        {
            string ara = txtDoktorAra.Text.ToLower();
            aramalar = new List<Kisi>();
            (this.MdiParent as FormGiris).doktorlar.Where(kisi => kisi.Ad.ToLower().Contains(ara) || kisi.Soyad.ToLower().Contains(ara) || kisi.TCKN.StartsWith(ara)).ToList().ForEach(kisi => aramalar.Add(kisi));

            FormuTemizle();
            lstDoktorlar.Items.AddRange(aramalar.ToArray());
        }

        private void btnDoktorGuncelle_Click_1(object sender, EventArgs e)
        {

            if (lstDoktorlar.SelectedItem == null) return;

            Doktor seciliKisi = (Doktor)lstDoktorlar.SelectedItem;// referans tip değişkenler !

            //static metod yap orda ara varsa varde yoksa yokdersin.
            try
            {
                BransliHemsireler.Add(tut as Hemsire);
                seciliKisi.Ad = txtDoktorAd.Text;
                seciliKisi.Soyad = txtDoktorSoyad.Text;
                seciliKisi.Email = txtDoktorEmail.Text;
                seciliKisi.Telefon = txtDoktorTelefon.Text;
                seciliKisi.TCKN = txtDoktorTCKN.Text;
                seciliKisi.Maas = txtDoktorMaas.Text;

                seciliKisi.HemsireSec = tut as Hemsire;

                if (memoryStream.Length > 0)
                {
                    seciliKisi.Fotograf = memoryStream.ToArray();
                }

                memoryStream = new MemoryStream();

                BransliHemsireler.Remove(cmbDoktorHemsire.SelectedItem as Hemsire);

                DoktorBranslari doktorBrans = (DoktorBranslari)Enum.Parse(typeof(DoktorBranslari), cmbDoktorBrans.SelectedItem.ToString());

                seciliKisi.DoktorBrans = doktorBrans;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            FormuTemizle();
            lstDoktorlar.Items.AddRange((this.MdiParent as FormGiris).doktorlar.ToArray());
            btnDoktorKaydet.Enabled = true;
            btnDoktorGuncelle.Enabled = false;
        }

        private void btnDoktorTemizle_Click(object sender, EventArgs e)
        {
            FormuTemizle();
            lstDoktorlar.Items.AddRange(((this.MdiParent as FormGiris).doktorlar).ToArray());
            btnDoktorKaydet.Enabled = true;
            btnDoktorGuncelle.Enabled = false;
        }

        private void FormDoktor_Click(object sender, EventArgs e)
        {
            lstDoktorlar.SelectedItem = null;
        }

        MemoryStream memoryStream = new MemoryStream();
        int bufferSize = 64;
        byte[] resimArray = new byte[64];

        private void btnFotografDoktor_Click(object sender, EventArgs e)
        {
            try
            {
                dosyaAc.Title = "Bir fotoğraf dosyasını seçiniz";
                dosyaAc.Filter = "JPG | *.jpg;*.png";
                dosyaAc.Multiselect = false;
                dosyaAc.FileName = string.Empty;
                dosyaAc.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (dosyaAc.ShowDialog() == DialogResult.OK)
                {
                    FileStream dosya = File.Open(dosyaAc.FileName, FileMode.Open);
                    while (dosya.Read(resimArray, 0, bufferSize) != 0)
                    {
                        memoryStream.Write(resimArray, 0, resimArray.Length);
                    }
                    dosya.Close();
                    dosya.Dispose();
                    pbDoktor.Image = new Bitmap(memoryStream);
                }

                MessageBox.Show($"Fotoğraf kaydedildi.");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        private void txtDoktorTCKN_KeyPress(object sender, KeyPressEventArgs e)
        {
            //SAdece RAkam girişi
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtDoktorTelefon_KeyPress(object sender, KeyPressEventArgs e)
        {
            //SAdece RAkam girişi
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtDoktorMaas_KeyPress(object sender, KeyPressEventArgs e)
        {
            //SAdece RAkam girişi
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cmbDoktorHemsire_DropDown(object sender, EventArgs e)
        {
            if ((this.MdiParent as FormGiris).doktorlar.Count == 0)
            {
                foreach (var item4 in lstDoktorlar.Items)
                {
                    (this.MdiParent as FormGiris).doktorlar.Add(item4 as Doktor);
                }
            }


            cmbDoktorHemsire.Items.Clear();
            cmbDoktorHemsire.Text = string.Empty;

            foreach (Hemsire item in BransliHemsireler)
            {
                if (item == null)
                {
                    break;
                }

                cmbDoktorHemsire.Items.Add(item);

            }

            if (lstDoktorlar.Items.Count != 0)
            {

                foreach (var item1 in (this.MdiParent as FormGiris).doktorlar)
                {


                    foreach (var item2 in cmbDoktorHemsire.Items)
                    {


                        if (cmbDoktorBrans.SelectedItem.ToString() != (item2 as Hemsire).HemsireBrans.ToString() || item1.HemsireSec.TCKN == (item2 as Hemsire).TCKN || item1.HemsireSec.HemsireBrans.ToString() != (item2 as Hemsire).HemsireBrans.ToString())
                        {
                            cmbDoktorHemsire.Items.Remove(item2);
                            break;
                        }

                    }
                }


                if (cmbDoktorHemsire.Items.Count > 0 && silKontrol == false)
                {
                    foreach (var item3 in cmbDoktorHemsire.Items)
                    {
                        if (cmbDoktorBrans.SelectedItem.ToString() != (item3 as Hemsire).HemsireBrans.ToString())
                        {
                            cmbDoktorHemsire.Items.Remove(item3);
                            break;
                        }
                    }
                }


            }
            else
            {
                foreach (Hemsire item in BransliHemsireler)
                {
                    if (cmbDoktorBrans.SelectedItem.ToString() != (item as Hemsire).HemsireBrans.ToString())
                    {
                        cmbDoktorHemsire.Items.Remove(item);
                    }
                }
            }

            if (cmbDoktorHemsire.Items.Count == 0)
            {
                MessageBox.Show("Şuanda seçilecek boşta bir hemşire yoktur.");

            }

            silKontrol = false;
        }
    }
}




