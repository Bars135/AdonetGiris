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
    public partial class Kategoriler : Form
    {
        public Kategoriler()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-3LRKI3G\\WINCC;Initial Catalog=Northwind;Integrated Security=True");

        private void Kategoriler_Load(object sender, EventArgs e)
        {
            kategoriler();
        }

        private void btnekle2_Click(object sender, EventArgs e)
        {
            string KategoriAdı = txtKategoriAdi.Text;
            string Tanımı = txtTanim.Text;
            if (KategoriAdı == "" || Tanımı == null)
            {
                MessageBox.Show("Lütfen Tüm Alanları Doldurunuz");
            }
            else
            {
                SqlCommand komut = new SqlCommand();
                komut.CommandText = string.Format("insert Kategoriler (KategoriAdi, Tanimi) values('{0}','{1}')", KategoriAdı, Tanımı);
                komut.Connection = baglanti;
                baglanti.Open();
                int etki = komut.ExecuteNonQuery();

                if (etki > 0)
                {
                    MessageBox.Show("Ekleme İşlemi Tamamlandı");
                    kategoriler();

                }
                else
                {
                    MessageBox.Show("Bir Hata Oluştu");
                }
                baglanti.Close();
            }
        }
        private void kategoriler()
        {
            SqlDataAdapter sqlDatadapter = new SqlDataAdapter("select*from Kategoriler", baglanti);
            DataTable dataTable = new DataTable();
            sqlDatadapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void btnSil2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["KategoriID"].Value);
                SqlCommand cmd = new SqlCommand(string.Format("delete Kategoriler where KategoriID={0}", id), baglanti);
                baglanti.Open();
                int etki = cmd.ExecuteNonQuery();

                if (etki > 0)
                {
                    MessageBox.Show("Kategori Silme İşlemi Gerçekleşmiştir");
                    kategoriler();
                }
                else
                {
                    MessageBox.Show("Hata");
                }
                baglanti.Close();
            }
        }
    }
}
