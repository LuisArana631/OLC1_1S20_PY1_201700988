﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_PY1_201700988.Estructuras.ANFD
{
    class nodoThompson
    {
        private int estado;
        private ArrayList transiciones;
        
        public nodoThompson(int estado)
        {
            this.estado = estado;
            this.transiciones = new ArrayList();
        }

        public void addTransicion(int estadoNext, string valor)
        {
            this.transiciones.Add(new nodoSiguientes(valor,estadoNext));
        }



    }
}
