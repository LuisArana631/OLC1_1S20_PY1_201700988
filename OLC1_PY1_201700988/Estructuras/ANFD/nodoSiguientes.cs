using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_PY1_201700988.Estructuras.ANFD
{
    class nodoSiguientes
    {
        private int estadoNext;
        private string valor;

        public nodoSiguientes(string valor, int estado)
        {
            this.estadoNext = estado;
            this.valor = valor;
        }

        public int getEstadoNext()
        {
            return estadoNext;
        }

        public void setEstadoNext(int estado)
        {
            this.estadoNext = estado;
        }

        public string getValor()
        {
            return valor;
        }

        public void setValor(string valor)
        {
            this.valor = valor;
        }
    }    
}
