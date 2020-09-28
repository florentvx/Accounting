using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Institution
    {
        public string InstitutionName;
        Currency _Ccy;

        public Currency Ccy {
            get { return _Ccy; }
            set { _Ccy = value; }
        }

        List<Account> _Accounts;
        public List<Account> Accounts { get { return _Accounts; } }

        public Institution(string name, Currency ccy)
        {
            InstitutionName = name;
            Ccy = ccy;
            _Accounts = new List<Account> { };
        }

        public void AddAccount(string name, Currency currency = Currency.None)
        {
            if (currency == Currency.None)
                currency = Ccy;
            Account account = new Account(name, currency);
            _Accounts.Add(account);
        }
    }
}
