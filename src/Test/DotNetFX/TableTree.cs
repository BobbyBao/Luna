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

            InitializeComponent();

            Luna.Print = this.Print;
            Luna.Error = this.Print;
            framework = new TestFramework();

            var it = Directory.EnumerateFiles(TestFramework.dataPath, "*.luna");
            
            foreach (var file in it)
            {
                var fileInfo = new FileInfo(file);
                treeView1.Nodes.Add(fileInfo.Name);
            }

            var g = Luna.Global();
            Refresh(g, treeView2.Nodes, 2);
        }

        void Print(string msg)
        {
            listBox1.Items.Add(msg);
        }

        void Refresh(LuaRef table, TreeNodeCollection node, int depth)
        {
            if(depth <= 0)
            {
                return;
            }

            foreach (var t in table)
            {
                var k = t.Key<string>();
                var v = t.Value();
                TreeNode n;
                if (!node.ContainsKey(k))
                {
                    n = node.Add(k, k);
                    n.Tag = v;
                }
                else
                {
                    n = node[k];
                }


                if (v.IsTable)
                {
                    Refresh(v, n.Nodes, depth - 1);
                }
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Luna.DoFile(e.Node.Text);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;
        }

        private void treeView2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            LuaRef v = (LuaRef)e.Node.Tag;
            if (v.IsTable)
            {
                Refresh(v, e.Node.Nodes, 2);
            }
        }

        private void treeView2_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            LuaRef v = (LuaRef)e.Node.Tag;
            if (v.IsTable)
            {
                Refresh(v, e.Node.Nodes, 2);
            }
        }
    }
}
