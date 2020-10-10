using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Interfaces;

namespace Accounting
{
    public class Presenter
    {
        private readonly IView _view;
        private IAccountingData _ad;

        public Presenter(IView view, IAccountingData ad)
        {
            _view = view;
            _ad = ad;
        }

        public void LoadAccounts()
        {
            _view.Reset();
            Dictionary<string, Dictionary<string, List<string>>> add = _ad.GetSummary();
            _view.SetUpTree(add);
            _view.ShowCategory(_ad.GetFirstCategory());
        }

        internal void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string fullPath = e.Node.FullPath;
            string[] split = fullPath.Split('\\');
            if (split.Count() == 1)
            {
                _view.ShowCategory(_ad.GetCategory(split[0]));
            }
            if (split.Count() == 2)
            {
                _view.ShowInstitution(_ad.GetInstitution(split[0], split[1]));
            }
            e.Node.Expand();
        }

        internal void TreeView_AfterLabelEdit(NodeLabelEditEventArgs e)
        {
            _ad.ChangeName(e.Node.Text, e.Label, (NodeType)e.Node.Tag);
            _view.ShowActive();
        }

        internal void TreeView_NodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
                _view.TreeView_NodeMouseRightClick(e);
        }

    }
}
