using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_PY1_201700988.Estructuras
{
    class lexema
    {

        private string cadena;
        private bool valido;

        public lexema(string cadena)
        {
            this.cadena = cadena;
            this.valido = false;
        }

        public string getCadena()
        {
            return cadena;
        }

        public bool getValido()
        {
            return valido;
        }

        public void setCadena(string cadena)
        {
            this.cadena = cadena;
        }

        public void setValido(bool valido)
        {
            this.valido = valido;
        }


    }
}
