using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_PY1_201700988
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            archivoToolStripMenuItem.ForeColor = Color.White;
            ayudaToolStripMenuItem.ForeColor = Color.White;
            abrirToolStripMenuItem.ForeColor = Color.White;
            nuevoToolStripMenuItem.ForeColor = Color.White;
            guardarComoToolStripMenuItem.ForeColor = Color.White;
            guardarToolStripMenuItem.ForeColor = Color.White;

            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MyColorTable());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        
        }

        private void archivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }        

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addTabPage();
            getFile();

        }

        private class MyColorTable : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground
            {
                get
                {
                    return Color.FromArgb(70, 70, 70);
                }
            } 

            public override Color ImageMarginGradientBegin
            {
                get
                {
                    return Color.FromArgb(70, 70, 70);
                }
            }
            

            public override Color ImageMarginGradientMiddle
            {
                get
                {
                    return Color.FromArgb(70, 70, 70);
                }
            }

            public override Color ImageMarginRevealedGradientEnd
            {
                get
                {
                    return Color.FromArgb(70, 70, 70);
                }
            }

            public override Color MenuItemBorder
            {
                get
                {
                    return Color.FromArgb(70, 70, 70);
                }
            }

            public override Color MenuBorder
            {
                get
                {
                    return Color.FromArgb(70, 70, 70);
                }
            }

            public override Color MenuItemSelected
            {
                get
                {
                    return Color.FromArgb(100, 100, 100);
                }
            }

            public override Color MenuStripGradientBegin
            {
                get
                {
                    return Color.FromArgb(70, 70, 70);
                }
            }

            public override Color MenuStripGradientEnd
            {
                get
                {
                    return Color.FromArgb(70, 70, 70);
                }
            }

            public override Color MenuItemSelectedGradientBegin
            {
                get
                {
                    return Color.FromArgb(100, 100, 100);
                }
            }

            public override Color MenuItemSelectedGradientEnd
            {
                get
                {
                    return Color.FromArgb(100, 100, 100);
                }
            }

            public override Color MenuItemPressedGradientBegin
            {
                get
                {
                    return Color.FromArgb(70, 70, 70);
                }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get
                {
                    return Color.FromArgb(70, 70, 70);
                }
            }
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsMethod();
        }

        private void saveAsMethod()
        {
            Control controlBox;
            string rutaString = "";

            if (tabControl1.SelectedTab.HasChildren)
            {
                foreach (Control item in tabControl1.SelectedTab.Controls)
                {
                    controlBox = item;

                    if (controlBox is RichTextBox)
                    {

                        SaveFileDialog saveFile = new SaveFileDialog()
                        {
                            Title = "Seleccione la ruta",
                            Filter = "Archivo Compiladores 1 | *.er",
                            AddExtension = true,
                        };

                        var result = saveFile.ShowDialog();

                        if(result == DialogResult.OK)
                        {
                            StreamWriter writer = new StreamWriter(saveFile.FileName);
                            writer.Write(controlBox.Text);
                            writer.Close();

                            String[] ruta = saveFile.FileName.Split('\\');
                            rutaString = saveFile.FileName;
                            tabControl1.SelectedTab.ToolTipText = saveFile.FileName;

                            tabControl1.SelectedTab.Text = ruta[ruta.Length - 1];
                        }                        
                    }
                }
            }
        }


        private void saveMethod()
        {
            Control controlBox;
            Control richText = null;           

            if (tabControl1.SelectedTab.HasChildren)
            {
                foreach (Control item in tabControl1.SelectedTab.Controls)
                {
                    controlBox = item;

                    if  (controlBox is RichTextBox)
                    {
                        richText = item;
                    }


                }
                StreamWriter writer = new StreamWriter(tabControl1.SelectedTab.ToolTipText);
                writer.Write(richText.Text);
                writer.Close();
            }
            else
            {
                mostrarError();
            }
        }


        private void mostrarError()
        {
            MessageBox.Show("Error al guardar el archivo", "Error al guardar",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        private void getFile()
        {
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Title = "Seleccionar Archivo",
                Filter = "Archivo Compiladores 1 | *.er",
                DefaultExt = "er"
            };
            var result = openFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                StreamReader read = new StreamReader(openFile.FileName);

                Control controlBox;

                if (tabControl1.SelectedTab.HasChildren)
                {
                    foreach (Control item in tabControl1.SelectedTab.Controls)
                    {
                        controlBox = item;

                        if (controlBox is RichTextBox)
                        {
                            controlBox.Text = read.ReadToEnd();
                            read.Close();
                        }
                    }
                }
                
                String[] ruta = openFile.FileName.Split('\\');
                tabControl1.SelectedTab.Text = ruta[ruta.Length - 1];
                tabControl1.SelectedTab.ToolTipText = openFile.FileName;

            }

        }
                

        private void addTabPage()
        {
            int numPage = tabControl1.TabPages.Count + 1;
            TabPage newPage = new TabPage("Untitled_"+numPage);

            RichTextBox textBox = new RichTextBox();
            textBox.SetBounds(50,0,594,295);
            textBox.BackColor = Color.FromArgb(70, 70, 70);
            textBox.ForeColor = Color.White;
            textBox.Font = new Font("Tahoma", 10, FontStyle.Regular);
            textBox.WordWrap = false;

            PictureBox num = new PictureBox();
            num.SetBounds(0, 0, 54, 291);
            num.BackColor = Color.FromArgb(50, 50, 50);

            newPage.Controls.Add(textBox);
            newPage.Controls.Add(num);

            tabControl1.TabPages.Add(newPage);
            tabControl1.SelectedTab = newPage;
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            if(tabControl1.SelectedTab.Text.Equals("Untitled"))
            {
                saveAsMethod();
            }
            else
            {
                saveMethod();
            }
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addTabPage();
        }
    }
}
