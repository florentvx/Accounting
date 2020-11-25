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
        ICcyAsset _Ccy;
        double _Amount;
        Currency _ConvertedCcy;
        double _ConvertedAmount;
        bool _IsCalculatedAccount;

        #region IAccount

        public string AccountName
        {
            get { return _AccountName; }
            set { _AccountName = value; }
        }

        public ICcyAsset Ccy
        {
            get { return _Ccy; }
            set { _Ccy = value; }
        }

        public Currency ConvertedCcy
        {
            get { return _ConvertedCcy; }
            set { _ConvertedCcy = value; }
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

        public ICcyAsset CcyRef { get { return _Ccy; } }

        public IEnumerable<IAccountingElement> GetItemList()
        {
            return new List<IAccountingElement> { };
        }

        public IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme)
        {
            return new List<IAccountingElement> { };
        }

        public NodeType GetNodeType() { return NodeType.Account; }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset convCcy, string name)
        {
            if (Ccy.IsCcy())
                RecalculateAmount(mkt, convCcy.Ccy);
            else
                RecalculateAmount(aMkt, convCcy.Ccy);
            return this;
        }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset convCcy)
        {
            return GetTotalAccount(mkt, aMkt, convCcy, null);
        }

        public void ModifyAmount(FXMarket mkt, AssetMarket aMkt, string v, object valueAmount)
        {
            throw new NotImplementedException();
        }

        public void ModifyCcy(FXMarket mkt, AssetMarket aMkt, string v, ICcyAsset valueCcy, bool isLastRow)
        {
            throw new NotImplementedException();
        }

        public void Delete(string v)
        {
            throw new NotImplementedException();
        }

        #endregion

        public Account(string name, ICcyAsset ccy, double amount = 0, bool isCalculatedAccount = false)
        {
            _AccountName = name;
            _Ccy = ccy;
            _ConvertedCcy = ccy.Ccy;
            _IsCalculatedAccount = isCalculatedAccount;
            _Amount = amount;
        }

        internal void RecalculateAmount(IMarket mkt, Currency ccyRef)
        {
            _ConvertedCcy = ccyRef;
            _ConvertedAmount = _Amount * mkt.GetQuote(_Ccy.CreateMarketInput(ccyRef));
        }

        internal Account Copy()
        {
            return new Account(_AccountName, _Ccy, _Amount, false);
        }
    }
}
