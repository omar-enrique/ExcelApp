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
     * Base abstract class for all the Nodes that can be initialized 
     */
    public abstract class Node
    {
        protected Node left, right;
        protected string data;

        public Node(string expression)
        {
            data = expression; // Expression in current node
            left = null;
            right = null;
        }

        public string getData()
        {
            return data;
        }

        public ref Node getLeft()
        {
            return ref left;
        }
        public ref Node getRight()
        {
            return ref right;
        }

        public void setLeft(Node left)
        {
            this.left = left;
        }
        public void setRight(Node right)
        {
            this.right = right;
        }

        /*
         * Each Node has an Eval function it carries out depending on the type of node it is
         */
        public abstract double Eval();
    }
}
