using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdonetGiris
{
    public partial class Form1 : Form
    {
        // Data Source = DESKTOP - 3LRKI3G\WINCC;Initial Catalog = Northwind; Integrated Security = True
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-3LRKI3G\\WINCC;Initial Catalog=Northwind;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();


            /*
            1. Soru Hangi sunucuya bağlanıyorum
            2. Soru Bu sunucudak hangi databaseye bağlanıyorum
            3. Soru Hangi Güvenlik modalini kullanıyorum 
           */
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Urunlist();

        }

        private void Urunlist()
        {
            SqlDataAdapter sqlDatadapter = new SqlDataAdapter("select*from Urunler", baglanti);
            DataTable dataTable = new DataTable();
            sqlDatadapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string urunAdi = txtUrunAdi.Text;
            decimal fiyat = NudFiyat.Value;
            decimal stok = nudStok.Value;
            if (urunAdi == "" || fiyat == null || stok == null)
            {
                MessageBox.Show("Lütfen Tüm Alanları Doldurunuz");
            }
            else
            {
                SqlCommand komut = new SqlCommand();
                komut.CommandText = string.Format("insert Urunler (UrunAdi, BirimFiyati, HedefStokDuzeyi) values('{0}',{1},{2})", urunAdi, fiyat, stok);
                komut.Connection = baglanti;
                baglanti.Open();
                int etki = komut.ExecuteNonQuery();

                if (etki > 0)
                {
                    MessageBox.Show("Ekleme İşlemi Tamamlandı");
                    Urunlist();

                }
                else
                {
                    MessageBox.Show("Bir Hata Oluştu");
                }
                baglanti.Close();
            }
        }

        private void btnKategori_Click(object sender, EventArgs e)
        {
            Kategoriler kt = new Kategoriler();
            kt.Show();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtUrunAdi.Text = dataGridView1.CurrentRow.Cells["UrunAdi"].Value.ToString();
            txtUrunAdi.Tag = dataGridView1.CurrentRow.Cells["UrunID"].Value.ToString();
            NudFiyat.Value = (Decimal)dataGridView1.CurrentRow.Cells["BirimFiyati"].Value;
            nudStok.Value = Convert.ToInt16(dataGridView1.CurrentRow.Cells["HedefStokDuzeyi"].Value);
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand();
            komut.CommandText = String.Format("update Urunler set UrunAdi = '{0}', BirimFiyati='{1}', HedefStokDuzeyi='{2}' where UrunID='{3}'", txtUrunAdi.Text, NudFiyat.Value, nudStok.Value, txtUrunAdi.Tag);
            baglanti.Open();
            komut.Connection = baglanti;
            int etki = komut.ExecuteNonQuery();
            if (etki > 0)
            {
                MessageBox.Show("Urun Güncellemesi Tamamlandı");
                Urunlist();
            }
            else
            {
                MessageBox.Show("Hata");
            }

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["UrunID"].Value);
                SqlCommand cmd = new SqlCommand(string.Format("delete Urunler where UrunID={0}", id),baglanti);
                baglanti.Open();
                int etki = cmd.ExecuteNonQuery();

                if (etki > 0)
                {
                    MessageBox.Show("Urun Silme İşlemi Gerçekleşmiştir");
                    Urunlist();
                }
                else
                {
                    MessageBox.Show("Hata");
                }
                baglanti.Close();
            }
        }
        private void btnAra_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = baglanti.CreateCommand();
            komut.CommandType = CommandType.Text;
            komut.CommandText = "select*from Urunler where UrunAdi like '%" + txtAra.Text + "%'";
            komut.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(komut);
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
    }
}
