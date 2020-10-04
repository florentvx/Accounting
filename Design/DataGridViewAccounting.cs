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
        }

        public void ShowCategory(IAccountingData iad, string catName)
        {
            ShowCategory(iad.GetCategory(catName));
        }

        #endregion

        public void CellValueChanged_Event(int rowIndex, int columnIndex)
        {
            if (CategoryShowed == null)
            {
                switch (columnIndex)
                {
                    case DataGridViewAccountingStatics.Column_Amount:
                        var value = Rows[rowIndex].Cells[columnIndex].Value;
                        InstitutionShowed.ModifyAmount(Rows[rowIndex].Cells[0].Value.ToString(),
                                                    value);
                        break;
                }
                ShowInstitution(InstitutionShowed);
            }
            else
            {
                ShowCategory(CategoryShowed);
            }
        }
    }
}
