using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core
{
    public class Account : IAccount, IAccountingElement
    {
        string _AccountName;
        Currency _Ccy;
        double _Amount;
        bool _IsCalculatedAccount;

        #region IAccount

        public string AccountName
        {
            get { return _AccountName; }
            set { _AccountName = value; }
        }

        public Currency Ccy
        {
            get { return _Ccy; }
            set { _Ccy = value; }
        }

        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        public bool IsCalculatedAccount
        {
            get { return _IsCalculatedAccount; }
        }

        #endregion

        #region IAccountingElement

        public string GetName() { return AccountName; }

        public IEnumerable<IAccountingElement> GetItemList()
        {
            return new List<IAccountingElement> { };
        }

        public IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme)
        {
            return new List<IAccountingElement> { };
        }

        public NodeType GetNodeType() { return NodeType.Account; }

        public IAccount GetTotalAccount()
        {
            return this;
        }

        public IAccount GetTotalAccount(string name)
        {
            return this;
        }

        public void ModifyAmount(string v, object valueAmount)
        {
            throw new NotImplementedException();
        }

        public void ModifyCcy(string v, object valueCcy, bool isLastRow)
        {
            throw new NotImplementedException();
        }

        public void Delete(string v)
        {
            throw new NotImplementedException();
        }

        #endregion

        public Account(string name, Currency ccy, double amount = 0, bool isCalculatedAccount = false)
        {
            _AccountName = name;
            _Ccy = ccy;
            _Amount = amount;
            _IsCalculatedAccount = isCalculatedAccount;
        }
        
    }
}
