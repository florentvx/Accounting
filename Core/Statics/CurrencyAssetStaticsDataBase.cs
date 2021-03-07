using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;
using Newtonsoft.Json;

namespace Core.Statics
{
    [Serializable]
    public class CurrencyAssetStaticsDataBase : IEquatable<CurrencyAssetStaticsDataBase>, ISerializable
    {
        [JsonProperty]
        public List<CurrencyStatics> DataBase { get; set; }
        [JsonProperty]
        public List<AssetStatics> AssetDataBase { get; set; }
        [JsonProperty]
        public Currency RefCcy { get; set; }

        public IEnumerable<string> Ccies { get { return DataBase.Select(x => x.Name); } }
        public IEnumerable<string> Assets { get { return AssetDataBase.Select(x => x.Name); } }

        public CurrencyAssetStaticsDataBase() {
            DataBase = new List<CurrencyStatics> { };
            AssetDataBase = new List<AssetStatics> { };
        }

        public void Reset()
        {
            DataBase = new List<CurrencyStatics> { };
            AssetDataBase = new List<AssetStatics> { };
        }

        internal void AddRefCcy(string ccy, CurrencyStatics cs)
        {
            RefCcy = new Currency(ccy);
            AddCcy(ccy, cs);
        }

        #region Currency Management

        public bool ContainsCcy(string ccy)
        {
            return !(DataBase.Where(x => x.Name == ccy).Count() == 0);
        }
        public bool ContainsAsset(string asset)
        {
            return !(AssetDataBase.Where(x => x.Name == asset).Count() == 0);
        }

        public CurrencyStatics GetCcyStatics(Currency ccy)
        {
            return DataBase.Where(x => x.Name == ccy.CcyString).FirstOrDefault();
        }

        public bool AddCcy(string newCcy, CurrencyStatics cs)
        {
            if (ContainsCcy(newCcy) || ContainsAsset(newCcy))
                return false;
            else
            {
                DataBase.Add(cs);
                return true;
            }
        }

        public IEnumerable<string> GetAvailableCurrencies()
        {
            return DataBase.Select(x => x.Name);
        }

        public string CcyToString(Currency ccy, double value)
        {
            return GetCcyStatics(ccy).ValueToString(value);
        }

        #endregion

        #region AssetManagement

        public bool AddAsset(string newAsset, AssetStatics asSt)
        {
            if (ContainsCcy(newAsset) || ContainsAsset(newAsset))
                return false;
            else
            {
                AssetDataBase.Add(asSt);
                return true;
            }
        }

        public IEnumerable<string> GetAvailableAssets()
        {
            return AssetDataBase.Select(x => x.Name);
        }

        #endregion

        #region IEquatable

        public bool Equals(CurrencyAssetStaticsDataBase casDb)
        {
            if (casDb == null)
                return false;
            return casDb.RefCcy == RefCcy
                && Tools.CompareList<CurrencyStatics>(casDb.DataBase, DataBase)
                && Tools.CompareList<AssetStatics>(casDb.AssetDataBase, AssetDataBase);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as CurrencyAssetStaticsDataBase);
        }

        public override int GetHashCode()
        {
            return RefCcy.GetHashCode();
        }

        public static bool operator ==(CurrencyAssetStaticsDataBase casDb1, CurrencyAssetStaticsDataBase casDb2)
        {
            if (casDb1 is null)
            {
                if (casDb1 is null) { return true; }
                return false;
            }
            return casDb1.Equals(casDb2);
        }

        public static bool operator !=(CurrencyAssetStaticsDataBase casDb1, CurrencyAssetStaticsDataBase casDb2)
        {
            return !(casDb1 == casDb2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("DataBase", DataBase, typeof(Dictionary<string, CurrencyStatics>));
            info.AddValue("AssetDataBase", AssetDataBase, typeof(Dictionary<string, AssetStatics>));
            info.AddValue("RefCcy", RefCcy, typeof(Currency));
        }

        public CurrencyAssetStaticsDataBase(SerializationInfo info, StreamingContext context)
        {
            DataBase = (List<CurrencyStatics>)info.GetValue("DataBase", typeof(List<CurrencyStatics>));
            AssetDataBase = (List<AssetStatics>)info.GetValue("AssetDataBase", typeof(List<AssetStatics>));
            RefCcy = (Currency)info.GetValue("RefCcy", typeof(Currency));
        }

        #endregion
    }
}
