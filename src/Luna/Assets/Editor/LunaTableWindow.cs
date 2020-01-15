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

        void OnEnable()
        {
            // Check whether there is already a serialized view state (state 
            // that survived assembly reloading)
            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState();

            m_TableTreeView = new LunaTableView(m_TreeViewState);
            m_TableTreeView.Reload();
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
            window.titleContent = new GUIContent("LunaGlobalTable");
            window.Show();
        }
    }

    public class LunaTableView : TreeView
    {
        public event Action<List<LuaRef>> selectionChanged;
        Dictionary<int, LuaRef> id2v = new Dictionary<int, LuaRef>();
        Dictionary<int, TreeViewItem> treeViewItems = new Dictionary<int, TreeViewItem>();
        public LunaTableView(TreeViewState treeViewState)
            : base(treeViewState)
        {
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "_G" };
           
            var luna = LunaClient.Luna;
            if (luna != null)
            {
                var item = new TreeViewItem { id = 0, depth = 1, displayName = "_G" };
                LuaRef luaRef = luna.Global();
                int hash = luaRef.GetHashCode();
                id2v[hash] = luaRef;
                treeViewItems[hash] = item;

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

            foreach (var t in parentLuaRef)
            {
                var k = t.Key<string>();
                if (k == "_G")
                {
                    continue;
                }

                var v = t.Value();
                int hash = v.GetHashCode();

                if (!treeViewItems.TryGetValue(hash, out var tvi))
                {
                    id2v[hash] = v;
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
