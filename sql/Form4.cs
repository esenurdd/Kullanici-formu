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

namespace sql
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            dataGridView1.CellClick += dataGridView1_CellContentClick;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.EnableHeadersVisualStyles = false; // Önemli!
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.LightGray;
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True");
        private object dataGridView;
        private void Calisanlistele()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM employees", baglanti);
            DataTable dt = new DataTable();
            baglanti.Close();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["EmployeeID"].ReadOnly = true;
        }
        private void BTNLİST_Click(object sender, EventArgs e)
        {
            Calisanlistele();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow satir = dataGridView1.Rows[e.RowIndex];

                textBox2.Text = satir.Cells["EmployeeID"].Value.ToString();
                textBox4.Text = satir.Cells["LastName"].Value.ToString();
                textBox1.Text = satir.Cells["FirstName"].Value.ToString();
                textBox3.Text = satir.Cells["Title"].Value.ToString();
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow satir = dataGridView1.Rows[e.RowIndex];

                textBox2.Text = satir.Cells["EmployeeID"].Value.ToString();
                textBox4.Text = satir.Cells["LastName"].Value.ToString();
                textBox1.Text = satir.Cells["FirstName"].Value.ToString();
                textBox3.Text = satir.Cells["Title"].Value.ToString();

            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)//SENDER: OLAYI TETİKLEYEN NESNE E: İSE OLAY PARAMETRELERİ
        {
            if (e.RowIndex >= 0) //GERÇEK SATIR MI KONTROLÜ YAPAR
            {
                try//HATA ÖNLEME
                {
                    DataGridViewRow satir = dataGridView1.Rows[e.RowIndex];

                    int id = Convert.ToInt32(satir.Cells["EmployeeID"].Value);
                    string yenicalisansoyadı = satir.Cells["LastName"].Value.ToString();
                    string yenicalisanadi = satir.Cells["FirstName"].Value.ToString();
                    string yenicalisangörevi= satir.Cells["Title"].Value.ToString();

                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("UPDATE Employees SET  LastName=@p1 , FirstName=@p2 , Title=@p3 WHERE EmployeeID = @p4", baglanti);
                    komut.Parameters.AddWithValue("@p1", yenicalisansoyadı);
                    komut.Parameters.AddWithValue("@p2", yenicalisanadi);
                    komut.Parameters.AddWithValue("@p3", yenicalisangörevi);
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
        private void Calisanekleme()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO employees (LastName,FirstName,Title) VALUES (@p1,@p2,@p3)", baglanti);
          
            komut.Parameters.AddWithValue("@p1", textBox4.Text);
            komut.Parameters.AddWithValue("@p2", textBox1.Text);
            komut.Parameters.AddWithValue("@p3", textBox3.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kategori Eklendi");
            Calisanlistele();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Calisanekleme();
        }
        private void Calisansilme()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("DELETE FROM Employees WHERE EmployeeID=@p1 and LastName=@p2 and FirstName=@p3 and Title=@p4", baglanti);
            komut.Parameters.AddWithValue("@p1", textBox2.Text);
            komut.Parameters.AddWithValue("@p2", textBox4.Text);
            komut.Parameters.AddWithValue("@p3", textBox1.Text);
            komut.Parameters.AddWithValue("@p4", textBox3.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Çalışan Silindi");
            Calisanlistele();
        }
        private void BTNKTGR_Click(object sender, EventArgs e)
        {
            Calisansilme();
        }
        private void CalisanGuncelle()
        {
            baglanti.Open();

            SqlCommand komut = new SqlCommand("UPDATE Employees SET  LastName=@p1 , FirstName=@p2 , Title=@p3 WHERE EmployeeID = @p4", baglanti);
            komut.Parameters.AddWithValue("@p4", textBox2.Text);
            komut.Parameters.AddWithValue("@p1", textBox4.Text);
            komut.Parameters.AddWithValue("@p2", textBox1.Text);
            komut.Parameters.AddWithValue("@p3", textBox3.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Çalışan Güncellendi");
            Calisanlistele();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            CalisanGuncelle();
        }
        private void CalisanAra()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Employees WHERE LastName LIKE @p1", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@p1", "%" + textBoxAra.Text + "%");
            DataTable dt = new DataTable();
            da.Fill(dt);
            baglanti.Close();
            dataGridView1.DataSource = dt;
        }
        private void buttonAra_Click(object sender, EventArgs e)
        {
            CalisanAra();
        }

        private void textBoxAra_TextChanged(object sender, EventArgs e)
        {
            CalisanAra();
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

        private void button1_Click(object sender, EventArgs e)
        {
            ExcelAktar();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            ExcelAktar();
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            Calisanekleme();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Calisanlistele();
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            Calisansilme();
        }

        private void guna2ImageButton5_Click(object sender, EventArgs e)
        {
            CalisanGuncelle(); 
        }
    }
}
