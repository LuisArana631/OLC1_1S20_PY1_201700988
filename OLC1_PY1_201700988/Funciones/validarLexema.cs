using OLC1_PY1_201700988.Estructuras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using OLC1_PY1_201700988.Estructuras.AFD;
using System.IO;
using System.Xml;

namespace OLC1_PY1_201700988.Funciones
{
    class validarLexema
    {
        public void validarLexemas(RichTextBox consola)
        {
            ArrayList listValidos = new ArrayList();

            //Crear los archivos xml
            foreach(er expresion in Program.listER)
            {
                int conteoLexemas = 1;

                foreach (string cadena in expresion.getLexemas())
                {
                    evaluarLexema(cadena, consola, expresion.getAfd(), expresion,conteoLexemas, listValidos);
                    conteoLexemas++;
                }
            }

            //Cargar los archivos a la app
            reporteLexemas ventanaReporte = new reporteLexemas();

            foreach (er expresion in Program.listER)
            {
                int conteoLexemas = 1;

                foreach (string cadena in expresion.getLexemas())
                {
                    TabPage nuevaTab = new TabPage(expresion.getId()+"_"+conteoLexemas.ToString());

                    string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    string pathFolder = pathDesktop + "\\ER_Analisis\\" + Program.conteoAnalisis + "\\ReporteLexemas";
                    string pathRep = pathFolder + "\\ReporteLexema" + conteoLexemas + "_" + expresion.getId() + ".xml";

                    WebBrowser pestaña = new WebBrowser();
                    pestaña.Dock = DockStyle.Fill;
                    pestaña.Size = nuevaTab.Size;
                    pestaña.Url = new Uri(pathRep);

                    nuevaTab.Controls.Add(pestaña);
                    ventanaReporte.tabControl1.TabPages.Add(nuevaTab);

                    conteoLexemas++;
                }
            }

            ventanaReporte.Show();

            //Cargar la tabla 
            generarReporte(listValidos);
            DisplayReportes ventana = new DisplayReportes();
            ventana.Show();
        }

        private void evaluarLexema(string cadena, RichTextBox consola, ArrayList Afd, er expresion, int conteo,ArrayList list)
        {
            nodoCabecera estadoActual = (nodoCabecera) Afd[0];
            string concatenado = "";
            bool siNo = true;

            //Console.WriteLine("--------------------------------------------------------------");
            //Console.WriteLine("cadena:\"" + cadena+"\" de la expresion "+expresion.getId());

            ArrayList reporteCadena = new ArrayList();            
            
            //Analizar cada caracter
            for(int i = 0; i < cadena.Length; i++)
            {
                string caracter = Char.ToString(cadena[i]);
                concatenado += caracter;
                string estadoSiguiente = estadoActual.permitirPaso(caracter, estadoActual.getIdEstado(), concatenado, reporteCadena);


                //Console.WriteLine("i" + i + ": " + estadoActual.getIdEstado() + "->" + caracter + "->" + estadoSiguiente);
                if (!estadoSiguiente.Equals("Error"))
                {
                    if (!estadoSiguiente.Equals(estadoActual.getIdEstado()))
                    {
                        concatenado = "";

                    }

                    estadoActual = expresion.getEstadoActual(estadoSiguiente);
                }
                else
                {
                    siNo = false;
                }                                      
                

            }

            if (siNo)
            {
                if (!estadoActual.getAceptacion())
                {
                    siNo = false;
                }
            }


            //Mostrar el reporte en consola de la interfaz
            if (siNo)
            {
                consola.Text += "VALIDA: El lexema \"" + cadena + "\" SI ES VALIDO con la expresion regular "+expresion.getId()+"\n";
            }
            else
            {
                consola.Text += "INVALIDA: El lexema \"" + cadena + "\" NO ES VALIDO con la expresion regular " + expresion.getId() + "\n";
            }

            list.Add(new reporteLexema(cadena, expresion.getId(), siNo));

            //Generar xml del analisis
            generarXML(reporteCadena, expresion,conteo);
        }

        private void generarXML(ArrayList reporte, er expresion, int lexemaCount)
        {
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathFolder = pathDesktop + "\\ER_Analisis\\" + Program.conteoAnalisis + "\\ReporteLexemas";

            try
            {
                if (!Directory.Exists(pathFolder))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(pathFolder);
                }

                string pathRep = pathFolder + "\\ReporteLexema"+lexemaCount+"_" + expresion.getId() + ".xml";
                XmlDocument doc = new XmlDocument();

                XmlDeclaration xmlDeclarar = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclarar, root);

                //Crear el primer contenedor
                XmlElement analisisLexico = doc.CreateElement(string.Empty, "Analisis_Lexico",string.Empty);
                doc.AppendChild(analisisLexico);

                // XmlElement errores = doc.CreateElement(string.Empty, "Error_Lexico", string.Empty);
                //doc.AppendChild(errores);

