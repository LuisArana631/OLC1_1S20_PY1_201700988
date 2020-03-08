using OLC1_PY1_201700988.Analizador;
using OLC1_PY1_201700988.Estructuras;
using OLC1_PY1_201700988.Estructuras.Conjunto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_PY1_201700988.Funciones
{
    class cargarDatos
    {
        private string idActual;

        public void upDateEr(ArrayList listaToken, RichTextBox consola)
        {
            int estado = 0;
            int conjEstado = 0;

            consola.Text += "------------------------Insertando expresiones regulares-------------------------";

            for (int i=0; i<listaToken.Count; i++)
            {
                token item = (token) listaToken[i];
                switch (estado)
                {
                    //Estado inicial para evaluar los tokens
                    case 0:
                        switch (item.getTipo()){
                            //Si es un conjunto evaluar tokens siguientes
                            case token.tipo.CONJ:
                                //Ir a estado para controlar ingreso de conjunto
                                estado = 1;
                                break;
                            //Si es un identificador inicial evaluar tokens siguientes
                            case token.tipo.IDENTIFICADOR:
                                token auxEval =(token) listaToken[i + 1];
                                //Evaluar si es asignacion de expresion regular
                                if(auxEval.getTipo() == token.tipo.ASIGNACION)
                                {
                                    //Ir a estado para controlar ingreso de expresionRegular
                                    estado = 2;
                                    idActual = item.getValor();
                                    //Si no existe la expresion insertarla
                                    if (!existeER(idActual))
                                    {
                                        Program.listER.Add(new er(idActual, Program.listER.Count + 1));
                                    }
                                    else
                                    {
                                        consola.Text += "(ID Duplicado) *No puede tener identificadores duplicados -" + idActual + "-* ";
                                        //Ir al estado para controlar expresion repetida
                                        estado = 3;
                                    }
                                }                                                                                                
                                break;
                        }
                        break;
                    case 1:
                        switch (conjEstado)
                        {
                            //Debemos encontrar un identificador para pasar al siguiente estado
                            case 0:
                                //Si encontramos un identificador podemos seguir
                                if(item.getTipo() == token.tipo.IDENTIFICADOR)
                                {
                                    conjEstado = 1;
                                    Program.listConj.Add(new nodoConj(item.getValor()));
                                }
                                //Ignorar caracter :
                                else if (item.getTipo() == token.tipo.DOS_PUNTOS)
                                {
                                    //No hacer nada
                                }
                                else
                                {
                                    consola.Text += "Error sintactico con el token -"+item.getValor()+"- se esperaba un identificador";
                                }
                                break;
                            case 1:
                                //Debemos encontrar un simbolo de asignacion
                                if(item.getTipo() == token.tipo.ASIGNACION)
                                {
                                    conjEstado = 2;
                                }
                                //Sino mostrar error sintactico
                                else
                                {
                                    consola.Text += "Error sintactico con el token -" + item.getValor() + "- se esperaba un identificador";
                                }
                                break;
                            case 2:
                                //Debemos insertar el conjunto
                                

                                break;

                        }
                        break;
                    case 2:
                        estado = 0;
                        break;
                    case 3:
                        //Omitir todo el contenido hasta encontrar un punto y coma
                        if(item.getTipo() == token.tipo.PUNTO_COMA)
                        {
                            estado = 0;
                        }
                        break;
                }
            }
        }

        private bool existeER(string id)
        {
            foreach (er exp in Program.listER)
            {
                if (exp.getId().Equals(id))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
