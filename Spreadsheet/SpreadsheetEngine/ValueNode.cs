//Author: Omar Finol-Evans
//ID: 11514759
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class ValueNode : Node
    {
        public ValueNode(string expression) : base(expression)
        {

        }

        public override double Eval()
        {
            return (double) Convert.ToInt32(data); // Simply return the number version of the data.
        }
    }
}
