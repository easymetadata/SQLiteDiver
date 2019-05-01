using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Windows.Forms;

namespace SQLiteDiverUI
{
    public partial class Form1 : Form
    {
        public static string dbFile = String.Empty;
        private Main main = new Main();

        public static string assVer = "v" +
            Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
            Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "." +
            Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
            //+  " Beta";



        public Form1()
        {
            InitializeComponent();

                  this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dbFile = string.Empty;
            Open();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count > 0)
            {
                string _selected = comboBox1.SelectedItem.ToString();
                
                dataGridView1.DataSource = Main.ds.Tables[_selected];

                GridViewFormating();
            }

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            Export();
            
        }

        //Apply some conditional gridview formatting.
        private void GridViewFormating()
        {
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                var ColName = c.Name.ToLower();

                if (ColName.StartsWith("id")  |  
                    ColName.EndsWith("id")  |
                    ColName.StartsWith("is")  )
                    c.Width = 150;
                else if (ColName.Contains("url") )
                    c.Width = 400;
                else if (ReadSQLite.ListOfDateIndicators.Contains(ColName))
                      c.Width = 180;
                else if (ColName.Contains("sql"))
                    c.Width = 1000;
                else
                    c.Width = 180;
                
            }

            dataGridView1.Refresh();
        }


      

        private void Open()
        {
            //reset from previous errors.
            ReadSQLite.iErrorHappened = 0;

            ReadSQLite._dtTableNames.Clear();
            Main.ds.Tables.Clear();
            Main.ds.Clear();
            Main.ds.AcceptChanges();

            comboBox1.SelectedText = string.Empty;
            comboBox1.Items.Clear();
            comboBox1.Text = string.Empty;

            dataGridView1.DataSource = null;
           // dataGridView1.Refresh();

            

            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
           // openFileDialog1.Filter = "SQLite |*.sqlite;*.db;*.";
          //  openFileDialog1.FilterIndex = 1;
            openFileDialog1.Title = "Select a sqlite database";

            if (String.IsNullOrEmpty(dbFile))
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    dbFile = openFileDialog1.FileName;

                }
            }

            dbOpen();
        }

        private void dbOpen()
        {
            //Sanity check
            if (String.IsNullOrEmpty(dbFile))
                return;

            //Do stuff to open and setup database for viewing

            string dbOpenedTitle = string.Empty;
            if (!String.IsNullOrEmpty(dbFile))
            {
                dbOpenedTitle = " Database: " + dbFile;

                groupBox1.Text = dbOpenedTitle;
                this.Text = "SQLiteDiver - " + dbOpenedTitle;
                //string _dbname = dbFile;
            }
            else { 
                this.Text = "SQLiteDiver";
                groupBox1.Text = " Database";
            }

                //Make sure dbFile has been assigned a file path.
                if (string.IsNullOrEmpty(dbFile))
                    return;

                //string _dbSourcefilename = Path.GetFileName(_dbname);
                if (!main.DoStuff(dbFile))
                    MessageBox.Show(main.FileStatus);

                if (ReadSQLite.iErrorHappened > 0)
                    MessageBox.Show("We may have an error");

                foreach (DataTable dt in Main.ds.Tables)
                {
                    comboBox1.Items.Add(dt.TableName);
                }

                //Select a table by default
                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;



                lblFileProperties.Text = Main.GetFileProps(dbFile);
        }


        private void Export()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //saveFileDialog1.Filter = "TXT|*.txt";
            saveFileDialog1.Title = "Save as TXT file";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                //export.ExportToFile(comboBox1.Text, Main.ds.Tables[comboBox1.Text], saveFileDialog1.FileName);

                if (export.ExportToFile(comboBox1.Text, Main.ds.Tables[comboBox1.Text], saveFileDialog1.FileName) &&
                    File.Exists(saveFileDialog1.FileName))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Export to: " + saveFileDialog1.FileName);
                    sb.AppendLine("Complete");
                    MessageBox.Show(sb.ToString());
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.SetOut(new TextBoxWriter(txtConsole));
            //Console.WriteLine("Now redirecting output to the text box");

            //Check if SQLiteDiver was passed a filename when it was opened from Explorer or command line.
            string[] args = Environment.GetCommandLineArgs();


            if (!System.Diagnostics.Debugger.IsAttached 
                && args.Length > 1)
            {

                if (!String.IsNullOrEmpty(args[1]))
                {
                    if (File.Exists(args[1]))
                    {
                        Console.WriteLine("Loading: " + args[1].ToString());
                        dbFile = args[1];
                        dbOpen();
                    }
                }
            }
            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }


        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //This is for ignoring the dreaded Byte[]/Blob error.
            try {

                        }
            catch (System.FormatException ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            ReadSQLite.bConvertEpochDates = checkBox1.Checked;

            Open();

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dBSchemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SELECT sql FROM sqlite_master WHERE type='table' and name = 'moz_cookies';

            if (ReadSQLite._dtTableNames.Rows.Count > 0)
            {
                DataRow[] dr = ReadSQLite._dtTableNames.Select("name = '" + comboBox1.Text + "'");
                dataGridView1.DataSource = dr[0].Table;

                GridViewFormating();
            }

        }

        

            void Form1_DragEnter(object sender, DragEventArgs e) {
              if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.All;
            }

            void Form1_DragDrop(object sender, DragEventArgs e) {
              string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
              //foreach (string file in files) Console.WriteLine(file);
                Console.WriteLine(files[0].ToString() );

                dbFile = string.Empty;

                dbFile = @files[0].ToString();

                Open();
            }

            private void rawCopyClipboardToolStripMenuItem_Click(object sender, EventArgs e)
            {
                //Copy/export raw blob data, when it can't be displayed in grid 
            }

          
 

    }

    public class TextBoxWriter : TextWriter
    {
        TextBox _output = null;

        public TextBoxWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _output.AppendText(value.ToString());
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
