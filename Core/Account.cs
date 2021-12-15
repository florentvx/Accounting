using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Finance;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Core
{
    [Serializable]
    public class Account : IAccount, IEquatable<Account>, ISerializable
    {
        [JsonProperty]
        string _AccountName;
        [JsonProperty]
        ICcyAsset _Ccy;
        [JsonProperty]
        double _Amount;

        Currency _ConvertedCcy;
        double _ConvertedAmount;

        Currency _TotalCcy = new Currency("NONE");
        double _TotalAmount = 0;

        Price _LastAmount;
        public Price LastAmount { get { return _LastAmount; } }

        public double TotalAmount { get { return _TotalAmount; } }

        readonly bool _IsCalculatedAccount; // used for TotalAccount purposes

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

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset convCcy, string name, Price lastAmount)
        {
            _LastAmount = null;
            if (lastAmount != null)
            {
                _LastAmount = lastAmount;
                if (!_LastAmount.Ccy.Equals(convCcy))
                    // lastAmount should already converted since you need to convert it with prevFxmkt
                    throw new Exception($"Last Amount {_LastAmount.Ccy} not converted in {convCcy.Ccy}!");
            }

            if (Ccy.IsCcy())
                RecalculateAmount(mkt, convCcy.Ccy);
            else
                RecalculateAmount(aMkt, convCcy.Ccy);
            return this;
        }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset convCcy)
        {
            return GetTotalAccount(mkt, aMkt, convCcy, null, null);
        }

        public void ModifyAmount(FXMarket mkt, AssetMarket aMkt, string v, object valueAmount) //TODO: Change of Ccy
        {
            _Amount = Convert.ToDouble(valueAmount);
            IMarket iMkt = mkt;
            if (!_Ccy.IsCcy())
                iMkt = aMkt;
            _ConvertedAmount = _Amount * iMkt.GetQuote(_Ccy.CreateMarketInput(_ConvertedCcy));
            _TotalAmount = _Amount * iMkt.GetQuote(_Ccy.CreateMarketInput(_TotalCcy));
        }

        public void ModifyCcy(FXMarket mkt, AssetMarket aMkt, string v, ICcyAsset valueCcy, bool isLastRow)
        {
            _Ccy = valueCcy;
            IMarket iMkt = mkt;
            if (!_Ccy.IsCcy())
                iMkt = aMkt;
            _ConvertedAmount = _Amount * iMkt.GetQuote(_Ccy.CreateMarketInput(_ConvertedCcy));
            _TotalAmount = _Amount * iMkt.GetQuote(_Ccy.CreateMarketInput(_TotalCcy));
        }

        public void ModifyTotalCcy(FXMarket mkt, AssetMarket aMkt, Currency ccy)
        {
            if (_TotalCcy != ccy)
            {
                _TotalCcy = ccy;
                IMarket iMkt = mkt;
                if (!_Ccy.IsCcy())
                    iMkt = aMkt;
                _TotalAmount = _Amount * iMkt.GetQuote(_Ccy.CreateMarketInput(_TotalCcy));
            }
        }

        public void Delete(string v)
        {
            throw new NotImplementedException();
        }

        public SummaryReport GetSummary()
        {
            return new SummaryReport(CcyRef, Amount);
        }

        public Price GetTotalAmount(Currency ccy, FXMarket fxMkt)
        {
            double value = TotalAmount * fxMkt.GetQuote(new CurrencyPair(_TotalCcy, ccy));
            return new Price(value, ccy);
        }

        #endregion

        #region IEquatable

        public bool Equals(Account acc)
        {
            if (acc == null)
                return false;
            return _AccountName == acc._AccountName
                && _Ccy.Ccy == acc._Ccy.Ccy
                && _Ccy.Asset == acc._Ccy.Asset
                && _Amount == acc._Amount;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Account);
        }

        internal void RecalculateConvertedAmount(Currency ccy, FXMarket mkt, AssetMarket aMkt)
        {
            _ConvertedCcy = ccy;
            IMarket iMkt = mkt;
            if (!_Ccy.IsCcy())
                iMkt = aMkt;
            _ConvertedAmount = _Amount * iMkt.GetQuote(_Ccy.CreateMarketInput(_ConvertedCcy));
        }

        public override int GetHashCode()
        {
            return _AccountName.GetHashCode() + _Ccy.GetHashCode() + _Amount.GetHashCode();
        }

        public static bool operator ==(Account acc1, Account acc2)
        {
            if (acc1 is null)
            {
                if (acc2 is null) { return true; }
                return false;
            }
            return acc1.Equals(acc2);
        }

        public static bool operator !=(Account acc1, Account acc2)
        {
            return !(acc1 == acc2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", _AccountName, typeof(string));
            info.AddValue("Currency", _Ccy.Ccy, typeof(Currency));
            info.AddValue("Asset", _Ccy.Asset, typeof(Asset));
            info.AddValue("Amount", _Amount, typeof(double));
        }

        public Account(SerializationInfo info, StreamingContext context)
        {
            _AccountName = (string)info.GetValue("Name", typeof(string));
            Currency ccy = (Currency)info.GetValue("Currency", typeof(Currency));
            if (!(ccy == null))
                _Ccy = (ICcyAsset)ccy;
            else
                _Ccy = (ICcyAsset)info.GetValue("Asset", typeof(Asset));
            _Amount = (double)info.GetValue("Amount", typeof(double));
        }

        #endregion

        public Account(string name, ICcyAsset ccy, double amount = 0, bool isCalculatedAccount = false, Price lastAmount = null)
        {
            _AccountName = name;
            _Ccy = ccy;
            _TotalCcy = ccy.Ccy;
            _ConvertedCcy = ccy.Ccy;
            _IsCalculatedAccount = isCalculatedAccount;
            _Amount = amount;
            _LastAmount = lastAmount;
            _ConvertedAmount = amount;
        }

        internal void RecalculateAmount(IMarket mkt, Currency ccyRef, bool forceRecalc = true)
        {
            if (forceRecalc || ccyRef != _ConvertedCcy)
            {
                _ConvertedCcy = ccyRef;
                _ConvertedAmount = _Amount * mkt.GetQuote(_Ccy.CreateMarketInput(ccyRef));
            }
        }

        internal void RefreshTotalAmount(FXMarket fXMarket, AssetMarket assetMarket)
        {
            IMarket iMkt = fXMarket;
            if (!Ccy.IsCcy())
                iMkt = assetMarket;
            _TotalAmount = _Amount * iMkt.GetQuote(_Ccy.CreateMarketInput(_TotalCcy));
        }

        // function to set TotalCcy for copying process purposes
        internal void SetTotalCcy(Currency totalCcy)
        {
            _TotalCcy = totalCcy;
        }

        internal Account Copy()
        {
            return new Account(_AccountName, _Ccy, _Amount, false);
        }
    }
}
