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
        void ShowActive();
        void ChangeActive(NodeAddress nd);
        void ShowTotal();
        void ShowCategory(ICategory cat);
        void ShowInstitution(IInstitution cat);
        void SetUpTree(NodeAddress na);
        void TreeView_NodeMouseRightClick(TreeNodeMouseClickEventArgs e);
        void TreeView_NodeMouseLeftClick(TreeNodeMouseClickEventArgs e);
    }
}
