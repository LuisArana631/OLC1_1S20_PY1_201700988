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
    public partial class DisplayReportes : Form
    {
        public DisplayReportes()
        {
            InitializeComponent();
            webBrowser1.Url = new Uri(Program.pathLexico);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void DisplayReportes_Load(object sender, EventArgs e)
        {

        }
    }
}
