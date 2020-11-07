using Core.Finance;
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
        Currency _Ccy;
        Dictionary<string, Institution> _Institutions;

        #region ICategory

        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        public Currency Ccy { get { return _Ccy; } }

        public IEnumerable<IInstitution> GetInstitutions(TreeViewMappingElement tvme)
        {
            return tvme.Nodes.Select(x => _Institutions[x.Name]);
        }

        public IAccount TotalInstitution(Market mkt, Currency convCcy, string overrideName)
        {
            double total = 0;
            foreach (var item in Institutions)
                total += item.TotalAccount(mkt, Ccy).ConvertedAmount;
            Account acc = new Account(overrideName, Ccy, total, true);
            acc.RecalculateAmount(mkt, convCcy);
            return acc;
        }

        public IAccount TotalInstitution(Market mkt, Currency convCcy)
        {
            return TotalInstitution(mkt, convCcy, "Total");
        }

        #endregion

        #region IAccountingElement

        public string GetName() { return _CategoryName; }

        public Currency CcyRef { get{ return Ccy; } }

        public IEnumerable<IAccountingElement> GetItemList()
        {
            return _Institutions.Values.ToList<IAccountingElement>();
        }

        public IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme)
        {
            return (IEnumerable<IAccountingElement>)GetInstitutions(tvme);
        }

        public NodeType GetNodeType() { return NodeType.Category; }

        public IAccount GetTotalAccount(Market mkt, Currency convCcy, string name)
        {
            return TotalInstitution(mkt, convCcy, name);
        }

        public IAccount GetTotalAccount(Market mkt, Currency convCcy)
        {
            return GetTotalAccount(mkt, convCcy, "Total");
        }

        public void ModifyAmount(Market mkt, string v, object valueAmount)
        {
            throw new NotImplementedException();
        }

        public void ModifyCcy(Market mkt, string v, object valueCcy, bool isLastRow)
        {
            if (isLastRow)
            {
                _Ccy = CurrencyFunctions.ToCurrency(valueCcy);
            }
        }

        public void Delete(string v)
        {
            if (GetItemList().Count() > 1)
            {
                _Institutions.Remove(v);
            }
        }

        #endregion

        public Category(string name, Currency ccy = Currency.USD)
        {
            _CategoryName = name;
            _Ccy = ccy;
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
