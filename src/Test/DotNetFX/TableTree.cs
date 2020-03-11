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

        public static ModuleInfo testModule = new ModuleInfo
        {
            typeof(UIManager),
        };

        public TableTree()
        {

            InitializeComponent();

            UIManager.Init(this);


            Luna.Print = this.Print;
            framework = new TestFramework(testModule);
            framework.Start();


            EnumerateFiles(null, TestFramework.dataPath);

            var g = Luna.Global;
            Refresh(g, treeView2.Nodes, 2);
        }

        void Print(string msg)
        {
            int last = 0;
            for(int i = 0; i < msg.Length; i++)
            {
                if(msg[i] == '\n')
                {
                    listBox1.Items.Add(msg.Substring(last, i - last));
                    last = i;
                }
            }

            if(last < msg.Length)
            {
                listBox1.Items.Add(msg.Substring(last, msg.Length - last));
            }

        }

        void EnumerateFiles(TreeNode node, string path)
        {
            var it = Directory.EnumerateDirectories(path);
            foreach(var dir in it)
            {
                DirectoryInfo info = new DirectoryInfo(dir);
                if (node != null)
                {
                    var child = node.Nodes.Add(info.Name);                    
                    EnumerateFiles(child, dir);
                    child.ExpandAll();
                }
                else
                {
                    var child = treeView1.Nodes.Add(info.Name);
                    EnumerateFiles(child, dir);
                    child.ExpandAll();
                }
            }

            it = Directory.EnumerateFiles(path, "*.luna");
            foreach (var file in it)
            {
                var fileInfo = new FileInfo(file);
                if(node != null)
                {
                    var child = node.Nodes.Add(fileInfo.Name);
                    child.Tag = node.FullPath + "/" + fileInfo.Name;
                }
                else
                {
                    var child = treeView1.Nodes.Add(fileInfo.Name);
                    child.Tag = fileInfo.Name;
                }
            }
        }

        void Refresh(LuaRef table, TreeNodeCollection node, int depth)
        {
            if(depth <= 0)
            {
                return;
            }

            foreach (var t in table)
            {
                var key = t.Key();
                var k = key.ToString();
                if (k == "_G")
                {
                    continue;
                }

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
            if(e.Node.Tag != null)
            Luna.DoFile(e.Node.Tag as string);
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

        public Action onRecharge;
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //UIManager.alertBox.Show();
            onRecharge?.Invoke();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            framework.GenerateWraps();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            var g = Luna.Global;
            Refresh(g, treeView2.Nodes, 2);
        }
    }
}
