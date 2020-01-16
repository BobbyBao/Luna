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

    public class LunaTreeViewItem : TreeViewItem
    {
        public LuaType luaType;
        public string fullPath;

        public override int CompareTo(TreeViewItem other)
        {
            if (other is LunaTreeViewItem item)
            {
                if (item.luaType != luaType)
                {
                    return Math.Sign(luaType - item.luaType);
                }

            }

            return base.CompareTo(other);
        }
       
    }

    public class LunaTableView : TreeView
    {
        Dictionary<int, LuaRef> id2v = new Dictionary<int, LuaRef>();
        Dictionary<int, string> id2fullPath = new Dictionary<int, string>();
        Dictionary<int, LunaTreeViewItem> treeViewItems = new Dictionary<int, LunaTreeViewItem>();
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
            SetExpanded(0, true);

            if (luna != null)
            {
                string fullPath = "_G";
                int hash = fullPath.GetHashCode();
                var item = new LunaTreeViewItem { id = hash, depth = 1, displayName = "_G" };
                LuaRef luaRef = luna.Global();
                item.luaType = luaRef.Type;
                item.fullPath = fullPath;
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

        void EnumerateDirectories(LunaTreeViewItem parent, LuaRef parentLuaRef, int depth)
        {
            if (depth <= 0)
            {
                return;
            }

            string parentPath = parent.fullPath;

            foreach (var t in parentLuaRef)
            {
                var key = t.Key();                
                if (key.Type != LuaType.String)
                {
                    continue;
                }

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
                    tvi = new LunaTreeViewItem { id = hash, depth = parent.depth + 1, displayName = k };
                    tvi.fullPath = fullPath;
                    tvi.luaType = v.Type;
                    treeViewItems[hash] = tvi;
                    parent.AddChild(tvi);
                }

                if (v.IsTable)
                {
                    tvi.icon = EditorGUIUtility.FindTexture("Folder Icon");
                    EnumerateDirectories(tvi, v, depth - 1);
                }

            }

            parent.children?.Sort();
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
