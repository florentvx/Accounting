using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Interfaces
{
    public interface IView
    {
        void Reset();
        //void ShowActive();
        void ChangeActive(NodeAddress nd);
        void ShowTotal();
        void ShowCategory(NodeAddress na);
        void ShowInstitution(NodeAddress na);
        void SetUpTree(TreeViewMapping na);
        void TreeView_NodeMouseRightClick(TreeNodeMouseClickEventArgs e);
        void TreeView_NodeMouseLeftClick(TreeNodeMouseClickEventArgs e);
    }
}
