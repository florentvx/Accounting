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
        void ShowCategory(ICategory cat);
        void ShowInstitution(IInstitution cat);
        void SetUpTree(Dictionary<string, Dictionary<string, List<string>>> sum);
        void TreeView_NodeMouseRightClick(TreeNodeMouseClickEventArgs e);
    }
}
