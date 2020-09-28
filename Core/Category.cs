using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Category
    {
        public string CategoryName;
        Dictionary<string, Institution> _Institutions;
        public List<Institution> Insitutions { get { return _Institutions.Values.ToList(); } }

        public Category(string name)
        {
            CategoryName = name;
            _Institutions = new Dictionary<string, Institution> { };
        }

        public void AddInstitution(string name, Currency currency = Currency.USD)
        {
            Institution instit = new Institution(name, currency);
            _Institutions.Add(instit.InstitutionName, instit);
        }

        public Institution GetInstitution(string name)
        {
            return _Institutions[name];
        }

        public void AddAccount(string name, string institutionName)
        {
            Institution instit = _Institutions[institutionName];
            instit.AddAccount(name, instit.Ccy);
        }
    }
}
