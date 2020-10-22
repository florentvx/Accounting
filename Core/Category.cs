using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Category : ICategory, IAccountingElement
    {
        private string _CategoryName;
        Currency Ccy;
        Dictionary<string, Institution> _Institutions;

        #region ICategory

        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        public IEnumerable<IInstitution> GetInstitutions(TreeViewMappingElement tvme)
        {
            return tvme.Nodes.Select(x => _Institutions[x.Name]);
        }

        public IAccount TotalInstitution(string overrideName)
        {
            double total = 0;
            foreach (var item in Institutions)
                total += item.TotalAccount().Amount;
            return new Account(overrideName, Ccy, total, true);
        }

        public IAccount TotalInstitution()
        {
            return TotalInstitution("Total");
        }

        #endregion

        #region IAccountingElement

        public string GetName() { return _CategoryName; }

        public IEnumerable<IAccountingElement> GetItemList()
        {
            return _Institutions.Values.ToList<IAccountingElement>();
        }

        public IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme)
        {
            return (IEnumerable<IAccountingElement>)GetInstitutions(tvme);
        }

        public NodeType GetNodeType() { return NodeType.Category; }

        public IAccount GetTotalAccount(string name)
        {
            return TotalInstitution(name);
        }

        public IAccount GetTotalAccount()
        {
            return GetTotalAccount("Total");
        }

        public void ModifyAmount(string v, object valueAmount)
        {
            throw new NotImplementedException();
        }

        public void ModifyCcy(string v, object valueCcy, bool isLastRow)
        {
            if (isLastRow)
            {
                Ccy = CurrencyFunctions.ToCurrency(valueCcy);
            }
        }

        #endregion

        public Category(string name, Currency ccy = Currency.USD)
        {
            _CategoryName = name;
            Ccy = ccy;
            _Institutions = new Dictionary<string, Institution> { };
        }

        public IEnumerable<IInstitution> Institutions
        {
            get { return _Institutions.Values.ToList<IInstitution>(); }
        }

        public Institution GetInstitution(string name)
        {
            return _Institutions[name];
        }

        public void AddInstitution(string name, Currency currency = Currency.None)
        {
            if (currency == Currency.None)
                currency = Ccy;
            Institution instit = new Institution(name, currency);
            _Institutions.Add(instit.InstitutionName, instit);
        }

        public Institution AddNewInstitution()
        {
            int i = 0;
            string newNameRef = "New Institution";
            string newName = newNameRef;
            while (_Institutions.ContainsKey(newName))
            {
                i++;
                newName = $"{newNameRef} - {i}";
            }
            AddInstitution(newName);
            _Institutions[newName].AddAccount("New Account");
            return _Institutions[newName];
        }

        public void AddAccount(string name, string institutionName)
        {
            Institution instit = _Institutions[institutionName];
            instit.AddAccount(name, instit.Ccy);
        }

        public bool ChangeName(string before, string after, NodeAddress nodeTag)
        {
            bool test = false;
            if (nodeTag.NodeType == NodeType.Institution)
            {
                if (_Institutions.ContainsKey(before) && !_Institutions.ContainsKey(after))
                {
                    _Institutions[after] = _Institutions[before];
                    _Institutions[after].InstitutionName = after;
                    _Institutions.Remove(before);
                    test = true;
                }
            }
            else
            {
                test = _Institutions[nodeTag.Address[1]].ChangeName(before, after, nodeTag);
            }
            return test;
        }

       
    }
}
