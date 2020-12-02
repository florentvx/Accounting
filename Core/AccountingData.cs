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

    public class AccountingData : IAccountingData
    {
        Currency _Ccy; // Ccy Total
        double _TotalValue; // Total value in Ccy Total
        CurrencyAssetStaticsDataBase _CcyDB; //Copy By Ref of CcyAssetDataBase
        Dictionary<string, Category> _Data = new Dictionary<string, Category> { };
        FXMarket _FXMarket;
        AssetMarket _AssetMarket;
        TreeViewMapping _Map;

        public Currency Ccy
        {
            get { return _Ccy; }
            ///set { _Ccy = value; }
        }

        public void SetCcyDB(CurrencyAssetStaticsDataBase ccyDB)
        {
            _CcyDB = ccyDB;
        }

        private Category GetCategory(string catName)
        {
            return _Data[catName];
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

        public ICategory GetFirstCategory()
        {
            return _Data[_Data.Keys.First().ToString()];
        }

        public IEnumerable<ICategory> Categories
        {
            get
            {
                IEnumerable<string> nameList = _Map.GetList(new NodeAddress(NodeType.All, ""));
                return nameList.Select(x => _Data[x]);
            }
        }

        public IAccount Total()
        {
            double total = 0;
            foreach (var item in _Data)
                total += item.Value.TotalInstitution(_FXMarket, _AssetMarket, Ccy).ConvertedAmount;
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
            if (nodeTag.NodeType == NodeType.Category)
            {
                if (!_Data.ContainsKey(after))
                {
                    _Data[after] = _Data[before];
                    _Data.Remove(before);
                    _Data[after].CategoryName = after;
                    test = true;
                }
            }
            else
            {
                test = _Data[nodeTag.Address[0]].ChangeName(before, after, nodeTag);
            }
            if (test)
                _Map.ChangeName(nodeTag, after);
            return test;
        }

        public void AddNewCcy(string ccyName, CurrencyStatics ccyStatics, CurrencyPair cp, double cpValue)
        {
            bool testAdd = _CcyDB.AddCcy(ccyName, ccyStatics);
            if (!testAdd)
                MessageBox.Show($"The new Currency [{ccyName}] does already exist.");
            else
            {
                _FXMarket.AddQuote(cp, cpValue);
            }
        }

        public void AddRefCcy(string ccyName, CurrencyStatics ccyStatics)
        {
            bool testAdd = _CcyDB.AddCcy(ccyName, ccyStatics);
            if (!testAdd)
                throw new Exception($"Add Ref Ccy Error {ccyName}");
            _Ccy = new Currency(ccyName);
            _FXMarket.SetCcyRef(_Ccy);
        }

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
            _Data = new Dictionary<string, Category> { };
            Map.Reset();
            _FXMarket.Reset();
            _CcyDB.AddRefCcy(ccy, cs);
            _AssetMarket.Reset();
            _Ccy = new Currency(ccy);
            AddItem(new NodeAddress(NodeType.Category, "TEMP"));
        }

        #endregion

        public AccountingData(CurrencyAssetStaticsDataBase ccyDB)
        {
            _CcyDB = ccyDB;
            _Ccy = ccyDB.RefCcy;
            _Data = new Dictionary<string, Category> { };
            _FXMarket = new FXMarket(Ccy);
            _AssetMarket = new AssetMarket();
            AddNewCategory();
            _Map = new TreeViewMapping(_Data);
        }

        public AccountingData(List<Category> input, FXMarket mkt, AssetMarket aMkt)
        {
            _Ccy = new Currency("USD");
            _Data = input.ToDictionary(x => x.CategoryName, x => x);
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
            string newNameRef = "New Category";
            string newName = newNameRef;
            while (_Data.ContainsKey(newName))
            {
                i++;
                newName = $"{newNameRef} - {i}";
            }
            Category cat = new Category(newName, _CcyDB.RefCcy);
            cat.AddInstitution("New Institution");
            cat.AddAccount("New Account", "New Institution");
            _Data.Add(cat.CategoryName, cat);
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
                    Institution newInstit = _Data[nodeAddress.Address[0]].AddNewInstitution();
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
                    _Data.Remove(na.Address[0]);
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

        internal void RecalculateTotal(Currency ccy)
        {
            if (ccy != _Ccy)
                _TotalValue = _TotalValue * _FXMarket.GetQuote(new CurrencyPair(_Ccy, ccy));
            _Ccy = (Currency)ccy.Clone();
        }

        public AccountingData Copy()
        {
            Dictionary<string, Category> newData = new Dictionary<string, Category> { };
            foreach (var text in _Data.Keys)
                newData[text] = _Data[text].Copy();

            FXMarket fxmkt = new FXMarket(_CcyDB.RefCcy);
            fxmkt.Copy(_FXMarket);

            AssetMarket aMkt = new AssetMarket();
            aMkt.Copy(_AssetMarket);

            AccountingData res = new AccountingData(_CcyDB)
            {
                _Ccy = (Currency)_Ccy.Clone(),
                _TotalValue = _TotalValue,
                _Data = newData,
                _FXMarket = fxmkt,
                _AssetMarket = aMkt,
                _Map = new TreeViewMapping(newData)
            };
            
            return res;
        }
    }
}
