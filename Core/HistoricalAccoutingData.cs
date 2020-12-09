using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Statics;
using Core.Finance;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Core
{
    public class HistoricalAccoutingData: IHistoricalAccountingData, ISerializable
    {
        SortedDictionary<DateTime, AccountingData> _Data;
        CurrencyAssetStaticsDataBase _CcyDB;

        Currency _TotalCcy;

        public SortedDictionary<DateTime, AccountingData> Data { get { return _Data; } set { _Data = value; } }
        public CurrencyAssetStaticsDataBase CcyDB { get { return _CcyDB; } set { _CcyDB = value; } }

        public IEnumerable<DateTime> Dates { get { return _Data.Keys; } }

        public HistoricalAccoutingData()
        {
            _Data = new SortedDictionary<DateTime, AccountingData> { };
            _CcyDB = new CurrencyAssetStaticsDataBase();
        }

        #region IHistoricalAccountingData

        public AccountingData GetData(DateTime date) { return _Data[date]; }

        public void Reset(DateTime date, string ccy, CurrencyStatics cs)
        {
            _Data.Clear();
            _CcyDB.Reset();
            _CcyDB.AddRefCcy(ccy, cs);
            AccountingData ad = new AccountingData(_CcyDB);
            _Data[date] = ad;
        }

        public void AddNewDate(DateTime date)
        {
            if (_Data.Count() == 0)
                return;
            DateTime t = _Data  .Where(x => x.Key <= date)
                                .Select(x=>x.Key)
                                .Last();
            AccountingData ad = _Data[t].Copy();
            _Data[date] = ad;
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TotalCcy", _TotalCcy, typeof(Currency));
        }

        #endregion

        private void ModifyCcy(object sender, ModifyCcyEventArgs e)
        {
            foreach (var date in _Data.Keys)
            {
                AccountingData ad = _Data[date];
                ad.RecalculateTotal(e.Ccy);
            }
        }

        public void AddData(DateTime date, AccountingData ad)
        {
            if (!_Data.ContainsKey(date))
            {
                ad.Total(_TotalCcy);
                _Data.Add(date, ad);
                ad.ModifyCcyEventHandler += this.ModifyCcy;
            }
            else
                throw new Exception($"the Following date already exists {date}");
        }


        public void SetCcyDB(CurrencyAssetStaticsDataBase ccyDB)
        {
            _CcyDB = ccyDB;
            _TotalCcy = ccyDB.RefCcy;
        }
    }
}
