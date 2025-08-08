using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace sql
{
    public partial class Form6 : Form
    {
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True";

        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddMonths(-1);
            dateTimePicker2.Value = DateTime.Now;
            KasaListele();
        }

        private void KasaListele()
        {
            string sorgu = @"SELECT 
                                o.OrderID, 
                                c.CompanyName AS Musteri, 
                                e.FirstName + ' ' + e.LastName AS Calisan,
                                o.OrderDate,
                                SUM(od.UnitPrice * od.Quantity) AS ToplamTutar
                            FROM Orders o
                            INNER JOIN [Order Details] od ON o.OrderID = od.OrderID
                            INNER JOIN Customers c ON o.CustomerID = c.CustomerID
                            INNER JOIN Employees e ON o.EmployeeID = e.EmployeeID
                            GROUP BY o.OrderID, c.CompanyName, e.FirstName, e.LastName, o.OrderDate
                            ORDER BY o.OrderDate DESC";

            using (var baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();
                    var da = new SqlDataAdapter(sorgu, baglanti);
                    var dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    ToplamGeliriHesapla(dt);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Genel hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ToplamGeliriHesapla(DataTable dt)
        {
            try
            {
                decimal toplam = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (row["ToplamTutar"] != DBNull.Value)
                    {
                        toplam += Convert.ToDecimal(row["ToplamTutar"]);
                    }
                }
                label3.Text = $"Toplam Gelir: ₺{toplam:N2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Toplam hesaplama hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void KasaFiltrele()
        {
            DateTime baslangic = dateTimePicker1.Value.Date;
            DateTime bitis = dateTimePicker2.Value.Date.AddDays(1).AddSeconds(-1);

            string sorgu = @"SELECT 
                                o.OrderID, 
                                c.CompanyName AS Musteri, 
                                e.FirstName + ' ' + e.LastName AS Calisan,
                                o.OrderDate,
                                SUM(od.UnitPrice * od.Quantity) AS ToplamTutar
                            FROM Orders o
                            INNER JOIN [Order Details] od ON o.OrderID = od.OrderID
                            INNER JOIN Customers c ON o.CustomerID = c.CustomerID
                            INNER JOIN Employees e ON o.EmployeeID = e.EmployeeID
                            WHERE o.OrderDate BETWEEN @t1 AND @t2
                            GROUP BY o.OrderID, c.CompanyName, e.FirstName, e.LastName, o.OrderDate
                            ORDER BY o.OrderDate DESC";

            using (var baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();
                    var komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@t1", baslangic);
                    komut.Parameters.AddWithValue("@t2", bitis);

                    var da = new SqlDataAdapter(komut);
                    var dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    ToplamGeliriHesapla(dt);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Genel hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExcelAktar()
        {
            try
            {
                var dt = (DataTable)dataGridView1.DataSource;
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Aktarılacak veri bulunamadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel Dosyası|*.xlsx";
                    sfd.FileName = "KasaRaporu.xlsx";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add(dt, "Kasa");
                            ws.Columns().AdjustToContents(); // Sütun genişliklerini otomatik ayarla
                            wb.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("Excel dosyası başarıyla oluşturuldu!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel aktarım hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            KasaListele();
        }

        private void btnFiltrele_Click(object sender, EventArgs e)
        {
            KasaFiltrele();
        }

        private void btnExcelAktar_Click(object sender, EventArgs e)
        {
            ExcelAktar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            KasaListele();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            KasaListele();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KasaFiltrele();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExcelAktar();
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}