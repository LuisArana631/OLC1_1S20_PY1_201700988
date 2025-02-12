﻿using OLC1_PY1_201700988.Analizador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using OLC1_PY1_201700988.Funciones;

namespace OLC1_PY1_201700988
{   

    static class Program
    {
        //Variables funcionales
        public static int conteoAnalisis = 0;
        public static string pathLexico = "";
        public static string pathImgActual = "";

        //Estructuras funcionales
        public static ArrayList listER = new ArrayList();
        public static ArrayList listTokensAnalisis = new ArrayList();
        public static ArrayList listConj = new ArrayList();

        //Fuciones del programa
        public static scanner analizador = new scanner();               
        public static cargarDatos upDate = new cargarDatos();
        public static validarLexema validar = new validarLexema();

        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            
            Application.Run(new Form1());                        
        }
        
    }
}
