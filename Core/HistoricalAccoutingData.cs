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
using Newtonsoft.Json;

namespace Core
{
    [Serializable]
    public class HistoricalAccoutingData: IHistoricalAccountingData, ISerializable
    {
        [JsonProperty]
        List<KeyValuePair<DateTime, AccountingData>> _Data;

        [JsonProperty]
        CurrencyAssetStaticsDataBase _CcyDB;

        [JsonProperty]
        Currency _TotalCcy;

        public SortedDictionary<DateTime, AccountingData> Data
        {
            get { return new SortedDictionary<DateTime, AccountingData>(_Data.ToDictionary(x => x.Key, x=> x.Value)); }
        }

        public CurrencyAssetStaticsDataBase CcyDB { get { return _CcyDB; } set { _CcyDB = value; } }

        public IEnumerable<DateTime> Dates { get { return _Data.Select(x => x.Key); } }

        public HistoricalAccoutingData()
        {
            _Data = new List<KeyValuePair<DateTime, AccountingData>> { };
            _CcyDB = new CurrencyAssetStaticsDataBase();
        }

        #region IHistoricalAccountingData

        public AccountingData GetData(DateTime date) { return Data[date]; }

        public void Reset(DateTime date, string ccy, CurrencyStatics cs)
        {
            _Data.Clear();
            _CcyDB.Reset();
            _CcyDB.AddRefCcy(ccy, cs);
            AccountingData ad = new AccountingData(_CcyDB);
            _Data.Add(new KeyValuePair<DateTime, AccountingData>(date, ad));
        }

        public void AddNewDate(DateTime date)
        {
            if (_Data.Count() == 0)
                return;
            int pos = _Data  .Select((item,index) => new
                                {
                                    Date = item.Key, 
                                    Position = index
                                })
                                .Where(x => x.Date <= date)
                                .Select(x=>x.Position)
                                .Last();
            AccountingData ad = _Data[pos].Value.Copy();
            _Data.Insert(pos + 1, new KeyValuePair<DateTime, AccountingData>(date, ad));
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
            foreach (var item in _Data)
            {
                item.Value.RecalculateTotal(e.Ccy);
            }
        }

        public void AddData(DateTime date, AccountingData ad)
        {
            if (_Data.Where(x => x.Key == date).Count() == 0)
            {
                ad.Total(_TotalCcy);
                _Data.Add(new KeyValuePair<DateTime, AccountingData>(date, ad));
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
