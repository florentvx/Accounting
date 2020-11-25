using Core.Finance;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Institution : IInstitution, IAccountingElement
    {
        string _InstitutionName;
        Currency _Ccy;
        List<Account> _Accounts;

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

        public void RecalculateAmount(Account acc, FXMarket mkt, AssetMarket aMkt)
        {
            if (acc.Ccy.IsCcy())
                acc.RecalculateAmount(mkt, Ccy);
            else
                acc.RecalculateAmount(aMkt, Ccy);
        }

        public void ModifyAmount(FXMarket mkt, AssetMarket aMkt, string accountName, object value)
        {
            Account acc = GetAccount(accountName);
            acc.Amount = Convert.ToDouble(value);
            RecalculateAmount(acc, mkt, aMkt);
        }

        public void ModifyCcy(FXMarket mkt, AssetMarket aMkt, string accountName, ICcyAsset value, bool IsLastRow)
        {
            if (IsLastRow)
            {
                Ccy = value.Ccy;
                foreach (Account item in Accounts)
                {
                    RecalculateAmount(item, mkt, aMkt);
                }
            }
            else
            {
                Account acc = GetAccount(accountName);
                acc.Ccy = value;
                RecalculateAmount(acc, mkt, aMkt);
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

        #endregion

        public Institution(string name, Currency ccy)
        {
            _InstitutionName = name;
            Ccy = ccy;
            _Accounts = new List<Account> { };
        }

        public Account GetAccount(string accountName)
        {
            foreach (Account item in Accounts)
                if (item.AccountName == accountName)
                    return item;
            throw new Exception($"Account Name [{accountName}] not found in Insitution [{InstitutionName}]");
        }

        public Account AddAccount(string name, Currency currency = null)
        {
            if (currency == null)
                currency = Ccy;
            Account account = new Account(name, currency);
            _Accounts.Add(account);
            return account;
        }

        private void AddAccount(Account acc)
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

        internal Institution Copy()
        {
            Institution res = new Institution(_InstitutionName, (Currency)_Ccy.Clone());
            foreach (var item in _Accounts)
            {
                res.AddAccount(item.Copy());
            }
            return res;
        }
    }
}
