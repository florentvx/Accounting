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
    public class AccountingData : IAccountingData, IEquatable<AccountingData>, ISerializable, ICloneable
    {
        [JsonProperty]
        Currency _Ccy;

        [JsonProperty]
        List<Category> _Data = new List<Category> { };

        [JsonProperty]
        FXMarket _FXMarket;

        [JsonProperty]
        AssetMarket _AssetMarket;

        TreeViewMapping _Map;

        public Currency Ccy => _Ccy;

        //public List<ICcyAsset> CciesAndAssets
        //{
        //    get
        //    {
        //        List<ICcyAsset> res = _CcyDB.Ccies.Select(c => (ICcyAsset)new Currency(c)).ToList();
        //        List<ICcyAsset> assets = _CcyDB.Assets.Select(a => (ICcyAsset)new Asset(a)).ToList();
        //        foreach (var item in assets)
        //            res.Add(item);
        //        return res;
        //    }
        //}

        public Dictionary<string, Category> GetData()
        {
            return _Data.ToDictionary(x => x.CategoryName, x => x);
        }

        //public void SetCcyDB(CurrencyAssetStaticsDataBase ccyDB)
        //{
        //    _CcyDB = ccyDB;
        //}

        private Category GetCategory(string catName)
        {
            var data = GetData();
            return data[catName];
        }

        private Institution GetInstitution(string catName, string institName)
        {
            return GetCategory(catName).GetInstitution(institName);
        }

        private Account GetAccount(string catName, string institName, string accName)
        {
            return GetInstitution(catName, institName).GetAccount(accName);
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

        public void ModifyCcy(Currency ccy)
        {
            _Ccy = ccy;
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
                IEnumerable<string> nameList = _Map.GetList(new NodeAddress(""));
                return nameList.Select(x => data[x]);
            }
        }

        public Price TotalPrice()
        {
            Price total = new Price();
            foreach (var item in _Data)
                total += item.TotalInstitution(_FXMarket, _AssetMarket, Ccy).Value;
            return total;
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

        //public void AddNewAsset(string assetName, AssetStatics aSt, double acpValue)
        //{
        //    bool testAdd = _CcyDB.AddAsset(assetName, aSt);
        //    if (!testAdd)
        //        MessageBox.Show($"The new Asset [{assetName}] does already exist.");
        //    else
        //    {
        //        AssetCcyPair acp = new AssetCcyPair(new Asset(assetName), aSt.Ccy);
        //        _AssetMarket.AddQuote(acp, acpValue);
        //    }
        //}

        public void AddNewAssetCcy(IMarketInput mi, double value)
        {
            if (mi.Ccy != null)
                _FXMarket.AddQuote(mi, value);
            else
                _AssetMarket.AddQuote(mi, value);
        }

        //public void Reset(string ccy, CurrencyStatics cs)
        //{
        //    _Data = new List<Category> { };
        //    Map.Reset();
        //    _FXMarket.Reset();
        //    _CcyDB.AddRefCcy(ccy, cs);
        //    _AssetMarket.Reset();
        //    _Ccy = new Currency(ccy);
        //    AddItem(new NodeAddress(NodeType.Category, "TEMP"));
        //}

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
            int res = _Ccy.GetHashCode();
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
            ReorgAccountingData(_Map);
            info.AddValue("Categories", _Data, typeof(List<Category>));
            info.AddValue("FXMarket", _FXMarket, typeof(FXMarket));
            info.AddValue("AssetMarket", _AssetMarket, typeof(AssetMarket));
        }

        public AccountingData(SerializationInfo info, StreamingContext context)
        {
            _Ccy = (Currency)info.GetValue("Currency", typeof(Currency));
            _Data = (List<Category>)info.GetValue("Categories", typeof(List<Category>));
            _FXMarket = (FXMarket)info.GetValue("FXMarket", typeof(FXMarket));
            _AssetMarket = (AssetMarket)info.GetValue("AssetMarket", typeof(AssetMarket));
            _Map = new TreeViewMapping(_Data);
        }

        #endregion

        public AccountingData() { }

        public AccountingData(Currency ccy)
        {
            _Ccy = ccy;
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

        public IAccountingElement GetAccount(NodeAddress na)
        {
            return GetAccount(na.Address[0], na.Address[1], na.Address[2]);
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
                    return GetAccount(na);
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
            Category cat = new Category(newName, Ccy);
            cat.AddInstitution("New Institution");
            _Data.Add(cat);
            //cat.ModifyAmountEventHandler += this.UpdateTotalAmount;
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

        private void ReorgAccountingData(TreeViewMapping tvm)
        {
            throw new NotImplementedException();
            //List<Category> res = new List<Category> { };
            //// Reorg each constituents
            //foreach(var itemC in tvm)
            //{
            //    Category cat = GetCategory(itemC.Name);
            //    foreach (var itemI in itemC.Nodes)
            //    {
            //        Institution inst = GetInstitution(itemC.Name, itemI.Name);
            //        inst.ReorgItems(itemI.Nodes.Select(x => x.Name));
            //    }
            //    cat.ReorgItems(itemC.Nodes.Select(x => x.Name));
            //    res.Add(cat.Copy());
            //}
            //_Data = res;
            //_Map = new TreeViewMapping(res);
        }

        public SummaryReport GetSummary()
        {
            SummaryReport sr = new SummaryReport();
            foreach (var item in Categories)
            {
                sr.Add(item.GetSummary());
            }
            return sr;
        }

        public string GetAmountToString(ICcyAsset ICcyAsset, CurrencyStatics ccy_statics, double amount)
        {
            if (ICcyAsset.IsCcy())
                return ccy_statics.ValueToString(amount);
            else
                return Convert.ToString(amount);
        }

        //public double GetQuote(ICcyAsset item, Currency ccy)
        //{ 
        //    if (item.IsCcy())
        //        return FXMarket.GetQuote(new CurrencyPair(item.Ccy, ccy));
        //    else
        //        return AssetMarket.GetQuote(new AssetCcyPair(item.Asset, Ccy));
        //}

        public Price GetValue(NodeAddress na)
        {
            if (na.NodeType == NodeType.All)
                return TotalPrice();
            else
            {
                try { return GetElement(na).GetTotalAmount(FXMarket, AssetMarket, Ccy); }
                catch { return new Price(0.0, Ccy); }
            }
        }

        public object Clone()
        {
            List<Category> newData = new List<Category> { };
            foreach (var cat in _Data)
            {
                Category copyCat = (Category)cat.Clone();
                newData.Add(copyCat);
            }

            // TODO: Clonable IMarket?
            FXMarket fxmkt = new FXMarket(FXMarket.CcyRef);
            fxmkt.Copy(_FXMarket);

            AssetMarket aMkt = new AssetMarket();
            aMkt.Copy(_AssetMarket);

            AccountingData res = new AccountingData()
            {
                _Ccy = (Currency)_Ccy.Clone(),
                _Data = newData,
                _FXMarket = fxmkt,
                _AssetMarket = aMkt,
                _Map = null
            };

            res.ReorgAccountingData(_Map);

            //foreach (Category cat in newData)
            //    cat.ModifyAmountEventHandler += res.UpdateTotalAmount;

            return res;
        }
    }
}
