//Author: Omar Finol-Evans
//ID: 11514759

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;

namespace ConsoleApp1
{
    public class Menu
    {
        int option;
        private ExpTree tree;
        string expression;

        public Menu()
        {
            expression = "A1+B1+C1";
            option = 0;
            tree = new ExpTree(expression);
            displayMenu();
        }

        public void displayMenu()
        {
            do
            {
                Console.WriteLine("Menu (current expression = " + expression + ")");
                Console.WriteLine("\t1 = Enter a new expression");
                Console.WriteLine("\t2 = Set a variable value");
                Console.WriteLine("\t3 = Evaluate tree");
                Console.WriteLine("\t4 = Quit");

                option = Convert.ToInt32(Console.ReadLine());

                if(option < 1 || option > 4)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please type in a number between 1 and 4!\n");
                }
                Console.WriteLine();
            } while (option < 1 || option > 4);

            switch(option)
            {
                case 1:
                    Console.Write("Type in the next expression: ");
                    expression = Console.ReadLine();
                    tree = new ExpTree(expression);
                    displayMenu();
                    break;
                case 2:
                    string variable = "";
                    double value = 0;
                    Console.Write("Enter variable name: ");
                    variable = Console.ReadLine();
                    Console.Write("Enter value: ");
                    value = Convert.ToDouble(Console.ReadLine());

                    tree.SetVar(variable, value);
                    displayMenu();
                    break;
                case 3:
                    double result = tree.Eval();
                    Console.WriteLine(result);
                    displayMenu();
                    break;
                case 4:
                    Console.WriteLine("Done");
                    break;
            }
        }
    }
}
