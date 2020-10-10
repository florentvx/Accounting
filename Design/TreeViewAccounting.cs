using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Interfaces;

namespace Design
{
    
    public static class TreeViewAccountingStatics
    {

    }

    public class TreeViewAccounting : TreeView
    {
        public void Reset()
        {
            Nodes.Clear();
        }

        public void SetUpTree(Dictionary<string, Dictionary<string, List<string>>> add)
        {
            foreach (var itemC in add)
            {
                TreeNode treeNodeC = new TreeNode(itemC.Key) { Tag = NodeType.Category };
                foreach (var itemI in itemC.Value)
                {
                    TreeNode treeNodeI = new TreeNode(itemI.Key) { Tag = NodeType.Institution };
                    foreach (string itemA in itemI.Value)
                    {
                        TreeNode treeNodeA = new TreeNode(itemA) { Tag = NodeType.Account };
                        treeNodeI.Nodes.Add(treeNodeA);
                    }
                    treeNodeC.Nodes.Add(treeNodeI);
                }
                Nodes.Add(treeNodeC);
            }
        }

        private void ContextMenu_Rename(object sender, System.EventArgs e)
        {
            var MI = (MenuItem)sender;
            TreeNodeMouseClickEventArgs Obj = (TreeNodeMouseClickEventArgs)MI.Tag;
            LabelEdit = true;
            Obj.Node.BeginEdit();
        }

        public void NodeMouseRightClick(TreeNodeMouseClickEventArgs e)
        {
            ContextMenu cm = new ContextMenu();
            MenuItem title = new MenuItem(e.Node.Text) { Enabled = false };
            MenuItem rename = new MenuItem("Rename", ContextMenu_Rename) { Tag = e };
            cm.MenuItems.Add(title);
            cm.MenuItems.Add(rename);
            cm.Show(this, new Point(e.X, e.Y), LeftRightAlignment.Right);
        }
    }
}
