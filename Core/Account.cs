using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core
{
    public class Account : IAccount
    {
        string _AccountName;
        Currency _Ccy;
        double _Amount;

        public string AccountName {
            get { return _AccountName; }
            set { _AccountName = value; }
        }

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
            _AccountName = name;
            _Ccy = ccy;
            _Amount = amount;
        }
        
    }
}
