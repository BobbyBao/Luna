using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SharpLuna;
using Tests;

namespace Test
{
    public partial class TableTree : Form
    {
        TestFramework framework;
        public Luna Luna => framework.Luna;
        public TableTree()
        {
            Luna.Print = this.Print;
            Luna.Error = this.Print;
            framework = new TestFramework();

            InitializeComponent();

            var it = Directory.EnumerateFiles("../Test/Scripts/", "*.luna");
            
            foreach (var file in it)
            {
                var fileInfo = new FileInfo(file);
                treeView1.Nodes.Add(fileInfo.Name);
            }

        }

        void Print(string msg)
        {
            listView1.Items.Add(msg);
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Luna.DoFile(e.Node.Text);
        }
    }
}
