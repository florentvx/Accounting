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
            ICategory icat = _ad.GetFirstCategory();
            _view.SetUpTree(_ad.Map);
            _view.ShowElement(new NodeAddress(NodeType.Category, icat.CategoryName));
        }

        internal async void TreeView_AfterLabelEdit(NodeLabelEditEventArgs e)
        {
            string before = e.Node.Text;
            string after = e.Label;
            if (after != null && after != "")
            {
                await Task.Run(() =>
                {
                    NodeAddress na = (NodeAddress)e.Node.Tag;
                    _ad.ChangeName(before, after, na);
                    na.ChangeAddress(after);
                    _view.ChangeActive(na);
                    _view.SetUpTree(_ad.Map);
                });
            }
            else
            {
                e.CancelEdit = true;
            }

        }

        internal void TreeView_NodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
                _view.TreeView_NodeMouseRightClick(e);
            if (e.Button.Equals(MouseButtons.Left))
                _view.TreeView_NodeMouseLeftClick(e);
        }

        internal void ButtonTotal()
        {
            _view.ShowTotal();
        }
    }
}
