using Core.Finance;
using Core.Statics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Design.DataGridViewDesign
{
    class DataGridViewStaticsCcy : DataGridView
    {
        CurrencyAssetStaticsDataBase _Memo;
        public List<CurrencyStatics> CcyList { get { return _Memo.DataBase; } }


        public void Update(CurrencyAssetStaticsDataBase CcyDB) 
        {
            Rows.Clear();
            ColumnCount = 5;
            Columns[0].HeaderText = "Name";
            Columns[1].HeaderText = "Symbol";
            Columns[2].HeaderText = "Decimal Number";
            Columns[3].HeaderText = "Thousand Marker";
            Columns[4].HeaderText = "Pricing CcyPair";
            Columns[0].Width = 50;
            for (int i = 0; i < ColumnCount - 1; i++)
            {
                Columns[1 + i].Width = 75;
            }
            AllowUserToAddRows = false;
            foreach (var item in CcyDB.DataBase)
            {
                CurrencyPair cp = item.PricingCcyPair;
                string cpStr = cp == null ? "Ref. Ccy" : cp.ToString();
                object[] values = { item.Name, item.Symbol, item.DecimalNumber, item.ThousandMark, cpStr };
                Rows.Add(values);
            }
            _Memo = CcyDB;
            Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
            ClearSelection();
        }

        public void OnMouseDown()
        {
            ClearSelection();
        }

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        int DragRefRow = -1;
        int LastDraggedOverRow = -1;

        internal int GetRowIndex(Point pt)
        {
            return HitTest(pt.X, pt.Y).RowIndex;
        }

        internal void ShowLine(int rowIndex, bool forceRecoloring = false)
        {
            Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
            if (rowIndex != -1)
            {
                if (rowIndex != LastDraggedOverRow || forceRecoloring)
                {
                    for (int itemp = 1; itemp < Rows.Count; itemp++)
                        Rows[itemp].DefaultCellStyle.BackColor = Color.White;
                    LastDraggedOverRow = DragRefRow;
                }
                DragRefRow = rowIndex;
                double alpha = 75.0;
                int redRow = DragRefRow;
                int greenRow = DragRefRow + 1;
                if (DragRefRow == rowIndexFromMouseDown)
                    redRow--;

                int rgbtemp = Convert.ToInt32(Math.Floor(255 * alpha / 100.0));
                if (redRow >= 0)
                    Rows[redRow].DefaultCellStyle.BackColor = Color.FromArgb(255, rgbtemp, rgbtemp);
                if (greenRow < Rows.Count)
                    Rows[greenRow].DefaultCellStyle.BackColor = Color.FromArgb(rgbtemp, 255, rgbtemp);
                Rows[rowIndexFromMouseDown].DefaultCellStyle.BackColor = Color.FromArgb(rgbtemp, rgbtemp, 255);
            }
        }

        public void CustomOnMouseDown(MouseEventArgs e)
        {
            rowIndexFromMouseDown = HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1 && rowIndexFromMouseDown != 0)
            {
                Size dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
                ShowLine(rowIndexFromMouseDown, true);
            }
            else
            {
                Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                dragBoxFromMouseDown = Rectangle.Empty;
                for (int itemp = 1; itemp < Rows.Count; itemp++)
                    Rows[itemp].DefaultCellStyle.BackColor = Color.White;
            }
            ClearSelection();
        }

        public void CustomOnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {                 
                    DragDropEffects dropEffect = DoDragDrop(
                    Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
            ClearSelection();
        }

        public void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        public void OnDragDop(object sender, DragEventArgs e)
        {
            Point clientPoint = PointToClient(new Point(e.X, e.Y));
            rowIndexOfItemUnderMouseToDrop =
                HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            if (e.Effect == DragDropEffects.Move)
            {
                DataGridViewRow rowToMove = e.Data.GetData(
                    typeof(DataGridViewRow)) as DataGridViewRow;
                if (rowIndexOfItemUnderMouseToDrop > -1)
                {
                    Rows.RemoveAt(rowIndexFromMouseDown);
                    if (rowIndexOfItemUnderMouseToDrop < rowIndexFromMouseDown)
                        rowIndexOfItemUnderMouseToDrop += 1;
                    Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);
                }
                List<CurrencyStatics> tempList = new List<CurrencyStatics> { };
                for (int i = 0; i < Rows.Count; i++)
                {
                    string ccyName = (string)Rows[i].Cells[0].Value;
                    CurrencyStatics a = _Memo.DataBase.Where(x => x.Name == ccyName).First();
                    CurrencyStatics ac = (CurrencyStatics)a.Clone();
                    tempList.Add(ac);
                }
                _Memo.DataBase = tempList;
            }
        }
    }
}
