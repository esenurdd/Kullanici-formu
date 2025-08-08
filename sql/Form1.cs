using sql;
using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using Guna.UI2.WinForms;

namespace sql
{
    public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Button1_Click(object sender, EventArgs e)
    {
        
    }

    private void BTNCSTMR_Click(object sender, EventArgs e)
    {
        Form3 yeniform = new Form3();
        yeniform.Show();
    }

    private void BTNKTGR_Click(object sender, EventArgs e)
    {
        Form2 yeniForm = new Form2();
        yeniForm.Show();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        Form4 yeniForm = new Form4();
        yeniForm.Show();
    }

    private void button4_Click(object sender, EventArgs e)
    {
        Form5 yeniForm = new Form5();
        yeniForm.Show();
    }

    private void button5_Click(object sender, EventArgs e)
    {
        Form6 yeniForm = new Form6();
        yeniForm.Show();
    }

    private void button6_Click(object sender, EventArgs e)
    {
        Form7 yeniForm = new Form7();
        yeniForm.Show();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        timer1.Interval = 1000; // 1 saniye
        timer1.Tick += timer1_Tick;
        timer1.Start();

        // İlk yüklemede siparişleri getir
        YeniSiparisleriGetir();
    }

    //private void Timer1_Tick(object sender, EventArgs e)
   // {
       // labelSaat.Text = "Saat: " + DateTime.Now.ToString("HH:mm:ss");
       // labelTarih.Text = "Tarih: " + DateTime.Now.ToString("dd.MM.yyyy");
    

    private void YeniSiparisleriGetir()
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True";
        string query = "SELECT TOP 10 OrderID, OrderDate FROM Orders ORDER BY OrderDate DESC";

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

               
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Yeni siparişler alınamadı: " + ex.Message);
        }
    }

    private void panelRight_Paint(object sender, PaintEventArgs e)
    {
        // Gerekirse buraya kod ekleyebilirsin
    }

    private void labelSaat_Click(object sender, EventArgs e)
    {
        // Gerekirse buraya kod ekleyebilirsin
    }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Form2 yeniForm = new Form2();
            yeniForm.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Form3 yeniform = new Form3();
            yeniform.Show();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Form4 yeniForm = new Form4();
            yeniForm.Show();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Form5 yeniForm = new Form5();
            yeniForm.Show();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form6 yeniForm = new Form6();
            yeniForm.Show();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Form7 yeniForm = new Form7();
            yeniForm.Show();
        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void lblsaat_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblsaat.Text = "Saat: " + DateTime.Now.ToString("HH:mm:ss");
        }
    }
}


