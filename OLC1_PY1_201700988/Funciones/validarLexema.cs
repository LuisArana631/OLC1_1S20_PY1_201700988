using OLC1_PY1_201700988.Estructuras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using OLC1_PY1_201700988.Estructuras.AFD;

namespace OLC1_PY1_201700988.Funciones
{
    class validarLexema
    {
        public void validarLexemas(RichTextBox consola)
        {            
            foreach(er expresion in Program.listER)
            {
                foreach(string cadena in expresion.getLexemas())
                {
                    evaluarLexema(cadena, consola, expresion.getAfd(), expresion);
                }
            }
        }


        private void evaluarLexema(string cadena, RichTextBox consola, ArrayList Afd, er expresion)
        {
            nodoCabecera estadoActual = (nodoCabecera) Afd[0];
            string concatenado = "";
            bool siNo = true;

            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("cadena:\"" + cadena+"\" de la expresion "+expresion.getId());
            
            for(int i = 0; i < cadena.Length; i++)
            {
                string caracter = Char.ToString(cadena[i]);
                concatenado += caracter;
                string estadoSiguiente = estadoActual.permitirPaso(caracter, estadoActual.getIdEstado(), concatenado);                

                if (!estadoSiguiente.Equals("-----Error-----"))
                {
                    
                    Console.WriteLine("i"+i+": "+ estadoActual.getIdEstado() +"->"+caracter+ "->"+estadoSiguiente);                    
                    if (!estadoSiguiente.Equals(estadoActual.getIdEstado()))
                    {
                        concatenado = "";
                        
                    }

                    estadoActual = expresion.getEstadoActual(estadoSiguiente);

                    if (estadoActual.getAceptacion())
                    {
                        siNo = true;
                    }
                    else
                    {
                        siNo = false;
                    }

                }
                else
                {
                    siNo = false;
                    break;
                }
            }

            if (siNo)
            {
                consola.Text += "VALIDA: El lexema \"" + cadena + "\" SI ES VALIDO con la expresion regular "+expresion.getId()+"\n";
            }
            else
            {
                consola.Text += "INVALIDA: El lexema \"" + cadena + "\" NO ES VALIDO con la expresion regular " + expresion.getId() + "\n";
            }
            

        }
    }
}
