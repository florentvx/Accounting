using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Finance;
using Core.Statics;
using Core.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Core
{

    public class ModifyCcyEventArgs : EventArgs
    {
        public Currency Ccy;

        public ModifyCcyEventArgs(object valueCcy)
        {
            Ccy = new Currency(valueCcy);
        }
    }

    [Serializable]
    public class AccountingData : IAccountingData, IEquatable<AccountingData>, ISerializable
    {
        [JsonProperty]
        Currency _Ccy; // Ccy Total

        private double _TotalValue; // Total value in Ccy Total
        public double TotalValue { get { return _TotalValue; } } 

        CurrencyAssetStaticsDataBase _CcyDB; //Copy By Ref of CcyAssetDataBase

        [JsonProperty]
        List<Category> _Data = new List<Category> { };

        [JsonProperty]
        FXMarket _FXMarket;

        [JsonProperty]
        AssetMarket _AssetMarket;

        [JsonProperty]
        TreeViewMapping _Map;

        public Currency Ccy
        {
            get { return _Ccy; }
            ///set { _Ccy = value; }
        }

        public Dictionary<string, Category> GetData()
        {
            return _Data.ToDictionary(x => x.CategoryName, x => x);
        }

        public void SetCcyDB(CurrencyAssetStaticsDataBase ccyDB)
        {
            _CcyDB = ccyDB;
        }

        private Category GetCategory(string catName)
        {
            var data = GetData();
            return data[catName];
        }

        private Institution GetInstitution(string catName, string institName)
        {
            return GetCategory(catName).GetInstitution(institName);
        }

        public IEnumerable<string> GetAvailableCurrencies()
        {
            return FXMarket.GetAvailableCurrencies();
        }

        #region IAccountingData

        public TreeViewMapping Map { get { return _Map; } }

        public FXMarket FXMarket { get { return _FXMarket; } }

        public AssetMarket AssetMarket { get { return _AssetMarket; } }

        public event EventHandler<ModifyCcyEventArgs> ModifyCcyEventHandler;

        public void ModifyCcy(object valueCcy)
        {
            _Ccy = new Currency(valueCcy);
            ModifyCcyEventArgs e = new ModifyCcyEventArgs(valueCcy);
            ModifyCcyEventHandler?.Invoke(this, e);
        }

        public void ModifyTotalCcy(Currency ccy)
        {
            _Ccy = ccy;
            _TotalValue = 0;
            foreach (Category item in Categories)
            {
                item.ModifyTotalCcy(_FXMarket, _AssetMarket, ccy);
                _TotalValue += item.TotalAmount;
            }
        }

        public ICategory GetFirstCategory()
        {
            return _Data.First();
        }

        public IEnumerable<ICategory> Categories
        {
            get
            {
                var data = GetData();
                IEnumerable<string> nameList = _Map.GetList(new NodeAddress(NodeType.All, ""));
                return nameList.Select(x => data[x]);
            }
        }

        public IAccount Total()
        {
            double total = 0;
            foreach (var item in _Data)
                total += item.TotalInstitution(_FXMarket, _AssetMarket, Ccy).ConvertedAmount;
            _TotalValue = total;
            return new Account("Total", Ccy, total);
        }

        public IAccount Total(Currency TotalCcy)
        {
            ModifyCcy(TotalCcy);
            return Total();
        }

        public bool ChangeName(string before, string after, NodeAddress nodeTag)
        {
            bool test = false;
            var data = GetData();
            if (nodeTag.NodeType == NodeType.Category)
            {
                if (!data.ContainsKey(after))
                {
                    Category cat = GetCategory(before);
                    cat.CategoryName = after;
                    //_Data[after] = _Data[before];
                    //_Data.Remove(before);
                    //_Data[after].CategoryName = after;
                    test = true;
                }
            }
            else
            {
                test = GetCategory(nodeTag.Address[0]).ChangeName(before, after, nodeTag);
            }
            if (test)
                _Map.ChangeName(nodeTag, after);
            return test;
        }

        //public void AddNewCcy(string ccyName, CurrencyStatics ccyStatics, CurrencyPair cp, double cpValue)
        //{
        //    bool testAdd = _CcyDB.AddCcy(ccyName, ccyStatics);
        //    if (!testAdd)
        //        MessageBox.Show($"The new Currency [{ccyName}] does already exist.");
        //    else
        //    {
        //        _FXMarket.AddQuote(cp, cpValue);
        //    }
        //}

        //public void AddRefCcy(string ccyName, CurrencyStatics ccyStatics)
        //{
        //    bool testAdd = _CcyDB.AddCcy(ccyName, ccyStatics);
        //    if (!testAdd)
        //        throw new Exception($"Add Ref Ccy Error {ccyName}");
        //    _Ccy = new Currency(ccyName);
        //    _FXMarket.SetCcyRef(_Ccy);
        //}

        public void AddNewAsset(string assetName, AssetStatics aSt, double acpValue)
        {
            bool testAdd = _CcyDB.AddAsset(assetName, aSt);
            if (!testAdd)
                MessageBox.Show($"The new Asset [{assetName}] does already exist.");
            else
            {
                AssetCcyPair acp = new AssetCcyPair(new Asset(assetName), aSt.Ccy);
                _AssetMarket.AddQuote(acp, acpValue);
            }
        }

        public void Reset(string ccy, CurrencyStatics cs)
        {
            _Data = new List<Category> { };
            Map.Reset();
            _FXMarket.Reset();
            _CcyDB.AddRefCcy(ccy, cs);
            _AssetMarket.Reset();
            _Ccy = new Currency(ccy);
            AddItem(new NodeAddress(NodeType.Category, "TEMP"));
        }

        #endregion

        #region IEquatable

        public bool Equals(AccountingData ad)
        {
            if (ad == null)
                return false;
            if (_Ccy == ad._Ccy
                //&& _CcyDB == ad._CcyDB
                && _FXMarket == ad._FXMarket
                && _AssetMarket == ad._AssetMarket
                && _Map == ad._Map
                && _Data.Count == ad._Data.Count)
            {
                for (int i = 0; i < _Data.Count; i++)
                {
                    if (_Data[i] != ad._Data[i])
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as AccountingData);
        }

        public override int GetHashCode()
        {
            int res = _Ccy.GetHashCode() + _CcyDB.GetHashCode();
            res += _FXMarket.GetHashCode() + _AssetMarket.GetHashCode();
            res += _Map.GetHashCode();
            foreach (Category item in _Data)
                res += item.GetHashCode();
            return res;
        }

        public static bool operator ==(AccountingData ad1, AccountingData ad2)
        {
            if (ad1 is null)
            {
                if (ad2 is null) { return true; }
                return false;
            }
            return ad1.Equals(ad2);
        }

        public static bool operator !=(AccountingData ad1, AccountingData ad2)
        {
            return !(ad1 == ad2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Currency", _Ccy, typeof(Currency));
            //info.AddValue("CcyDB", _CcyDB, typeof(CurrencyAssetStaticsDataBase));
            info.AddValue("Categories", _Data, typeof(List<Category>));
            info.AddValue("FXMarket", _FXMarket, typeof(FXMarket));
            info.AddValue("AssetMarket", _AssetMarket, typeof(AssetMarket));
            info.AddValue("Map", _Map, typeof(TreeViewMapping));
        }

        public AccountingData(SerializationInfo info, StreamingContext context)
        {
            _Ccy = (Currency)info.GetValue("Currency", typeof(Currency));
            //_CcyDB = (CurrencyAssetStaticsDataBase)info.GetValue("CcyDB", typeof(CurrencyAssetStaticsDataBase));
            _Data = (List<Category>)info.GetValue("Categories", typeof(List<Category>));
            _FXMarket = (FXMarket)info.GetValue("FXMarket", typeof(FXMarket));
            _AssetMarket = (AssetMarket)info.GetValue("AssetMarket", typeof(AssetMarket));
            _Map = (TreeViewMapping)info.GetValue("Map", typeof(TreeViewMapping));
        }

        #endregion

        public AccountingData(CurrencyAssetStaticsDataBase ccyDB)
        {
            _CcyDB = ccyDB;
            _Ccy = ccyDB.RefCcy;
            _Data = new List<Category> { };
            _FXMarket = new FXMarket(Ccy);
            _AssetMarket = new AssetMarket();
            AddNewCategory();
            _Map = new TreeViewMapping(_Data);
        }

        public AccountingData(List<Category> input, FXMarket mkt, AssetMarket aMkt)
        {
            _Ccy = new Currency("USD");
            _Data = input;
            _FXMarket = mkt;
            _AssetMarket = aMkt;
            _Map = new TreeViewMapping(_Data);
        }

        public IAccountingElement GetInstitution(NodeAddress na)
        {
            return GetInstitution(na.Address[0], na.Address[1]);
        }

        public Category GetCategory(NodeAddress na)
        {
            return GetCategory(na.Address[0]);
        }

        public IAccountingElement GetElement(NodeAddress na)
        {
            switch (na.NodeType)
            {
                case NodeType.All:
                    break;
                case NodeType.Category:
                    return GetCategory(na);
                case NodeType.Institution:
                    return GetInstitution(na);
                case NodeType.Account:
                    break;
                default:
                    break;
            }
            throw new Exception("ERROR");
        }

        public Category AddNewCategory()
        {
            int i = 0;
            var data = GetData();
            string newNameRef = "New Category";
            string newName = newNameRef;
            while (data.ContainsKey(newName))
            {
                i++;
                newName = $"{newNameRef} - {i}";
            }
            Category cat = new Category(newName, _CcyDB.RefCcy);
            cat.AddInstitution("New Institution");
            cat.AddAccount("New Account", "New Institution");
            _Data.Add(cat);
            cat.ModifyAmountEventHandler += this.UpdateTotalAmount;
            return cat;
        }

        public NodeAddress AddItem(NodeAddress nodeAddress)
        {
            switch (nodeAddress.NodeType)
            {
                case NodeType.All:
                    break;
                case NodeType.Category:
                    Category newCat = AddNewCategory();
                    Map.AddItem(nodeAddress, newCat);
                    nodeAddress.ChangeAddress(newCat.CategoryName);
                    return nodeAddress;
                case NodeType.Institution:
                    Institution newInstit = GetCategory(nodeAddress.Address[0]).AddNewInstitution();
                    Map.AddItem(nodeAddress, newInstit);
                    nodeAddress.ChangeAddress(newInstit.InstitutionName);
                    return nodeAddress;
                case NodeType.Account:
                    Account newAccount = GetCategory(nodeAddress.Address[0]).GetInstitution(nodeAddress.Address[1]).AddNewAccount();
                    Map.AddItem(nodeAddress, newAccount);
                    nodeAddress.ChangeAddress(newAccount.AccountName);
                    return nodeAddress;
                default:
                    break;
            }
            throw new Exception("Issue");
        }

        public NodeAddress DeleteItem(NodeAddress na)
        {
            NodeAddress res;
            if (na.NodeType == NodeType.Category)
            {
                if (_Data.Count > 1)
                {
                    _Data.Remove(GetCategory(na.Address[0]));
                    res = _Map.DeleteNode(na);
                }
                else
                    res = na;
            }
            else
            {
                GetElement(na.GetParent()).Delete(na.GetLast());
                res= _Map.DeleteNode(na);
            }
            if (res.IsEqual(na))
                MessageBox.Show($"You cannot delete {na.GetLast()} as it is the last element of [{na.GetParent().GetLabelText()}]!");
            return res;
        }

        public void UpdateAssetMarket()
        {
            _AssetMarket.PopulateWithFXMarket(_FXMarket);
        }

        public void UpdateTotalAmount(object sender, ModifyAmountEventArgs e)
        {
            _TotalValue = 0;
            foreach (Category item in Categories)
            {
                _TotalValue += item.TotalAmount;
            }
        }

        public void RefreshTotalAmount(FXMarket fXMarket, AssetMarket assetMarket)
        {
            _TotalValue = 0;
            foreach (Category item in Categories)
            {
                item.RefreshTotalAmount(fXMarket, assetMarket);
                _TotalValue += item.TotalAmount;
            }
        }

        public AccountingData Copy()
        {
            List<Category> newData = new List<Category> { };
            foreach (var cat in _Data)
            {
                Category copyCat = cat.Copy();
                newData.Add(copyCat);
            }
                

            FXMarket fxmkt = new FXMarket(_CcyDB.RefCcy);
            fxmkt.Copy(_FXMarket);

            AssetMarket aMkt = new AssetMarket();
            aMkt.Copy(_AssetMarket);
            aMkt.PopulateWithFXMarket(fxmkt);

            AccountingData res = new AccountingData(_CcyDB)
            {
                _Ccy = (Currency)_Ccy.Clone(),
                _TotalValue = _TotalValue,
                _Data = newData,
                _FXMarket = fxmkt,
                _AssetMarket = aMkt,
                _Map = new TreeViewMapping(newData)
            };

            foreach (Category cat in newData)
                cat.ModifyAmountEventHandler += res.UpdateTotalAmount;
            
            return res;
        }
    }
}
