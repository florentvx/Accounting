using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Finance;

namespace Core
{
    public class Account : IAccount, IAccountingElement
    {
        string _AccountName;
        Currency _Ccy;
        double _Amount;
        double _ConvertedAmount;
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

        public double ConvertedAmount
        {
            get { return _ConvertedAmount; }
        }


        public bool IsCalculatedAccount
        {
            get { return _IsCalculatedAccount; }
        }

        #endregion

        #region IAccountingElement

        public string GetName() { return AccountName; }

        public Currency CcyRef { get { return _Ccy; } }

        public IEnumerable<IAccountingElement> GetItemList()
        {
            return new List<IAccountingElement> { };
        }

        public IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme)
        {
            return new List<IAccountingElement> { };
        }

        public NodeType GetNodeType() { return NodeType.Account; }

        public IAccount GetTotalAccount(Market mkt, Currency convCcy, string name)
        {
            RecalculateAmount(mkt, convCcy);
            return this;
        }

        public IAccount GetTotalAccount(Market mkt, Currency convCcy)
        {
            return GetTotalAccount(mkt, convCcy, null);
        }

        public void ModifyAmount(Market mkt, string v, object valueAmount)
        {
            throw new NotImplementedException();
        }

        public void ModifyCcy(Market mkt, string v, object valueCcy, bool isLastRow)
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
            _IsCalculatedAccount = isCalculatedAccount;
            //if (_IsCalculatedAccount)
            //    _ConvertedAmount = amount;
            //else
            _Amount = amount;
        }

        internal void RecalculateAmount(Market mkt, Currency ccyRef)
        {
            _ConvertedAmount = _Amount * mkt.GetQuote(new CurrencyPair(_Ccy, ccyRef));
        }
    }
}
