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
            int erEstado = 0;

            consola.Text += "-------------------------------------------------------------------------------\n";
            consola.Text += "                       [Insertando expresiones regulares]                        \n";
            consola.Text += "-------------------------------------------------------------------------------\n";
            

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
                                        Program.listER.Add(new er(idActual, Program.listER.Count + 1, Program.conteoAnalisis));
                                    }
                                    else
                                    {
                                        consola.Text += "(ID Duplicado) *No puede tener identificadores duplicados -" + idActual + "-* \n";
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
                                    idActual = item.getValor();
                                }
                                //Ignorar caracter :
                                else if (item.getTipo() == token.tipo.DOS_PUNTOS)
                                {
                                    //No hacer nada
                                }
                                else
                                {
                                    printError(consola, item.getValor(), "identificador");
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
                                    printError(consola, item.getValor(), "asignacion");
                                }
                                break;
                            case 2:
                                //Debemos insertar el conjunto
                                if (item.getTipo() == token.tipo.CARACTER || item.getTipo() == token.tipo.NUMERO || item.getTipo() == token.tipo.SIMBOLO)
                                {
                                    //Si encontramos un token valido de un conjunto insertar
                                    posConj(idActual).addItem(item.getValor());
                                }
                                else if (item.getTipo() == token.tipo.COMA)
                                {
                                    //Si encontramos una coma seguir en este case
                                    //Ignorar la coma solo mantenerse en el estado 2
                                }
                                else if(item.getTipo() == token.tipo.MACRO)
                                {
                                    //Si encontramos un macro ir al case para tratar macros
                                    conjEstado = 3;
                                }
                                else if(item.getTipo() == token.tipo.PUNTO_COMA)
                                {
                                    //Si encontramos un punto y coma regresar al estado 0
                                    conjEstado = 0;
                                    estado = 0;
                                }
                                else
                                {
                                    //Notificar un error sintactico
                                    printError(consola, item.getValor(), "conjunto");
                                }

                                break;
                            case 3:
                                if (item.getTipo() == token.tipo.CARACTER)
                                {
                                    //Insertar los caracteres
                                    string itemInicio = (string)posConj(idActual).getConjunto()[0];
                                    char caracterInicial = itemInicio[0];

                                    for (int j = caracterInicial + 1; j < (char)item.getValor()[0]; j++)
                                    {
                                        char charInsert = (char)j;
                                        if (char.IsLetter(charInsert))
                                        {
                                            posConj(idActual).getConjunto().Add(charInsert.ToString());
                                        }
                                    }
                                }
                                else if (item.getTipo() == token.tipo.NUMERO)
                                {
                                    //Insertar el conjunto de numeros
                                    int itemInicio = Int32.Parse((string) posConj(idActual).getConjunto()[0]);

                                    for (int j  = itemInicio+1; j <= int.Parse(item.getValor()); j++)
                                    {
                                        posConj(idActual).getConjunto().Add(j.ToString());
                                    }
                                }
                                else if(item.getTipo() == token.tipo.SIMBOLO)
                                {
                                    //Insertar los caracteres
                                    string itemInicio = (string)posConj(idActual).getConjunto()[0];
                                    char caracterInicial = itemInicio[0];

                                    for (int j = caracterInicial + 1; j <= (char)item.getValor()[0]; j++)
                                    {
                                        char charInsert = (char)j;
                                        if (!char.IsLetterOrDigit(charInsert))
                                        {
                                            posConj(idActual).getConjunto().Add(charInsert.ToString());
                                        }
                                    }
                                }
                                else if(item.getTipo() == token.tipo.PUNTO_COMA)
                                {
                                    //Regresar todo al inicio
                                    conjEstado = 0;
                                    estado = 0;
                                }
                                else
                                {
                                    //Notificar un error sintactico
                                    printError(consola, item.getValor(), "caracter|numero|simbolo");
                                }
                                break;
                        }
                        break;
                    case 2:
                        switch (erEstado)
                        {
                            case 0:
                                //Debemos encontrar un token asignacion
                                if (item.getTipo() == token.tipo.ASIGNACION)
                                {                                    
                                    erEstado = 1;
                                }
                                else if(item.getTipo()  == token.tipo.DOS_PUNTOS)
                                {
                                    estado = 3;
                                }
                                else
                                {
                                    //Notificar un error sintactico
                                    printError(consola, item.getValor(), "asignacion");
                                }
                                break;
                            case 1:                                
                                if(item.getTipo() == token.tipo.CADENA || item.getTipo() == token.tipo.IDENTIFICADOR || item.getTipo() == token.tipo.TABULACION || item.getTipo() == token.tipo.SALTO_LINEA || item.getTipo() == token.tipo.COMILLA_DOBLE || item.getTipo() == token.tipo.COMILLA_SIMPLE || item.getTipo()==token.tipo.CUALQUIER_CARACTER)
                                {
                                    //Si encontramos una cadena insertar un nodo hoja
                                    if(item.getTipo() == token.tipo.CADENA)
                                    {
                                        posEr(idActual).addNodoArbol(item.getValor().Replace('"'.ToString(), ""), 0,0);
                                    }
                                    else if(item.getTipo() == token.tipo.IDENTIFICADOR)
                                    {
                                        posEr(idActual).addNodoArbol(item.getValor(), 0,1);
                                    }
                                    else
                                    {
                                        posEr(idActual).addNodoArbol(item.getValor(), 0,0);
                                    }                                    
                                }
                                else if (item.getTipo() == token.tipo.CERRADURA_DUDA || item.getTipo() == token.tipo.CERRADURA_KLEENE || item.getTipo() == token.tipo.CERRADURA_POSITIVA)
                                {
                                    //Si insertamos una cerradura, ingresar un nodo cerradura
                                    posEr(idActual).addNodoArbol(item.getValor(), 2,0);
                                }
                                else if(item.getTipo() == token.tipo.CONCATENACION || item.getTipo() == token.tipo.DISYUNCION)
                                {
                                    //Si encontramos una operacion insertar el nodo
                                    posEr(idActual).addNodoArbol(item.getValor(), 1,0);
                                }
                                else if(item.getTipo() == token.tipo.LLAVE_DER || item.getTipo() == token.tipo.LLAVE_IZQ)
                                {
                                    //Ignorar llaves
                                }
                                else if(item.getTipo() == token.tipo.PUNTO_COMA)
                                {
                                    try
                                    {
                                        //Generar AFND
                                        posEr(idActual).funcionAFND();
                                        posEr(idActual).funcionAFD();
                                        consola.Text += "-------------------------------------------------------------------------------\n";
                                        consola.Text += "                   [Expresion " +idActual+ " insertada con exito]                       \n";
                                        consola.Text += "-------------------------------------------------------------------------------\n";
                                    }
                                    catch(Exception e)
                                    {
                                        consola.Text += "***Error al generar la expresion regular "+idActual+", verifica que este escrita correctamente.*** \n";
                                        consola.Text += e+ "\n";
                                    }                                    
                                    //Regresar al estado inicial
                                    erEstado = 0;
                                    estado = 0;
                                }
                                else
                                {
                                    //Notificar un error sintactico
                                    printError(consola, item.getValor(), "Expresion Regular");
                                }

                                break;
                        }
                        break;
                    case 3:
                        //Omitir todo el contenido hasta encontrar un punto y coma
                        if(item.getTipo() == token.tipo.PUNTO_COMA)
                        {
                            //Regresar todo al inicio
                            conjEstado = 0;
                            erEstado = 0;
                            estado = 0;
                        }
                        break;
                }
            }


        }

        private void printError(RichTextBox consola, string valor, string espera)
        {
            consola.Text += "Error sintactico con el lexema -" + valor + "- se esperaba un " +espera +"\n";
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

        private nodoConj posConj(string id)
        {            

            foreach(nodoConj nodo in Program.listConj)
            {
                if (nodo.getId().Equals(id))
                {
                    return nodo;
                }                
            }

            return null;
        }

        private er posEr(string id)
        {
            foreach(er expresion in Program.listER)
            {
                if (expresion.getId().Equals(id))
                {
                    return expresion;
                }
            }
            return null;
        }

        public void upDateLexema(ArrayList listaToken, RichTextBox consola)
        {
            int estado = 0;

            consola.Text += "-------------------------------------------------------------------------------\n";
            consola.Text += "                               [Validando Lexemas]                        \n";
            consola.Text += "-------------------------------------------------------------------------------\n";

            for (int i = 0; i < listaToken.Count; i++)
            {
                token item = (token)listaToken[i];
                switch (estado)
                {
                    //Debemos encontrar un identificador para continuar con el analisis
                    case 0:
                        switch (item.getTipo())
                        {
                            case token.tipo.CONJ:
                                estado = 1;
                                break;
                            case token.tipo.IDENTIFICADOR:
                                token auxEval = (token)listaToken[i + 1];
                                if (auxEval.getTipo() == token.tipo.DOS_PUNTOS)
                                {
                                    //Ir a estado para controlar ingreso de expresionRegular
                                    estado = 2;
                                    idActual = item.getValor();
                                    //Si no existe la expresion insertarla
                                    if (!existeER(idActual))
                                    {
                                        //Notificar que no existe esa expresion regular
                                        consola.Text += "(ID No Existe) *No se encontro una expresion regular con ese identificador -" + idActual + "-* \n";
                                        estado = 1;
                                    }
                                }
                                break;
                        }
                        break;
                    case 1:
                        if(item.getTipo() == token.tipo.PUNTO_COMA)
                        {
                            //Hasta encontrar un punto y coma omitir lo que viene
                            estado = 0;
                        }
                        break;
                    case 2:
                        //Si encontramos una cadena
                        if(item.getTipo() == token.tipo.CADENA)
                        {
                            posEr(idActual).addLexema(item.getValor().Replace('"'.ToString(),""));
                            consola.Text += "Cadena: "+item.getValor()+" insertada en la expresion: "+idActual+"\n";
                        }
                        //Si encontramos un punto y coma
                        else if(item.getTipo() == token.tipo.PUNTO_COMA)
                        {
                            estado = 0;
                        }
                        //Ignorar los dos puntos
                        else if(item.getTipo() == token.tipo.DOS_PUNTOS)
                        {
                            //No hacer nada
                        }
                        //Cualquier otro tipo de token
                        else
                        {
                            printError(consola,item.getValor(),"cadena");
                        }
                        break;
                }
            }
        }


    }
}