                int conteo = 0;
                //Crear todo el reporte de tokens
                foreach(nodoReporte item in reporte)
                {                    
                    XmlElement elemento = doc.CreateElement(string.Empty,"Token", string.Empty);
                    

                  //  if (!item.getTipo().Equals("Error Lexico"))
                    //{
                        analisisLexico.AppendChild(elemento);
                    //}
                    //else
                   // {
                     //   errores.AppendChild(elemento);
                   // }

                    //Agregar el tipo del token
                    XmlElement tipoToken = doc.CreateElement(string.Empty, "Tipo_Token", string.Empty);
                    XmlText tipoString = doc.CreateTextNode(item.getTipo());
                    tipoToken.AppendChild(tipoString);
                    elemento.AppendChild(tipoToken);

                    //Agregar el lexema
                    XmlElement lexemaToken = doc.CreateElement(string.Empty, "Lexema", string.Empty);
                    XmlText lexemaString = doc.CreateTextNode("\"" + item.getToken()+ "\"" );
                    lexemaToken.AppendChild(lexemaString);
                    elemento.AppendChild(lexemaToken);

                    //Agregar Transicion
                    XmlElement transicionToken = doc.CreateElement(string.Empty, "Transicion", string.Empty);
                    XmlText transicionString = doc.CreateTextNode(item.getTransicion());
                    transicionToken.AppendChild(transicionString);
                    elemento.AppendChild(transicionToken);
                    
                    conteo++;
                }

                doc.Save(pathRep);

            }
            catch (Exception e)
            {

            }
        }

        private void generarReporte(ArrayList listaReporte)
        {
            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathFolder = pathDesktop + "\\ER_Analisis\\" + Program.conteoAnalisis + "\\LexemasValidos";

            try
            {
                //Existe el directorio para el archivo, sino crearlo
                if (!Directory.Exists(pathFolder))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(pathFolder);
                }

                //Crear el reporte
                string pathRep = pathFolder + "\\Lexemas_" + Program.conteoAnalisis + ".html";
                StreamWriter repHtml = new StreamWriter(pathRep);
                Program.pathLexico = pathRep;

                //Escribir la tabla html
                repHtml.WriteLine("<!DOCTYPE html>");
                repHtml.WriteLine("<html>");
                repHtml.WriteLine("<head>");
                repHtml.WriteLine("<title>Analisis_" + Program.conteoAnalisis + "</title>");
                repHtml.WriteLine("<meta charset=\"utf-8\">");
                repHtml.WriteLine("<h1 style=\"text-align:center\">Universidad de San Carlos de Guatemala</h1>");
                repHtml.WriteLine("<h2 style=\"text-align: center\">Organizacion de lenguajes y compiladores 1</h2>");
                repHtml.WriteLine("<h4 style=\"text-align: center\">Luis Fernando Arana Arias - 201700988</h4>");
                repHtml.WriteLine("</head>");
                repHtml.WriteLine("<body>");
                repHtml.WriteLine("<center>");
                repHtml.WriteLine("<table border=\"1\" style=\"width:60%\">");
                repHtml.WriteLine("<caption><h3>Lexemas Validos</h3></caption>");
                repHtml.WriteLine("<colgroup>");
                repHtml.WriteLine("<col style=\"width: 20% \"/>");
                repHtml.WriteLine("<col style=\"width: 20% \"/>");
                repHtml.WriteLine("<col style=\"width: 60% \"/>");
                repHtml.WriteLine("</colgroup>");
                repHtml.WriteLine("<thead>");
                repHtml.WriteLine("<tr>");
                repHtml.WriteLine("<th rowspan=\"2\">Validez</th>");
                repHtml.WriteLine("<th colspan=\"2\">Lexemas</th>");
                repHtml.WriteLine("</tr>");
                repHtml.WriteLine("<tr>");
                repHtml.WriteLine("<th>Expresión Regular</th>");
                repHtml.WriteLine("<th>Lexema</th>");
                repHtml.WriteLine("</tr>");
                repHtml.WriteLine("</thead>");
                repHtml.WriteLine("<tfoot>");
                repHtml.WriteLine("<tr>");
                repHtml.WriteLine("<td colspan=\"3\">Fin de Reporte</td>");
                repHtml.WriteLine("</tr>");
                repHtml.WriteLine("</tfoot>");
                repHtml.WriteLine("<tbody>");

                foreach (reporteLexema lex in listaReporte)
                {
                    repHtml.WriteLine("<tr>");
                    if (lex.getValido())
                    {
                        repHtml.WriteLine("<th>VALIDO</th>");
                    }
                    else
                    {
                        repHtml.WriteLine("<th>INVALIDO</th>");
                    }                    
                    repHtml.WriteLine("<td>" + lex.getEr() + "</td>");
                    repHtml.WriteLine("<td>" + lex.getLexema() + "</td>");
                    repHtml.WriteLine("</tr>");
                }

                repHtml.WriteLine("</tbody>");
                repHtml.WriteLine("</table>");
                repHtml.WriteLine("</center>");
                repHtml.WriteLine("</body>");
                repHtml.WriteLine("</html>");

                repHtml.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show("Error al generar el reporte", "Error con reporte lexico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            



        }

    }
}
