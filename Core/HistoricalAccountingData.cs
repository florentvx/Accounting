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
using System.Windows.Forms;

namespace Core
{
    [Serializable]
    public class HistoricalAccountingData: IHistoricalAccountingData, IEquatable<HistoricalAccountingData>, ISerializable
    {
        [JsonProperty]
        List<KeyValuePair<DateTime, AccountingData>> _Data;

        [JsonProperty]
        CurrencyAssetStaticsDataBase _CcyDB;

        [JsonProperty]
        Currency _TotalCcy;

        public Currency TotalCcy { get { return _TotalCcy; } }

        public SortedDictionary<DateTime, AccountingData> Data
        {
            get { return new SortedDictionary<DateTime, AccountingData>(_Data.ToDictionary(x => x.Key, x=> x.Value)); }
        }

        public CurrencyAssetStaticsDataBase CcyDB { get { return _CcyDB; } set { _CcyDB = value; } }

        public IEnumerable<DateTime> Dates { get { return _Data.Select(x => x.Key); } }

        public HistoricalAccountingData()
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
            ad.ModifyCcyEventHandler += this.ModifyCcy;
            _Data.Insert(pos + 1, new KeyValuePair<DateTime, AccountingData>(date, ad));
        }

        public void AddNewCcy(string ccyName, CurrencyStatics ccyStatics, CurrencyPair ccyPair, double ccyPairQuote)
        {
            bool testAdd = _CcyDB.AddCcy(ccyName, ccyStatics);
            if (!testAdd)
                MessageBox.Show($"The new Currency [{ccyName}] does already exist.");
            else
            {
                foreach (var item in _Data)
                {
                    item.Value.SetCcyDB(_CcyDB);
                    item.Value.FXMarket.AddQuote(ccyPair, ccyPairQuote);
                    item.Value.AssetMarket.PopulateWithFXMarket(item.Value.FXMarket);
                }
            }
        }

        public void AddNewAsset(string assetName, AssetStatics aSt, double acpValue)
        {
            bool testAdd = _CcyDB.AddAsset(assetName, aSt);
            if (!testAdd)
                MessageBox.Show($"The new Asset [{assetName}] does already exist.");
            else
            {
                AssetCcyPair acp = new AssetCcyPair(new Asset(assetName), aSt.Ccy);
                foreach (var item in _Data)
                {
                    item.Value.SetCcyDB(_CcyDB);
                    item.Value.AssetMarket.AddQuote(acp, acpValue);
                    item.Value.AssetMarket.PopulateWithFXMarket(item.Value.FXMarket);
                }
            }
        }

        /// <summary>
        /// Historical freahly Loaded from .json needs to be properly set up
        /// 1. Populate Event Handlers For Modify Total Ccy (for Accounting Data to HAD)
        /// 2. Populate Event Handlers for Modify Amount (for Accounting Data level)
        /// 3. Calculate All Total Amounts (under Ccy = RefCcy)
        /// </summary>
        public void PrepareForLoading()
        {
            _TotalCcy = _CcyDB.RefCcy;
            foreach (var item in _Data)
            {
                AccountingData data = item.Value;
                data.PrepareForLoading(_TotalCcy);
                data.ModifyCcyEventHandler += this.ModifyCcy;
            }
        }

        #endregion

        #region IEquatable

        public bool Equals(HistoricalAccountingData had)
        {
            if (had == null)
                return false;
            if (_TotalCcy == had._TotalCcy
                && _CcyDB == had._CcyDB)
            {
                for (int i = 0; i < _Data.Count; i++)
                {
                    if (_Data[i].Key != had._Data[i].Key
                        && _Data[i].Value != had._Data[i].Value)
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as HistoricalAccountingData);
        }

        public override int GetHashCode()
        {
            int res = _TotalCcy.GetHashCode() + _CcyDB.GetHashCode();
            //res += _FXMarket.GetHashCode() + _AssetMarket.GetHashCode();
            //res += _Map.GetHashCode();
            //foreach (Category item in _Data)
            //    res += item.GetHashCode();
            return res;
        }

        public static bool operator ==(HistoricalAccountingData had1, HistoricalAccountingData had2)
        {
            if (had1 is null)
            {
                if (had2 is null) { return true; }
                return false;
            }
            return had1.Equals(had2);
        }

        public static bool operator !=(HistoricalAccountingData had1, HistoricalAccountingData had2)
        {
            return !(had1 == had2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TotalCcy", _TotalCcy, typeof(Currency));
            info.AddValue("CcyDataBase", _CcyDB, typeof(CurrencyAssetStaticsDataBase));
            info.AddValue("AccountingDataCollection", _Data, typeof(List<KeyValuePair<DateTime, AccountingData>>));
        }

        public HistoricalAccountingData(SerializationInfo info, StreamingContext context)
        {
            _TotalCcy = (Currency)info.GetValue("TotalCcy", typeof(Currency));
            _CcyDB = (CurrencyAssetStaticsDataBase)info.GetValue("CcyDataBase", typeof(CurrencyAssetStaticsDataBase));
            _Data = (List<KeyValuePair<DateTime, AccountingData>>)info.GetValue("AccountingDataCollection", typeof(List<KeyValuePair<DateTime, AccountingData>>));
            foreach (var item in _Data)
            {
                item.Value.SetCcyDB(_CcyDB);
            }
        }

        #endregion

        private void ModifyCcy(object sender, ModifyCcyEventArgs e)
        {
            foreach (var item in _Data)
            {
                item.Value.ModifyTotalCcy(e.Ccy);
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
