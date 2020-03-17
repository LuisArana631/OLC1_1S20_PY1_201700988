using OLC1_PY1_201700988.Estructuras;
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
        int caracter = 0;

        public Form1()
        {
            InitializeComponent();
            archivoToolStripMenuItem.ForeColor = Color.White;
            ayudaToolStripMenuItem.ForeColor = Color.White;
            abrirToolStripMenuItem.ForeColor = Color.White;
            nuevoToolStripMenuItem.ForeColor = Color.White;
            guardarComoToolStripMenuItem.ForeColor = Color.White;
            guardarToolStripMenuItem.ForeColor = Color.White;
            herramientasToolStripMenuItem.ForeColor = Color.White;
            acercaDeToolStripMenuItem.ForeColor = Color.White;
            borrarExpresionesToolStripMenuItem.ForeColor = Color.White;

            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MyColorTable());

            treeView1.ExpandAll();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 10;
            timer1.Start();            
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
            Control controlBox;
            int posPage = 0;
            bool createPage = true;

            tabControl1.SelectedIndex = posPage;
            foreach (TabPage paginaActual in tabControl1.TabPages)
            {
                if (paginaActual.ToolTipText.Equals(""))
                {
                    //--------
                    if (tabControl1.SelectedTab.HasChildren)
                    {
                        foreach (Control item in tabControl1.SelectedTab.Controls)
                        {
                            controlBox = item;

                            if (controlBox is RichTextBox)
                            {                                
                                if (controlBox.Text.Equals(""))
                                {
                                    createPage = false;
                                    break;
                                }
                            }
                        }
                    }
                    //--------
                }
                if (createPage)
                {
                    posPage++;
                }
                tabControl1.SelectedIndex = posPage;
            }

            if (createPage)
            {
                addTabPage();
            }

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
                            FileName = tabControl1.SelectedTab.Text,
                            AddExtension = true,
                        };

                        var result = saveFile.ShowDialog();

                        if (result == DialogResult.OK)
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

                    if (controlBox is RichTextBox)
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
            MessageBox.Show("Error al guardar el archivo", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            TabPage newPage = new TabPage("Untitled_" + numPage);

            RichTextBox textBox = new RichTextBox();
            textBox.SetBounds(58, 0, 695, 432);
            textBox.BackColor = Color.FromArgb(70, 70, 70);
            textBox.ForeColor = Color.White;
            textBox.Font = new Font("Tahoma", 10, FontStyle.Regular);
            textBox.WordWrap = false;

            PictureBox num = new PictureBox();
            num.SetBounds(0, 0, 58, 427);
            num.BackColor = Color.FromArgb(50, 50, 50);
            num.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);

            newPage.Controls.Add(textBox);
            newPage.Controls.Add(num);

            tabControl1.TabPages.Add(newPage);
            tabControl1.SelectedTab = newPage;
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.ToolTipText.Equals(""))
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            Control controlBox;
            if (tabControl1.SelectedTab.HasChildren)
            {
                foreach (Control item in tabControl1.SelectedTab.Controls)
                {
                    controlBox = item;

                    if (controlBox is PictureBox)
                    {
                        controlBox.Refresh();
                    }
                }
            }
            pictureBox1.Refresh();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            pintarNumeros(e);
        }

        private void pintarNumeros(PaintEventArgs e)
        {
            caracter = 0;
            int altura = 1;
            PictureBox numeros = null;
            RichTextBox aux = null;

            Control controlBox;
            if (tabControl1.SelectedTab.HasChildren)
            {
                foreach (Control item in tabControl1.SelectedTab.Controls)
                {
                    controlBox = item;

                    if (controlBox is RichTextBox)
                    {
                        aux = (RichTextBox)item;
                    }

                    if (controlBox is PictureBox)
                    {
                        numeros = (PictureBox)item;
                    }
                }
            }

            if (aux.Lines.Length > 0)
            {
                for (int i = 0; i < aux.Lines.Length; i++)
                {
                    e.Graphics.DrawString((i + 1).ToString(), aux.Font, Brushes.SkyBlue, numeros.Width - (e.Graphics.MeasureString((i + 1).ToString(), aux.Font).Width + 10), altura);
                    caracter += aux.Lines[i].Length + 1;
                    altura = aux.GetPositionFromCharIndex(caracter).Y;
                }
            }
            else
            {
                e.Graphics.DrawString("1", aux.Font, Brushes.SkyBlue, numeros.Width - (e.Graphics.MeasureString("1", aux.Font).Width + 10), altura);
            }
        }

        private void btnAnalizar_Click(object sender, EventArgs e)
        {
            Program.conteoAnalisis++;
            ConsolaLexema.Clear();

            Control controlBox;            
            if (tabControl1.SelectedTab.HasChildren)
            {
                foreach (Control item in tabControl1.SelectedTab.Controls)
                {
                    controlBox = item;

                    if (controlBox is RichTextBox)
                    {
                        //Realizar analisis lexico
                        Program.listTokensAnalisis = Program.analizador.scannerMethod(controlBox.Text);
                        Program.analizador.imprimirConsola(consolaLexico);
                        Program.analizador.reporteGlobal();
                        DisplayReportes reporteLexico = new DisplayReportes();
                        reporteLexico.Show();

                        //Cargar las expresiones regulares
                        Program.upDate.upDateEr(Program.listTokensAnalisis, ConsolaLexema);
                    }
                }
            }

            //Actualiza el tree view
            treeNodos();

        }

        private void treeNodos()
        {
            treeView1.Nodes.Clear();
            //Raiz del tree
            treeView1.Nodes.Add("Inicio");
            //Divisiones del tree
            treeView1.Nodes[0].Nodes.Add("AFND");
            treeView1.Nodes[0].Nodes.Add("AFD");
            treeView1.Nodes[0].Nodes.Add("Tabla de Transiciones");

            //Insertar los hijos
            foreach(er expresiones in Program.listER)
            {
                treeView1.Nodes[0].Nodes[0].Nodes.Add(expresiones.getId());
                treeView1.Nodes[0].Nodes[1].Nodes.Add(expresiones.getId());
                treeView1.Nodes[0].Nodes[2].Nodes.Add(expresiones.getId());
            }

            //Expandir el tree
            treeView1.ExpandAll();
        }

        private void btnLexemas_Click(object sender, EventArgs e)
        {
            if(Program.listER.Count != 0)
            {
                Control controlBox;
                if (tabControl1.SelectedTab.HasChildren)
                {
                    foreach (Control item in tabControl1.SelectedTab.Controls)
                    {
                        controlBox = item;

                        if (controlBox is RichTextBox)
                        {
                            //Realizar analisis lexico
                            Program.listTokensAnalisis = Program.analizador.scannerMethod(controlBox.Text);

                            Program.upDate.upDateLexema(Program.listTokensAnalisis, ConsolaLexema);
                            ConsolaLexema.Text += "-------------------------------------------------------------------------------\n";
                            Program.validar.validarLexemas(ConsolaLexema);                            

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Debes cargar alguna expresion regular antes de validar lexemas.","No tienes expresiones cargadas",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
           
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            
            try
            {                
                foreach (er item in Program.listER)
                {
                    if (item.getId().Equals(treeView1.SelectedNode.Text))
                    {
                        pictureBox1.Refresh();
                        path += "\\ER_Analisis\\"+item.getConteoAnalisis()+"\\"+treeView1.SelectedNode.Parent.Text +"\\" + treeView1.SelectedNode.Parent.Text + item.getConteoAnalisis()+item.getNumEr()+".png";
                        Program.pathImgActual = path;
                        pictureBox1.Image = Image.FromFile(path);
                        pictureBox1.Refresh();
                    }
                }
            }
            catch (Exception error)
            {

            }
        }

        private void treeView1_Click(object sender, EventArgs e)
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            try
            {
                foreach (er item in Program.listER)
                {
                    if (item.getId().Equals(treeView1.SelectedNode.Text))
                    {
                        pictureBox1.Refresh();
                        path += "\\ER_Analisis\\" + item.getConteoAnalisis() + "\\" + treeView1.SelectedNode.Parent.Text + "\\" + treeView1.SelectedNode.Parent.Text + item.getConteoAnalisis() + item.getNumEr() + ".png";
                        pictureBox1.Image = Image.FromFile(path);
                        pictureBox1.Refresh();
                    }
                }
            }
            catch (Exception error)
            {

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                verImagen imgForm = new verImagen();
                imgForm.Text = treeView1.SelectedNode.Text;                
                imgForm.Show();
                imgForm.pictureBox1.Image = Image.FromFile(Program.pathImgActual);                
            }
            catch(Exception error)
            {

            }
        }

        private void borrarExpresionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.listConj = new System.Collections.ArrayList();
            Program.listER = new System.Collections.ArrayList();
            Program.listTokensAnalisis = new System.Collections.ArrayList();
            treeNodos();
            pictureBox1.Image = null;
            consolaLexico.Clear();
            ConsolaLexema.Clear();
        }
    }
}
