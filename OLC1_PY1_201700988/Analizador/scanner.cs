using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using static OLC1_PY1_201700988.Analizador.token;
using System.Windows.Forms;
using System.IO;

namespace OLC1_PY1_201700988.Analizador
{
    class scanner
    {
        private ArrayList listaTokens;
        private int estado;
        private string auxiliarLexico = "";
        private int linea = 1;

        public ArrayList scannerMethod(string entrada)
        {
            entrada += "#";
            listaTokens = new ArrayList();
            estado = 0;
            auxiliarLexico = "";
            linea = 1;

            char caracter;
            for (int i = 0; i < entrada.Length; i++)
            {
                caracter = entrada[i];
                if (caracter != 13)
                {
                    switch (estado)
                    {
                        case 0:                            
                            //Evaluar letra
                            if (Char.IsLetter(caracter))
                            {
                                auxiliarLexico += caracter;
                                char prevChar = entrada[i - 1];
                                char posChar = entrada[i + 1];
                                try
                                {
                                    if (posChar == 126 || prevChar == 126 || posChar == 44 || prevChar == 44)
                                    {
                                        addToken(tipo.CARACTER);
                                        break;
                                    }
                                }
                                catch(Exception e)
                                {

                                }
                                estado = 7;
                            }
                            //Evaluar numero
                            else if (Char.IsDigit(caracter))
                            {
                                auxiliarLexico += caracter;
                                estado = 3;                                
                            }
                            //Evaluar simbolo en el rango
                            else if (caracter >= 32 && caracter <= 125)
                            {
                                //Detectar si es un simbolo o un caracter especial
                                try
                                {
                                    char prevChar = entrada[i - 1];                                    
                                    char posChar = entrada[i + 1];
                                    if (posChar == 126 || prevChar == 126 || posChar == 44 || prevChar == 44)
                                    {                                        
                                        auxiliarLexico += caracter;
                                        addToken(tipo.SIMBOLO);
                                        break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    //No mostrar nada
                                }
                                //Evaluar si viene un /
                                if (caracter == 47)
                                {
                                    auxiliarLexico += caracter;
                                    estado = 1;                                    
                                }
                                //Evaluar si viene un <
                                else if(caracter == 60)
                                {
                                    auxiliarLexico += caracter;
                                    estado = 4;                                    
                                }
                                //Evaluar si viene un "
                                else if(caracter == 34 || caracter == 39)
                                {
                                    auxiliarLexico += caracter;
                                    estado = 5;                                    
                                }
                                //Evaluar si viene \
                                else if(caracter == 92)
                                {
                                    auxiliarLexico += caracter;
                                    estado = 6;
                                }
                                //Evaluar si viene [
                                else if(caracter == 91)
                                {
                                    auxiliarLexico += caracter;
                                    estado = 10;
                                }
                                //Evaluar fin del analisis
                                else if(caracter == 35 && i == entrada.Length - 1)
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.ULTIMO);
                                    linea = 0;
                                }
                                //Evaluar si viene un -
                                else if(caracter == 45)
                                {
                                    auxiliarLexico += caracter;
                                    estado = 12;                                    
                                }
                                //Evaluar si viene un {
                                else if(caracter == 123)
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.LLAVE_IZQ);
                                }
                                //Evaluar si viene un }
                                else if(caracter == 125)
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.LLAVE_DER);
                                }
                                //Evaluar concatenacion .
                                else if(caracter  == 46)
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.CONCATENACION);
                                }
                                //Evaluar disyuncion |
                                else if(caracter == 124)
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.DISYUNCION);
                                }
                                //Evaluar cerradura *
                                else if(caracter == 42)
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.CERRADURA_KLEENE);
                                }
                                //Evaluar cerrdura +
                                else if(caracter == 43)
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.CERRADURA_POSITIVA);
                                }
                                //Evaluar cerradura ?
                                else if(caracter == 63)
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.CERRADURA_DUDA);
                                }
                                //Evaluar coma
                                else if(caracter == 44)
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.COMA);
                                }
                                //Evaluar dos puntos
                                else if(caracter == 58)
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.DOS_PUNTOS);
                                }
                                //Evaluar punto y coma
                                else if(caracter == 59)
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.PUNTO_COMA);
                                }
                                //Espacio
                                else if(caracter  == 32)
                                {
                                    //Ignorar espacio
                                }
                                else
                                {
                                    auxiliarLexico += caracter;
                                    addToken(tipo.SIMBOLO);
                                }
                            }
                            //Evaluar salto de linea
                            else if(caracter == 10)
                            {
                                linea++;
                            }
                            //Evaluar macro
                            else if(caracter == 126)
                            {
                                auxiliarLexico += caracter;
                                addToken(tipo.MACRO);
                            }
                            //Errores lexicos
                            else
                            {
                                addError(tipo.ERROR_LEXICO, Char.ToString(caracter));
                            }
                            break;

                        case 1:
                            //Evaluar si continua el comentario lineal /
                            if(caracter == 47)
                            {
                                estado = 8;
                                auxiliarLexico += caracter;
                            }
                            //Guardarlo como simbolo
                            else
                            {
                                addToken(tipo.SIMBOLO);
                                i--;
                            }
                            break;

                        case 2:
                            //Aceptacion no hacer nada
                            break;

                        case 3:
                            //Evaluar si continuan los numeros
                            if(Char.IsDigit(caracter))
                            {
                                auxiliarLexico += caracter;
                            }
                            //Insertar como numero
                            else
                            {
                                addToken(tipo.NUMERO);
                                i--;
                            }
                            break;

                        case 4:
                            //Evaluar continuacion comentario multilinea !
                            if(caracter  == 33)
                            {
                                estado = 9;
                                auxiliarLexico += caracter;
                            }
                            //Guardarlo como simbolo;
                            else
                            {
                                addToken(tipo.SIMBOLO);
                                i--;
                            }
                            break;

                        case 5:
                            //Evaluar que venga cualquier contenido hasta encontrar "
                            auxiliarLexico += caracter;
                            if (caracter == 34)
                            {                                
                                addToken(tipo.CADENA);
                            }
                            break;

                        case 6:
                            //Evaluar si viene una n
                            if(caracter == 110)
                            {
                                auxiliarLexico += caracter;
                                addToken(tipo.SALTO_LINEA);
                            }
                            //Evaluar si viene '
                            else if(caracter  == 39)
                            {
                                auxiliarLexico += caracter;
                                addToken(tipo.COMILLA_SIMPLE);
                            }
                            //Evaluar si viene "
                            else if(caracter == 34)
                            {
                                auxiliarLexico += caracter;
                                addToken(tipo.COMILLA_DOBLE);
                            }
                            //Evaluar si viene una t
                            else if(caracter  == 116)
                            {
                                auxiliarLexico += caracter;
                                addToken(tipo.TABULACION);
                            }
                            break;

                        case 7:
                            //Evaluar letra o identificador
                            if (Char.IsLetterOrDigit(caracter))
                            {
                                auxiliarLexico += caracter;
                            }
                            //Evaluar si viene _
                            else if(caracter == 95)
                            {
                                auxiliarLexico += caracter;
                            }
                            //Evaluar que tipo es
                            else
                            {
                                //Si es la palabra Reservada CONJ
                                if (auxiliarLexico.Equals("CONJ"))
                                {
                                    addToken(tipo.CONJ);
                                }                                
                                //Es un identificador
                                else
                                {
                                    addToken(tipo.IDENTIFICADOR);
                                }
                                i--;
                            }
                        break;

                        case 8:
                            //Evaluar salto de linea
                            if(caracter == 10)
                            {
                                addToken(tipo.COMENTARIO_LINEAL);
                                linea++;
                            }
                            //Concatenar contenido
                            else
                            {
                                auxiliarLexico += caracter;
                            }
                            break;

                        case 9:
                            auxiliarLexico += caracter;
                            if (caracter == 10)
                            {
                                linea++;
                            }
                            //Evaluar comentario multilinea
                            if (caracter == 33)
                            {
                                estado = 11;                                
                            }
                            break;

                        case 10:
                            //Evaluar si no es ]
                            if (caracter == 93)
                            {
                                auxiliarLexico += caracter;
                                addToken(tipo.CUALQUIER_CARACTER);
                            }
                            else
                            {
                                if(caracter  == 10)
                                {
                                    addError(tipo.ERROR_LEXICO, Char.ToString(caracter));
                                }
                                else
                                {
                                    auxiliarLexico += caracter;
                                }
                            }
                            break;

                        case 11:
                            //Evaluar fin de comentario multilinea
                            auxiliarLexico += caracter;
                            if(caracter == 62)
                            {
                                addToken(tipo.COMENTARIO_MULTILINEA);
                            }
                            //Concatenar contenido
                            else
                            {
                                if(caracter == 10)
                                {
                                    linea++;
                                }
                                estado = 9;
                            }
                            break;

                        case 12:
                            //Evaluar > asignacion
                            if(caracter == 62)
                            {
                                auxiliarLexico += caracter;
                                addToken(tipo.ASIGNACION);
                            }
                            //Añadir simbolo
                            else
                            {
                                addToken(tipo.SIMBOLO);
                                i--;
                            }
                            break;

                    }
                }
            }

            return listaTokens;
        }

        private void addToken(tipo tokenType)
        {
            listaTokens.Add(new token(tokenType, auxiliarLexico, linea));
            limpiarVariables();
        }

        private void addError(tipo tokenType, string datoError)
        {
            listaTokens.Add(new token(tokenType, datoError, linea));
        }

        private void limpiarVariables()
        {
            auxiliarLexico = "";
            estado = 0;
        }

        public void imprimirConsola(RichTextBox consola)
        {
            consola.Text = "------------------------------------------------\n";
            consola.Text += "          [Reporte de análisis léxico]          \n";
            consola.Text += "------------------------------------------------\n";

            foreach(token item in listaTokens)
            {
                consola.Text += item.getTipoString() + " <-> " + item.getValor() + " <-> "+ item.getLinea().ToString() +"\n";
                consola.Text += "------------------------------------------------\n";
            }
            
            consola.Text += "                 [Fin de Reporte]               \n";
            consola.Text += "------------------------------------------------\n";
        }

        public void reporteGlobal()
        {
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathFolder = pathDesktop + "\\ER_Analisis\\Analisis_Lexico";

            try
            {
                //Existe el directorio para el archivo, sino crearlo
                if (!Directory.Exists(pathFolder))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(pathFolder);
                }

                //Crear el reporte
                string pathRep = pathFolder + "\\Analisis_" + Program.conteoAnalisis + ".html";
                StreamWriter repHtml = new StreamWriter(pathRep);

                //Escribir la tabla html
                repHtml.WriteLine("<!DOCTYPE html>");
                repHtml.WriteLine("<html>");
                repHtml.WriteLine("<head>");
                repHtml.WriteLine("<title>Analisis_" + Program.conteoAnalisis + "</title>");
                repHtml.WriteLine("<meta charset=\"utf-8\">");
                repHtml.WriteLine("<h1 style=\"text-align:center\">Universidad de San Carlos de Guatemala</h1>");
                repHtml.WriteLine("<h2 style=\"text - align: center\">Organizacion de lenguajes y compiladores 1</h2>");
                repHtml.WriteLine("</head>");
                repHtml.WriteLine("<body>");
                repHtml.WriteLine("<table border=\"1\" style=\"width =100%\">");
                repHtml.WriteLine("<caption><h3>Reporte de analisis lexico</h3></caption>");
                repHtml.WriteLine("<colgroup>");
                repHtml.WriteLine("<col style=\"width: 20 % \"/>");
                repHtml.WriteLine("<col style=\"width: 40 % \"/>");
                repHtml.WriteLine("<col style=\"width: 40 % \"/>");
                repHtml.WriteLine("</colgroup>");
                repHtml.WriteLine("<thead>");
                repHtml.WriteLine("<tr>");
                repHtml.WriteLine("<th rowspan=\"2\">Fila</th>");
                repHtml.WriteLine("<th colspan=\"2\">Analisis de archivo</th>");
                repHtml.WriteLine("</tr>");
                repHtml.WriteLine("<tr>");
                repHtml.WriteLine("<th>Tipo de token</th>");
                repHtml.WriteLine("<th>Lexema</th>");
                repHtml.WriteLine("</tr>");
                repHtml.WriteLine("</thead>");
                repHtml.WriteLine("<tfoot>");
                repHtml.WriteLine("<tr>");
                repHtml.WriteLine("<td colspan=\"3\">Fin de Reporte</td>");
                repHtml.WriteLine("</tr>");
                repHtml.WriteLine("</tfoot>");
                repHtml.WriteLine("<tbody>");




                repHtml.WriteLine("</tbody>");
                repHtml.WriteLine("</table>")
                repHtml.WriteLine("</body>");
                repHtml.WriteLine("</html>");

                repHtml.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show("Error al generar el reporte","Error con reporte lexico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

    }
}
