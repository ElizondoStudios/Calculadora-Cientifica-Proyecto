using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculadora_Segundo_Parcial
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public double Val1=0, Val2=0;
        bool SHIFT= false, HYP= false;

        bool Is_Operador(char simbolo)
        {
            if (simbolo == '+')
                return true;
            else if (simbolo == '^')
                return true;
            else if (simbolo == '*')
                return true;
            else if (simbolo == '/')
                return true;
            else if (simbolo == '%')
                return true;
            else
                return false;
        }

        bool Is_Number(char num)
        {
            if (num == '1' || num == '2' || num == '3' || num == '4' || num == '5' || num == '6' || num == '7' || num == '8' || num == '9' || num == '0' || num == '0')
                return true;
            else
                return false;
        }

        bool Syntax_Check(string expresion)
        {
            //Variable de control de puntos
            int puntos = 0;

            //Variable de control de parentesis
            int parentesis = 0;

            for(int i=0; i < expresion.Length; i++)
            {
                //Comprobar doble punto
                if (expresion[i] == '.')
                    puntos++;
                if (Is_Operador(expresion[i]))
                    puntos = 0;
                if (puntos > 1)
                    return true;

                //Doble operador
                if(i != (expresion.Length - 1))
                {
                    if (Is_Operador(expresion[i]) && Is_Operador(expresion[i + 1]))
                        return true;
                }

                //Factorial con operador
                if((expresion[i]=='!' || expresion[i] == 'Σ') && i!=0)
                {
                    if (Is_Operador(expresion[i - 1]))
                        return true;
                }
            }

            //Termina con operador
            string[] OpFinales = new string[] {"+", "*", "-", "/", "%", "sen", "senh", "cos","cosh","tan","tanh","ln","log", "√","^" };
            foreach(string a in OpFinales)
            {
                if (expresion.EndsWith(a))
                    return true;
            }

            //Operador en indice 0
            string[] OpInicial = new string[] {"*", "/", "%", "^", "!", "Σ" };
            foreach (string a in OpInicial)
            {
                if (expresion[0].ToString()==a)
                    return true;
            }

            //Parentesis incompletos
            foreach(char a in expresion)
            {
                if (a == '(')
                    parentesis++;
                else if (a == ')')
                    parentesis--;
            }
            if (parentesis != 0)
                return true;

            //Parentesis vacios
            for(int i=0; i<expresion.Length; i++)
            {
                if (expresion[i] == '(' && expresion[i + 1] == ')')
                    return true;
            }

            //Sin error
            return false;
        }
        string Resolver(string expresion) {

            int InParAbierto, InParCerrado=0, parentesis=0;
            InParAbierto = expresion.IndexOf('(');

            if (InParAbierto >= 0)
            {
                //Asignar valor a parentesis cerrado
                for(int k = InParAbierto + 1; k < expresion.Length; k++)
                {
                    if (expresion[k] == ')' && parentesis == 0)
                    {
                        InParCerrado = k;
                    }
                    else if (expresion[k] == '(')
                        parentesis++;
                    else if (expresion[k] == ')')
                        parentesis--;
                }

                while(InParAbierto >= 0)
                {
                    expresion = expresion.Replace(expresion.Substring(InParAbierto, InParCerrado - InParAbierto + 1), Resolver(expresion.Substring(InParAbierto + 1, InParCerrado - InParAbierto - 1)));
                    InParAbierto = expresion.IndexOf('(');
                    for (int k = InParAbierto + 1; k < expresion.Length; k++)
                    {
                        if (expresion[k] == ')' && parentesis == 0)
                        {
                            InParCerrado = k;
                        }
                        else if (expresion[k] == '(')
                            parentesis++;
                        else if (expresion[k] == ')')
                            parentesis--;
                    }
                }
                return expresion;
            }
            else
                return expresion;
        }

        /*string Parse_String(string expresion)
        {
            string expresionRaiz3;
            int aparicion_raiz3;
            aparicion_raiz3=expresion.IndexOf("³√");
            
            if (expresion[aparicion_raiz3 + 1] != '(')
            {
                int k= aparicion_raiz3;
                while (Is_Number(expresion[k]))
                {
                    k++;
                }
                expresionRaiz3 = expresion.Substring(aparicion_raiz3, k-1);
                expresion = expresion.Replace("³√"+ expresionRaiz3, expresionRaiz3+"^(1/3)");
            }
            
            

            expresion = expresion.Replace("³", "^3");
            expresion = expresion.Replace("²", "^2");
            expresion = expresion.Replace("⁻¹", "^(-1)");
            expresion = expresion.Replace("√", "sqrt");

            return expresion;
        }*/

        //Sumatoria y factorial
        public int Fact_Sigma(int n, int flag)
        {
            if (n == 1)
                return n;
            else
            {
                if(flag==1)
                   return n * Fact_Sigma(n - 1, flag);
                else
                    return n + Fact_Sigma(n - 1, flag);
            }
        }

        private void Factorial_Click(object sender, EventArgs e)
        {
            if (SHIFT)
            {
                PantallaEcuacion.Text += "C";
                SHIFT = false;
                PantallaShift.Text = "";
            }
            else
                PantallaEcuacion.Text+="!";
        }

        private void Sumatoria_Click(object sender, EventArgs e)
        {
            if (SHIFT)
            {
                PantallaEcuacion.Text += "P";
                SHIFT = false;
                PantallaShift.Text = "";
            }
            else
                PantallaEcuacion.Text += "Σ";

        }

        private void Modulo_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "%";
        }

        private void Igual_Click(object sender, EventArgs e)
        {
            if (Syntax_Check(PantallaEcuacion.Text))
                PantallaEcuacion.Text = "Syntax Error";
            else
            {
                PantallaResultados.Text = Resolver(PantallaEcuacion.Text);
            }
                

        }

        private void Sumar_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "+";
        }

        private void Multiplicar_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "*";
        }

        private void Restar_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "-";
        }

        private void Dividir_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "/";
        }

        private void ClearError_Click(object sender, EventArgs e)
        {
            if (PantallaEcuacion.Text == "Syntax Error")
                PantallaEcuacion.Text = "";
            else if (PantallaEcuacion.Text != "")
                PantallaEcuacion.Text = PantallaEcuacion.Text.Remove(PantallaEcuacion.Text.Length - 1);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            if (SHIFT)
                Application.Exit();
            else
            {
                PantallaEcuacion.Text = "";
                PantallaResultados.Text = "";
            }
                
        }

        private void Punto_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += ".";
        }

        private void Cero_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "0";
        }

        private void Tres_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "3";
        }

        private void Dos_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "2";
        }

        private void Uno_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "1";
        }

        private void Seis_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "6";
        }

        private void Cinco_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "5";
        }

        private void Cuatro_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "4";
        }

        private void Nueve_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "9";
        }

        private void Ocho_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "8";
        }

        private void Seno_Click(object sender, EventArgs e)
        {
            if (SHIFT && HYP)
            {
                PantallaEcuacion.Text += "arsenh";
                SHIFT = HYP = false;
                PantallaShift.Text = PantallaHyp.Text = "";
            }
            else if (SHIFT && !HYP)
            {
                PantallaEcuacion.Text += "arcsen";
                SHIFT = false;
                PantallaShift.Text = "";
            }   
            else if (!SHIFT && HYP)
            {
                PantallaEcuacion.Text += "senh";
                HYP = false;
                PantallaHyp.Text = "";
            }  
            else
                PantallaEcuacion.Text += "sen";
        }

        private void Coseno_Click(object sender, EventArgs e)
        {
            if (SHIFT && HYP)
            {
                PantallaEcuacion.Text += "arcosh";
                SHIFT = HYP = false;
                PantallaShift.Text = PantallaHyp.Text = "";
            }
            else if (SHIFT && !HYP)
            {
                PantallaEcuacion.Text += "arccos";
                SHIFT = false;
                PantallaShift.Text = "";
            }
            else if (!SHIFT && HYP)
            {
                PantallaEcuacion.Text += "cosh";
                HYP = false;
                PantallaHyp.Text = "";
            }
            else
                PantallaEcuacion.Text += "cos";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SHIFT && HYP)
            {
                PantallaEcuacion.Text += "artanh";
                SHIFT = HYP = false;
                PantallaShift.Text = PantallaHyp.Text = "";
            }
            else if (SHIFT && !HYP)
            {
                PantallaEcuacion.Text += "arctan";
                SHIFT = false;
                PantallaShift.Text = "";
            }
            else if (!SHIFT && HYP)
            {
                PantallaEcuacion.Text += "tanh";
                HYP = false;
                PantallaHyp.Text = "";
            }
            else
                PantallaEcuacion.Text += "tan";
        }

        private void Log_Click(object sender, EventArgs e)
        {
            if (SHIFT)
            {
                PantallaEcuacion.Text += "10^";
                SHIFT = false;
                PantallaShift.Text = "";
            }
            else
                PantallaEcuacion.Text += "log";
        }

        private void LogN_Click(object sender, EventArgs e)
        {
            if (SHIFT)
            {
                PantallaEcuacion.Text += "e^";
                SHIFT = false;
                PantallaShift.Text = "";
            }
            else
                PantallaEcuacion.Text += "ln";
        }

        private void ParAbierto_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "(";
        }

        private void ParCerrado_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += ")";
        }

        private void Hyperbolic_Click(object sender, EventArgs e)
        {
            if (HYP)
            {
                HYP = false;
                PantallaHyp.Text = "";
            }
            else
            {
                HYP = true;
                PantallaHyp.Text = "hyp";
            }
        }

        private void RaizCuadrada_Click(object sender, EventArgs e)
        {
            if (SHIFT)
            {
                PantallaEcuacion.Text += "³√";
                SHIFT = false;
                PantallaShift.Text = "";
            }
            else
                PantallaEcuacion.Text += "√";
        }

        private void X_cuadrada_Click(object sender, EventArgs e)
        {
            if (SHIFT)
            {
                PantallaEcuacion.Text += "³";
                SHIFT = false;
                PantallaShift.Text = "";
            }
            else
                PantallaEcuacion.Text += "²";
        }

        private void Potencia_Click(object sender, EventArgs e)
        {
            if (SHIFT)
            {
                PantallaEcuacion.Text += "ˣ√";
                SHIFT = false;
                PantallaShift.Text = "";
            }
            else
                PantallaEcuacion.Text += "^";
        }

        private void X_inversa_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "⁻¹";
        }

        private void Pi_Click(object sender, EventArgs e)
        {
            if (SHIFT)
            {
                PantallaEcuacion.Text += "e";
                SHIFT = false;
                PantallaShift.Text = "";
            }
            else
                PantallaEcuacion.Text += "π";
        }

        private void EXP_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "E";
        }

        private void Shift_Click(object sender, EventArgs e)
        {
            if (SHIFT)
            {
                SHIFT = false;
                PantallaShift.Text = "";
            }
            else
            {
                SHIFT = true;
                PantallaShift.Text = "Shift";
            }
        }

        private void Siete_Click(object sender, EventArgs e)
        {
            PantallaEcuacion.Text += "7";
        }
    }
}
