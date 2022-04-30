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
    public class Account : IAccount, IEquatable<Account>, ISerializable, ICloneable
    {
        [JsonProperty]
        string _AccountName;
        [JsonProperty]
        Price _Value;

        public Price Value { get { return _Value; } }

        #region IAccountingElement

        public string GetName() { return AccountName; }

        public IAccountingElement GetItem(NodeAddress na)
        {
            if (na.NodeType == NodeType.Account && na.GetLast() == AccountName)
                return (Account)this.Clone();
            throw new KeyNotFoundException($"Node Address not Found: Account: {AccountName} vs NodeAddress {na.GetLast()}");
        }

        public TreeViewMapping GetTreeStructure()
        {
            TreeViewMapping tvm = new TreeViewMapping();
            tvm.AddItem_Simple(AccountName);
            return tvm;
        }

        public NodeType GetNodeType() { return NodeType.Account; }

        public Price GetTotalAmount(Currency ccy, FXMarket fxMkt, AssetMarket aMkt)
        {
            return fxMkt.Convert(_Value, ccy, aMkt);
        }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, Currency ccy, string name)
        {
            return new Account(name, GetTotalAmount(ccy, mkt, aMkt));
        }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, Currency ccy)
        {
            return GetTotalAccount(mkt, aMkt, ccy, "TOTAL_ACCOUNT");
        }

        public Price GetTotalAmount(FXMarket mkt, AssetMarket aMkt, Currency ccy)
        {
            return GetTotalAccount(mkt, aMkt, ccy).Value;
        }

        public void Delete(string name)
        {
            throw new NotImplementedException();
        }

        public SummaryReport GetSummary()
        {
            return new SummaryReport(Ccy, Amount);
        }

        public bool ChangeName(NodeAddress na, string after)
        {
            if (na.NodeType == NodeType.Account)
                if (na.GetLast() == AccountName)
                {
                    AccountName = after;
                    return true;
                }
                else
                    return false;
            else
                throw new InvalidOperationException();
        }

        #endregion

        #region IAccount

        public string AccountName
        {
            get { return _AccountName; }
            set { _AccountName = value; }
        }

        public ICcyAsset Ccy
        {
            get { return _Value.Ccy; }
            set { _Value.Ccy = value; }
        }

        public double Amount
        {
            get { return _Value.Amount; }
            set { Value.Amount = value; }
        }

        // explain what is string v?
        public void ModifyAmount(double valueAmount)
        {
            Amount = Convert.ToDouble(valueAmount);
        }

        public void ModifyCcy(ICcyAsset valueCcy)
        {
            Ccy = valueCcy;
        }

        #endregion

        #region IEquatable

        public bool Equals(Account acc)
        {
            if (acc == null)
                return false;
            return AccountName == acc.AccountName
                    && Value == acc.Value;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Account);
        }


        public override int GetHashCode()
        {
            return AccountName.GetHashCode() + Value.GetHashCode();
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
            info.AddValue("Name", AccountName, typeof(string));
            info.AddValue("Value", Value, typeof(Currency));
        }

        public Account(SerializationInfo info, StreamingContext context)
        {
            _AccountName = (string)info.GetValue("Name", typeof(string));
            _Value = (Price)info.GetValue("Value", typeof(Price));
        }

        #endregion

        public Account() { }

        public Account(string name, ICcyAsset ccy, double amount = 0)
        {
            _AccountName = name;
            _Value = new Price(amount, ccy);
        }
        public Account(string name, Price price)
        {
            _AccountName = name;
            _Value = (Price)price.Clone();
        }

        public object Clone()
        {
            return new Account(AccountName, (Price)Value.Clone());
        }
    }
}
