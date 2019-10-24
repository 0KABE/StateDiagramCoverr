using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StateMachine.MdlReader;
using System.IO;

namespace StateMachine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GetMdlFilePathButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "MDL File(*.mdl)|*.mdl";
            if (file.ShowDialog() == DialogResult.OK)
            {
                MdlFilePathTextBox.Text = file.FileName;
            }
        }

        private void ParseMdlFileButton_Click(object sender, EventArgs e)
        {
            MdlParser parser = new MdlParser(MdlFilePathTextBox.Text);
            MDLFile file = parser.Parse();
            Graph graph = new Graph(file);
            var verticesPaths = graph.VerticesCover();
            OutPutTextBox.Text = "满足所有点覆盖情况下的最少路径\r\n" + string.Join("\r\n", verticesPaths);
            var edgesPaths = graph.EdgesCover();
            OutPutTextBox.Text += "满足所有边覆盖情况下的所有路径\r\n" + string.Join("\r\n", edgesPaths);
        }
    }
}
