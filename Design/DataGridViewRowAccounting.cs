﻿using System.Drawing;
using System.Windows.Forms;
using Core.Interfaces;
using Core.Finance;
using System;

namespace Design
{
    class DataGridViewRowAccounting : DataGridViewRow
    {
        public DataGridViewRowAccounting(   DataGridViewAccounting table, 
                                            IAccount account, 
                                            bool isTotalRow = false,
                                            bool isTotalView = false)
        {
            CreateCells(table);
            string amount = table.CcyToString(account.Ccy, account.Amount);
            string convAmount = table.CcyToString(account.ConvertedCcy, account.ConvertedAmount);
            Price lastAmount;
            
            lastAmount = account.LastAmount;
                
            if (isTotalRow)
            {
                convAmount = amount;
                if (!(isTotalView && lastAmount.HasValue))
                    amount = "";
                else
                {
                    double ret = account.Amount / lastAmount.Value - 1;
                    amount = (ret > 0) ? "+" : "-";
                    amount += System.Math.Round(System.Math.Abs(ret) * 100, 1).ToString() + " %";
                }
            }

            double? changeAmount = null;
            string changeData = "";
            if (lastAmount.HasValue)
            {
                if (!account.ConvertedCcy.Equals(lastAmount.Ccy))
                    throw new Exception($"Account converted Ccy [{account.ConvertedCcy}] is different from lastAmount Ccy [{lastAmount.Ccy}]");
                changeAmount = account.ConvertedAmount - lastAmount.Value;
                changeData = table.CcyToString(lastAmount.Ccy, changeAmount.Value);
            }

            var titles = new object[] {
                account.AccountName, account.Ccy, amount, convAmount, changeData
            };

            SetValues(titles);
            if (!account.IsCalculatedAccount || isTotalRow)    
                Cells[DataGridViewAccountingStatics.Column_Currency] = 
                    new DataGridViewComboBoxCellAccounting( account.Ccy,
                                                            isTotalRow ? table.Ccies : table.CciesAndAssets);
            if (account.IsCalculatedAccount)
                Cells[DataGridViewAccountingStatics.Column_Amount].ReadOnly = true;
            Cells[DataGridViewAccountingStatics.Column_ConvertedAmount].ReadOnly = true;

            if (isTotalRow)
            {
                for (int i = 0; i < Cells.Count; i++)
                {
                    Cells[i].Style.BackColor = Color.LightGray;
                    if (i != DataGridViewAccountingStatics.Column_Currency)
                        Cells[i].ReadOnly = true;
                }
            }

            if (account.Amount < 0)
            {
                for (int i = 2; i < 4; i++)
                {
                    Cells[i].Style.ForeColor = Color.Red;
                }
            }

            if (lastAmount.HasValue)
            {
                if (isTotalRow)
                    Cells[DataGridViewAccountingStatics.Column_Amount].Style.ForeColor = (changeAmount.Value < 0) ? Color.Red : Color.Green;
                Cells[DataGridViewAccountingStatics.Column_Change].Style.ForeColor = (changeAmount.Value < 0) ? Color.Red : Color.Green;
            }

        }
    }
}
