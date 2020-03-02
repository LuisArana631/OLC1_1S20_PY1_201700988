using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}
