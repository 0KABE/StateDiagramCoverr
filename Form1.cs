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
using System.Diagnostics;

namespace StateMachine.Algorithm
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
            // Graph graph = new Graph(file);
            // var verticesPaths = graph.VerticesCover();
            VerticesCover verticesCover = new VerticesCover(new Graph(file));
            OutPutTextBox.Text = "满足所有点覆盖情况下的最少路径\r\n" + string.Join("\r\n", verticesCover.VerticesCoverPaths());
            // var edgesPaths = graph.EdgesCover();
            EdgesCover edgesCover = new EdgesCover(new Graph(file));
            OutPutTextBox.Text += "\r\n满足所有边覆盖情况下的所有路径\r\n" + string.Join("\r\n", edgesCover.EdgesCoverPaths());
        }

        private void GenerateStateDiagramButton_Click(object sender, EventArgs e)
        {
            MdlParser parser = new MdlParser(MdlFilePathTextBox.Text);
            MDLFile file = parser.Parse();
            Graph graph = new Graph(file);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PlantUML file(*.wsd)|*.wsd";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, graph.ToPlantuml());
                Process process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c plantuml " + dialog.FileName;
                process.Start();
                process.WaitForExit();
                PlantUML plantUML = new PlantUML(dialog.FileName.Replace(".wsd", ".png"));
                plantUML.Show();
            }
        }
    }
}
