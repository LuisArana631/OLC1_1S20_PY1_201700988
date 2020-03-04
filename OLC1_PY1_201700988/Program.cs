using OLC1_PY1_201700988.Analizador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_PY1_201700988
{   

    static class Program
    {
        public static scanner analizador = new scanner();

        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            
            Application.Run(new Form1());                        
        }
        
    }
}
