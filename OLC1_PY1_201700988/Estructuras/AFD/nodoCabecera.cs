using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using OLC1_PY1_201700988.Estructuras.Conjunto;

namespace OLC1_PY1_201700988.Estructuras.AFD
{
    class nodoCabecera
    {
        private string idEstado;
        private ArrayList transiciones;
        private string conjuntoContenido;
        private string conjuntoGuia; 
        private bool aceptacion;

        public nodoCabecera(string id, string conjuntoGuia, bool aceptar)
        {
            this.idEstado = id;
            this.conjuntoGuia = conjuntoGuia;
            this.aceptacion = aceptar;
            this.conjuntoContenido = "";
            this.transiciones = new ArrayList();
        }

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

        public void setConjuntoGuia(string conjunto)
        {
            this.conjuntoGuia = conjunto;
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

        public string getConjuntoGuia()
        {
            return conjuntoGuia;
        }
        
        public void addTransicion(string estado, string valor, int esConj)
        {
            this.transiciones.Add(new nodoTransicion(estado,valor, esConj));
        }

        public string permitirPaso(string caracter, string estadoActual, string cadena, ArrayList reporte)
        {            

            int movs = 1;
            foreach(nodoTransicion next in transiciones)
            {
                bool esConjunto = false;                

                //Reconocer si es conjunto
                switch (next.getEsConj())
                {
                    case 0:
                        break;
                    case 1:
                        esConjunto = true;
                        break;
                }

                //Console.WriteLine("valor: \"" + next.getValor() + "\" char: \"" + caracter+ "\"");                
                //Si es conjunto
                if (esConjunto)
                {                    
                    if (getConjEvaluar(next.getValor()) != null)
                    {
                        if (getConjEvaluar(next.getValor()).existeChar(caracter))
                        {
                                reporte.Add(new nodoReporte(caracter, next.getValor(), estadoActual + "->" + next.getEstadoSiguientes()));
                                return next.getEstadoSiguientes();                            
                        }
                    }                
                }                
                //Si no es un conjunto y si el caracter es igual al que se encuentra en la expresion
                else if (next.getValor().Equals(caracter))
                {                    
                    reporte.Add(new nodoReporte(caracter, next.getValor(), estadoActual + "->" + next.getEstadoSiguientes()));
                    return next.getEstadoSiguientes();
                }  
                //Evaluar los caracteres \t \n [:asfgasfasf:]
                else if (next.getValor().Equals(("\\t").ToString()))
                {                    
                    if (Convert.ToInt32(caracter[0]) == 9)
                    {
                        reporte.Add(new nodoReporte(caracter, next.getValor(), estadoActual + "->" + next.getEstadoSiguientes()));
                        return next.getEstadoSiguientes();
                    }
                }
                else if (next.getValor().Equals(("\\n").ToString()))
                {                    
                    if (Convert.ToInt32(caracter[0]) == 10)
                    {                        
                        reporte.Add(new nodoReporte(caracter, next.getValor(), estadoActual + "->" + next.getEstadoSiguientes()));
                        return next.getEstadoSiguientes();
                    }
                }
                //Evaluar si la cadena es de mas caracteres
                else if(next.getValor().Length > 1 && cadena.Length <= next.getValor().Length)
                {                    
                    //Console.WriteLine("Cadena: " + cadena + " valor: " + next.getValor());
                    if (cadena.Equals(next.getValor()))
                    {
                        reporte.Add(new nodoReporte(cadena, next.getValor(), estadoActual + "->" + next.getEstadoSiguientes()));
                        return next.getEstadoSiguientes();
                    }
                    else
                    {
                        //Saber si estamos en la ultima transicion a evaluar
                        if (movs == transiciones.Count)
                        {                            
                            //Console.WriteLine("Retornando desde aca movs");
                            return estadoActual;
                        }                        
                        
                    }
                }

                movs++;
            }

            //Insertar error
            reporte.Add(new nodoReporte(cadena,"Error Lexico con el caracter "+caracter, "Se vuelve el estado actual ("+estadoActual+") por el error"));
            return "Error";
        }

        private nodoConj getConjEvaluar(string id)
        {
            foreach (nodoConj conjunto in Program.listConj)
            {
                if (conjunto.getId().Equals(id))
                {
                    return conjunto;
                }
            }

            return null;

        }        
        
    }
}
