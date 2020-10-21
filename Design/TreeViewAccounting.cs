using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;
using Core.Interfaces;

namespace Design
{
    public static class TreeViewStatics
    {
        public static TreeNode GetNode(this TreeNodeCollection tnc, string name)
        {
            foreach (TreeNode item in tnc)
            {
                if (item.Text == name)
                    return item;
            }
            throw new Exception($"Node Not Found: [{name}]");
        }
    }

    public class TreeViewAccounting : TreeView
    {
        //TreeViewMapping Mapping;

        public void Reset()
        {
            Nodes.Clear();
        }

        // Nodes ref to a list not a dictionary
        // i can use that by creating a custom mapping
        // that way I can have a control on the representing
        public void ExpandNode(NodeAddress na)
        {
            if (na.NodeType != NodeType.Category)
            {
                TreeNode nodeC = Nodes.GetNode(na.Address[0]);
                nodeC.Expand();
                if (na.NodeType == NodeType.Account)
                {
                    TreeNode nodeI = nodeC.Nodes.GetNode(na.Address[1]);
                    nodeI.Expand();
                }
            }
        }

        public void SetUpTree(TreeViewMapping tvm)
        {
            Reset();
            //if (Mapping == null)
            //    Mapping = new TreeViewMapping(add);
            //else
            //    Mapping.Modify(add, na, isNodeAddition);

            foreach (TreeViewMappingElement itemC in tvm)
            {
                string path_C = itemC.Name;
                TreeNode treeNodeC = new TreeNode(itemC.Name) { Tag = new NodeAddress(NodeType.Category, path_C) };
                path_C += NodeAddress.Separator;
                foreach (var itemI in itemC)
                {
                    string path_I = path_C + itemI.Name;
                    TreeNode treeNodeI = new TreeNode(itemI.Name) { Tag = new NodeAddress(NodeType.Institution, path_I) };
                    path_I += NodeAddress.Separator;
                    foreach (var itemA in itemI)
                    {
                        string path_A = path_I + itemA.Name;
                        TreeNode treeNodeA = new TreeNode(itemA.Name) { Tag = new NodeAddress(NodeType.Account, path_A) };
                        treeNodeI.Nodes.Add(treeNodeA);
                    }
                    treeNodeC.Nodes.Add(treeNodeI);
                }
                Nodes.Add(treeNodeC);
            }
            //if (na != null)
            //    ExpandNode(na);
        }

        private void ContextMenu_Rename(object sender, EventArgs e)
        {
            var MI = (MenuItem)sender;
            TreeNodeMouseClickEventArgs Obj = (TreeNodeMouseClickEventArgs)MI.Tag;
            LabelEdit = true;
            Obj.Node.BeginEdit();
        }

        public event TreeNodeMouseClickEventHandler NodeAdded;

        public virtual void OnNodeAddition(TreeNodeMouseClickEventArgs e)
        {
            TreeNodeMouseClickEventHandler handler = NodeAdded;
            handler?.Invoke(this, e);
        }

        private void ContextMenu_AddItem(object sender, EventArgs e)
        {
            var MI = (MenuItem)sender;
            TreeNodeMouseClickEventArgs Obj = (TreeNodeMouseClickEventArgs)MI.Tag;
            LabelEdit = true;
            OnNodeAddition(Obj);
        }

        public void NodeMouseRightClick(TreeNodeMouseClickEventArgs e)
        {
            ContextMenu cm = new ContextMenu();
            MenuItem title = new MenuItem(e.Node.Text) { Enabled = false };
            cm.MenuItems.Add(title);
            MenuItem rename = new MenuItem("Rename", ContextMenu_Rename) { Tag = e };
            cm.MenuItems.Add(rename);
            MenuItem addItem = new MenuItem("Add Item", ContextMenu_AddItem) { Tag = e };
            cm.MenuItems.Add(addItem);
            cm.Show(this, new Point(e.X, e.Y), LeftRightAlignment.Right);
        }
    }
}
