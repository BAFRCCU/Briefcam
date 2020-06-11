using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;

namespace WindowsFormsApp3
{
    public partial class Excellebrite : Form
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().ManifestModule.FullyQualifiedName);

            

    public Excellebrite()
        {
            InitializeComponent();
            FillDossiers(); 

        }

        private void panel4_DragDrop(object sender, DragEventArgs e)
        {
            //panel4.BackColor = Color.WhiteSmoke;
            if (!CheckDataFilled())
            {
                return;
            }
                

            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            //if (comboBox1.SelectedItem == null)
            //{
            //    MessageBox.Show("Veuillez choisir un ZP ou PJF CHA ");
            //    comboBox1.BackColor = Color.Red;
            //    return;
            //}
            //else
            //    comboBox1.BackColor = Color.WhiteSmoke;

            FileInfo fichier = new FileInfo(fileList[0]);
            string rootPath = "";
            try
            {

                
                if (config.AppSettings.Settings["RootPath"].Value != "NULL")
                {

                    rootPath = config.AppSettings.Settings["RootPath"].Value;

                }


                string nomDossier = "";
                if (dossierPJF.Text != "" && comboBox1.SelectedIndex == 3 && (comboBox2.SelectedIndex == -1 || comboBox2.SelectedIndex == 0))
                {

                    nomDossier = dossierPJF.Text.ToUpper();
                    //if (!Directory.Exists(rootPath + comboBox1.SelectedItem.ToString() + @"\" + nomDossier))
                    //{
                    //    Directory.CreateDirectory(rootPath + comboBox1.SelectedItem.ToString() + @"\" + nomDossier);
                    //}


                }
                else
                    if (comboBox1.SelectedIndex == 3 && comboBox2.SelectedIndex != -1 )
                {
                    nomDossier = comboBox2.SelectedItem.ToString();
                    comboBox2.BackColor = Color.WhiteSmoke;
                }
                else
                if (comboBox1.SelectedIndex == 3 && comboBox2.SelectedIndex == -1)
                {
                    comboBox2.BackColor = Color.Red;
                    MessageBox.Show("Veuillez choisir un dossier dans la liste ");
                    return;
                }


                string nomFichier = textBox1.Text + "-" + textBox2.Text + "-" + textBox3.Text;
                if(textBox4.Text != "")
                    nomFichier += "-" + textBox4.Text;

                if (nomDossier == "")
                {
                    if (File.Exists(@rootPath + comboBox1.SelectedItem + @"\" + nomFichier + fichier.Extension))
                    {
                        return;
                    }

                    if(MessageBox.Show("Etes-vous certain de copier ce fichier " + rootPath + comboBox1.SelectedItem + @"\" + nomFichier + fichier.Extension, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        File.Copy(fileList[0], rootPath + comboBox1.SelectedItem + @"\" + nomFichier + fichier.Extension);
                        label5.Text = @"\\192.168.91.242\Ft\MicroILP-GSM\" + comboBox1.SelectedItem + @"\" + nomFichier + fichier.Extension;
                        status.Visible = true;
                        status.Text = "LE FICHIER A ETE COPIE";

                        panel4.BackColor = Color.Green;
                        panel4.Refresh();
                    }
                    
                }
                else
                {
                    if (File.Exists(@rootPath + comboBox1.SelectedItem + @"\" + nomDossier + @"\" + nomFichier + fichier.Extension))
                    {
                        return;
                    }

                    if (MessageBox.Show("Etes-vous certain de copier ce fichier " + rootPath + comboBox1.SelectedItem + @"\" + nomDossier + @"\" + nomFichier + fichier.Extension, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        if (!Directory.Exists(rootPath + comboBox1.SelectedItem.ToString() + @"\" + nomDossier))
                            Directory.CreateDirectory(rootPath + comboBox1.SelectedItem.ToString() + @"\" + nomDossier);                        


                        File.Copy(fileList[0], rootPath + comboBox1.SelectedItem + @"\" + nomDossier + @"\" + nomFichier + fichier.Extension);
                        label5.Text = @"\\192.168.91.242\Ft\MicroILP-GSM\" + comboBox1.SelectedItem + @"\" + nomDossier + @"\" + nomFichier + fichier.Extension;

                        status.Visible = true;
                        status.Text = "LE FICHIER A ETE COPIE";
                        panel4.BackColor = Color.Green;
                        panel4.Refresh();
                    }
                    
                }

                               

              
            }
            catch (Exception ex)
            {
                panel4.BackColor = Color.Red;
                status.Visible = true;
                status.Text = "ERREUR : LE FICHIER N'A PAS ETE COPIE.\n " + ex.Message;
            }

            
         }

        private string MoveTo(string [] files)
        {
                        
            return "OK";
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_MouseLeave(object sender, EventArgs e)
        {
            panel4.BackColor = Color.WhiteSmoke;
            //textBox1.Text = "";
            //textBox2.Text = "";
            //textBox3.Text = "";
            //textBox4.Text = "";
            //comboBox1.SelectedIndex = -1;
            //comboBox2.SelectedIndex = -1;
            //dossierPJF.Text = "";
            status.Visible= false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dossierPJF.Text = "";
            if (comboBox1.SelectedIndex == 3)
                panel1.Visible = true;
            else
                panel1.Visible = false;
        }

        private bool FillDossiers()
        {
            string rootPath = config.AppSettings.Settings["RootPath"].Value;
            if(Directory.Exists(rootPath + "\\PJF CHA\\"))
            {
                foreach (string dir in Directory.GetDirectories(rootPath + "\\PJF CHA\\"))
                {
                    DirectoryInfo direct = new DirectoryInfo(dir);
                    comboBox2.Items.Add(direct.Name);
                }

                return true;
            }
            else
            {
                if(MessageBox.Show("Le chemin vers " + rootPath + " est inaccessible.\n\nVeuillez vérifier vos connexions réseau", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)== DialogResult.OK)
                    this.Close();
                return false;
            }
            
        }

        private bool CheckDataFilled()
        {
            bool filled = true;
            string message = "";
            foreach(Control c in this.Controls)
            {
                
                if (c.GetType() == typeof(MaskedTextBox))
                {
                    if (((MaskedTextBox)c).Text == "")
                    {
                        ((MaskedTextBox )c).BackColor = Color.Red;
                        filled = false;
                        if(message == "")
                        {
                            message = "Veuillez remplir les champs en rouge\n";
                        }
                    }
                    else
                    {
                        ((MaskedTextBox)c).BackColor = Color.White;
                        textBox1.Text = FillYear(textBox2.Text);

                    }
                }

                if (c.GetType() == typeof(ComboBox))
                {
                    if (((ComboBox)c).SelectedItem == null)
                    {
                        message += "Veuillez choisir une ZP ou PJF CHA\n";
                        filled = false;
                        ((ComboBox)c).BackColor = Color.Red;

                    }
                    else
                        ((ComboBox)c).BackColor = Color.WhiteSmoke;

                }

                
            }

            if (!filled)
            {
                MessageBox.Show(message);
            }

            return filled;
        }

        private string FillYear(string year)
        {
            int caseNumber = 2020;
            bool converted = Int32.TryParse(year, out caseNumber);

            if (converted)
            {
                int case_number = caseNumber;

                if (case_number <= 5641)
                    return "2016";

                if (case_number <= 6446)
                    return "2017";

                if (case_number <= 7407)
                    return "2018";

                if (case_number <= 8425)
                    return "2019";

                return "2020";


            }

            return "2020";
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            textBox1.Text = FillYear(textBox2.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel4.BackColor = Color.WhiteSmoke;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            label5.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            dossierPJF.Text = "";
            comboBox1.Focus();
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            textBox2.SelectionStart = 0;
        }

        private void textBox3_MouseClick(object sender, MouseEventArgs e)
        {
            textBox3.SelectionStart = 0;
        }

        private void textBox4_MouseClick(object sender, MouseEventArgs e)
        {
            textBox4.SelectionStart = 0;
        }

        private void dossierPJF_MouseClick(object sender, MouseEventArgs e)
        {
            dossierPJF.SelectionStart = 0;
        }
    }
    }

