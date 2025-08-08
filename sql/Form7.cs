using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace sql
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=northwind;Integrated Security=True");
        private object dataGridView;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Aylık veya Yıllık bilgisi
            string selectedPeriod = comboBox1.SelectedItem.ToString();
            // Başlangıç ve bitiş tarihleri
            DateTime startDate = dateTimePicker1.Value.Date;
            DateTime endDate = dateTimePicker2.Value.Date;           
            string groupFormat = selectedPeriod == "Aylık" ? "yyyy-MM" : "yyyy";
            // Grafiği çizer
            LoadSalesChart(startDate, endDate, groupFormat);

        }
        private void LoadSalesChart(DateTime startDate, DateTime endDate, string groupFormat)
        {
            string query = $@"
               SELECT FORMAT(OrderDate, '{groupFormat}') AS Period,
               SUM(Quantity) AS TotalQuantity
               FROM Orders o
               JOIN [Order Details] od ON o.OrderID = od.OrderID
               WHERE OrderDate BETWEEN @startDate AND @endDate
               GROUP BY FORMAT(OrderDate, '{groupFormat}')
               ORDER BY Period";

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(baglanti.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Satış verileri yüklenirken hata oluştu: " + ex.Message);
                return;
            }

            chart3.Series.Clear();
            Series series = new Series("Satış Adedi") { ChartType = SeriesChartType.Pie };

            foreach (DataRow row in dt.Rows)
            {
                string period = row["Period"].ToString();
                int qty = Convert.ToInt32(row["TotalQuantity"]);
                series.Points.AddXY(period, qty);
            }

            chart3.Series.Add(series);
            dataGridView3.DataSource = dt;
        }
        private string GetPeriodFormat()
        {
            if (comboBox1.SelectedItem == null)
                return "yyyy-MM"; // Varsayılan aylık

            string selected = comboBox1.SelectedItem.ToString();

            if (selected == "Yıllık")
                return "yyyy";
            else
                return "yyyy-MM";
        }


        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            UpdateCurrentTabData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            UpdateCurrentTabData();
        }
        private void UpdateCurrentTabData()
        {
            DateTime startDate = dateTimePicker1.Value.Date;
            DateTime endDate = dateTimePicker2.Value.Date;
            string groupFormat = GetPeriodFormat();

            if (tabControl1.SelectedTab == tabPage1)
                LoadSalesChart(startDate, endDate, groupFormat);
          
            else if (tabControl1.SelectedTab == tabPage3)
                LoadCustomerChart(startDate, endDate, groupFormat);
        }
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Form7_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Aylık");
            comboBox1.Items.Add("Yıllık");
            comboBox1.SelectedIndex = 0;
            DateTime startDate = dateTimePicker1.Value.Date;
            DateTime endDate = dateTimePicker2.Value.Date;
            string groupFormat = GetPeriodFormat();
            LoadSalesChart(startDate, endDate, groupFormat);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)

        {
            DateTime startDate = dateTimePicker1.Value.Date;
            DateTime endDate = dateTimePicker2.Value.Date;
            string groupFormat = GetPeriodFormat();

            if (tabControl1.SelectedTab == tabPage1)
                LoadSalesChart(startDate, endDate, groupFormat);
           
            else if (tabControl1.SelectedTab == tabPage3)
                LoadCustomerChart(startDate, endDate, groupFormat);


        }
        private void LoadPerformanceChart(DateTime startDate, DateTime endDate, string groupFormat)
        {
            string query = @"
             SELECT 
             e.FirstName + ' ' + e.LastName AS EmployeeName,
             COUNT(o.OrderID) AS TotalOrders
             FROM Orders o
             JOIN Employees e ON o.EmployeeID = e.EmployeeID
             WHERE o.OrderDate BETWEEN @startDate AND @endDate
             GROUP BY e.FirstName, e.LastName
             ORDER BY TotalOrders DESC";

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(baglanti.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Çalışan performans verileri yüklenirken hata oluştu: " + ex.Message);
                return;
            }

           

            
        }

        private void LoadCustomerChart(DateTime startDate, DateTime endDate, string groupFormat)
        {
            string query = $@"
             SELECT FORMAT(OrderDate, '{groupFormat}') AS Period,
             COUNT(DISTINCT CustomerID) AS UniqueCustomers
             FROM Orders
             WHERE OrderDate BETWEEN @startDate AND @endDate
             GROUP BY FORMAT(OrderDate, '{groupFormat}')
             ORDER BY Period";

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(baglanti.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Müşteri verileri yüklenirken hata oluştu: " + ex.Message);
                return;
            }

            chart1.Series.Clear();
            Series series = new Series("Benzersiz Müşteri Sayısı") { ChartType = SeriesChartType.Pie };

            foreach (DataRow row in dt.Rows)
            {
                string period = row["Period"].ToString();
                int count = Convert.ToInt32(row["UniqueCustomers"]);
                series.Points.AddXY(period, count);
            }

            chart1.Series.Add(series);
            dataGridView1.DataSource = dt;
        }
    }

}

