//Author: Omar Finol-Evans
//ID: 11514759

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /*
     * Node holding an operator from the expression and the children nodes associated with the operator
     */
    public class ExpressionNode : Node
    {
        public ExpressionNode(string expression) : base(expression)
        {

        }

        /*
         * Evals the children of the Node depending on the current operator in this node
         */
        public override double Eval()
        {
            double num1 = left.Eval(), num2 = right.Eval();

            switch (data)
            {
                case "*":
                    return (num2 * num1);
                case "/":
                    return (num1 / num2);
                case "+":
                    return (num1 + num2);
                case "-":
                    return (num1 - num2);
                default:
                    return 0;
            }
        }
    }
}
