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
    public class CurrencyPair : IMarketInput, ISerializable
    {
        [JsonProperty]
        Currency _Ccy1;

        [JsonProperty]
        Currency _Ccy2;

        public CurrencyPair()
        {
        }

        public CurrencyPair(Currency ccy1, Currency ccy2)
        {
            _Ccy1 = ccy1;
            _Ccy2 = ccy2;
        }

        public CurrencyPair(ICcyAsset ccy1, ICcyAsset ccy2)
        {
            if (ccy1.IsCcy() && ccy2.IsCcy())
            {
                _Ccy1 = ccy1.Ccy;
                _Ccy2 = ccy2.Ccy;
            }
            else { throw new Exception($"CurrencyPair needs 2 currencies {ccy1.ToString()} {ccy2.ToString()}!"); }
        }

        public CurrencyPair(string ccy1, string ccy2)
        {
            _Ccy1 = new Currency(ccy1);
            _Ccy2 = new Currency(ccy2);
        }

        #region IMarketInput

        public Currency Ccy1 { get { return _Ccy1; } }

        public Asset Asset1 { get { return null; } }

        public Currency Ccy2 { get { return _Ccy2; } }

        public object Item1 { get => Ccy1; }

        public ICcyAsset OtherAsset(ICcyAsset ccy)
        {
            if (ccy.Ccy == Ccy1)
                return Ccy2;
            if (ccy.Ccy == Ccy2)
                return Ccy1;
            throw new Exception();
        }

        public bool IsIdentity { get { return Ccy1 == Ccy2; } }

        internal bool IsEqual(CurrencyPair ccyPair)
        {
            return Ccy1 == ccyPair.Ccy1 && Ccy2 == ccyPair.Ccy2;
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
            return (ccy.Ccy == Ccy1 || ccy.Ccy == Ccy2);
        }

        #endregion

        #region IEquatable

        public bool Equals(IMarketInput cp)
        {
            if (cp == null)
                return false;
            return cp.Ccy1 == Ccy1 && cp.Ccy2 == Ccy2;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IMarketInput);
        }

        public override int GetHashCode()
        {
            return Ccy1.GetHashCode() + Ccy2.GetHashCode();
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
            info.AddValue("Ccy1", Ccy1, typeof(Currency));
            info.AddValue("Ccy2", Ccy2, typeof(Currency));
        }

        public CurrencyPair(SerializationInfo info, StreamingContext context)
        {
            _Ccy1 = (Currency)info.GetValue("Ccy1", typeof(Currency));
            _Ccy2 = (Currency)info.GetValue("Ccy2", typeof(Currency));
        }

        #endregion

        public override string ToString()
        {
            return $"{Ccy1.ToString()}/{Ccy2.ToString()}";
        }

        public bool Contains(Currency ccy)
        {
            return ccy == Ccy1 || ccy == Ccy2;
        }

        internal CurrencyPair GetInverse()
        {
            return new CurrencyPair(Ccy2, Ccy1);
        }

        public object Clone()
        {
            return new CurrencyPair((Currency)Ccy1.Clone(), (Currency)Ccy2.Clone());
        }
    }
}
