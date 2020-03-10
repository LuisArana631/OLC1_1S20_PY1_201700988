using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using OLC1_PY1_201700988.Estructuras.ANFD;
using System.IO;
using OLC1_PY1_201700988.Estructuras.AFD;

namespace OLC1_PY1_201700988.Estructuras
{
    class arbol
    {
        private nodoArbol raiz;
        private int estado = 0;
        private Boolean insertBoolean = false;
        private ArrayList tablaAfnd;
        private ArrayList tablaAfd;

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
            if (nodo.getValor().Equals(".") && nodo.getTipo() == 1)
            {
                nodo.setEstadoInicio(nodo.getLeft().getEstadoInicio());
                nodo.setEstadoFin(nodo.getRight().getEstadoFin());
                nodo.getRight().setEstadoInicio(nodo.getLeft().getEstadoFin());
                int estadoContinuo = nodo.getLeft().getEstadoFin();                
                nodoArbol aux = nodo.getRight();

                while (aux != null)
                {
                    aux.setEstadoInicio(estadoContinuo);
                    if (aux.getValor().Equals(".") && nodo.getTipo() == 1)
                    {                        
                        aux = aux.getLeft();
                    }
                    else
                    {
                        break;
                    }
                }
                
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

            char epsilon = (char)603;
            //Insertar transiciones
            switch (nodo.getTipo())
            {
                case 0:
                    //Conectar nodos de hoja
                    foreach (nodoThompson item in tablaAfnd)
                    {
                        if (item.getEstado() == nodo.getEstadoInicio())
                        {
                            item.addTransicion(nodo.getEstadoFin(), nodo.getValor(), 0);
                        }
                    }                    
                    break;
                case 1:
                    switch (nodo.getValor())
                    {
                        case ".":
                            //Ignorar
                            break;
                        case "|":
                            //Insertar conexiones
                            foreach (nodoThompson item in tablaAfnd)
                            {
                                //Conexiones iniciales
                                if (item.getEstado() == nodo.getEstadoInicio())
                                {
                                    item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString(), 0);
                                    item.addTransicion(nodo.getRight().getEstadoInicio(), epsilon.ToString(), 0);
                                }

                                //Conectar al final
                                if (item.getEstado() == nodo.getLeft().getEstadoFin())
                                {
                                    item.addTransicion(nodo.getEstadoFin(), epsilon.ToString(), 0);
                                }
                                if (item.getEstado() == nodo.getRight().getEstadoFin())
                                {
                                    item.addTransicion(nodo.getEstadoFin(), epsilon.ToString(), 0);
                                }

                            }
                            break;
                    }
                    break;
                case 2:
                    switch (nodo.getValor())
                    {
                        case "*":
                            //Insertar conexiones
                            foreach (nodoThompson item in tablaAfnd)
                            {
                                //Conexiones iniciales
                                if (item.getEstado() == nodo.getEstadoInicio())
                                {
                                    item.addTransicion(nodo.getEstadoFin(), epsilon.ToString(), 0);
                                    item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString(), 0);
                                }

                                //Conexiones finales nodo hijo
                                if (item.getEstado() == nodo.getLeft().getEstadoFin())
                                {
                                    item.addTransicion(nodo.getEstadoFin(), epsilon.ToString(), 0);
                                    item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString(), 1);
                                }
                            }
                            break;
                        case "?":
                            //Insertar conexiones
                            foreach (nodoThompson item in tablaAfnd)
                            {
                                //Conexiones iniciales
                                if (item.getEstado() == nodo.getEstadoInicio())
                                {
                                    item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString(), 0);
                                    item.addTransicion(nodo.getEstadoFin(), epsilon.ToString(), 0);
                                }

                                //Conexiones finales
                                if (item.getEstado() == nodo.getLeft().getEstadoFin())
                                {
                                    item.addTransicion(nodo.getEstadoFin(), epsilon.ToString(), 0);
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
                                    item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString(),0);
                                }

