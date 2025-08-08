using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ClosedXML.Excel;
using System.IO;

//excel için eklenilen kütüphane eeplus

namespace sql
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True");
        private object dataGridView;


        private void Kategorilistele()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM categories", baglanti);
            DataTable dt = new DataTable();
            baglanti.Close();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["CategoryID"].ReadOnly = true;
        }

        private void BTNLİST_Click(object sender, EventArgs e)
        {
            Kategorilistele();
        }

        private void Kategoriekleme()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO categories (CategoryName, Description) VALUES (@p1,@p2)", baglanti);
            komut.Parameters.AddWithValue("@p1", textBox1.Text);
            komut.Parameters.AddWithValue("@p2", textBox2.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kategori Eklendi");
            Kategorilistele();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Kategoriekleme();
        }
        private void Kategorisilme()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("DELETE FROM categories WHERE CategoryName=@p1 and Description=@p2", baglanti);
            komut.Parameters.AddWithValue("@p1", textBox1.Text);
            komut.Parameters.AddWithValue("@p2", textBox2.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kategori Silindi");
            Kategorilistele();
        }
        private void BTNKTGR_Click(object sender, EventArgs e)
        {
            Kategorisilme();
        }
        private void KategoriGuncelle()
        {
            baglanti.Open();

            SqlCommand komut = new SqlCommand("UPDATE Categories SET CategoryName = @p1, Description = @p2 WHERE CategoryID = @p3", baglanti);
            komut.Parameters.AddWithValue("@p1", textBox1.Text);
            komut.Parameters.AddWithValue("@p2", textBox2.Text);
            komut.Parameters.AddWithValue("@p3", Convert.ToInt32(textBox3.Text));
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kategori Güncellendi");
            Kategorilistele();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            KategoriGuncelle();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow satir = dataGridView1.Rows[e.RowIndex];

                textBox3.Text = satir.Cells["CategoryID"].Value.ToString();
                textBox1.Text = satir.Cells["CategoryName"].Value.ToString();
                textBox2.Text = satir.Cells["Description"].Value.ToString();
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)//SENDER: OLAYI TETİKLEYEN NESNE E: İSE OLAY PARAMETRELERİ
        {
            if (e.RowIndex >= 0) //GERÇEK SATIR MI KONTROLÜ YAPAR
            {
                try//HATA ÖNLEME
                {
                    DataGridViewRow satir = dataGridView1.Rows[e.RowIndex];

                    int id = Convert.ToInt32(satir.Cells["CategoryID"].Value);
                    string yeniKategoriAdi = satir.Cells["CategoryName"].Value.ToString();
                    string yeniAciklama = satir.Cells["Description"].Value.ToString();

                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("UPDATE Categories SET CategoryName=@ad, Description=@aciklama WHERE CategoryID=@id", baglanti);
                    komut.Parameters.AddWithValue("@ad", yeniKategoriAdi);
                    komut.Parameters.AddWithValue("@aciklama", yeniAciklama);
                    komut.Parameters.AddWithValue("@id", id);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Güncelleme yapıldı");
                }
                catch (Exception ex)//HATA ÖNLEME 
                {
                    MessageBox.Show("Güncelleme Hatası: " + ex.Message);
                    baglanti.Close();
                }
            }
        }
        private void KategoriAra()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Categories WHERE CategoryName LIKE @p1", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@p1", "%" + textBoxAra.Text + "%");
            DataTable dt = new DataTable();
            da.Fill(dt);
            baglanti.Close();
            dataGridView1.DataSource = dt;
        }
        private void buttonAra_Click(object sender, EventArgs e)
        {
            KategoriAra();
        }

        private void textBoxAra_TextChanged(object sender, EventArgs e)
        {
            KategoriAra();
        }

        private void ExcelAktar()
        {
            // 1. Kaydetme penceresini aç
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Dosyası|*.xlsx";
            sfd.Title = "Excel dosyasını kaydet";
            sfd.FileName = "Veriler.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 2. DataGridView'in DataSource'unu DataTable olarak al
                    DataTable dt = (DataTable)dataGridView1.DataSource;

                    // 3. Eğer DataTable boşsa uyarı ver
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Excel'e aktarılacak veri bulunamadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 4. Yeni bir Excel dosyası oluştur
                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("MüşteriListesi");

                        // 5. Sütun başlıklarını yaz (Excel'in ilk satırına)
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            ws.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                            ws.Cell(1, i + 1).Style.Font.Bold = true; // Kalın yaz
                        }

                        // 6. Tüm satırları Excel'e yaz
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                ws.Cell(i + 2, j + 1).Value = dt.Rows[i][j]?.ToString(); // Null kontrolü
                            }
                        }

                        // 7. Sütun genişliklerini otomatik ayarla
                        ws.Columns().AdjustToContents();

                        // 8. Dosyayı kaydet
                        workbook.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Excel dosyası başarıyla oluşturuldu!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        
        }

        private void btnExcelAktar_Click(object sender, EventArgs e)
        {
            ExcelAktar();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Kategorilistele();
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            Kategoriekleme();
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            Kategorisilme();
        }

        private void guna2ImageButton5_Click(object sender, EventArgs e)
        {
            KategoriGuncelle();
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            ExcelAktar();
        }

        private void guna2DataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}








//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True