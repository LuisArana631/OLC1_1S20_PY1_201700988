using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_PY1_201700988.Estructuras
{
    class arbol
    {
        private nodoArbol raiz;
        private int estado = 0;
        private Boolean insertBoolean = false;

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
                insertNodo(valor, tipo, raiz);
            }
            else
            {
                insertBoolean = false;
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
                nodo.getLeft().setEstadoFin(nodo.getRight().getEstadoInicio());
                nodo.setEstadoInicio(nodo.getLeft().getEstadoInicio());
                nodo.setEstadoFin(nodo.getRight().getEstadoFin());
            }
            else
            {
                nodo.setEstadoInicio(estado);
                estado++;
                nodo.setEstadoFin(estado);
                estado++;
            }
        }



    }

    // 0 -> hoja
    // 1 -> operacion
    // 2 -> cerradura
}
