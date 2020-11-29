using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Finance;

namespace Core.Statics
{
    public class AssetStatics
    {
        private string _Symbol;
        private Currency _Ccy;

        public Currency Ccy { get { return _Ccy; } }

        public AssetStatics(string symbol, Currency ccy)
        {
            _Symbol = symbol;
            _Ccy = ccy;
        }
    }
}
