using Core.Finance;
using Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ModifyAmountEventArgs : EventArgs { }

    [Serializable]
    public class Institution : IInstitution, IEquatable<Institution>, ISerializable
    {
        [JsonProperty]
        string _InstitutionName;
        [JsonProperty]
        Currency _Ccy;
        [JsonProperty]
        List<Account> _Accounts;

        Currency _TotalCcy = new Currency("NONE");
        double _TotalAmount = 0;

        public double TotalAmount { get { return _TotalAmount; } }

        #region IInstitution

        public string InstitutionName
        {
            get { return _InstitutionName; }
            set { _InstitutionName = value; }
        }

        public Currency Ccy
        {
            get { return _Ccy; }
            set { _Ccy = value; }
        }

        public IEnumerable<IAccount> Accounts { get { return _Accounts; } }

        public IEnumerable<IAccount> GetAccounts(TreeViewMappingElement tvme)
        {
            return tvme.Nodes.Select(x => GetAccount(x.Name));
        }

        public IAccount TotalAccount(FXMarket mkt, AssetMarket aMkt, Currency convCcy, string overrideAccountName)
        {
            double total = 0;
            foreach (var x in _Accounts)
                total += x.GetTotalAccount(mkt, aMkt, Ccy).ConvertedAmount;
            Account res = new Account(overrideAccountName, Ccy, total, isCalculatedAccount: true);
            res.RecalculateAmount(mkt, convCcy);
            return res;
        }

        public IAccount TotalAccount(FXMarket mkt, AssetMarket aMkt, Currency convCcy)
        {
            return TotalAccount(mkt, aMkt, convCcy, "Total");
        }

        #endregion

        #region IAccountingElement

        public string GetName() { return InstitutionName; }

        public ICcyAsset CcyRef { get { return _Ccy; } }

        public IEnumerable<IAccountingElement> GetItemList() => _Accounts;

        public IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme)
        {
            return (IEnumerable<IAccountingElement>)GetAccounts(tvme);
        }

        public NodeType GetNodeType() { return NodeType.Institution; }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset convCcy, string name)
        {
            return TotalAccount(mkt, aMkt, convCcy.Ccy, name);
        }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset convCcy)
        {
            return GetTotalAccount(mkt, aMkt, convCcy, "Total");
        }

        public void RecalculateAmount(Account acc, FXMarket mkt, AssetMarket aMkt, bool forceRecalc = true)
        {
            if (acc.Ccy.IsCcy())
                acc.RecalculateAmount(mkt, Ccy, forceRecalc);
            else
                acc.RecalculateAmount(aMkt, Ccy, forceRecalc);
        }

        public void RefreshTotal(FXMarket fxMkt, AssetMarket aMkt, Currency ccy)
        {
            _TotalAmount = 0;
            foreach (Account acc in Accounts)
            {
                acc.ModifyTotalCcy(fxMkt, aMkt, ccy);
                _TotalAmount += acc.TotalAmount;
            }
        }

        public event EventHandler<ModifyAmountEventArgs> ModifyAmountEventHandler;

        public void ModifyAmount(FXMarket mkt, AssetMarket aMkt, string accountName, object value)
        {
            Account acc = GetAccount(accountName);
            acc.ModifyAmount(mkt, aMkt, "", value);
            RefreshTotal(mkt, aMkt, _TotalCcy);
            ModifyAmountEventArgs e = new ModifyAmountEventArgs();
            ModifyAmountEventHandler?.Invoke(this, e);
        }

        public void ModifyCcy(FXMarket mkt, AssetMarket aMkt, string accountName, ICcyAsset value, bool IsLastRow)
        {
            if (IsLastRow)
            {
                Ccy = value.Ccy;
                foreach (Account item in Accounts)
                {
                    item.RecalculateConvertedAmount(Ccy, mkt, aMkt);
                }
            }
            else
            {
                Account acc = GetAccount(accountName);
                acc.ModifyCcy(mkt, aMkt, "", value, false);
            }
        }

        public void ModifyTotalCcy(FXMarket mkt, AssetMarket aMkt, Currency ccy)
        {
            if (_TotalCcy != ccy)
            {
                _TotalCcy = ccy;
                RefreshTotal(mkt, aMkt, ccy);
            }
        }

        public void Delete(string v)
        {
            if(GetItemList().Count() > 1)
            {
                Account accDel = _Accounts.Where(x => x.AccountName == v).First();
                _Accounts.Remove(accDel);
            }
        }

        public SummaryReport GetSummary()
        {
            SummaryReport sr = new SummaryReport();
            foreach (var item in Accounts)
            {
                sr.Add(item.GetSummary());
            }
            return sr;
        }

        public double GetTotalAmount()
        {
            return TotalAmount;
        }

        #endregion

        #region IEquatable

        public bool Equals(Institution instit)
        {
            if (instit == null)
                return false;
            if (_InstitutionName == instit._InstitutionName
                && _Ccy == instit._Ccy
                && _Accounts.Count == instit._Accounts.Count)
            {
                for (int i = 0; i < _Accounts.Count; i++)
                {
                    if (_Accounts[i] != instit._Accounts[i])
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Institution);
        }

        public override int GetHashCode()
        {
            int res = _InstitutionName.GetHashCode() + _Ccy.GetHashCode();
            foreach (Account item in _Accounts)
                res += item.GetHashCode();
            return res;
        }

        public static bool operator ==(Institution in1, Institution in2)
        {
            if (in1 is null)
            {
                if (in2 is null) { return true; }
                return false;
            }
            return in1.Equals(in2);
        }

        public static bool operator !=(Institution in1, Institution in2)
        {
            return !(in1 == in2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", _InstitutionName, typeof(string));
            info.AddValue("Currency", _Ccy, typeof(Currency));
            info.AddValue("Accounts", _Accounts, typeof(List<Account>));
        }

        public Institution(SerializationInfo info, StreamingContext context)
        {
            _InstitutionName = (string)info.GetValue("Name", typeof(string));
            _Ccy = (Currency)info.GetValue("Currency", typeof(Currency));
            _Accounts = (List<Account>)info.GetValue("Accounts", typeof(List<Account>));
        }

        #endregion

        public Institution(string name, Currency ccy)
        {
            _InstitutionName = name;
            Ccy = ccy;
            _TotalCcy = ccy;
            _Accounts = new List<Account> { };
        }

        public Account GetAccount(string accountName)
        {
            foreach (Account item in Accounts)
                if (item.AccountName == accountName)
                    return item;
            throw new Exception($"Account Name [{accountName}] not found in Insitution [{InstitutionName}]");
        }

        public Account AddAccount(string name, ICcyAsset currency = null, double amount = 0)
        {
            if (currency == null)
                currency = Ccy;
            Account account = new Account(name, currency, amount);
            _Accounts.Add(account);
            return account;
        }

        public void AddAccount(Account acc)
        {
            _Accounts.Add(acc);
        }

        private string GetNewAccountName()
        {
            int i = 0;
            string newNameRef = "New Account";
            string newName = newNameRef;
            while (_Accounts.Where(x => x.AccountName == newName).Count() > 0)
            {
                i++;
                newName = $"{newNameRef} - {i}";
            }
            return newName;
        }

        public Account AddNewAccount()
        {
            string newName = GetNewAccountName();
            return AddAccount(newName);
        }

        public bool ChangeName(string before, string after, NodeAddress nodeTag)
        {
            if (nodeTag.NodeType != NodeType.Account)
                throw new Exception($"Node Tag Unknown! [{nodeTag.NodeType}]");
            if (Accounts.Where(x => x.AccountName == after).Count() == 0)
            {
                var acc = Accounts.Where(x => x.AccountName == before).FirstOrDefault();
                acc.AccountName = after;
                return true;
            }
            return false;
        }

        internal void ReorgItems(IEnumerable<string> enumerable)
        {
            List<Account> res = new List<Account> { };
            foreach (string item in enumerable)
            {
                res.Add(GetAccount(item).Copy());
            }
            _Accounts = res;
        }

        internal Institution Copy()
        {
            Institution res = new Institution(_InstitutionName, (Currency)_Ccy.Clone());
            foreach (var item in _Accounts)
            {
                Account copyItem = item.Copy();
                copyItem.SetTotalCcy(_TotalCcy);
                res.AddAccount(copyItem);
            }
            return res;
        }

        internal void RefreshTotalAmount(FXMarket fXMarket, AssetMarket assetMarket)
        {
            _TotalAmount = 0;
            foreach (Account item in Accounts)
            {
                item.RefreshTotalAmount(fXMarket, assetMarket);
                _TotalAmount += item.TotalAmount;
            }
        }

        internal void PrepareForLoading(Currency ccy, FXMarket fXMarket, AssetMarket assetMarket)
        {
            _TotalCcy = ccy;
            _TotalAmount = 0;
            foreach (Account item in Accounts)
            {
                item.ModifyTotalCcy(fXMarket, assetMarket, ccy);
                _TotalAmount += item.TotalAmount;
            }
        }
    }
}
