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
        private string _Name;
        private Currency _Ccy;

        public string Name { get { return _Name; } set { _Name = value; } }
        public Currency Ccy { get { return _Ccy; } set { _Ccy = value; } }

        public AssetStatics(string name, Currency ccy)
        {
            _Name = name;
            _Ccy = ccy;
        }

        #region IEquatable

        public bool Equals(AssetStatics aS)
        {
            if (aS == null)
                return false;
            return _Ccy == aS._Ccy && _Name == aS._Name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as AssetStatics);
        }

        public override int GetHashCode()
        {
            return _Ccy.GetHashCode() + _Name.GetHashCode();
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
            info.AddValue("Name", _Name, typeof(string));
            _Ccy.GetObjectData(info, context);
        }

        public AssetStatics(SerializationInfo info, StreamingContext context)
        {
            _Name = (string)info.GetValue("Name", typeof(string));
            _Ccy = new Currency(info, context);
        }

        #endregion

        public object Clone()
        {
            return new AssetStatics((string)_Name.Clone(), (Currency)_Ccy.Clone());
        }
    }
}
