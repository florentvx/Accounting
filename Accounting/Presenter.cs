using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Finance;
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
            ICategory icat = _ad.GetFirstCategory();
            _view.Reset();
            _view.SetUpMarket(_ad.Market);
            _view.SetUpTree(_ad.Map);
            _view.ShowElement(new NodeAddress(NodeType.Category, icat.CategoryName));
        }

        internal async void TreeView_AfterLabelEdit(NodeLabelEditEventArgs e)
        {
            string before = e.Node.Text;
            string after = e.Label;
            await Task.Run(() =>
            {
                bool test = false;
                if (after != null && after != "")
                {
                    NodeAddress na = (NodeAddress)e.Node.Tag;
                    if (_ad.ChangeName(before, after, na))
                    {
                        na.ChangeAddress(after);
                        _view.ChangeActive(na);
                        test = true;
                    }
                }
                else
                {
                    e.CancelEdit = true;
                }
                _view.SetUpTree(_ad.Map);
                if(!test)
                    MessageBox.Show($"Unable to give the element [{before}] the name [{after}]");
            });
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

        internal void AddNewCcy(string ccyName, CurrencyStatics ccyStatics, CurrencyPair ccyPair, double ccyPairQuote)
        {
            _ad.AddNewCcy(ccyName, ccyStatics, ccyPair, ccyPairQuote);
            _view.SetUpMarket(_ad.Market);
        }
    }
}
