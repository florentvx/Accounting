using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Statics
{
    public class CurrencyAssetStaticsDataBase: IEquatable<CurrencyAssetStaticsDataBase>, ISerializable
    {
        public Dictionary<string, CurrencyStatics> DataBase { get; set; }
        public Dictionary<string, AssetStatics> AssetDataBase { get; set; }
        public Currency RefCcy { get; set; }

        public CurrencyAssetStaticsDataBase() {
            DataBase = new Dictionary<string, CurrencyStatics> { };
            AssetDataBase = new Dictionary<string, AssetStatics> { };
        }

        public void Reset()
        {
            DataBase = new Dictionary<string, CurrencyStatics> { };
            AssetDataBase = new Dictionary<string, AssetStatics> { };
        }

        internal void AddRefCcy(string ccy, CurrencyStatics cs)
        {
            RefCcy = new Currency(ccy);
            AddCcy(ccy, cs);
        }

        #region Currency Management

        public bool AddCcy(string newCcy, CurrencyStatics cs)
        {
            if (DataBase.ContainsKey(newCcy) || AssetDataBase.ContainsKey(newCcy))
                return false;
            else
            {
                DataBase.Add(newCcy, cs);
                return true;
            }
        }

        public IEnumerable<string> GetAvailableCurrencies()
        {
            return DataBase.Keys;
        }

        public string CcyToString(Currency ccy, double value)
        {
            CurrencyStatics cs = DataBase[ccy.ToString()];
            return cs.ValueToString(value);
        }

        #endregion

        #region AssetManagement

        public bool AddAsset(string newAsset, AssetStatics asSt)
        {
            if (DataBase.ContainsKey(newAsset) || AssetDataBase.ContainsKey(newAsset))
                return false;
            else
            {
                AssetDataBase.Add(newAsset, asSt);
                return true;
            }
        }

        public IEnumerable<string> GetAvailableAssets()
        {
            return AssetDataBase.Keys;
        }

        #endregion

        #region IEquatable

        public bool Equals(CurrencyAssetStaticsDataBase casDb)
        {
            if (casDb == null)
                return false;
            return casDb.RefCcy == RefCcy 
                && Tools.CompareDictionary<string, CurrencyStatics>(casDb.DataBase, DataBase)
                && Tools.CompareDictionary<string, AssetStatics>(casDb.AssetDataBase, AssetDataBase);
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
            info.AddValue("CcyDB", DataBase, typeof(Dictionary<string, CurrencyStatics>));
            info.AddValue("AssetDB", AssetDataBase, typeof(Dictionary<string, AssetStatics>));
        }

        public CurrencyAssetStaticsDataBase(SerializationInfo info, StreamingContext context)
        {
            DataBase = (Dictionary<string, CurrencyStatics>)info.GetValue("CcyDB", typeof(Dictionary<string, CurrencyStatics>));
            AssetDataBase = (Dictionary<string, AssetStatics>)info.GetValue("AssetDB", typeof(Dictionary<string, AssetStatics>));
        }

        #endregion
    }
}
