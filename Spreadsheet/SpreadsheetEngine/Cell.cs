//Author: Omar Finol-Evans
//ID: 11514759
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public abstract class Cell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ExpTree evalTree;
        public delegate void Evaluate();
        public Evaluate evaluate;
        private List<Cell> list = new List<Cell>();

        private readonly int _RowIndex;
        public int RowIndex
        {
            get { return _RowIndex; }
        }

        private readonly int _ColumnIndex;
        public int ColumnIndex
        {
            get { return _ColumnIndex; }
        }

        protected string _Text;

        public string Text
        {
            get { return _Text; }
            set {
                if(value != _Text)
                {
                    _Text = value;

                    foreach (Cell cell in list)
                    {
                        cell.evaluate -= this.valueChanged;
                    }
                    list.Clear();
                    evalTree = null;

                    OnPropertyChanged("Text"); // Broadcast Property Changed event when text is changed

                    if(evaluate != null)
                        evaluate();
                }
            }
        }

        protected string _Value;

        public string Value
        {
            get { return _Value; }
        }

        public Cell(int rowInt, int colInt)
        {
            evalTree = null;

            _Text = "";
            _Value = "";
            _RowIndex = rowInt;
            _ColumnIndex = colInt;
        }

        protected void OnPropertyChanged(string text) // Function for broadcasting a Property changed event
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(text));
        }

        public void buildTree(string expression, Spread sheet)
        {
            evalTree = new ExpTree(expression, ref sheet);
            _Value = evalTree.Eval().ToString();
        }

        public void addDependentCell(ref Cell cell)
        {
            list.Add(cell);
        }

        public void valueChanged()
        {
            _Value = evalTree.Eval().ToString();
            OnPropertyChanged("Value");
        }

        public int getRowIndex()
        {
            return RowIndex;
        }
        
        public int getColumnIndex()
        {
            return ColumnIndex;
        }
    }
}
