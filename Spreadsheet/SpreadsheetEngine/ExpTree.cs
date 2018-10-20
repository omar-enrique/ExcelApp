//Author: Omar Finol-Evans
//ID: 11514759

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CptS321
{
    public class ExpTree
    {
        private enum Precedence { LOW, MED, HIGH } // Enum values for precedence
        private Node root;
        private Spread sheet;

        //private Dictionary<string, double> variables; // Dict holding defined variables from the expression
        private Stack<string> postStack; // Stack used to hold the expression after it has been coverted to postfix

        /*
         * Dictionary defining the precedence level of each operator allowed in the expression
         */ 
        private Dictionary<string, Precedence> precedence = new Dictionary<string, Precedence>
        {
            { "*", Precedence.MED },
            { "/", Precedence.MED },
            { "+", Precedence.LOW },
            { "-", Precedence.LOW },
        };

        public ExpTree(string expression, ref Spread sheet)
        {
            this.sheet = sheet;
            //variables = new Dictionary<string, double>();
            postStack = new Stack<string>();
            convertToPostFix(expression); // Convert expression to postfix
            GenerateTree(); // Then Generate the tree from postfix expression form
        }

        private void convertToPostFix(string expression)
        {
            Stack<string> opStack = new Stack<string>();

            Regex regex = new Regex(@"[\+\-\(\)*/&!%^]|\b[a-zA-Z][a-zA-Z0-9]*|\b[0-9]+"); // Regex for parsing all operators found in an expression

            // Find all matches in the expression for these regex's
            MatchCollection matches1 = regex.Matches(expression); 

            foreach (Match match in matches1) // For each operator in the expression
            {
                // If match.Value is an operator and the opstack is empty, or this is an opening parenthesis
                if (match.Value == "(" || (opStack.Count == 0 && precedence.ContainsKey(match.Value))) 
                {
                    opStack.Push(match.Value);
                }
                // If this is an operator and the opstack currently has a Count of at least 1
                else if (precedence.ContainsKey(match.Value))
                {
                    if (opStack.Peek() != "(" && precedence[opStack.Peek()] > precedence[match.Value])
                    {
                        postStack.Push(opStack.Pop());
                    }
                    opStack.Push(match.Value);
                }
                // If we have reached the end of a parenthesis
                else if (match.Value == ")")
                {
                    while (opStack.Peek() != "(")
                    {
                        postStack.Push(opStack.Pop());
                    }
                    opStack.Pop();
                }
                else // If match.Value is a variable or a number
                {
                    postStack.Push(match.Value);
                }

            }

            // Push whatever operators remain to the stack
            foreach (string exp in opStack)
            {
                postStack.Push(exp);
            }
        }

        // Function Generates the tree, must be run AFTER the expression is converted to postfix and stored in the postStack
        private void GenerateTree()
        {
            string currentTop = postStack.Peek();
            Stack<Node> opStack = new Stack<Node>();
            Stack<string> newStack = new Stack<string>();
            int col = 0; 
            int row = 0; 

            Node cur = null, left = null, right = null;

            while(postStack.Count > 0)
            {
                newStack.Push(postStack.Pop()); // Reverse PostStack so the left hand side is now on the top
            }

            while(newStack.Count > 0) // While postStack reversed is not empty
            {
                if(precedence.ContainsKey(newStack.Peek())) // Check if top of stack is an operator
                {
                    // Pop left and right operands
                    right = opStack.Pop();
                    left = opStack.Pop();

                    // Create tree with Expression as root and push to the opStack
                    cur = new ExpressionNode(newStack.Peek());
                    cur.setLeft(left);
                    cur.setRight(right);
                    opStack.Push(cur);

                    cur = null;  left = null; right = null;
                }
                else
                {
                    if (Char.IsDigit(newStack.Peek()[0])) // If top of stack is a number
                        opStack.Push(new ValueNode(newStack.Peek()));
                    else // If top of stack is a variable
                    {
                        col = (newStack.Peek()[0] - 65); // Get ascii value of column and then subtract to convert to correspond to cell coordinates (0 - 26)
                        row = Convert.ToInt32(newStack.Peek().Substring(1, newStack.Peek().Length - 1)) - 1; // Convert second digit to integer to get row
                        opStack.Push(new CellReferenceNode(newStack.Peek(), sheet.GetCell(row, col))); // Variable will be stored in CellReferenceNode until Spreadsheet application
                    }
                }
                newStack.Pop();
            }

            root = (opStack.Pop()); //Pop created tree off, set root as our root.
        }

        public double Eval()
        {
            double product = root.Eval();
            return product;
        }
    }
}
