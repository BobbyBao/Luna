using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using SharpLuna.Unity;
using SharpLuna;

namespace Assets.Editor
{
    class LunaTableWindow : EditorWindow
    {
        [SerializeField] TreeViewState m_TreeViewState;

        LunaTableView m_TableTreeView;
        Luna luna;
        void OnEnable()
        {
            // Check whether there is already a serialized view state (state 
            // that survived assembly reloading)
            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState();

            m_TableTreeView = new LunaTableView(m_TreeViewState);      
            luna = LunaClient.Luna; 
            m_TableTreeView.Refresh(luna);

            LunaClient.LunaCreate += LunaClient_LunaCreate;
            LunaClient.LunaDestroy += LunaClient_LunaDestroy;
             
        }

        private void LunaClient_LunaCreate(Luna obj)
        {
            if (luna != obj)
            {
                luna = obj;
                m_TableTreeView.Refresh(luna);
                
            }
        }

        private void LunaClient_LunaDestroy(Luna obj)
        {
            if (luna == obj)
            {
                luna = null;
                m_TableTreeView.Refresh(luna);
            }

        }

        void OnGUI()
        {
            m_TableTreeView.OnGUI(new Rect(0, 0, position.width, position.height));
        }

        [MenuItem("Luna/Table View")]
        static void ShowWindow()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<LunaTableWindow>();
            window.titleContent = new GUIContent("GlobalTable");
            window.Show();
        }
    }

    public class LunaTableView : TreeView
    {
        Dictionary<int, LuaRef> id2v = new Dictionary<int, LuaRef>();
        Dictionary<int, string> id2fullPath = new Dictionary<int, string>();
        Dictionary<int, TreeViewItem> treeViewItems = new Dictionary<int, TreeViewItem>();
        Luna luna;

        public event Action<List<LuaRef>> selectionChanged;
        public LunaTableView(TreeViewState treeViewState)
            : base(treeViewState)
        {
        }

        public void Refresh(Luna ln)
        {
            luna = ln;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "_G" };
           
            if (luna != null)
            {
                string fullPath = "_G";
                int hash = fullPath.GetHashCode();
                var item = new TreeViewItem { id = hash, depth = 1, displayName = "_G" };
                LuaRef luaRef = luna.Global();
             
                id2v[hash] = luaRef;
                id2fullPath[hash] = fullPath;
                treeViewItems[hash] = item;

                lastExpendedIDs.Clear();
                lastExpendedIDs.Add(hash);

                SetExpanded(hash, true);
                item.icon = EditorGUIUtility.FindTexture("Folder Icon");
                root.AddChild(item);
          
                EnumerateDirectories(item, luaRef, 2);
            }
            else
            {
                var item = new TreeViewItem { id = 0, depth = 1, displayName = "_G" };
                root.AddChild(item);
            }

            // Return root of the tree
            return root;
        }

        void EnumerateDirectories(TreeViewItem parent, LuaRef parentLuaRef, int depth)
        {
            if (depth <= 0)
            {
                return;
            }

            string parentPath = id2fullPath[parent.id];

            foreach (var t in parentLuaRef)
            {
                var k = t.Key<string>();
                if (k == "_G")
                {
                    continue;
                }

                var v = t.Value();

                string fullPath = string.Join(".", parentPath, k);
                int hash = fullPath.GetHashCode();

                if (!treeViewItems.TryGetValue(hash, out var tvi))
                {
                    id2v[hash] = v;
                    id2fullPath[hash] = fullPath;
                    tvi = new TreeViewItem(hash, parent.depth + 1, k);
                    tvi.icon = EditorGUIUtility.FindTexture("Folder Icon");
                    treeViewItems[hash] = tvi;
                    parent.AddChild(tvi);
                }

                if (v.IsTable)
                {
                    EnumerateDirectories(tvi, v, depth - 1);
                }

            }
        }

        List<int> lastExpendedIDs = new List<int>();
        protected override void ExpandedStateChanged()
        {
            base.ExpandedStateChanged();

            var ids = this.state.expandedIDs;
            foreach (var id in ids)
            {
                if (!lastExpendedIDs.Contains(id))
                {
                    if (id2v.TryGetValue(id, out var luaRef))
                    {
                        EnumerateDirectories(treeViewItems[id], luaRef, 2);
                    }
                }
            }

            lastExpendedIDs.Clear();
            lastExpendedIDs.AddRange(ids);
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            List<LuaRef> sels = new List<LuaRef>();
            foreach (int id in selectedIds)
            {
                if (id2v.ContainsKey(id))
                {
                    sels.Add(id2v[id]);
                }
            }

            selectionChanged?.Invoke(sels);
        }
    }
}
