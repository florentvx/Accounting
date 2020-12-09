using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Statics
{
    [Serializable]
    public class AssetStatics : IEquatable<AssetStatics>, ISerializable
    {
        private string _Symbol;
        private Currency _Ccy;

        public string Symbol { get { return _Symbol; } set { _Symbol = value; } }
        public Currency Ccy { get { return _Ccy; } set { _Ccy = value; } }

        public AssetStatics(string symbol, Currency ccy)
        {
            _Symbol = symbol;
            _Ccy = ccy;
        }

        #region IEquatable

        public bool Equals(AssetStatics aS)
        {
            if (aS == null)
                return false;
            return _Ccy == aS._Ccy && _Symbol == aS._Symbol;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as AssetStatics);
        }

        public override int GetHashCode()
        {
            return _Ccy.GetHashCode() + _Symbol.GetHashCode();
        }

        public static bool operator ==(AssetStatics as1, AssetStatics as2)
        {
            if (as1 is null)
            {
                if (as1 is null) { return true; }
                return false;
            }
            return as1.Equals(as2);
        }

        public static bool operator !=(AssetStatics as1, AssetStatics as2)
        {
            return !(as1 == as2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Symbol", _Symbol, typeof(string));
            _Ccy.GetObjectData(info, context);
        }

        public AssetStatics(SerializationInfo info, StreamingContext context)
        {
            _Symbol = (string)info.GetValue("Symbol", typeof(string));
            _Ccy = new Currency(info, context);
        }

        #endregion
    }
}
