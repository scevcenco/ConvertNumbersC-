using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertNumbers
{
    public enum Systems
    {
        Двоичная,
        Восьмиричная,
        Десятиричная,
        Шестнадцатиричная
    }
    public partial class Form1 : Form
    {
        Dictionary<char, double> Pace16 = new Dictionary<char, double>
        {
            {'0', 0},
            {'1', 1},
            {'2', 2},
            {'3', 3},
            {'4', 4},
            {'5', 5},
            {'6', 6},
            {'7', 7},
            {'8', 8},
            {'9', 9},
            {'A', 10},
            {'B', 11},
            {'C', 12},
            {'D', 13},
            {'E', 14},
            {'F', 15},
        };
        public Form1()
        {
            InitializeComponent();
            comboBox1.DataSource = Enum.GetValues(typeof(Systems));   //gets the type, turns enum to array      
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Systems baseFrom;
            double number, result10;
            bool flag;
            int precise;
            baseFrom = (Systems)comboBox1.SelectedIndex;
            flag = int.TryParse(textBox2.Text, out precise);
            if (!flag)
            {
                MessageBox.Show("Введите целое число", "Ошибка ввода");
                return;   
            }
            switch (baseFrom)
            {
                case Systems.Двоичная:
                    if (!ConvertToDouble(textBox1.Text, out number))
                        return;
                    result10 = ConvertToBase10(number, precise, 2);
                    textBox6.Text = result10.ToString();
                    textBox3.Text = textBox1.Text;
                    textBox4.Text = ConvertToString(ConvertToNewBase(result10, 10, 8, precise));
                    textBox5.Text = ConvertToString(ConvertToNewBase(result10, 10, 16, precise));
                    break;
                case Systems.Восьмиричная:
                    if (!ConvertToDouble(textBox1.Text, out number))
                        return;
                    result10 = ConvertToBase10(number, precise, 8);
                    textBox6.Text = result10.ToString();
                    textBox4.Text = textBox1.Text;
                    textBox3.Text = ConvertToString(ConvertToNewBase(result10, 10, 2, precise));
                    textBox5.Text = ConvertToString(ConvertToNewBase(result10, 10, 16, precise));
                    break;
                case Systems.Десятиричная:
                    if (!ConvertToDouble(textBox1.Text, out number))
                        return;
                    textBox3.Text = ConvertToString(ConvertToNewBase(number, 10, 2, precise));
                    textBox4.Text = ConvertToString(ConvertToNewBase(number, 10, 8, precise));
                    textBox6.Text = ConvertToString(ConvertToNewBase(number, 10, 10, precise));
                    textBox5.Text = ConvertToString(ConvertToNewBase(number, 10, 16, precise));
                    break;
                case Systems.Шестнадцатиричная:
                    result10 = Convert16To10(textBox1.Text, precise);
                    textBox6.Text = result10.ToString();
                    textBox5.Text = textBox1.Text;
                    textBox3.Text = ConvertToString(ConvertToNewBase(result10, 10, 2, precise));
                    textBox4.Text = ConvertToString(ConvertToNewBase(result10, 10, 8, precise));
                    break;
            }
            
        }
        private List<string> ConvertToNewBase (double number, int baseNumber, int convertBaseNumber, int precise)
        {
            List<string> result = new List<string>();
            int integer, integer2;
            double fractional;
            integer = Convert.ToInt32(Math.Truncate(number));
            fractional = number - integer;
            while (integer>0)
            {
                result.Add((integer % convertBaseNumber).ToString());
                integer /= convertBaseNumber;
            }
            result.Reverse();
            result.Add(".");
            for (int i=0; i<=precise; i++)
            {
                fractional*=convertBaseNumber;
                integer2 = Convert.ToInt32(Math.Truncate(fractional));
                result.Add(integer2.ToString());
                fractional-=integer2;
            }
            return result;
        }
        private string ConvertToString(List<string> result)
        {
            string stringResult = "";
            foreach (string element in result)
            {
                switch(element)
                {
                    case "10":
                        stringResult+="A";
                        break;
                    case "11":
                        stringResult+="B";
                        break;
                    case "12":
                        stringResult+="C";
                        break;
                    case "13":
                        stringResult+="D";
                        break;
                    case "14":
                        stringResult+="E";
                        break;
                    case "15":
                        stringResult+="F";
                        break;
                    default: stringResult+=element;
                        break;
                }
            }
            return stringResult;
        }
        private double ConvertToBase10(double input, int precise, int fromBase)
        {
            double result = 0;
            int integer = Convert.ToInt32(Math.Truncate(input));
            double fractional = input - integer;
            fractional = Math.Round(fractional, precise);
            for (int i=0, j=10; i<integer.ToString().Length; i++, j*=10)
            {
                result += (integer % j) / (j / 10) * Math.Pow(fromBase, i);
            }
            for (int i=-1, j=10; Math.Abs(i)<=precise; i--, j*=10)
            {
                result += Math.Truncate(fractional * j) % 10 * Math.Pow(fromBase, i);
            }
            return result;
        }
        private double Convert16To10(string input, int precise)
        {
            string integer, fractional="";
            integer = input.Split(new char[] { ',', '.' })[0];
            if (input.Length-1>integer.Length+1)
            fractional = input.Substring(integer.Length + 1);
            double result = 0;
            for (int i = 0; i < integer.Length; i++)
            {
                result += Math.Pow(16, i) * Pace16[integer[integer.Length - 1 - i]];
            }
            for (int i = -1; Math.Abs(i) <= precise && Math.Abs(i)<=fractional.Length; i--)
            {
                result += Math.Pow(16, i) * Pace16[fractional[Math.Abs(i) - 1]];
            }
            return result;
        }
        private bool ConvertToDouble(string input, out double number)
        {
            bool flag = double.TryParse(input, out number);
            if (!flag)
            {
                MessageBox.Show( "Введите число", "Ошибка ввода");
            }
            return flag;
        }
                  
    }
}
