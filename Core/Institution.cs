using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Institution : IInstitution
    {
        string _InstitutionName;
        Currency _Ccy;

        public string InstitutionName
        {
            get { return _InstitutionName; }
            set { _InstitutionName = value; }
        }

        public Currency Ccy {
            get { return _Ccy; }
            set { _Ccy = value; }
        }

        List<Account> _Accounts;
        public IEnumerable<IAccount> Accounts { get { return _Accounts; } }

        public Institution(string name, Currency ccy)
        {
            _InstitutionName = name;
            Ccy = ccy;
            _Accounts = new List<Account> { };
        }

        public void AddAccount(string name, Currency currency = Currency.None)
        {
            if (currency == Currency.None)
                currency = Ccy;
            Account account = new Account(name, currency);
            _Accounts.Add(account);
        }

        public List<string> GetAccountList()
        {
            return Accounts.Select(x => x.AccountName).ToList();
        }

        public Account GetAccount(string accountName)
        {
            foreach (Account item in Accounts)
                if (item.AccountName == accountName)
                    return item;
            throw new Exception($"Account Name [{accountName}] not found in Insitution [{InstitutionName}]");
        }

        public IAccount TotalAccount(string overrideAccountName)
        {
            double total = 0;
            foreach (var x in _Accounts)
                total += x.Amount;
            return new Account(overrideAccountName, Ccy, total, isCalculatedAccount: true);
        }

        public IAccount TotalAccount()
        {
            return TotalAccount("Total");
        }

        public void ModifyAmount(string accountName, object value)
        {
            Account acc = GetAccount(accountName);
            acc.Amount = Convert.ToDouble(value);
        }

        public void ModifyCcy(string accountName, object value, bool IsLastRow)
        {
            if (IsLastRow)
                Ccy = CurrencyFunctions.ToCurrency(value);
            else
            {
                Account acc = GetAccount(accountName);
                acc.Ccy = CurrencyFunctions.ToCurrency(value);
            }
        }

        public void ChangeName(string before, string after, NodeAddress nodeTag)
        {
            if (nodeTag.NodeType != NodeType.Account)
                throw new Exception($"Node Tag Unknown! [{nodeTag.NodeType}]");
            if (Accounts.Where(x => x.AccountName == after).Count() == 0)
            {
                var acc = Accounts.Where(x => x.AccountName == before).FirstOrDefault();
                acc.AccountName = after;
            }
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

        internal NodeAddress AddItem(NodeAddress nodeAddress)
        {
            if (nodeAddress.NodeType == NodeType.Account)
            {
                string newName = GetNewAccountName();
                AddAccount(newName);
                nodeAddress.ChangeAddress(newName);
                return nodeAddress;
            }
            else
                throw new Exception($"Unknown NodeType: [{nodeAddress.NodeType}]");
        }
    }
}
