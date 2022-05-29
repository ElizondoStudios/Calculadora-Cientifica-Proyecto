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
    /*
     To do:
        -Resolver errores de sintaxis
        -Pasar el resultado a la pantalla de ecuacion
        -Resolver longitud en la pantalla de ecuacion 
        -Implementar lo de tuli 
     */
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool SHIFT= false, HYP= false;

        /* ###########################################################
                             Funciones Sintaxis
           ########################################################### */
        bool Is_OperadorBas(char simbolo)
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
            if (num == '1' || num == '2' || num == '3' || num == '4' || num == '5' || num == '6' || num == '7' || num == '8' || num == '9' || num == '0' || num == '.')
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
                if (Is_OperadorBas(expresion[i]))
                    puntos = 0;
                if (puntos > 1)
                    return true;

                //Doble operador
                if(i != (expresion.Length - 1))
                {
                    if (Is_OperadorBas(expresion[i]) && Is_OperadorBas(expresion[i + 1]))
                        return true;
                }

                //Factorial con operador
                if((expresion[i]=='!' || expresion[i] == 'Σ') && i!=0)
                {
                    if (Is_OperadorBas(expresion[i - 1]))
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

        /* ###########################################################
                             Funciones Resolver
           ########################################################### */

        bool Is_Operador(char digito)
        {
            if (digito == '+' || digito == '*' || digito == '/' || digito == '-')
                return true;
            else if (digito == '%' || digito == '^' || digito == '√' || digito == '√' || digito == '³' || digito == '²' || digito == '⁻')
                return true;
            else if (digito == 'a' || digito == 's' || digito == 'c' || digito == 't' || digito == 'h' || digito == 'n')
                return true;
            else if (digito == 'l' || digito == 'g' || digito == '!' || digito == 'Σ' || digito == '(' || digito == ')' || digito == 'E' || digito == 'P' || digito == 'C')
                return true;
            else
                return false;
        }

        //Tomar lados
        double Tomar_Derecha(string expresion)
        {
            int k = 0;
            if (expresion[k] == '-')
            {
                k++;
                while (!Is_Operador(expresion[k]) && k < expresion.Length - 1)
                {
                    k++;
                }

                if (k == expresion.Length - 1)
                    return Convert.ToDouble(expresion.Substring(1, k))*(-1.0);
                else
                    return Convert.ToDouble(expresion.Substring(1, k-1))*(-1.0);
            }
            else
            {
                while (!Is_Operador(expresion[k]) && k < expresion.Length - 1)
                {
                    k++;
                }
                if (k == expresion.Length - 1)
                    return Convert.ToDouble(expresion.Substring(0, k + 1));
                else
                    return Convert.ToDouble(expresion.Substring(0, k));
            }
        }

        double Tomar_Izquierda(string expresion)
        {
            int k = expresion.Length - 1;
            while (!Is_Operador(expresion[k]) && k > 0)
            {
                k--;
            }
            if (k == 0)
                return Convert.ToDouble(expresion);
            else
            {
                if (expresion[k-1]=='-')
                    return Convert.ToDouble(expresion.Substring(k))*(-1.0);
                else
                    return Convert.ToDouble(expresion.Substring(k + 1));
            }
                
        }

        //Resolver
        string Resolver(string expresion, int jerarquia)
        {
            double Val1, Val2, Resultado = 0;
            switch (jerarquia)
            {
                
                case 1: //Jerarquia 1
                    for (int k = 1; k < expresion.Length; k++)
                    {
                        if (expresion[k] == '+')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Val2 = Tomar_Derecha(expresion.Substring(k + 1));
                            Resultado = Val1 + Val2;
                            expresion = expresion.Replace(Val1.ToString() + "+" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 1);
                        }
                        else if (expresion[k] == '-')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Val2 = Tomar_Derecha(expresion.Substring(k + 1));
                            Resultado = Val1 - Val2;
                            expresion = expresion.Replace(Val1.ToString() + "-" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 1);
                        }
                        else if(expresion[k]== 'Σ')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Resultado = Fact_Sigma((int)Val1, 2);
                            expresion = expresion.Replace(Val1.ToString() + "Σ", Resultado.ToString());
                            return Resolver(expresion, 1);
                        }

                    }
                    break;

                case 2: //Jerarquia 2
                    for (int k = 1; k < expresion.Length; k++)
                    {
                        if (expresion[k] == '*')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Val2 = Tomar_Derecha(expresion.Substring(k + 1));
                            Resultado = Val1 * Val2;
                            expresion = expresion.Replace(Val1.ToString() + "*" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 2);
                        }
                        else if (expresion[k] == '/')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Val2 = Tomar_Derecha(expresion.Substring(k + 1));
                            Resultado = Val1 / Val2;
                            expresion = expresion.Replace(Val1.ToString() + "/" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 2);
                        }
                        else if(expresion[k] == '!')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Resultado = Fact_Sigma((int)Val1, 1);
                            expresion = expresion.Replace(Val1.ToString() + "!", Resultado.ToString());
                            return Resolver(expresion, 2);
                        }
                        else if(expresion[k] == 'C')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Val2 = Tomar_Derecha(expresion.Substring(k + 1));
                            Resultado = (Fact_Sigma((int)Val1, 1)) / (Fact_Sigma((int)Val2, 1) * Fact_Sigma((int)Val1 - (int)Val2, 1));
                            expresion = expresion.Replace(Val1.ToString() + "C" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 2);
                        }
                        else if (expresion[k] == 'P')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Val2 = Tomar_Derecha(expresion.Substring(k + 1));
                            Resultado = (Fact_Sigma((int)Val1, 1)) / (Fact_Sigma((int)Val1 - (int)Val2, 1));
                            expresion = expresion.Replace(Val1.ToString() + "P" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 2);
                        }
                    }
                    return Resolver(expresion, 1);

                case 3: //Jerarquia 3
                    for (int k = 0; k < expresion.Length; k++)
                    {
                        if (expresion[k] == '^')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Val2 = Tomar_Derecha(expresion.Substring(k + 1));
                            Resultado = Math.Pow(Val1, Val2);
                            expresion = expresion.Replace(Val1.ToString() + "^" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 3);
                        }
                        else if (expresion[k] == '√')
                        {
                            Val2 = Tomar_Derecha(expresion.Substring(k + 1));
                            Resultado = Math.Sqrt(Val2);
                            expresion = expresion.Replace("√" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 3);
                        }
                        else if (expresion[k] == '³')
                        {
                            if (k < expresion.Length - 1)
                            {
                                if (expresion[k + 1] == '√')
                                {
                                    Val2 = Tomar_Derecha(expresion.Substring(k + 2));
                                    Resultado = Math.Pow(Val2, (1.0 / 3.0));
                                    expresion = expresion.Replace("³√" + Val2.ToString(), Resultado.ToString());
                                    return Resolver(expresion, 3);
                                }
                                else
                                {
                                    Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                                    Resultado = Math.Pow(Val1, 3);
                                    expresion = expresion.Replace(Val1.ToString() + "³", Resultado.ToString());
                                    return Resolver(expresion, 3);
                                }
                            }
                            else
                            {
                                Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                                Resultado = Math.Pow(Val1, 3);
                                expresion = expresion.Replace(Val1.ToString() + "³", Resultado.ToString());
                                return Resolver(expresion, 3);
                            }

                        }
                        else if (expresion[k] == '²')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Resultado = Math.Pow(Val1, 2);
                            expresion = expresion.Replace(Val1.ToString() + "²", Resultado.ToString());
                            return Resolver(expresion, 3);
                        }
                        else if(expresion[k]== 'ˣ' && expresion[k+1] == '√')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Val2 = Tomar_Derecha(expresion.Substring(k + 2));
                            Resultado = Math.Pow(Val2, (1.0 / Val1));
                            expresion = expresion.Replace(Val1.ToString()+"ˣ√" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 3);
                        }
                        else if(expresion[k] == 'ˣ')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Val2 = Tomar_Derecha(expresion.Substring(k + 1));
                            Resultado = Math.Pow(Val1, Val2);
                            expresion = expresion.Replace(Val1.ToString()+"ˣ" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 3);
                        }
                        else if (expresion[k] == '⁻')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Resultado = 1.0 / Val1;
                            expresion = expresion.Replace(Val1.ToString() + "⁻¹", Resultado.ToString());
                            return Resolver(expresion, 3);
                        }
                        else if (expresion[k] == 'E')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Val2 = Tomar_Derecha(expresion.Substring(k + 1));
                            Resultado = Val1*(Math.Pow(10, Val2));
                            expresion = expresion.Replace(Val1.ToString() + "E" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 3);
                        }
                        else if(expresion[k] == '%')
                        {
                            Val1 = Tomar_Izquierda(expresion.Substring(0, k));
                            Val2 = Tomar_Derecha(expresion.Substring(k + 1));
                            Resultado = Val1 % Val2;
                            expresion = expresion.Replace(Val1.ToString() + "%" + Val2.ToString(), Resultado.ToString());
                            return Resolver(expresion, 3);
                        }
                    }
                    return Resolver(expresion, 2);

                case 4: //jerarquia 4
                    for(int k = 0; k < expresion.Length; k++)
                    {
                        if (expresion[k] == 'a')
                        {
                            if(expresion[k+3] == 's')
                            {
                                if (expresion[k + 6] == 'h')
                                {
                                    Val2 = Tomar_Derecha(expresion.Substring(k + 7));
                                    Resultado=Math.Asin((Val2 * Math.PI) / 180);  //Reemplazar por Asinh
                                    expresion = expresion.Replace("arcsenh" + Val2.ToString(), Resultado.ToString());
                                    return Resolver(expresion, 4);
                                }
                                else
                                {
                                    Val2 = Tomar_Derecha(expresion.Substring(k + 6));
                                    Resultado=Math.Asin((Val2 * Math.PI) / 180);
                                    expresion = expresion.Replace("arcsen" + Val2.ToString(), Resultado.ToString());
                                    return Resolver(expresion, 4);
                                }
                            }
                            else if (expresion[k + 3] == 'c')
                            {
                                if (expresion[k + 6] == 'h')
                                {
                                    Val2 = Tomar_Derecha(expresion.Substring(k + 7));
                                    Resultado=Math.Acos((Val2 * Math.PI) / 180);  //Reemplazar por Acosh
                                    expresion = expresion.Replace("arccosh" + Val2.ToString(), Resultado.ToString());
                                    return Resolver(expresion, 4);
                                }
                                else
                                {
                                    Val2 = Tomar_Derecha(expresion.Substring(k + 6));
                                    Resultado=Math.Acos((Val2 * Math.PI) / 180);
                                    expresion = expresion.Replace("arccos" + Val2.ToString(), Resultado.ToString());
                                    return Resolver(expresion, 4);
                                }
                            }
                            else if (expresion[k + 3] == 't')
                            {
                                if (expresion[k + 4] == 'h')
                                {
                                    Val2 = Tomar_Derecha(expresion.Substring(k + 7));
                                    Resultado=Math.Atan((Val2 * Math.PI) / 180); //Reemplazar por Atanh
                                    expresion = expresion.Replace("arctanh" + Val2.ToString(), Resultado.ToString());
                                    return Resolver(expresion, 4);
                                }
                                else
                                {
                                    Val2 = Tomar_Derecha(expresion.Substring(k + 6));
                                    Resultado=Math.Atan((Val2 * Math.PI) / 180);
                                    expresion = expresion.Replace("arctan" + Val2.ToString(), Resultado.ToString());
                                    return Resolver(expresion, 4);
                                }
                            }
                        }
                        else if (expresion[k] == 's')
                        {
                            if (expresion[k + 3] == 'h')
                            {
                                Val2 = Tomar_Derecha(expresion.Substring(k + 4));
                                Resultado=Math.Sinh((Val2 * Math.PI) / 180);
                                expresion = expresion.Replace("senh" + Val2.ToString(), Resultado.ToString());
                                return Resolver(expresion, 4);
                            }
                            else
                            {
                                Val2 = Tomar_Derecha(expresion.Substring(k + 3));
                                Resultado=Math.Sin((Val2 * Math.PI) / 180);
                                expresion = expresion.Replace("sen" + Val2.ToString(), Resultado.ToString());
                                return Resolver(expresion, 4);
                            }
                        }
                        else if (expresion[k] == 'c')
                        {
                            if (expresion[k + 3] == 'h')
                            {
                                Val2 = Tomar_Derecha(expresion.Substring(k + 4));
                                Resultado=Math.Cosh((Val2 * Math.PI) / 180);
                                expresion = expresion.Replace("cosh" + Val2.ToString(), Resultado.ToString());
                                return Resolver(expresion, 4);
                            }
                            else
                            {
                                Val2 = Tomar_Derecha(expresion.Substring(k + 3));
                                Resultado =Math.Cos((Val2 * Math.PI) / 180);
                                expresion = expresion.Replace("cos" + Val2.ToString(), Resultado.ToString());
                                return Resolver(expresion, 4);
                            }
                        }
                        else if (expresion[k] == 't')
                        {
                            if (expresion[k + 3] == 'h')
                            {
                                Val2 = Tomar_Derecha(expresion.Substring(k + 4));
                                Resultado=Math.Tanh((Val2 * Math.PI) / 180);
                                expresion = expresion.Replace("tanh" + Val2.ToString(), Resultado.ToString());
                                return Resolver(expresion, 4);
                            }
                            else
                            {
                                Val2 = Tomar_Derecha(expresion.Substring(k + 3));
                                Resultado=Math.Tan((Val2 * Math.PI) / 180);
                                expresion = expresion.Replace("tan" + Val2.ToString(), Resultado.ToString());
                                return Resolver(expresion, 4);
                            }
                        }
                        else if (expresion[k] == 'l')
                        {
                            if (expresion[k + 1] == 'n')
                            {
                                Val2 = Tomar_Derecha(expresion.Substring(k + 2));
                                Resultado = Math.Log(Val2);
                                expresion = expresion.Replace("ln" + Val2.ToString(), Resultado.ToString());
                                return Resolver(expresion, 4);
                            }
                            else
                            {
                                Val2 = Tomar_Derecha(expresion.Substring(k + 3));
                                Resultado = Math.Log10(Val2);
                                expresion = expresion.Replace("log" + Val2.ToString(), Resultado.ToString());
                                return Resolver(expresion, 4);
                            }
                        }
                    }
                    return Resolver(expresion, 3);
            }
            return expresion;
        }

        //Entrar a los parentesis para resolver desde adentro
        string Entrar_parentesis(string expresion) {

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

                //Resolver en caso de que haya más de un parentesis en el mismo nivel
                while(InParAbierto >= 0)
                {
                    expresion = expresion.Replace(expresion.Substring(InParAbierto, InParCerrado - InParAbierto + 1), Entrar_parentesis(expresion.Substring(InParAbierto + 1, InParCerrado - InParAbierto - 1)));
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
                expresion = Resolver(expresion, 4);
                return expresion;
            }
            else
            {
                //Aquí se manda a resolver
                expresion = Resolver(expresion, 4);
                return expresion;
            }
                
        }
        /* ###########################################################
                         Funcion sumatoria y factorial
           ########################################################### */

        public int Fact_Sigma(int n, int flag)
        {
            if (n == 1 || n==0)
                return 1;
            else
            {
                if(flag==1)
                   return n * Fact_Sigma(n - 1, flag);
                else
                    return n + Fact_Sigma(n - 1, flag);
            }
        }

        /* ###########################################################
                             Funciones Botones
           ########################################################### */

        private void Igual_Click(object sender, EventArgs e)
        {
            if (Syntax_Check(PantallaEcuacion.Text))
                PantallaEcuacion.Text = "Syntax Error";
            else
            {
                if(PantallaEcuacion.Text.IndexOf('(')!=-1)
                    PantallaResultados.Text = Entrar_parentesis(PantallaEcuacion.Text);
                else
                    PantallaResultados.Text = Resolver(PantallaEcuacion.Text, 4);
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
