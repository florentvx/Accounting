using Core.Interfaces;
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
        public IInstitution InstitutionShowed;
        public ICategory CategoryShowed;

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

        #region Show Institution

        private void AddRow(IAccount item, bool isTotal = false)
        {
            DataGridViewRowAccounting dgvr = new DataGridViewRowAccounting(this, item, isTotal);
            Rows.Add(dgvr);
        }

        public void ShowInstitution(IInstitution instit)
        {
            Rows.Clear();
            foreach (IAccount item in instit.Accounts)
                AddRow(item);
            AddRow(instit.TotalAccount(), isTotal: true);
            InstitutionShowed = instit;
            CategoryShowed = null;
            Rows[0].Cells[0].Selected = false;
        }

        public void ShowInstitution(IAccountingData iad, string catName, string instName)
        {
            IInstitution instit = iad.GetInstitution(catName, instName);
            ShowInstitution(instit);            
        }

        #endregion

        #region Show Category

        private void AddRow(IInstitution item)
        {
            IAccount sum = item.TotalAccount(item.InstitutionName);
            AddRow(sum, false);
        }

        public void ShowCategory(ICategory cat)
        {
            Rows.Clear();
            foreach (IInstitution item in cat.Institutions)
                AddRow(item);
            AddRow(cat.TotalInstitution(), isTotal: true);
            InstitutionShowed = null;
            CategoryShowed = cat;
            Rows[0].Cells[0].Selected = false;
        }

        public void ShowCategory(IAccountingData iad, string catName)
        {
            ShowCategory(iad.GetCategory(catName));
        }

        #endregion

        public void ShowActive()
        {
            if (InstitutionShowed != null)
                ShowInstitution(InstitutionShowed);
            if (CategoryShowed != null)
                ShowCategory(CategoryShowed);
            if (InstitutionShowed == null && CategoryShowed == null)
                throw new Exception("Nothing to be showed!");
        }

        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            bool IsLastRow = e.RowIndex == Rows.Count - 1;
            if (CategoryShowed == null)
            {
                switch (e.ColumnIndex)
                {
                    case DataGridViewAccountingStatics.Column_Amount:
                        if (!IsLastRow)
                        {
                            var valueAmount = Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                            InstitutionShowed.ModifyAmount(Rows[e.RowIndex].Cells[0].Value.ToString(),
                                                        valueAmount);
                        }
                        break;

                    case DataGridViewAccountingStatics.Column_Currency:
                        var valueCcy = Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                        InstitutionShowed.ModifyCcy(Rows[e.RowIndex].Cells[0].Value.ToString(),
                                                    valueCcy,
                                                    IsLastRow);
                        break;
                }
                ShowInstitution(InstitutionShowed);
            }
            else if (CategoryShowed != null && IsLastRow)
            {
                switch (e.ColumnIndex)
                {
                    case DataGridViewAccountingStatics.Column_Currency:
                        var valueCcy = Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                        CategoryShowed.ModifyCcy(valueCcy);
                        break;
                }
                ShowCategory(CategoryShowed);
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
                        if (InstitutionShowed != null && !IsLastRow)
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
                        if (InstitutionShowed != null || IsLastRow)
                        {
                            cell.Selected = true;
                            BeginEdit(true);
                            if (cell.Value.ToString() != cell.EditedFormattedValue.ToString())
                                ShowActive();
                        }
                        break;

                    default:
                        cell.Selected = false;
                        break;
                }
            }
        }
    }
}
