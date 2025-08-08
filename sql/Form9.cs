using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace sql
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }

        private void Form9_Load(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
            
                string firstName = guna2TextBox1.Text.Trim();
            string lastName = guna2TextBox2.Text.Trim();
            string email = guna2TextBox3.Text.Trim().ToLower();
            string password = guna2TextBox4.Text.Trim();

            if (firstName == "" || lastName == "" || email == "" || password == "")
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Aynı email ile kullanıcı var mı kontrolü
                string kontrolQuery = "SELECT COUNT(*) FROM Employees WHERE Email = @Email";
                using (SqlCommand cmdCheck = new SqlCommand(kontrolQuery, conn))
                {
                    cmdCheck.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmdCheck.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Bu email ile kayıtlı bir kullanıcı zaten var.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Kayıt işlemi
                string insertQuery = "INSERT INTO Employees (FirstName, LastName, Email, Password, HireDate, Title) " +
                                     "VALUES (@FirstName, @LastName, @Email, @Password, GETDATE(), 'User')";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Kayıt başarılı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close(); // veya login formuna yönlendir
                    }
                    else
                    {
                        MessageBox.Show("Kayıt sırasında hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
