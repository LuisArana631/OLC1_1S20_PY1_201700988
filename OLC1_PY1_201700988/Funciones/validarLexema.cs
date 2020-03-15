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

            //Crear los archivos xml
            foreach(er expresion in Program.listER)
            {
                int conteoLexemas = 1;

                foreach (string cadena in expresion.getLexemas())
                {
                    evaluarLexema(cadena, consola, expresion.getAfd(), expresion,conteoLexemas);
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

        }


        private void evaluarLexema(string cadena, RichTextBox consola, ArrayList Afd, er expresion, int conteo)
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


            //Mostrar el reporte en consola de la interfaz
            if (siNo)
            {
                consola.Text += "VALIDA: El lexema \"" + cadena + "\" SI ES VALIDO con la expresion regular "+expresion.getId()+"\n";
            }
            else
            {
                consola.Text += "INVALIDA: El lexema \"" + cadena + "\" NO ES VALIDO con la expresion regular " + expresion.getId() + "\n";
            }

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
                    XmlElement elemento = doc.CreateElement(string.Empty,"Bloque", string.Empty);
                    

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

    }
}
