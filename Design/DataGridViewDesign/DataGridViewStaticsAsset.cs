using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Statics;

namespace Design.DataGridViewDesign
{
    class DataGridViewStaticsAsset : DataGridView
    {
        CurrencyAssetStaticsDataBase _Memo;
        public List<AssetStatics> AssetList { get { return _Memo.AssetDataBase; } }

        public DataGridViewStaticsAsset() : base()
        {
        }

        public void Update(CurrencyAssetStaticsDataBase ccyDb)
        {
            if (ccyDb != null)
            {
                if (ccyDb.AssetDataBase.Count > 0)
                {
                    Rows.Clear();
                    ColumnCount = 2;
                    Columns[0].HeaderText = "Name";
                    Columns[1].HeaderText = "Ccy";
                    Columns[0].Width = 50;
                    Columns[1].Width = 50;
                    AllowUserToAddRows = false;
                    foreach (var item in ccyDb.AssetDataBase)
                    {
                        object[] values = { item.Name, item.Ccy.CcyString };
                        Rows.Add(values);
                    }
                    _Memo = ccyDb;
                }
            }
            ClearSelection();
        }

        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            Update(_Memo);
        }

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        //Graphics _Graphics;
        int DragRefRow = -1;
        int LastDraggedOverRow = -1;


        public void CustomOnMouseDown(MouseEventArgs e)
        {
            //Get the index of the item the mouse is below.
            rowIndexFromMouseDown = HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
                ShowLine(rowIndexFromMouseDown, true);
            }
            else
            {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
                for (int itemp = 0; itemp < Rows.Count; itemp++)
                    Rows[itemp].DefaultCellStyle.BackColor = Color.White;
            }
            ClearSelection();
        }

        public void CustomOnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Proceed with the drag and drop, passing in the list item.                    
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
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop =
                HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
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

                //Rows[rowIndexOfItemUnderMouseToDrop].DefaultCellStyle.BackColor = Color.Aqua;

                List<AssetStatics> tempList = new List<AssetStatics> { };
                for (int i = 0; i < Rows.Count; i++)
                {
                    string assetName = (string)Rows[i].Cells[0].Value;
                    AssetStatics a = _Memo.AssetDataBase.Where(x => x.Name == assetName).First();
                    AssetStatics ac = (AssetStatics)a.Clone();
                    tempList.Add(ac);
                }
                _Memo.AssetDataBase = tempList;

            }

        }


        internal int GetRowIndex(Point pt)
        {
            return HitTest(pt.X, pt.Y).RowIndex;
        }

        internal void ShowLine(int rowIndex, bool forceRecoloring = false)
        {
            if (rowIndex != -1)
            {
                if (rowIndex != LastDraggedOverRow || forceRecoloring)
                {
                    for (int itemp = 0; itemp < Rows.Count; itemp++)
                        Rows[itemp].DefaultCellStyle.BackColor = Color.White;
                    LastDraggedOverRow = DragRefRow;
                }
                DragRefRow = rowIndex;
                double alpha = 75.0;
                int redRow = DragRefRow;
                int greenRow = DragRefRow + 1;
                if (DragRefRow == rowIndexFromMouseDown)
                    redRow--;
                
                //while(alpha < 99)
                //{
                int rgbtemp = Convert.ToInt32(Math.Floor(255 * alpha / 100.0));
                if (redRow >= 0)
                    Rows[redRow].DefaultCellStyle.BackColor = Color.FromArgb(255, rgbtemp, rgbtemp);
                if (greenRow < Rows.Count)
                    Rows[greenRow].DefaultCellStyle.BackColor = Color.FromArgb(rgbtemp, 255, rgbtemp);
                Rows[rowIndexFromMouseDown].DefaultCellStyle.BackColor = Color.FromArgb(rgbtemp, rgbtemp, 255);
                //alpha+=0.000001;
                //}   
            }
        }
    }
}
