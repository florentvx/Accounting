using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Statics
{
    public class CurrencyAssetStaticsDataBase
    {
        Dictionary<string, CurrencyStatics> DataBase = new Dictionary<string, CurrencyStatics> { };
        Dictionary<string, AssetStatics> AssetDataBase = new Dictionary<string, AssetStatics> { };

        public Currency RefCcy { get; set; }

        public CurrencyAssetStaticsDataBase() { }

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
    }
}
