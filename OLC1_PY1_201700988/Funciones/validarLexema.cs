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

namespace OLC1_PY1_201700988.Funciones
{
    class validarLexema
    {
        public void validarLexemas(RichTextBox consola)
        {            

            foreach(er expresion in Program.listER)
            {
                int conteoLexemas = 1;

                foreach (string cadena in expresion.getLexemas())
                {
                    evaluarLexema(cadena, consola, expresion.getAfd(), expresion,conteoLexemas);
                    conteoLexemas++;
                }
            }
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
                StreamWriter repTT = new StreamWriter(pathRep);

                foreach(nodoReporte rep in reporte)
                {
                    repTT.WriteLine(rep.getTipo() + " -> " + rep.getToken() + " -> " + rep.getTransicion());
                }

                repTT.Close();
                
            }
            catch (Exception e)
            {

            }
        }

    }
}
