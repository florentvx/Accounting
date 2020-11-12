using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Finance
{
    public class Currency : IEquatable<Currency>
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
                if (ccy2 is null){ return true; }
                return false;
            }
            return ccy1.Equals(ccy2);
        }

        public static bool operator !=(Currency ccy1, Currency ccy2)
        {
            return !(ccy1 == ccy2);
        }
    }
}
