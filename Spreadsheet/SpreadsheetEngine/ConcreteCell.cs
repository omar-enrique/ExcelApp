//Author: Omar Finol-Evans
//ID: 11514759
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class ConcreteCell : Cell // Derived Concrete Cell for instantiation
    {
        public ConcreteCell(int rowInt, int colInt) : base(rowInt, colInt)
        {
            
        }

        public void setValue(string val)
        {
            _Value = val;
        }
    }
}
