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
        Currency _CcyRef;
        [JsonProperty]
        List<Account> _Accounts;

        public Currency CcyRef
        {
            get { return _CcyRef; }
            set { _CcyRef = value; }
        }


        #region IAccountingElement

        public string GetName() { return InstitutionName; }

        public ICcyAsset Ccy { get { return CcyRef; } }

        public IEnumerable<IAccountingElement> GetItemList() => _Accounts;

        public IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme)
        {
            return GetAccounts(tvme);
        }

        public NodeType GetNodeType() { return NodeType.Institution; }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, Currency ccy, string name)
        {
            return TotalAccount(mkt, aMkt, ccy, name);
        }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, Currency ccy)
        {
            return GetTotalAccount(mkt, aMkt, ccy, "Total");
        }

        public void Delete(string v)
        {
            if (GetItemList().Count() > 1)
            {
                Account accDel = _Accounts.Where(x => x.AccountName == v).First();
                _Accounts.Remove(accDel);
            }
        }

        //public SummaryReport GetSummary()
        //{
        //    SummaryReport sr = new SummaryReport();
        //    foreach (var item in Accounts)
        //    {
        //        sr.Add(item.GetSummary());
        //    }
        //    return sr;
        //}

        #endregion

        #region IInstitution

        public string InstitutionName
        {
            get { return _InstitutionName; }
            set { _InstitutionName = value; }
        }

        public IEnumerable<IAccount> Accounts { get { return _Accounts; } }

        public IEnumerable<IAccount> GetAccounts(TreeViewMappingElement tvme)
        {
            return tvme.Nodes.Select(x => GetAccount(x.Name));
        }

        public IAccount TotalAccount(FXMarket mkt, AssetMarket aMkt, Currency convCcy)
        {
            return TotalAccount(mkt, aMkt, convCcy, "Total");
        }

        #endregion



        #region IEquatable

        public bool Equals(Institution instit)
        {
            if (instit == null)
                return false;
            if (_InstitutionName == instit._InstitutionName
                && Ccy == instit.Ccy
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
            int res = _InstitutionName.GetHashCode() + Ccy.GetHashCode();
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
            info.AddValue("Currency", _CcyRef, typeof(Currency));
            info.AddValue("Accounts", _Accounts, typeof(List<Account>));
        }

        public Institution(SerializationInfo info, StreamingContext context)
        {
            _InstitutionName = (string)info.GetValue("Name", typeof(string));
            _CcyRef = (Currency)info.GetValue("Currency", typeof(Currency));
            _Accounts = (List<Account>)info.GetValue("Accounts", typeof(List<Account>));
        }

        #endregion

        public Institution(string name, Currency ccy)
        {
            _InstitutionName = name;
            _CcyRef = ccy;
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
                res.Add((Account)GetAccount(item).Clone());
            }
            _Accounts = res;
        }

        public IAccount TotalAccount(FXMarket mkt, AssetMarket aMkt, Currency ccy, string overrideAccountName)
        {
            Price total = new Price(0, (Currency)ccy.Clone());
            foreach (var x in _Accounts)
                total += x.GetTotalAccount(mkt, aMkt, ccy).Value;
            Account res = new Account(overrideAccountName, total);
            return res;
        }

        internal Institution Copy()
        {
            Institution res = new Institution(InstitutionName, (Currency)Ccy.Clone());
            foreach (var item in _Accounts)
            {
                Account copyItem = (Account)item.Clone();
                res.AddAccount(copyItem);
            }
            return res;
        }
    }
}
