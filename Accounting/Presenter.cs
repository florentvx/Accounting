﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;
using Core.Finance;
using Core.Interfaces;
using Core.Statics;

namespace Accounting
{
    public class Presenter
    {
        private readonly IView _view;
        private IHistoricalAccountingData _had;

        internal void SetHistoricalData(IHistoricalAccountingData dataHistory)
        {
            _had = dataHistory;
            _had.PrepareForLoading();
            _view.UpdateDates();
        }

        private IAccountingData _ad { get { return _had.GetData(_view.CurrentDate); } }

        public Presenter(IView view, IHistoricalAccountingData had)
        {
            _view = view;
            _had = had;
        }

        public void LoadAccounts(bool showTotal = false, NodeAddress specifiedAddress = null)
        {
            _view.Reset();
            _view.SetUpAccountingData(_view.CcyDB, _ad);
            if (showTotal)
            {
                _view.ShowTotal();
            }
            else if(specifiedAddress != null)
            {
                _view.ShowElement(specifiedAddress);
            }
            else
            {
                ICategory icat = _ad.GetFirstCategory();
                _view.ShowElement(new NodeAddress(NodeType.Category, icat.CategoryName));
            }
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
            _had.AddNewCcy(ccyName, ccyStatics, ccyPair, ccyPairQuote);
            _view.SetUpMarkets(_view.CcyDB, _ad.FXMarket, _ad.AssetMarket, null);
        }

        internal void AddNewAsset(string assetName, AssetStatics assetStatics, double assetCcyPairQuote)
        {
            _had.AddNewAsset(assetName, assetStatics, assetCcyPairQuote);
            _view.SetUpMarkets(_view.CcyDB, _ad.FXMarket, _ad.AssetMarket, null);
        }

        internal void AddNewDate(long date_ticks)
        {
            DateTime new_date = new DateTime(date_ticks);
            _had.AddNewDate(date_ticks);
            _view.CurrentDate = new_date;
            _view.UpdateDates();
            _view.SetUpAccountingData(_view.CcyDB, _ad);
        }

        internal void ResetAndAddRefCcy(DateTime date, string ccyName, CurrencyStatics ccyStatics)
        {
            _had.Reset(date, ccyName, ccyStatics);
            AccountingData prev_ac = _had.GetPreviousData(date);
            _view.SetUpMarkets(_view.CcyDB, _ad.FXMarket, _ad.AssetMarket, prev_ac.FXMarket);
            _view.UpdateDates();
        }

        internal void UpdateChart()
        {
            _view.Chart_Update();
        }

        internal void UpdateStatics()
        {
            _view.Statics_Update();
        }

        internal void UpdateSummary()
        {
            _view.Summary_Update();
        }
    }
}
