﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Finance
{
    public class Currency : ICcyAsset, IEquatable<Currency>, ICloneable
    {
        string _Ccy;

        public Currency(string ccy)
        {
            _Ccy = ccy.ToUpper();
        }

        public Currency(object ccy)
        {
            _Ccy = Convert.ToString(ccy).ToUpper();
        }

        public bool IsNone { get { return _Ccy == "NONE"; } }

        public override string ToString() { return _Ccy; }

        #region ICcyAsset

        public Currency Ccy => this;

        public Asset Asset => null;

        public bool IsCcy()
        {
            return true;
        }

        public IMarketInput CreateMarketInput(Currency ccyRef)
        {
            return new CurrencyPair(this, ccyRef);
        }

        #endregion

        #region IEquatable

        public bool Equals(Currency ccy)
        {
            if (ccy == null)
                return false;
            return _Ccy == ccy._Ccy;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Currency);
        }

        public override int GetHashCode()
        {
            return _Ccy.GetHashCode();
        }

        public static bool operator ==(Currency ccy1, Currency ccy2)
        {
            if (ccy1 is null)
            {
                if (ccy2 is null) { return true; }
                return false;
            }
            return ccy1.Equals(ccy2);
        }

        public static bool operator !=(Currency ccy1, Currency ccy2)
        {
            return !(ccy1 == ccy2);
        }

        #endregion

        #region ICloneable

        public object Clone()
        {
            return new Currency(_Ccy);
        }

        #endregion
    }
}
