﻿using Core;
using Core.Interfaces;
using Core.Finance;
using Core.Statics;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Design
{
    static class DataGridViewAccountingStatics
    {
        public static string[] ColumnNames = { "Account Name", "Currency", "Amount", "Converted Amount" };
        public const int Column_AccountName = 0;
        public const int Column_Currency = 1;
        public const int Column_Amount = 2;
        public const int Column_ConvertedAmount = 3;
        public const int ColumnNumber = 4;
    }

    public class DataGridViewAccounting: DataGridView
    {
        public IAccountingData TotalShowed;
        public IAccountingElement ElementShowed;
        private TreeViewMappingElement _Memory;
        private FXMarket FXMarketUsed;
        private AssetMarket AssetMarketUsed;
        private CurrencyAssetStaticsDataBase _CcyDB;
        public IEnumerable<string> Ccies;
        public IEnumerable<string> Assets;

        public IEnumerable<string> CciesAndAssets { get { return Ccies.Union(Assets); } }


        private void SetUpTable()
        {
            ColumnCount = DataGridViewAccountingStatics.ColumnNumber;
            for (int i = 0; i < ColumnCount; i++)
                Columns[i].Name = DataGridViewAccountingStatics.ColumnNames[i];
            AllowUserToAddRows = false;
            foreach (DataGridViewColumn column in Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public DataGridViewAccounting() : base()
        {
            SetUpTable();
        }

        internal void SetUpMarkets(CurrencyAssetStaticsDataBase ccyDB, FXMarket mkt, AssetMarket aMkt)
        {
            _CcyDB = ccyDB;
            FXMarketUsed = mkt;
            AssetMarketUsed = aMkt;
            Ccies = mkt.GetAvailableCurrencies();
            Assets = aMkt.GetAvailableAssets();
        }

        public string CcyToString(ICcyAsset ccy, double value)
        {
            if (ccy.IsCcy())
                return _CcyDB.CcyToString(ccy.Ccy, value);
            else
                return Convert.ToString(value);
        }

        #region ShowElement

        private void AddRow(IAccount item, bool isTotal = false)
        {
            DataGridViewRowAccounting dgvr = new DataGridViewRowAccounting(this, item, isTotal);
            Rows.Add(dgvr);
        }

        private void AddRow(IAccountingElement item, ICcyAsset convCcy, bool isTotal = false)
        {
            IAccount sum = item.GetTotalAccount(FXMarketUsed, AssetMarketUsed, convCcy, item.GetName());
            AddRow(sum, isTotal);
        }

        public void ShowElement(IAccountingElement iElmt, TreeViewMappingElement tvme = null)
        {
            if (tvme == null) { tvme = _Memory; }
            else
                _Memory = tvme;
            Rows.Clear();
            foreach (IAccountingElement item in iElmt.GetItemList(tvme))
                AddRow(item, iElmt.CcyRef);
            AddRow( iElmt.GetTotalAccount(FXMarketUsed, AssetMarketUsed, iElmt.CcyRef), 
                    isTotal: iElmt.GetNodeType() != NodeType.Account);
            ElementShowed = iElmt;
            TotalShowed = null;
            Rows[0].Cells[0].Selected = false;
        }

        #endregion

        #region ShowTotal

        private void AddRow(ICategory item, Currency ccy)
        {
            IAccount sum = item.TotalInstitution(FXMarketUsed, AssetMarketUsed, ccy, item.CategoryName);
            AddRow(sum, false);
        }

        internal void ShowTotal(IAccountingData iad)
        {
            Rows.Clear();
            foreach (ICategory icat in iad.Categories)
                AddRow(icat, iad.Ccy);
            AddRow(iad.Total(), isTotal: true);
            ElementShowed = null;
            TotalShowed = iad;
            Rows[0].Cells[0].Selected = false;
        }

        #endregion

        public void ShowActive()
        {
            if (ElementShowed != null)
                ShowElement(ElementShowed);
            else
                if (TotalShowed != null)
                    ShowTotal(TotalShowed);
        }

        private double ValueFromStringToDouble(object value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch (Exception)
            {
                string valueStr = "";
                foreach (var c in Convert.ToString(value))
                    if (Char.IsDigit(c) || c == '.')
                        valueStr += c;
                return Convert.ToDouble(valueStr);
            }
        }

        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            bool IsLastRow = e.RowIndex == Rows.Count - 1;
            if (ElementShowed != null)
            {
                switch (e.ColumnIndex)
                {
                    case DataGridViewAccountingStatics.Column_Amount:
                        if (!IsLastRow && ElementShowed.GetNodeType() == NodeType.Institution)
                        {
                            var valueAmount = Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                            ElementShowed.ModifyAmount( FXMarketUsed,
                                                        AssetMarketUsed,
                                                        Rows[e.RowIndex].Cells[0].Value.ToString(),
                                                        ValueFromStringToDouble(valueAmount));
                        }
                        break;

                    case DataGridViewAccountingStatics.Column_Currency:
                        var valueCcy = Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                        ICcyAsset ccyAsset = new Currency(valueCcy);
                        if (Assets.Contains(valueCcy))
                            ccyAsset = new Asset(valueCcy);
                        ElementShowed.ModifyCcy(    FXMarketUsed,
                                                    AssetMarketUsed,
                                                    Rows[e.RowIndex].Cells[0].Value.ToString(),
                                                    ccyAsset,
                                                    IsLastRow);
                        break;
                }
                ShowElement(ElementShowed);
            }
            else if (TotalShowed != null)
            {
                switch (e.ColumnIndex)
                {
                    case DataGridViewAccountingStatics.Column_Currency:
                        var valueCcy = Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                        TotalShowed.ModifyCcy(valueCcy);
                        break;
                }
                ShowTotal(TotalShowed);
            }
        }

        protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = Rows[e.RowIndex].Cells[e.ColumnIndex];

                bool IsLastRow = cell.RowIndex == Rows.Count - 1;
                switch (cell.ColumnIndex)
                {
                    case DataGridViewAccountingStatics.Column_Amount:
                        if (ElementShowed != null && ElementShowed.GetNodeType() == NodeType.Institution && !IsLastRow)
                        {
                            cell.Selected = true;
                            BeginEdit(true);
                        }
                        else
                        {
                            cell.Selected = false;
                        }
                        break;

                    case DataGridViewAccountingStatics.Column_Currency:
                        if (TotalShowed != null || ElementShowed.GetNodeType() == NodeType.Institution || IsLastRow)
                        {
                            cell.Selected = true;
                            BeginEdit(true);
                        }
                        else
                            cell.Selected = false;
                        break;

                    default:
                        cell.Selected = false;
                        break;
                }
            }
        }
    }
}
