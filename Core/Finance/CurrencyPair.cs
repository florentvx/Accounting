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
    [JsonObject(MemberSerialization.OptIn)]
    public class CurrencyPair : IMarketInput, ISerializable, ICloneable
    {
        [JsonProperty]
        Currency _Ccy;

        [JsonProperty]
        Currency _CcyPrice;

        public CurrencyPair(Currency ccy1, Currency ccy2)
        {
            _Ccy = ccy1;
            _CcyPrice = ccy2;
        }

        public CurrencyPair(ICcyAsset ccy1, ICcyAsset ccy2)
        {
            if (ccy1.IsCcy() && ccy2.IsCcy())
            {
                _Ccy = ccy1.Ccy;
                _CcyPrice = ccy2.Ccy;
            }
            else { throw new Exception($"CurrencyPair needs 2 currencies {ccy1} {ccy2}!"); }
        }

        public CurrencyPair() { }

        public CurrencyPair(string ccy1, string ccy2)
        {
            _Ccy = new Currency(ccy1);
            _CcyPrice = new Currency(ccy2);
        }

        #region IMarketInput

        public Currency Ccy { get { return _Ccy; } }

        public Asset Asset { get { return null; } }

        public Currency CcyPrice { get { return _CcyPrice; } }

        public ICcyAsset Other(ICcyAsset ccy)
        {
            if (ccy.Ccy == Ccy) return CcyPrice;
            if (ccy.Ccy == CcyPrice) return Ccy;
            return null;
        }

        public bool IsIdentity { get { return Ccy == CcyPrice; } }

        internal bool IsEqual(CurrencyPair ccyPair)
        {
            return Ccy == ccyPair.Ccy && CcyPrice == ccyPair.CcyPrice;
        }

        public bool IsEqual(IMarketInput imi)
        {
            if (imi.GetType() == typeof(CurrencyPair))
                return IsEqual((CurrencyPair)imi);
            else
                return false;
        }

        internal bool IsEquivalent(CurrencyPair ccyPair)
        {
            if (IsEqual(ccyPair))
                return true;
            else
            {
                CurrencyPair cp = ccyPair.GetInverse();
                return IsEqual(cp);
            }
        }

        public bool IsEquivalent(IMarketInput imi)
        {
            if (imi.GetType() == typeof(CurrencyPair))
                return IsEquivalent((CurrencyPair)imi);
            else
                return false;
        }

        public bool Contains(ICcyAsset ccy)
        {
            return (ccy.Ccy == Ccy || ccy.Ccy == CcyPrice);
        }

        #endregion

        #region IEquatable

        public bool Equals(IMarketInput cp)
        {
            if (cp == null)
                return false;
            return cp.Ccy == Ccy && cp.CcyPrice == CcyPrice;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IMarketInput);
        }

        public override int GetHashCode()
        {
            return Ccy.GetHashCode() + CcyPrice.GetHashCode();
        }

        public static bool operator ==(CurrencyPair cp1, IMarketInput cp2)
        {
            if (cp1 is null)
            {
                if (cp2 is null) { return true; }
                return false;
            }
            return cp1.Equals(cp2);
        }

        public static bool operator !=(CurrencyPair cp1, IMarketInput cp2)
        {
            return !(cp1 == cp2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Ccy", Ccy, typeof(Currency));
            info.AddValue("CcyPrice", CcyPrice, typeof(Currency));
        }

        public CurrencyPair(SerializationInfo info, StreamingContext context)
        {
            _Ccy = (Currency)info.GetValue("Ccy", typeof(Currency));
            _CcyPrice = (Currency)info.GetValue("CcyPrice", typeof(Currency));
        }

        #endregion

        public override string ToString()
        {
            return $"{Ccy}/{CcyPrice}";
        }

        public bool Contains(Currency ccy)
        {
            return ccy == Ccy || ccy == CcyPrice;
        }

        internal CurrencyPair GetInverse()
        {
            return new CurrencyPair(CcyPrice, Ccy);
        }

        public object Clone()
        {
            return new CurrencyPair((Currency)Ccy.Clone(), (Currency)CcyPrice.Clone());
        }
    }
}
