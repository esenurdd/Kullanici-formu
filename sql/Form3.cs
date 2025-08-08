using ClosedXML.Excel;
using Microsoft.Data.SqlClient;
using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace sql
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
          
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow satir = dataGridView1.Rows[e.RowIndex];

                textBox4.Text = satir.Cells["CustomerID"].Value.ToString();
                textBox1.Text = satir.Cells["CompanyName"].Value.ToString();
                textBox3.Text = satir.Cells["ContactName"].Value.ToString();
                textBox2.Text = satir.Cells["ContactTitle"].Value.ToString();
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)//SENDER: OLAYI TETİKLEYEN NESNE E: İSE OLAY PARAMETRELERİ
        {
            if (e.RowIndex >= 0) //GERÇEK SATIR MI KONTROLÜ YAPAR
            {
                try//HATA ÖNLEME
                {
                    DataGridViewRow satir = dataGridView1.Rows[e.RowIndex];

                    string yenimusteriID = satir.Cells["CustomerID"].Value.ToString();
                    string yensirketAdi = satir.Cells["CompanyName"].Value.ToString();
                    string yeniiletisimadi = satir.Cells["ContactName"].Value.ToString();
                    string yeniiletisimbasligi = satir.Cells["ContactTitle"].Value.ToString();

                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("UPDATE Customers SET CompanyName=@p2 , ContactName=@p3 ,ContactTitle=@p4 WHERE CustomerID = @p1", baglanti);
                    komut.Parameters.AddWithValue("@p1", yenimusteriID);
                    komut.Parameters.AddWithValue("@p2", yensirketAdi);
                    komut.Parameters.AddWithValue("@p3", yeniiletisimadi);
                    komut.Parameters.AddWithValue("@p4", yeniiletisimbasligi);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True");
        private object dataGridView;
        private void Musterilerlistele()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM customers", baglanti);
            DataTable dt = new DataTable();
            baglanti.Close();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["customerID"].ReadOnly = true;
        }
        private void BTNLİST_Click(object sender, EventArgs e)
        {
            Musterilerlistele();
        }
        private void Musteriekleme()
        {
            try
            {
                using (SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True"))
                {
                    baglanti.Open();

                    using (SqlCommand komut = new SqlCommand("INSERT INTO customers (CustomerID,CompanyName, ContactName, ContactTitle) VALUES (@CustomerID,@CompanyName, @ContactName, @ContactTitle)", baglanti))
                    {
                        komut.Parameters.AddWithValue("@CustomerID", textBox4.Text.Trim());
                        komut.Parameters.AddWithValue("@CompanyName", textBox1.Text.Trim());
                        komut.Parameters.AddWithValue("@ContactName", textBox3.Text.Trim());
                        komut.Parameters.AddWithValue("@ContactTitle", textBox2.Text.Trim());

                        komut.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Müşteri başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Musteriekleme();
    
        
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void Musterisilme()
        {
            try
            {
                using (SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True"))
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("DELETE FROM customers WHERE CustomerID=@p1 AND  CompanyName=@p2 AND  ContactName=@p3 AND ContactTitle=@p4", baglanti);
                    komut.Parameters.AddWithValue("@p1", textBox4.Text.Trim());
                    komut.Parameters.AddWithValue("@p2", textBox1.Text.Trim());
                    komut.Parameters.AddWithValue("@p3", textBox3.Text.Trim());
                    komut.Parameters.AddWithValue("@p4", textBox2.Text.Trim());
                    int sonuc = komut.ExecuteNonQuery();

                    if (sonuc > 0)
                        MessageBox.Show("Müşteri başarıyla silindi.");
                    else
                        MessageBox.Show("Silinecek müşteri bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        private void BTNKTGR_Click(object sender, EventArgs e)
        {
            Musterisilme();
        }
        private void MusteriGuncelle()
        {
            baglanti.Open();

            SqlCommand komut = new SqlCommand("UPDATE Customers SET CompanyName=@p2 , ContactName=@p3 ,ContactTitle=@p4 WHERE CustomerID = @p1", baglanti);
            komut.Parameters.AddWithValue("@p1", textBox4.Text);
            komut.Parameters.AddWithValue("@p2", textBox1.Text);
            komut.Parameters.AddWithValue("@p3", textBox3.Text);
            komut.Parameters.AddWithValue("@p4", textBox2.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Müşteri Güncellendi");
            Musterilerlistele();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            MusteriGuncelle();
        }
        private void MusteriAra()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customers WHERE ContactName LIKE @p1", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@p1", "%" + textBoxAra.Text + "%");
            DataTable dt = new DataTable();
            da.Fill(dt);
            baglanti.Close();
            dataGridView1.DataSource = dt;
        }
        private void buttonAra_Click(object sender, EventArgs e)
        {
            MusteriAra();
        }

        private void textBoxAra_TextChanged(object sender, EventArgs e)
        {
            MusteriAra();
        }
        private void ExcelAktar()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Dosyası|*.xlsx";
            sfd.Title = "Excel dosyasını kaydet";
            sfd.FileName = "Veriler.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("Liste");

                        // Sütun başlıklarını yaz
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            ws.Cell(1, i + 1).Value = dataGridView1.Columns[i].HeaderText;
                            ws.Cell(1, i + 1).Style.Font.Bold = true;
                        }

                        // Verileri yaz
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            // Skip if the row is the new row (not yet committed)
                            if (dataGridView1.Rows[i].IsNewRow) continue;

                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                var cellValue = dataGridView1.Rows[i].Cells[j].Value;
                                ws.Cell(i + 2, j + 1).Value = cellValue != null ? cellValue.ToString() : string.Empty;
                            }
                        }

                        ws.Columns().AdjustToContents(); // Sütun genişliklerini otomatik ayarla

                        workbook.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Excel dosyası başarıyla oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ExcelAktar();
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            Musteriekleme();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Musterilerlistele();
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            Musterisilme();
        }

        private void guna2ImageButton5_Click(object sender, EventArgs e)
        {
            MusteriGuncelle();
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            ExcelAktar();
        }
    }
}

