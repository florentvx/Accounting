using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Finance
{
    public class Price
    {
        double? _Amount;
        ICcyAsset _Ccy;

        public Price(double? amount, ICcyAsset ccy)
        {
            _Amount = amount;
            _Ccy = ccy;
        }

        public ICcyAsset Ccy { get { return _Ccy; } }

        public bool HasValue { get { return _Amount.HasValue; } }
        public double Value { get { return _Amount.Value; } }
    }
}
