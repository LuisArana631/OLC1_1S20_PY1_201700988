using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using OLC1_PY1_201700988.Estructuras.ANFD;

namespace OLC1_PY1_201700988.Estructuras
{
    class er
    {
        private string id;
        private arbol arbolGuia;
        private ArrayList afn;
        private ArrayList afd;
        private int numDoc;

        public er(string id, int numDoc)
        {
            this.id = id;
            this.numDoc = numDoc;
            this.arbolGuia = new arbol();
            this.afn = new ArrayList();
            this.afd = new ArrayList();
        }

        public void addNodoArbol(string valor, int tipo)
        {
            arbolGuia.insert(valor, tipo);
        }

        public void afnInsert()
        {
            afn = arbolGuia.crearAfnd();
        }
        
        public void agregarEstados()
        {
            this.arbolGuia.calcularEstados(arbolGuia.getRaiz());
        }

        public void graficarArbol()
        {
            this.arbolGuia.graficarArbol();
        }

        public void graficarAfnd()
        {
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathFolder = pathDesktop + "\\ER_Analisis\\AFND";

            try
            {
                if (!Directory.Exists(pathFolder))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(pathFolder);
                }

                string pathRep = pathFolder + "\\AFND_"+Program.conteoAnalisis + numDoc + ".dot";
                StreamWriter repAFND = new StreamWriter(pathRep);
                
                //Escribir el archivo dot
                repAFND.WriteLine("digraph AFD{");
                repAFND.WriteLine("rankdir=LR;");
                repAFND.WriteLine("size=\"13\";");
                //Nodo de aceptacion
                int numEstados = afn.Count - 1;
                nodoThompson aceptacion = (nodoThompson) afn[numEstados];
                repAFND.WriteLine(aceptacion.getEstado() + "[peripheries = 2, shape=circle];");
                //Configuracion de los nodos
                repAFND.WriteLine("node[shape=circle, peripheries=1];");
                repAFND.WriteLine("node[fontcolor=black];");
                repAFND.WriteLine("edge[color=black];");
                //Insertar nodos transicion
                foreach(nodoThompson item in afn)
                {
                    foreach(nodoSiguientes next in item.getTransiciones())
                    {
                        repAFND.WriteLine(item.getEstado() + " -> " + next.getEstadoNext() + "[label=\"" +next.getValor() +"\"];" );
                    }
                }
                repAFND.WriteLine("}");
                repAFND.Close();

            }
            catch (Exception e)
            {

            }

        }
    }
}
