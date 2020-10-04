using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Category : ICategory
    {
        public string CategoryName;
        Currency Ccy;
        Dictionary<string, Institution> _Institutions;
        public IEnumerable<IInstitution> Institutions {
            get { return _Institutions.Values.ToList<IInstitution>(); }
        }

        public Category(string name, Currency ccy = Currency.USD)
        {
            CategoryName = name;
            Ccy = ccy;
            _Institutions = new Dictionary<string, Institution> { };
        }

        public void AddInstitution(string name, Currency currency = Currency.None)
        {
            if (currency == Currency.None)
                currency = Ccy;
            Institution instit = new Institution(name, currency);
            _Institutions.Add(instit.InstitutionName, instit);
        }

        public IInstitution GetInstitution(string name)
        {
            return _Institutions[name];
        }

        public void AddAccount(string name, string institutionName)
        {
            Institution instit = _Institutions[institutionName];
            instit.AddAccount(name, instit.Ccy);
        }

        public Dictionary<string, List<string>> GetCategorySummary()
        {
            Dictionary<string, List<string>> res = new Dictionary<string, List<string>> { };
            foreach (Institution item in Institutions)
                res[item.InstitutionName] = item.GetAccountList();
            return res;
        }

        public IAccount TotalInstitution()
        {
            double total = 0;
            foreach (var item in Institutions)
                total += item.TotalAccount().Amount;
            return new Account("Total", Ccy, total);
        }
    }
}
