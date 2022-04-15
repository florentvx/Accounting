using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Newtonsoft.Json;

namespace Core.Finance
{
    [Serializable]
    public class Price : ICloneable, ISerializable, IEquatable<Price>
    {
        [JsonProperty]
        double? _Amount;

        [JsonProperty]
        ICcyAsset _Ccy;

        public Price() { }

        public Price(double? amount, ICcyAsset ccy)
        {
            _Amount = amount;
            _Ccy = ccy;
        }

        public ICcyAsset Ccy { get { return _Ccy; } set { _Ccy = value; } }

        public bool HasValue { get { return _Amount.HasValue; } }
        public double Amount { get { return _Amount.Value; } set { _Amount = value; } }

        public object Clone()
        {
            return new Price(_Amount, (ICcyAsset)_Ccy.Clone());
        }

        public static Price operator +(Price p1, Price p2)
        {
            if (!p1.Ccy.Equals(p2.Ccy))
            {
                throw new InvalidOperationException("sum of price in different ccies");
            }
            return new Price(p1.Amount + p2.Amount, p1.Ccy);
        }

        public override string ToString()
        {
            return $"PRICE: {_Amount} {_Ccy}";
        }

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Amount", Amount, typeof(double));
            info.AddValue("Currency", Ccy.Ccy, typeof(Currency));
            info.AddValue("Asset", Ccy.Asset, typeof(Asset));
        }

        public Price(SerializationInfo info, StreamingContext context)
        {
            Amount = (double)info.GetValue("Amount", typeof(double));
            Currency ccy = (Currency)info.GetValue("Currency", typeof(Currency));
            if (!(ccy == null))
                Ccy = (ICcyAsset)ccy;
            else
                Ccy = (ICcyAsset)info.GetValue("Asset", typeof(Asset));
        }

        #endregion

        #region IEquatable

        public bool Equals(Price other)
        {
            if (other == null)
                return false;
            return Amount == other.Amount && Ccy.Equals(other.Ccy);
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Price);
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode() + Ccy.GetHashCode();
        }

        public static bool operator ==(Price p1, Price p2)
        {
            if (p1 is null)
            {
                if (p2 is null) { return true; }
                return false;
            }
            return p1.Equals(p2);
        }

        public static bool operator !=(Price p1, Price p2)
        {
            return !(p1 == p2);
        }

        #endregion
    }
}
