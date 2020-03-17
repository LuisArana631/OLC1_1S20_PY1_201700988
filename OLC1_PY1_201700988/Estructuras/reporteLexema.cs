using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_PY1_201700988.Estructuras
{
    class reporteLexema
    {
        private string lexema;
        private string er;
        private bool valido;

        public string getLexema()
        {
            return lexema;
        }

        public string getEr()
        {
            return er;
        }

        public bool getValido()
        {
            return valido;
        }

        public void setLexema(string lexema)
        {
            this.lexema = lexema;
        }

        public void setEr(string er)
        {
            this.er = er;
        }

        public void setValido(bool valido)
        {
            this.valido = valido;
        }
        
        public reporteLexema(string lexema, string er, bool valido)
        {
            this.lexema = lexema;
            this.er = er;
            this.valido = valido;
        }


    }
}
