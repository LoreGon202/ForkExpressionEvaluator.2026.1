using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ExpressionEvaluator.Core;
using System.Linq; //evalua los parentesis.

namespace ExpressionEvaluator.UI.Win
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private string expression = "";

        private void AddToExpression(string value)
        {
            expression = txtDisplay.Text;

            //Aquí se evita ingresar dos operadores seguidos,
            //es decir si el usuario ingresa un operador después de otro operador,
            //el segundo operador no se agregará a la expresión, lo que evita errores en la evaluación de la expresión,
            //por ejemplo si el usuario ingresa "2++3" o "4*/5", el segundo operador no se agregará a la expresión
            //y la evaluación de la expresión no causará un error, sino que se evaluará como "2+3" o "4/5" respectivamente 
            {
                string operators = "+-*/^";

                if (operators.Contains(value))
                {
                    if (expression.Length > 0 && operators.Contains(expression[^1]))
                    {
                        return;
                    }
                }

                // VALIDACIÓN DE PARÉNTESIS
                if (value == ")")
                {
                    int open = expression.Count(c => c == '(');
                    int close = expression.Count(c => c == ')');

                    if (close >= open)
                    {
                        txtResult.Text = "Falta abrir un Parentesis";
                        return;
                    }
                }

                // esto me permite agregar un dígito en la ubicación del cursor,
                // es decir el usuario puede ingresar la operación en cualquier parte del display,
                // no solo al final de la operación,
                // y el resultado se mostrará en el mismo display donde se ingresa la operación,
                // lo que permite al usuario corregir la operación ingresada antes de obtener el resultado

                int cursorPosition = txtDisplay.SelectionStart;

                // Multiplicación implícita: número seguido de '('
                if (value == "(" && expression.Length > 0)
                {
                    char lastChar = expression[cursorPosition - 1];

                    if (char.IsDigit(lastChar) || lastChar == ')')
                    {
                        expression = expression.Insert(cursorPosition, "*");
                        cursorPosition++;
                    }
                }

                expression = expression.Insert(cursorPosition, value);

                txtDisplay.Text = expression;

                txtDisplay.SelectionStart = cursorPosition + value.Length;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            string value = btn.Text;

            //en la siguiente linea se reemplaza el simbolo visible en los botones
            //por el simbolo que se utiliza en la evaluacion de la expresion,
            //es decir el simbolo "X" se reemplaza por "*" y el simbolo "÷" se reemplaza por "/"
            if (value == "X")
                value = "*";
            if (value == "÷")
                value = "/";

            AddToExpression(value);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void btnEqual_Click(object sender, EventArgs e)
        {
            try

            {// VALIDACIÓN DE PARÉNTESIS
                int open = txtDisplay.Text.Count(c => c == '(');
                int close = txtDisplay.Text.Count(c => c == ')');

                if (open != close)
                {
                    txtResult.Text = "Hay paréntesis sin cerrar";
                    return;
                }

                string evalExpression = txtDisplay.Text;

                // corregir negativos
                if (evalExpression.StartsWith("-"))
                {
                    evalExpression = "0" + evalExpression;
                }

                evalExpression = evalExpression.Replace("(-", "(0-");

                var result = Evaluator.Evaluate(evalExpression);

                txtResult.Text = result.ToString();
                expression = "";
            }

            catch
            {
                // esta linea muestra el mensaje de error en el mismo display donde se ingresa la operación,
                // pero se borra la operación ingresada,
                // es decir el usuario no puede corregir la operación ingresada
                //txtDisplay.Text = "Error";

                txtResult.Text = "Error"; //El error se muesra en el segundo display (txtResult)
                expression = "";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            expression = "";
            txtDisplay.Text = "";
            txtResult.Text = "";
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //mantiene el foco en el display donde el usuario coloque el cursor para
            //agresar o corregir un dato
            txtDisplay.Focus();
            expression = txtDisplay.Text;

            if (txtResult.Text == "Error")
            {
                txtResult.Text = "";

            }

            //ellimina el caracter a la izquierda del cursor,
            //es decir si el usuario tiene el cursor en medio de la operación ingresada,
            //el boton delete eliminará el caracter a la izquierda del cursor,
            //lo que permite al usuario corregir la operación ingresada antes de obtener el resultado
            int cursorPosition = txtDisplay.SelectionStart;

            // esta linea es para evitar que se intente eliminar un caracter de una cadena vacía,
            // lo que causaría un error, es decir el boton delete no funcionara dentro del resultado de una operación
            if (!string.IsNullOrEmpty(expression) && expression.Length > 0)
            {
               expression = expression.Remove(cursorPosition - 1, 1);
                txtDisplay.Text = expression;
                txtDisplay.SelectionStart = cursorPosition - 1;
            }

        }

        //El siguiente metodo permite al usuario usar el teclado para ingresar la operación y obtener el resultado,
        //puede usar las teclas numéricaas y enter para obtener el resultado, y la tecla backspace para eliminar un caracter,
        //lo que mejora la experiencia de usuario al permitirle usar el teclado en lugar de solo los botones del mouse
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnEqual_Click(this, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.Back)
            {
                btnDelete_Click(this, EventArgs.Empty);
            }
        }
    }
}