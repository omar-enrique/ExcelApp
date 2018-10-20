//Author: Omar Finol-Evans
//ID: 11514759

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml;

namespace CptS321
{
    public class Spread
    {
        private Cell[,] cells;
        private int numRows, numCols;
        public event PropertyChangedEventHandler CellPropertyChanged;

        public int ColumnCount
        {
            get { return numCols; }
        }
        public int RowCount
        {
            get { return numRows; }
        }

        public Spread(int numRows, int numCols)
        {
            this.numCols = numCols;
            this.numRows = numRows;
            cells = new Cell[numRows, numCols];
            for(int i = 0; i < numRows; i++)
            {
                for(int j = 0; j < numCols; j++)
                {
                    cells[i, j] = new ConcreteCell(i, j);
                    cells[i, j].PropertyChanged += OnCellPropertyChanged; // Every cell is subscribed to by the spreadsheet
                }
            }
        }


        public Cell GetCell(int row, int col)
        {
            return cells[row, col];
        }

        /*
         * Called when cell's text is changed
         */
        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Regex regex = new Regex(@"\b[a-zA-Z][a-zA-Z0-9]*"); // Regex for parsing all operators found in an expression
            int row = 0;
            int col = 0;

            // Find all matches in the expression for these regex's
            MatchCollection matches;

            if (e.PropertyName == "Text")
            {
                ConcreteCell tmpCell = sender as ConcreteCell;
                if (tmpCell.Text.Length != 0)
                {
                    if (tmpCell.Text[0] != '=') // If first character is not '='
                    {
                        tmpCell.setValue(tmpCell.Text); // Simply value to new text
                    }
                    else
                    {
                        matches = regex.Matches(tmpCell.Text);

                        foreach(Match match in matches)
                        {
                            col = (match.Value[0] - 65); // Get ascii value of column and then subtract to convert to correspond to cell coordinates (0 - 26)
                            row = Convert.ToInt32(match.Value.Substring(1, match.Value.Length - 1)) - 1;

                            if(cells[row, col].Value == "" || cells[row, col].Value == null)
                            {
                                tmpCell.setValue("#REF!");
                                CellPropertyChanged(sender, new PropertyChangedEventArgs("Value"));
                                return;
                            }

                            cells[tmpCell.RowIndex, tmpCell.ColumnIndex].addDependentCell(ref cells[row, col]);

                            cells[row, col].evaluate += cells[tmpCell.RowIndex, tmpCell.ColumnIndex].valueChanged;
                        }

                        tmpCell.buildTree(tmpCell.Text.Substring(1, tmpCell.Text.Length - 1), this);
                    }
                }
                else
                    tmpCell.setValue(tmpCell.Text);
            }
            CellPropertyChanged(sender, new PropertyChangedEventArgs("Value"));

        }

        public void ReadFromXML(string fileName)
        {
            XmlReader xmlReader = XmlReader.Create(fileName);
            int row = 0, col = 0;

            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "spreadsheet"))
                {
                    while (xmlReader.Read())
                    {
                        if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "cell"))
                        {
                            col = (xmlReader.GetAttribute("name")[0] - 65); // Get ascii value of column and then subtract to convert to correspond to cell coordinates (0 - 26)
                            row = Convert.ToInt32(xmlReader.GetAttribute("name").Substring(1, xmlReader.GetAttribute("name").Length - 1)) - 1; // Convert second digit to integer to get row

                            xmlReader.Read();
                            cells[row, col].Text = xmlReader.Value;
                        }
                    }
                }
            }

            for (int i = numRows - 1; i >= 0; i--)
            {
                for (int j = numCols - 1; j >= 0; j--)
                {
                    if (cells[i, j].Text != null && cells[i, j].Text != "")
                        cells[i, j].Text = cells[i, j].Text;
                }
            }
        }

        public void WriteToXML(string fileName)
        {
            XmlWriter xmlWriter = XmlWriter.Create(fileName);
            string cellName = "";

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("spreadsheet");
            int row = 0; 
            char col = ' ';

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if(cells[i, j].Text != null && cells[i, j].Text != "")
                    {
                        col = (char) (j + 65); // Get ascii value of column and then subtract to convert to correspond to cell coordinates (0 - 26)
                        row = (i + 1); // Convert second digit to integer to get row
                        cellName = col.ToString() + row.ToString();
                        
                        xmlWriter.WriteStartElement("cell");
                        xmlWriter.WriteAttributeString("name", cellName);
                        xmlWriter.WriteString(cells[i, j].Text);
                        xmlWriter.WriteEndElement();
                    }
                }
            }

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }
    }
}
