using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Account
    {
        public string AccountName;
        Currency _Ccy;
        double _Amount;

        public Currency Ccy {
            get { return _Ccy; }
            set { _Ccy = value; }
        }

        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        public Account(string name, Currency ccy, double amount = 0)
        {
            AccountName = name;
            _Ccy = ccy;
            _Amount = amount;
        }

        
    }
}
