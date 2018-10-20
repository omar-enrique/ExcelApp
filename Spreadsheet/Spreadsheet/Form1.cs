//Author: Omar Finol-Evans
//ID: 11514759
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml;
using CptS321;

namespace Spreadsheet
{
    public partial class Form1 : Form
    {
        private Spread sheet;

        public Form1()
        {
            InitializeComponent();
            dataGridView1.Columns.Clear();
            this.TopMost = true;
            this.WindowState = FormWindowState.Maximized;
            toolStripSplitButton1.Text = "";
            toolStripSplitButton2.Text = "";

            for (int i = 65; i < 91; i++) // Add all of the columns
                dataGridView1.Columns.Add(((char) i).ToString(), ((char) i).ToString());
            dataGridView1.Rows.Add(50);

            int j = 1;
            foreach (DataGridViewRow row in dataGridView1.Rows) // Number the rows 1 to 50
            {
                row.HeaderCell.Value = j.ToString();
                j++;
            }

            sheet = new Spread(50, 26); // Initialize Spreadsheet
            sheet.CellPropertyChanged += OnPropertyChanged; // Subscribe to CellPropertyChanged Event from Spreadsheet
        }
        
        /* Event handling setting the DataGridView cell to the corresponding Spreadsheet
         * cell's value after it has been computed
         */
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == "Value")
            {
                Cell tmp = sender as Cell;

                dataGridView1.Rows[tmp.RowIndex].Cells[tmp.ColumnIndex].Value = tmp.Value;
            }
        }

        /*
         * Controls button that runs demo
         */ 
        private void button1_Click(object sender, EventArgs e)
        {
            // No longer needed
        }

        /*
         * Event which is initiated by the editing of a cell from the DataGridView
         * If a cell is edited, then change the Cell's text to the new input text
         */
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1[e.ColumnIndex, e.RowIndex].Value != null)
            {
                sheet.GetCell(e.RowIndex, e.ColumnIndex).Text = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
                OnPropertyChanged(sheet.GetCell(e.RowIndex, e.ColumnIndex), new PropertyChangedEventArgs("Value"));
            }
        }

        // If user begins editing the cell
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = sheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
        }

        // Display text of the current cell onto text box
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = sheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
        }

        // When we hit enter while using the text box
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.KeyCode == Keys.Enter)
            {
                int col = dataGridView1.CurrentCell.ColumnIndex, row = dataGridView1.CurrentCell.RowIndex;
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (textBox1.Text != null)
                {
                    sheet.GetCell(row, col).Text = textBox1.Text;
                    OnPropertyChanged(sheet.GetCell(row, col), new PropertyChangedEventArgs("Value"));
                }
            }
        }

        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();

            saveFile.Filter = "XML Files (*.xml)|*.xml";
            saveFile.FilterIndex = 0;
            saveFile.RestoreDirectory = true;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                sheet.WriteToXML(saveFile.FileName);
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog opFile = new OpenFileDialog();

            opFile.InitialDirectory = "C:\\";
            opFile.Filter = "XML Files (*.xml)|*.xml";
            opFile.FilterIndex = 0;
            opFile.RestoreDirectory = true;

            if (opFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sheet.ReadFromXML(opFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error, file cannot be read. " + ex.Message);
                }
            }
        }

        private void toolStripSplitButton1_MouseHover(object sender, EventArgs e)
        {
            toolStripSplitButton1.ToolTipText = "Load File";
        }

        private void toolStripSplitButton1_MouseLeave(object sender, EventArgs e)
        {
            toolStripSplitButton1.ToolTipText = "";
        }

        private void toolStripSplitButton3_ButtonClick(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.Show();
            about.TopMost = true;
        }
    }
}
