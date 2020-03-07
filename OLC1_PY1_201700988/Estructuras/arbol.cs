using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using OLC1_PY1_201700988.Estructuras.ANFD;
using System.IO;

namespace OLC1_PY1_201700988.Estructuras
{
    class arbol
    {
        private nodoArbol raiz;
        private int estado = 0;
        private Boolean insertBoolean = false;
        private ArrayList tablaAfnd;

        public arbol()
        {
            this.raiz = null;
        }

        public nodoArbol getRaiz()
        {
            return raiz;
        }

        public void insert(string valor, int tipo)
        {
            if(raiz != null)
            {
                insertBoolean = false;
                insertNodo(valor, tipo, raiz);
            }
            else
            {                
                this.raiz = new nodoArbol(valor, tipo);
            }
        }

        private void insertNodo(string valor, int tipo, nodoArbol nodo)
        {
            if(nodo.getLeft() != null)
            {
                insertNodo(valor, tipo, nodo.getLeft());
            }

            if(nodo.getRight() != null)
            {
                insertNodo(valor, tipo, nodo.getRight());
            }

            if (!insertBoolean)
            {
                switch (nodo.getTipo())
                {
                    case 0:                        
                        break;
                    case 1:
                        if (nodo.getLeft() == null)
                        {
                            insertLeft(valor, tipo, nodo);
                            insertBoolean = true;
                        }
                        else if (nodo.getRight() == null)
                        {
                            insertRight(valor, tipo, nodo);
                            insertBoolean = true;
                        }
                        break;
                    case 2:
                        if(nodo.getLeft() == null)
                        {
                            insertLeft(valor, tipo, nodo);
                            insertBoolean = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void insertLeft(string valor, int tipo, nodoArbol nodo)
        {
            nodoArbol nodoInsert = new nodoArbol(valor, tipo);
            nodo.setLeft(nodoInsert);
        }

        private void insertRight(string valor, int tipo, nodoArbol nodo)
        {
            nodoArbol nodoInsert = new nodoArbol(valor, tipo);
            nodo.setRight(nodoInsert);
        }

        public void calcularEstados(nodoArbol nodo)
        {
            if(nodo.getLeft() != null)
            {
                calcularEstados(nodo.getLeft());
            }

            if(nodo.getRight() != null)
            {
                calcularEstados(nodo.getRight());
            }

            //Evaluar si el nodo es una concatenacion
            if (nodo.getValor().Equals("."))
            {
                //Concatenar los valores
                nodo.getRight().setEstadoInicio(nodo.getLeft().getEstadoFin());
                nodo.setEstadoInicio(nodo.getLeft().getEstadoInicio());
                nodo.setEstadoFin(nodo.getRight().getEstadoFin());
            }
            else if (nodo.getValor().Equals("+"))
            {
                nodo.setEstadoInicio(estado);
                estado += 2;
                nodo.setEstadoFin(estado);
                estado++;
            }
            else
            {
                nodo.setEstadoInicio(estado);
                estado++;
                nodo.setEstadoFin(estado);
                estado++;
            }
        }

        public ArrayList crearAfnd()
        {
            tablaAfnd = new ArrayList();

            llenarTabla(raiz);

            return tablaAfnd;
        }

        private void llenarTabla(nodoArbol nodo)
        {
            if(nodo.getLeft() != null)
            {
                llenarTabla(nodo.getLeft());
            }

            if(nodo.getRight() != null)
            {
                llenarTabla(nodo.getRight());
            }

            //Insertar estados
            if (!existeEstado(nodo.getEstadoInicio())){
                tablaAfnd.Add(new nodoThompson(nodo.getEstadoInicio()));
            }

            if (!existeEstado(nodo.getEstadoFin()))
            {
                tablaAfnd.Add(new nodoThompson(nodo.getEstadoFin()));
            }

            if (nodo.getValor().Equals("+"))
            {
                tablaAfnd.Add(new nodoThompson(nodo.getEstadoFin()-1));
            }

            char epsilon = (char)603;
            //Insertar transiciones
            switch (nodo.getValor())
            {
                case ".":
                    //Ignorar
                    break;
                case "|":
                    //Insertar conexiones
                    foreach(nodoThompson item in tablaAfnd)
                    {
                        //Conexiones iniciales
                        if(item.getEstado() == nodo.getEstadoInicio())
                        {
                            item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString());
                            item.addTransicion(nodo.getRight().getEstadoInicio(), epsilon.ToString());
                        }

                        //Conectar al final
                        if(item.getEstado() == nodo.getLeft().getEstadoFin())
                        {
                            item.addTransicion(nodo.getEstadoFin(), epsilon.ToString());
                        }
                        if(item.getEstado() == nodo.getRight().getEstadoFin())
                        {
                            item.addTransicion(nodo.getEstadoFin(), epsilon.ToString());
                        }

                    }
                    break;
                case "*":
                    //Insertar conexiones
                    foreach(nodoThompson item in tablaAfnd)
                    {
                        //Conexiones iniciales
                        if(item.getEstado() == nodo.getEstadoInicio())
                        {
                            item.addTransicion(nodo.getEstadoFin(), epsilon.ToString());
                            item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString());
                        }

                        //Conexiones finales nodo hijo
                        if(item.getEstado() == nodo.getLeft().getEstadoFin())
                        {
                            item.addTransicion(nodo.getEstadoFin(), epsilon.ToString());
                            item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString());
                        }
                    }
                    break;
                case "?":                    
                    //Insertar conexiones
                    foreach(nodoThompson item in tablaAfnd)
                    {
                        //Conexiones iniciales
                        if(item.getEstado() == nodo.getEstadoInicio())
                        {
                            item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString());
                            item.addTransicion(nodo.getEstadoFin(), epsilon.ToString());
                        }

                        //Conexiones finales
                        if(item.getEstado() == nodo.getLeft().getEstadoFin())
                        {
                            item.addTransicion(nodo.getEstadoFin(), epsilon.ToString());
                        }                        

                    }
                    break;
                case "+":                    
                    //Insertar conexiones
                    foreach (nodoThompson item in tablaAfnd)
                    {
                        //Conexiones iniciales
                        if (item.getEstado() == nodo.getEstadoInicio())
                        {
                            item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString());
                            item.addTransicion(nodo.getEstadoFin() - 1, epsilon.ToString());
                        }

                        //Conexiones del final nodo hijo
                        if(item.getEstado() == nodo.getLeft().getEstadoFin())
                        {
                            item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString());
                            item.addTransicion(nodo.getEstadoFin() - 1, epsilon.ToString());
                        }

                        //Conexiones del estado fin - 1
                        if(item.getEstado() == (nodo.getEstadoFin() - 1))
                        {
                            item.addTransicion(nodo.getEstadoFin(), epsilon.ToString());
                        }
                    }

