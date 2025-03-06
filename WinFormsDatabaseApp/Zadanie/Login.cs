using System;
using System.Data;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace Zadanie
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public string PrihlasovacieMeno
        {
            get { return tbLogin.Text; }
        }

        public string Heslo
        {
            get { return tbPassword.Text; }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                
                string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=147.175.137.84)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=student)));User Id=admin;Password=admin;";
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    
                    string sql = "SELECT COUNT(*) FROM USERS WHERE LOGIN = :Login AND PASSWORD = :Password";
                    using (OracleCommand cmd = new OracleCommand(sql, connection))
                    {
                        cmd.Parameters.Add(new OracleParameter(":Login", tbLogin.Text));
                        cmd.Parameters.Add(new OracleParameter(":Password", tbPassword.Text));

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 1)
                        {
                            this.DialogResult = DialogResult.OK; 
                        }
                        else
                        {
                            MessageBox.Show("Zadané údaje sú nesprávne.", "Chyba");
                            this.DialogResult = DialogResult.None; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba pri overovaní údajov: {ex.Message}", "Chyba");
                this.DialogResult = DialogResult.None;
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                Application.Exit();  
            }
        }
    }
}

