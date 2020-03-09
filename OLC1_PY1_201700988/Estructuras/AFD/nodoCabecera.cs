using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace OLC1_PY1_201700988.Estructuras.AFD
{
    class nodoCabecera
    {
        private string idEstado;
        private ArrayList transiciones;
        private string conjuntoContenido;
        private bool aceptacion;

        public void setIdEstado(string id)
        {
            this.idEstado = id;
        }

        public void setTransiciones(ArrayList transiciones)
        {
            this.transiciones = transiciones;
        }

        public void setConjunto(string conjunto)
        {
            this.conjuntoContenido = conjunto;
        }

        public void setAceptacion(bool aceptar)
        {
            this.aceptacion = aceptar;
        }

        public string getIdEstado()
        {
            return this.idEstado;
        }

        public ArrayList getTransiciones()
        {
            return transiciones;
        }

        public string getConjunto()
        {
            return conjuntoContenido;
        }

        public bool getAceptacion()
        {
            return aceptacion;
        }


    }
}