                    break;
                default:
                    //Conectar nodos de hoja
                    foreach(nodoThompson item in tablaAfnd)
                    {
                        if(item.getEstado() == nodo.getEstadoInicio())
                        {
                            item.addTransicion(nodo.getEstadoFin(), nodo.getValor());
                        }
                    }
                    break;
            }

        }

        private bool existeEstado(int estado)
        {
            foreach(nodoThompson item in tablaAfnd)
            {
                if(item.getEstado() == estado)
                {
                    return true;
                }
            }

            return false;
        }

        public void graficarArbol()
        {
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathFolder = pathDesktop + "\\ER_Analisis\\Arbol";

            try
            {
                if (!Directory.Exists(pathFolder))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(pathFolder);
                }

                string pathRep = pathFolder + "\\Arbol_" + Program.conteoAnalisis+ ".dot";
                StreamWriter repArb = new StreamWriter(pathRep);

                repArb.WriteLine("digraph Arbol{");
                repArb.WriteLine("node[shape = record, height = .1];");

                crearArbol(repArb, raiz);

                repArb.WriteLine("}");
                repArb.Close();

            }
            catch (Exception e)
            {

            }
        }

        private void crearArbol(StreamWriter write, nodoArbol nodo)
        {
            if (nodo != null)
            {
                crearArbol(write, nodo.getLeft());

                try
                {
                    if (nodo.getValor().Length == 1)
                    {
                        write.WriteLine("\"node" + nodo.getValor() + "\"[label = \"<f0>" + nodo.getEstadoInicio() + " |<f1>\\" + nodo.getValor() + " |<f2>" + nodo.getEstadoFin() + " \"];");
                    }

                    if (nodo.getLeft() != null)
                    {
                        write.WriteLine("\"node" + nodo.getValor() + "\":f0 -> \"node" + nodo.getLeft().getValor() + "\";");
                    }

                    if (nodo.getRight() != null)
                    {
                        write.WriteLine("\"node" + nodo.getValor() + "\":f2 -> \"node" + nodo.getRight().getValor() + "\";");
                    }
                }
                catch(Exception e)
                {

                }
                crearArbol(write, nodo.getRight());
            }
        }
    }



    // 0 -> hoja
    // 1 -> operacion
    // 2 -> cerradura
}