                                //Conexiones finales
                                if (item.getEstado() == nodo.getLeft().getEstadoFin())
                                {
                                    item.addTransicion(nodo.getLeft().getEstadoInicio(), epsilon.ToString(),1);
                                    item.addTransicion(nodo.getEstadoFin(), epsilon.ToString(),0);
                                }
                            }
                            break;
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

        public void graficarArbol(int er)
        {
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathFolder = pathDesktop + "\\ER_Analisis\\Arbol";

            try
            {
                if (!Directory.Exists(pathFolder))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(pathFolder);
                }

                string pathRep = pathFolder + "\\Arbol_" + er+ ".dot";
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
                        write.WriteLine("\"node" + nodo.getValor() + nodo.getEstadoFin() + nodo.getEstadoInicio() + "\"[label = \"<f0>" + nodo.getEstadoInicio() + " |<f1>\\" + nodo.getValor() + " |<f2>" + nodo.getEstadoFin() + " \"];");
                    }

                    if (nodo.getLeft() != null)
                    {
                        write.WriteLine("\"node" + nodo.getValor() + nodo.getEstadoFin() + nodo.getEstadoInicio() + "\":f0 -> \"node" + nodo.getLeft().getValor() + nodo.getLeft().getEstadoFin() + nodo.getLeft().getEstadoInicio() + "\";");
                    }

                    if (nodo.getRight() != null)
                    {
                        write.WriteLine("\"node" + nodo.getValor() + nodo.getEstadoFin() + nodo.getEstadoInicio() + "\":f2 -> \"node" + nodo.getRight().getValor() + nodo.getRight().getEstadoFin() + nodo.getRight().getEstadoInicio() + "\";");
                    }
                }
                catch(Exception e)
                {

                }
                crearArbol(write, nodo.getRight());
            }
        }
        
        public ArrayList crearAfd()
        {
            tablaAfd = new ArrayList();
            int estados = 0;

            //Extraer el inicio del AFND
            tablaAfd.Add(new nodoCabecera("S0",raiz.getEstadoInicio().ToString(),false));

            int repetirFor = 1;

            while(repetirFor != 0){
                repetirFor--;

                //Desglozar los estados
                foreach(nodoCabecera item in tablaAfd)
                {
                    //Extraer el conjunto de transiciones de epsilon y agregarlo al nodo
                    item.setConjunto(getConjunto(item.getConjuntoGuia()));

                    //Crear los estados a la cabecera
                    estados = crearCabeceras(item.getConjunto(), estados);

                    //Agregar las transiciones al estado actual

                }
            }

            return tablaAfd;
        }

        private string getConjunto(string conjuntoGuia)
        {
            string conjunto = "";
            string[] listaItems = conjuntoGuia.Split(',');

            ArrayList numEstados = new ArrayList();

            //Insertar los estados guia en el conjunto de estados 
            foreach(string item in listaItems)
            {
                numEstados.Add(Int32.Parse(item));
                conjunto += item; 
            }
            
            int repetirWhile = 1;

            //While para encontrar todos los estados que se pueden llegar 
            while(repetirWhile != 0)
            {
                repetirWhile--;

                //Recorrer los estados del conjunto (Arreglo)
                foreach(int estadoNum in numEstados)
                {
                    //Evaluar los estados en mi afnd
                    foreach(nodoThompson nodo in tablaAfnd)
                    {
                        //Si encuentro al estado de mi conjunto guia en mi tabla afnd
                        if(estadoNum == nodo.getEstado())
                        {
                            //Tomar las transiciones y evaluar si ya existen en mi tabla de conjunto
                            foreach(nodoSiguientes next in nodo.getTransiciones())
                            {
                                //Si la transicion es por medio de un epsilon agregar a mi arreglo de estados
                                if (next.getValor().Equals(((char) 603).ToString()))
                                {
                                    //Si es con epsilon
                                    //Evaluar si ya existe en mi arreglo para no duplicar
                                    if (numEstados.Contains(next.getEstadoNext()))
                                    {
                                        //Si se encuentra en la lista ignorar
                                    }
                                    else
                                    {
                                        //Si no se encuentra insertar a arreglo
                                        numEstados.Add(next.getEstadoNext());
                                        repetirWhile++;
                                    }
                                }
                                else
                                {
                                    //Continuar buscando
                                }
                                
                            }
                        }
                    }
                }
                
            }

            //Concatenar el conjunto en un string
            foreach(int numero in numEstados)
            {
                conjunto += "," + numero.ToString();
            }

            return conjunto;
        }

        private bool existeEstado(string id)
        {
            foreach (nodoCabecera cabecera in tablaAfd)
            {
                if (cabecera.getIdEstado().Equals(id))
                {
                    return true;
                }
            }

            return false;
        }

        private int crearCabeceras(string conjunto, int estados)
        {
            string guiaNuevoEstado = "";
            string[] stringNum = conjunto.Split(',');
            ArrayList numEstados = new ArrayList();

            //Pasar los estados a numero en arrayList
            foreach(string num in stringNum)
            {
                numEstados.Add(Int32.Parse(num));
            }

            //Encontrar los estados a los que se puede ir por medio de las hojas
            foreach(int state in numEstados)
            {
                //Extraer el estado del afnd definido por el state
                nodoThompson auxAfnd = getEstadoAfnd(state);

                //Evaluar las transiciones no iguales a epsilon
                foreach(nodoSiguientes stateNext in auxAfnd.getTransiciones())
                {
                    
                }
            }    

            return 0;

        }

        private nodoThompson getEstadoAfnd(int id)
        {
            foreach(nodoThompson nodo in tablaAfnd)
            {
                if(nodo.getEstado() == id)
                {
                    return nodo;
                }
            }

            return null;
        }

    }
    
    // 0 -> hoja
    // 1 -> operacion
    // 2 -> cerradura
}
