using OLC1_PY1_201700988.Analizador;
using OLC1_PY1_201700988.Estructuras;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_PY1_201700988.Funciones
{
    class cargarDatos
    {
        private string idActual;

        public void upData(ArrayList listaToken)
        {
            int estado = 0;
            int CONJ = 0;
            int er = 0;

            foreach (token item in listaToken)
            {
                switch (estado)
                {
                    case 0:
                        switch (item.getTipo()){
                            case token.tipo.CONJ:
                                estado = 1;
                                break;
                            case token.tipo.IDENTIFICADOR:
                                if(er == 0)
                                {
                                    estado = 2;
                                    idActual = item.getValor();

                                }
                                break;
                        }
                        break;
                    case 1:

                        break;
                    case 2:

                        break;
                }
            }
        }

        private bool existeER(string id)
        {
            foreach (er exp in Program.listER)
            {
                
            }
            return false;
        }
    }
}
