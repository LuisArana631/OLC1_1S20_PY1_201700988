using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using OLC1_PY1_201700988.Estructuras.ANFD;
using System.Diagnostics;
using OLC1_PY1_201700988.Estructuras.AFD;

namespace OLC1_PY1_201700988.Estructuras
{
    class er
    {
        private string id;
        private arbol arbolGuia;
        private ArrayList afn;
        private ArrayList afd;
        private int numEr;
        private int conteoAnalisis;

        public int getConteoAnalisis()
        {
            return conteoAnalisis;
        }

        public string getId()
        {
            return id;
        }

        public arbol getArbol()
        {
            return arbolGuia;
        }

        public ArrayList getAfn()
        {
            return afn;
        }

        public ArrayList getAfd()
        {
            return afd;
        }

        public int getNumEr()
        {
            return numEr;
        }

        public void setId(string id)
        {
            this.id = id;
        }

        public void setArbol(arbol tree)
        {
            this.arbolGuia = tree;
        }

        public void setAfn(ArrayList afn)
        {
            this.afn = afn;
        }

        public void setAfd(ArrayList afd)
        {
            this.afd = afd;
        }

        public er(string id, int numEr, int conteo)
        {
            this.id = id;            
            this.arbolGuia = new arbol();
            this.afn = new ArrayList();
            this.afd = new ArrayList();
            this.numEr = numEr;
            this.conteoAnalisis = conteo;
        }

        public void addNodoArbol(string valor, int tipo)
        {
            arbolGuia.insert(valor, tipo);
        }

        private void afnInsert()
        {
            afn = arbolGuia.crearAfnd();
        }
        
        private void agregarEstados()
        {
            this.arbolGuia.calcularEstados(arbolGuia.getRaiz());
        }

        private void graficarArbol()
        {
            this.arbolGuia.graficarArbol(numEr, conteoAnalisis);
        }

        private void graficarAfnd()
        {
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathFolder = pathDesktop + "\\ER_Analisis\\"+conteoAnalisis+"\\AFND";

            try
            {
                if (!Directory.Exists(pathFolder))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(pathFolder);
                }

                string pathRep = pathFolder + "\\AFND"+ conteoAnalisis + +numEr+".dot";
                StreamWriter repAFND = new StreamWriter(pathRep);
                
                //Escribir el archivo dot
                repAFND.WriteLine("digraph AFND{");
                repAFND.WriteLine("rankdir=LR;");
                repAFND.WriteLine("size=\"13\";");
                //Nodo de aceptacion
                int numEstados = afn.Count-1;
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
                        if(next.getDir() == 1)
                        {
                            repAFND.Write(next.getEstadoNext() + " -> " + item.getEstado() + "[label=\"" + next.getValor() + "\", dir=back];\n");
                        }
                        else
                        {
                            repAFND.Write(item.getEstado() + " -> " + next.getEstadoNext() + "[label=\"" + next.getValor() + "\"];\n");
                        }
                    }
                }
                repAFND.WriteLine("}");
                repAFND.Close();

                string pathPng = pathFolder + "\\AFND"+ conteoAnalisis  +numEr +".png";
                this.crearImagen(pathRep,pathPng);
            }
            catch (Exception e)
            {

            }

        }

        private void crearImagen(string rutaDot, string rutaPng)
        {
            try
            {
                string comando = "dot.exe -Tpng \"" +rutaDot+"\" -o \""+ rutaPng+"\" ";
                var command = string.Format(comando);
                var procInicio = new ProcessStartInfo("cmd","/C"+command);
                var prc = new Process();

                procInicio.CreateNoWindow = true;
                procInicio.WindowStyle = ProcessWindowStyle.Hidden;

                prc.StartInfo = procInicio;
                prc.Start();
                prc.WaitForExit();
            }
            catch(Exception e)
            {

            }

        }

        public void funcionAFND()
        {
            agregarEstados();
            //graficarArbol();
            afnInsert();
            graficarAfnd();
        }

        public void funcionAFD()
        {
            afd = arbolGuia.crearAfd();
            graficarAfd();
            graficarTablaTransiciones();
        }

        private void graficarAfd()
        {
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathFolder = pathDesktop + "\\ER_Analisis\\" + conteoAnalisis + "\\AFD";

            try
            {
                if (!Directory.Exists(pathFolder))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(pathFolder);
                }

                string pathRep = pathFolder + "\\AFD" + conteoAnalisis +numEr + ".dot";
                StreamWriter repAFD = new StreamWriter(pathRep);

                //Escribir el archivo dot
                repAFD.WriteLine("digraph AFD{");
                repAFD.WriteLine("rankdir=LR;");
                repAFD.WriteLine("size=\"13\";");                
                //Configuracion de los nodos
                repAFD.WriteLine("node[shape=circle, peripheries=1];");
                repAFD.WriteLine("node[fontcolor=black];");
                repAFD.WriteLine("edge[color=black];");
                //Insertar nodos transicion
                foreach (nodoCabecera item in afd)
                {
                    if (item.getAceptacion())
                    {
                        repAFD.WriteLine(item.getIdEstado() + "[peripheries = 2, shape=circle];");
                    }

                    foreach (nodoTransicion next in item.getTransiciones())
                    {
                        
                        repAFD.Write(item.getIdEstado() + " -> " + next.getEstadoSiguientes() + "[label=\"" + next.getValor() + "\"];\n");
                        
                    }
                }
                repAFD.WriteLine("}");
                repAFD.Close();

                string pathPng = pathFolder + "\\AFD" + conteoAnalisis + numEr + ".png";
                this.crearImagen(pathRep, pathPng);
            }
            catch (Exception e)
            {

            }
        }

        public void graficarTablaTransiciones()
        {
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathFolder = pathDesktop + "\\ER_Analisis\\" + conteoAnalisis + "\\Tabla de Transiciones";

            try
            {
                if (!Directory.Exists(pathFolder))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(pathFolder);
                }

                string pathRep = pathFolder + "\\Tabla de Transiciones" + conteoAnalisis + numEr + ".dot";
                StreamWriter repTT = new StreamWriter(pathRep);

                //Escribir el archivo dot
                repTT.WriteLine("digraph Tabla_Transiciones{");
                repTT.WriteLine("tbl [");
                repTT.WriteLine("shape =  plaintext");
                repTT.WriteLine("label = <");
                repTT.WriteLine("<table border='0' cellborder='1' color='black' cellspacing='0'>");
                                
                //Crear cabecera                
                

                //Insertar nodos transicion
                foreach (nodoCabecera item in afd)
                {
                    repTT.WriteLine("<tr>");
                    //Insertar transiciones
                    foreach(nodoTransicion node in item.getTransiciones())
                    {
                        
                    }
                    repTT.WriteLine("</tr>");
                }

                repTT.WriteLine("</table>");
                repTT.WriteLine(">];");
                repTT.WriteLine("}");
                repTT.Close();

                string pathPng = pathFolder + "\\Tabla de Transiciones" + conteoAnalisis + numEr + ".png";
                this.crearImagen(pathRep, pathPng);
            }
            catch (Exception e)
            {

            }
        }

    }
}
