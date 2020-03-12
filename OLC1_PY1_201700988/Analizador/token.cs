using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_PY1_201700988.Analizador
{
    public class token
    {
        public enum tipo
        {
            //Palabra Reservada
            CONJ,
            //Token de signos
            COMA,
            DOS_PUNTOS,
            PUNTO_COMA,
            ASIGNACION,
            LLAVE_IZQ,
            LLAVE_DER,
            CONCATENACION,
            DISYUNCION,
            CERRADURA_KLEENE,
            CERRADURA_POSITIVA,
            CERRADURA_DUDA,
            MACRO,
            EPSILON,
            //Tokens extra
            NUMERO,
            CARACTER,
            SIMBOLO,
            IDENTIFICADOR,
            COMENTARIO_LINEAL,
            COMENTARIO_MULTILINEA,
            CADENA,
            ULTIMO,                        
            SALTO_LINEA,
            COMILLA_SIMPLE,
            COMILLA_DOBLE,
            TABULACION,
            CUALQUIER_CARACTER,
            ERROR_LEXICO
        }

        private tipo tipoToken;
        private string valor;
        private int linea;

        public token(tipo tipoToken, string valorToken, int lineaToken)
        {
            this.tipoToken = tipoToken;
            this.valor = valorToken;
            this.linea = lineaToken;
        }

        public tipo getTipo()
        {
            return tipoToken;
        }

        public string getValor()
        {
            return valor;
        }

        public int getLinea()
        {
            return linea;
        }

        public string getTipoString()
        {
            switch (tipoToken)
            {
                case tipo.CONJ:
                    return "Palabra reservada CONJ";
                case tipo.COMA:
                    return "Coma";
                case tipo.DOS_PUNTOS:
                    return "Dos puntos";
                case tipo.PUNTO_COMA:
                    return "Punto y coma";
                case tipo.ASIGNACION:
                    return "Asignacion";
                case tipo.LLAVE_DER:
                    return "Llave derecha";
                case tipo.LLAVE_IZQ:
                    return "Llave izquierda";
                case tipo.CONCATENACION:
                    return "Concatenacion";
                case tipo.DISYUNCION:
                    return "Disyuncion";
                case tipo.CERRADURA_KLEENE:
                    return "Cerradura *";
                case tipo.CERRADURA_POSITIVA:
                    return "Cerradura +";
                case tipo.CERRADURA_DUDA:
                    return "Cerradura ?";
                case tipo.MACRO:
                    return "Conjunto";
                case tipo.EPSILON:
                    return "Epsilon";
                case tipo.NUMERO:
                    return "Numero";
                case tipo.IDENTIFICADOR:
                    return "Identificador";
                case tipo.COMENTARIO_LINEAL:
                    return "Comentario lineal";
                case tipo.COMENTARIO_MULTILINEA:
                    return "Comentario multilinea";
                case tipo.CADENA:
                    return "Cadena";
                case tipo.ULTIMO:
                    return "Ultimo";                
                case tipo.CARACTER:
                    return "Caracter";
                case tipo.SIMBOLO:
                    return "Simbolo";
                case tipo.SALTO_LINEA:
                    return "Salto de linea";
                case tipo.COMILLA_SIMPLE:
                    return "Comilla simple";
                case tipo.COMILLA_DOBLE:
                    return "Comilla doble";
                case tipo.TABULACION:
                    return "Tabulacion";
                case tipo.CUALQUIER_CARACTER:
                    return "Cualquier caracter";
                case tipo.ERROR_LEXICO:
                    return "Error Lexico";
                default:
                    return "Desconocido";
            }
        }
    }
}
