using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace OLC1_PY1_201700988.Estructuras.Conjunto
{
    class nodoConj
    {
        private string idConj = "";
        private ArrayList conjunto;

        public nodoConj(string id)
        {
            this.idConj = id;
            this.conjunto = new ArrayList();
        }

        public bool existeChar(String caracter)
        {
            foreach(string item in conjunto)
            {                
                //Console.WriteLine("Conjunto: evaluar:"+caracter+" item:"+item);
                if (item.Equals(caracter))
                {
                    return true;
                }
                else if (item.Equals("\\\"") && caracter.Equals("\\\""))
                {
                    return true;
                }
                else if (item.Equals("\\t") && caracter.Equals("\t"))
                {
                    return true;
                }
                else if (item.Equals("\\n") && caracter.Equals("\n"))
                {
                    return true;
                }
                else if (item.Equals("\\'") && caracter.Equals("\\'"))
                {
                    return true;
                }
                else if (item.Equals("[:todo:]"))
                {
                    return true;
                }
            }
            return false;
        }

        public string getId()
        {
            return this.idConj;
        }

        public ArrayList getConjunto()
        {
            return this.conjunto;
        }

        public void setId(string id)
        {
            this.idConj = id;
        }

        public void setConjunto(ArrayList conj)
        {
            this.conjunto = conj;
        }

        public void addItem(string item)
        {
            conjunto.Add(item);
        }





    }
}
