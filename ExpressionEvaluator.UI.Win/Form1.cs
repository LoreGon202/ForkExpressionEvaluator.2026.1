using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ExpressionEvaluator.Core;

namespace ExpressionEvaluator.UI.Win
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string expression = "";

        private void AddToExpression(string value)
        {
            expression += value;
            txtDisplay.Text = expression;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            string value = btn.Text;

            //en la siguiente linea se reemplaza el simbolo visible en los botones por el simbolo que se utiliza en la evaluacion de la expresion, es decir el simbolo "X" se reemplaza por "*" y el simbolo "÷" se reemplaza por "/"
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
            {
                var result = Evaluator.Evaluate(txtDisplay.Text);
                txtResult.Text = result.ToString();

                // las siguientes lineas muestran el resultado en el mismo display donde se ingresa la operación
                //txtDisplay.Text = result.ToString();
                //expression = "";
            }
            catch
            {
                //txtDisplay.Text = "Error"; // esta linea muestra el mensaje de error en el mismo display donde se ingresa la operación, pero se borra la operación ingresada, es decir el usuario no puede corregir la operación ingresada
                txtResult.Text = "Error";
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
            if (!string.IsNullOrEmpty(expression) && expression.Length > 0) // esta linea es para evitar que se intente eliminar un caracter de una cadena vacía, lo que causaría un error, es decir el boton delete no funcionara dentro del resultado de una operación
            {
                expression = expression.Substring(0, expression.Length - 1);
                txtDisplay.Text = expression;
            }
        }
    }
}
