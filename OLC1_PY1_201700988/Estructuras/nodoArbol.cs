using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_PY1_201700988.Estructuras
{
    class nodoArbol
    {
        private string valor;
        private int estadoInicio;
        private int estadoFin;
        private nodoArbol left;
        private nodoArbol right;
        private int tipo;
        private int esConj = 0;

        public nodoArbol(string valor, int tipo, int esConj)
        {
            this.valor = valor;
            this.tipo = tipo;
            this.esConj = esConj;
        }

        public void setValor(string valor)
        {
            this.valor = valor;
        }

        public void setEstadoInicio(int estado)
        {
            this.estadoInicio = estado;
        }

        public void setEstadoFin(int estado)
        {
            this.estadoFin = estado;
        }

        public void setLeft(nodoArbol left)
        {
            this.left = left;
        }

        public void setRight(nodoArbol right)
        {
            this.right = right;
        }

        public void setTipo(int tipo)
        {
            this.tipo = tipo;
        }

        public string getValor()
        {
            return this.valor;
        }

        public int getEstadoInicio()
        {
            return this.estadoInicio;
        }

        public int getEstadoFin()
        {
            return this.estadoFin;
        }
        
        public nodoArbol getLeft()
        {
            return this.left;
        }

        public nodoArbol getRight()
        {
            return this.right;
        }

        public int getTipo()
        {
            return this.tipo;
        }

        public void setEsConj(int esConj)
        {
            this.esConj=esConj;
        }

        public int getEsConj()
        {
            return esConj;
        }

    }
}
