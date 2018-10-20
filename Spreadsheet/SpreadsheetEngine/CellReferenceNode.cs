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
     * Node will hold the variables that reference cells from the Spreadsheet
     * Currently only holds user input variables that show up in the expression entered by the user
     */
    public class CellReferenceNode : Node
    {
        private Cell cell; 

        public CellReferenceNode(string expression, Cell cell) : base(expression)
        {
            this.cell = cell; // Get reference of the dictionary that defines the values of each variable in the expression
        }

        public override double Eval()
        {
            return Convert.ToDouble(cell.Value);

        }
    }
}
