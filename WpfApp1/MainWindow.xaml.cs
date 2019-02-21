using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string strNum = "";
        string intermediateResult = "";
        private List<string> strOfNumbers;
        private List<string> strOfOperators;

        public MainWindow()
        {
            InitializeComponent();

            strOfNumbers = new List<string>();
            strOfOperators = new List<string>();

            foreach(UIElement el in Numbers.Children)
            {
                if (el is Button)
                    ((Button)el).Click += NumberButtonClick;
            }
            foreach (UIElement el in Operations.Items)
            {
                if(el is Button)
                {
                    if(((Button)el).Name == "Clear")
                    {
                        ((Button)el).Click += ClearButtonClick;
                    }
                    else
                    {
                        ((Button)el).Click += OperatorButtonClick;
                    }
                }
            }
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            calculationView.Text = "";
            strOfNumbers.Clear();
            strOfOperators.Clear();
            intermediateResult = "";
            strNum = "";
        }

        private void OperatorButtonClick(object sender, RoutedEventArgs e)
        {
            string symb = ((Button)e.OriginalSource).Content.ToString();

            switch (symb)
            {
                case "+": case "-": case "*": case "/":  ArithmeticOperationHahdler(symb); break;
                case "=": FinalHandler();  break;
            }                     
        }

        private void NumberButtonClick(object sender, RoutedEventArgs e)
        {
            string symb = ((Button)e.OriginalSource).Content.ToString();
            if(intermediateResult != "")
            {
                calculationView.Text = "";
                intermediateResult = "";
            }

            calculationView.Text += symb;

            strNum += symb;
        }

        private void ArithmeticOperationHahdler(string operationSymbol)
        {
            if (strNum == "")
            {
                if (strOfNumbers.Count() == 0)
                {
                    string addedNum = intermediateResult == "" ? "0" : intermediateResult;
                    calculationView.Text = addedNum + operationSymbol;
                    strOfNumbers.Add(addedNum);
                    intermediateResult = "";
                }
                else
                {
                    calculationView.Text = calculationView.Text.Remove(calculationView.Text.Count() - 1, 1) + operationSymbol;
                    strOfOperators.RemoveAt(strOfOperators.Count() - 1);
                }
            }
            else
            {
                strOfNumbers.Add(strNum);
                calculationView.Text += operationSymbol;
            }

            strOfOperators.Add(operationSymbol);
            strNum = "";
        }

        private void FinalHandler()
        {
            if(strNum!="")
                strOfNumbers.Add(strNum);

            if (strOfNumbers.Count() != 0 && strOfOperators.Count() != 0 && strOfNumbers.Count() > strOfOperators.Count())
            {
                intermediateResult = Calculate();
                calculationView.Text += "="+ intermediateResult;
                strOfNumbers.Clear();
                strOfOperators.Clear();
                strNum = "";
            }                
        }

        private string Calculate()
        {            
            for (int i = 0; i < strOfOperators.Count(); i++)
            {
                string key = strOfOperators[i];

                if(key == "*" || key == "/")
                {
                    float res = 0;

                    switch (key)
                    {
                        case "*": res = float.Parse(strOfNumbers[i]) * float.Parse(strOfNumbers[i + 1]); break;
                        case "/": res = float.Parse(strOfNumbers[i]) / float.Parse(strOfNumbers[i + 1]); break;                        
                    }

                    strOfNumbers[i] = res.ToString();
                    strOfNumbers.RemoveAt(i + 1);
                    strOfOperators.RemoveAt(i);
                    i--;
                }
                
            }

            for (int i = 0; i < strOfOperators.Count(); i++)
            {
                string key = strOfOperators[i];

                if (key == "+" || key == "-")
                {
                    float res = 0;

                    switch (key)
                    {
                        case "+": res = float.Parse(strOfNumbers[i]) + float.Parse(strOfNumbers[i + 1]); break;
                        case "-": res = float.Parse(strOfNumbers[i]) - float.Parse(strOfNumbers[i + 1]); break;
                    }

                    strOfNumbers[i] = res.ToString();
                    strOfNumbers.RemoveAt(i + 1);
                    strOfOperators.RemoveAt(i);
                    i--;
                }

            }

            return strOfNumbers[0];
        }
    }
}
