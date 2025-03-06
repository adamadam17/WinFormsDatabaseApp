using System;
using System.Data;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Zadanie
{
    public partial class Form1 : Form
    {
        private OracleConnection spojenie;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
            spojenie = new OracleConnection();
            bool prihlaseny = false;

            
            while (!prihlaseny)
            {
                try
                {
                    
                    Login frmLogin = new Login();
                    DialogResult odpoved = frmLogin.ShowDialog();

                    
                    if (odpoved != DialogResult.OK)
                    {
                        Application.Exit();
                        return;
                    }

                    
                    spojenie.ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=147.175.137.84)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=student)));User Id=" + frmLogin.tbLogin.Text + ";Password=" + frmLogin.tbPassword.Text + ";";

                    
                    spojenie.Open();

                    
                    if (spojenie.State == ConnectionState.Open)
                    {
                        prihlaseny = true;
                        this.Text += $" ({frmLogin.tbLogin.Text})";
                        MessageBox.Show("Prihlásenie úspešné.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (OracleException)
                {
                    
                    MessageBox.Show("Nesprávne prihlasovacie údaje. Skúste znova.", "Chyba prihlásenia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show($"Neočakávaná chyba: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        
        private void NacitatTabulku()
        {
            try
            {
                
                if (spojenie.State != ConnectionState.Open)
                {
                    MessageBox.Show("Spojenie s databázou nie je otvorené.", "Chyba spojenia");
                    return;
                }

                
                string sql = "SELECT ID, ZNACKA, MODEL, POCET_KM, OBSADENOST FROM VOZIDLO";
                OracleDataAdapter adapter = new OracleDataAdapter(sql, spojenie);

                
                DataTable tabulka = new DataTable();
                adapter.Fill(tabulka);

                
                if (tabulka.Rows.Count == 0)
                {
                    MessageBox.Show("Tabuľka neobsahuje žiadne záznamy.");
                }

                
                dataGridView1.AutoGenerateColumns = true;

                
                dataGridView1.DataSource = tabulka;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba pri načítaní údajov: {ex.Message}", "Chyba pri načítaní údajov");
            }
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            
            NacitatTabulku();
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            
            FormPridatVozidlo formPridatVozidlo = new FormPridatVozidlo();

            
            if (formPridatVozidlo.ShowDialog() == DialogResult.OK)
            {
                
                string znacka = formPridatVozidlo.Znacka;
                string model = formPridatVozidlo.Model;
                int pocetKm = formPridatVozidlo.PocetKm;
                string obsadenost = formPridatVozidlo.Obsadenost;

                
                PridatZaznam(znacka, model, pocetKm, obsadenost);
            }
        }

        
        private void PridatZaznam(string znacka, string model, int pocetKm, string obsadenost)
        {
            try
            {
                if (spojenie.State != ConnectionState.Open)
                {
                    MessageBox.Show("Spojenie s databázou nie je otvorené.", "Chyba spojenia");
                    return;
                }

                
                string sqlInsert = "INSERT INTO VOZIDLO (ZNACKA, MODEL, POCET_KM, OBSADENOST) " +
                                   "VALUES (:Znacka, :Model, :PocetKm, :Obsadenost)";

                using (OracleCommand command = new OracleCommand(sqlInsert, spojenie))
                {
                    
                    command.Parameters.Add(new OracleParameter(":Znacka", znacka));
                    command.Parameters.Add(new OracleParameter(":Model", model));
                    command.Parameters.Add(new OracleParameter(":PocetKm", pocetKm));
                    command.Parameters.Add(new OracleParameter(":Obsadenost", obsadenost));

                    
                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Záznam bol pridaný úspešne.");
                        NacitatTabulku(); 
                    }
                    else
                    {
                        MessageBox.Show("Nepodarilo sa pridať záznam.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba pri pridávaní záznamu");
            }
        }
        private void OdstranitVozidlo()
        {
            try
            {
                
                if (spojenie.State != ConnectionState.Open)
                {
                    MessageBox.Show("Spojenie s databázou nie je otvorené.", "Chyba spojenia");
                    return;
                }

                
                string deleteQuery = "DELETE FROM VOZIDLO WHERE ID = :ID";

                using (OracleCommand cmd = new OracleCommand(deleteQuery, spojenie))
                {
                    
                    cmd.Parameters.Add(":ID", OracleDbType.Int32).Value = int.Parse(textBox1.Text);

                    
                    int rowsAffected = cmd.ExecuteNonQuery();

                    
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Záznam bol úspešne odstránený.");
                        NacitatTabulku(); 
                    }
                    else
                    {
                        MessageBox.Show("Záznam s týmto ID neexistuje.");
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Chyba pri odstraňovaní záznamu: " + ex.Message);
            }
            catch (FormatException)
            {
                MessageBox.Show("Zadajte platné ID (číslo).");
            }
        }


        
        private void buttonOdstranit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Zadajte ID vozidla na odstránenie.");
                return;
            }

            
            OdstranitVozidlo();
        }
    }
}


