using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        Graphics _Graphics;
        TreeNode DragRefNode = null;
        TreeNode LastDraggedOverNode = null;

        public TreeViewAccounting(): base()
        {
            _Graphics = this.CreateGraphics();
        }

        public void Reset()
        {
            Nodes.Clear();
        }

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
                    if (itemI.Expand)
                        treeNodeI.Expand();
                    treeNodeC.Nodes.Add(treeNodeI);
                }
                if (itemC.Expand)
                    treeNodeC.Expand();
                Nodes.Add(treeNodeC);
            }
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

        public event TreeNodeMouseClickEventHandler NodeDeleted;

        public virtual void OnNodeDeletion(TreeNodeMouseClickEventArgs e)
        {
            TreeNodeMouseClickEventHandler handler = NodeDeleted;
            handler?.Invoke(this, e);
        }

        private void ContextMenu_DeleteItem(object sender, EventArgs e)
        {
            var MI = (MenuItem)sender;
            TreeNodeMouseClickEventArgs Obj = (TreeNodeMouseClickEventArgs)MI.Tag;
            LabelEdit = true;
            OnNodeDeletion(Obj);
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
            MenuItem deleteItem = new MenuItem("Delete Item", ContextMenu_DeleteItem) { Tag = e };
            cm.MenuItems.Add(deleteItem);
            cm.Show(this, new Point(e.X, e.Y), LeftRightAlignment.Right);
        }

        internal void ResetGraphics()
        {
            DragRefNode = null;
            LastDraggedOverNode = null;
            _Graphics.Dispose();
        }

        internal void ShowLine(Point pt)
        {
            SelectedNode = GetNodeAt(pt);
            if (DragRefNode != null && SelectedNode != LastDraggedOverNode)
            {
                Invalidate();
                LastDraggedOverNode = SelectedNode;
            }
            Pen customPen = new Pen(Color.DimGray, 1) { DashStyle = DashStyle.Dash };
            _Graphics.DrawLine(customPen, new Point(0, SelectedNode.Bounds.Bottom),
                new Point(1000, SelectedNode.Bounds.Bottom));
            customPen.Dispose();
        }

        internal void DefineRef(DragEventArgs e)
        {
            Point pt = PointToClient(new Point(e.X, e.Y));
            DragRefNode = GetNodeAt(pt);
            _Graphics = this.CreateGraphics();
        }
    }
}
