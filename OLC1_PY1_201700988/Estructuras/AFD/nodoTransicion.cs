using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_PY1_201700988.Estructuras.AFD
{
    class nodoTransicion
    {
        private string estadoSiguiente;
        private string valor;
        
        public nodoTransicion(string estado, string valor)
        {
            this.estadoSiguiente = estado;
            this.valor = valor;
        }

        public void setEstadoSiguiente(string estado)
        {
            this.estadoSiguiente = estado;
        }

        public void setValor(string valor)
        {
            this.valor = valor;
        }

        public string getEstadoSiguientes()
        {
            return estadoSiguiente;
        }

        public string getValor()
        {
            return valor;
        }

        

    }
}
