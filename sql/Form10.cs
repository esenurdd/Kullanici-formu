using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace sql
{
    public partial class Form10 : Form
    {
     
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True";

        public Form10()
        {
            InitializeComponent();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
           
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string email = guna2TextBox1.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Lütfen e-posta adresinizi girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string resetCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Employees SET ResetCode = @ResetCode WHERE Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ResetCode", resetCode);
                        cmd.Parameters.AddWithValue("@Email", email);

                        int updated = cmd.ExecuteNonQuery();

                        if (updated > 0)
                        {
                            guna2TextBox2.Text = resetCode;
                            MessageBox.Show($"Şifre sıfırlama kodunuz: {resetCode}", "Kod Gönderildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Bu e-posta adresine kayıtlı kullanıcı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                string email = guna2TextBox1.Text.Trim().ToLower();
                string code = guna2TextBox2.Text.Trim().ToUpper();
                string newPassword = guna2TextBox3.Text.Trim();

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(newPassword))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Employees SET Password = @NewPassword, ResetCode = NULL WHERE Email = @Email AND ResetCode = @Code";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Code", code);

                        int updated = cmd.ExecuteNonQuery();

                        if (updated > 0)
                        {
                            MessageBox.Show("Şifreniz başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("E-posta veya kod hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }
    }
}
