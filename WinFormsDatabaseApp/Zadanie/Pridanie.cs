using System;
using System.Windows.Forms;

namespace Zadanie
{
    public partial class FormPridatVozidlo : Form
    {
        
        public string Znacka
        {
            get { return textBox1.Text; }
        }

        public string Model
        {
            get { return textBox2.Text; }
        }

        public int PocetKm
        {
            get
            {
                int km;
                if (int.TryParse(textBox3.Text, out km))
                {
                    return km;
                }
                else
                {
                    MessageBox.Show("Zadajte platný počet kilometrov.");
                    return 0;
                }
            }
        }

        public string Obsadenost
        {
            get { return comboBox1.SelectedItem.ToString(); }
        }

        public FormPridatVozidlo()
        {
            InitializeComponent();

            
            comboBox1.Items.Add("Dostupné");
            comboBox1.Items.Add("Obsadené");

            
            comboBox1.SelectedIndex = 0; 
        }

        
        private void buttonPridat_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(textBox1.Text) ||
                string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text) ||
                comboBox1.SelectedIndex == -1) 
            {
                MessageBox.Show("Všetky polia musia byť vyplnené.");
                return;
            }

            
            this.DialogResult = DialogResult.OK;

            
            this.Close();
        }
    }
}


