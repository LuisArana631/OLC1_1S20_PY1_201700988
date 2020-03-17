using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_PY1_201700988.Estructuras
{
    class nodoReporte
    {
        private string valorToken;
        private string valorTipo;
        private string transicion;

        public nodoReporte(string token, string tipo, string transicion)
        {
            this.valorTipo = tipo;
            this.valorToken = token;
            this.transicion = transicion;
        }

        public string getTransicion()
        {
            return transicion;
        }

        public void setTransicion(string transicion)
        {
            this.transicion = transicion;
        }

        public string getToken()
        {
            return valorToken;
        }

        public void setToken(string tok)
        {
            this.valorToken = tok;
        }

        public string getTipo()
        {
            return valorTipo;
        }

        public void setTipo(string tipo)
        {
            this.valorTipo = tipo;
        }


    }
}
