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

        public IAccount TotalInstitution(FXMarket mkt, AssetMarket aMkt, Currency convCcy, string overrideName)
        {
            double total = 0;
            foreach (var item in Institutions)
                total += item.TotalAccount(mkt, aMkt, Ccy).ConvertedAmount;
            Account acc = new Account(overrideName, Ccy, total, true);
            acc.RecalculateAmount(mkt, convCcy);
            return acc;
        }

        public IAccount TotalInstitution(FXMarket mkt, AssetMarket aMkt, Currency convCcy)
        {
            return TotalInstitution(mkt, aMkt, convCcy, "Total");
        }

        #endregion

        #region IAccountingElement

        public string GetName() { return _CategoryName; }

        public ICcyAsset CcyRef { get{ return Ccy; } }

        public IEnumerable<IAccountingElement> GetItemList()
        {
            return _Institutions.Values.ToList<IAccountingElement>();
        }

        public IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme)
        {
            return (IEnumerable<IAccountingElement>)GetInstitutions(tvme);
        }

        public NodeType GetNodeType() { return NodeType.Category; }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset convCcy, string name)
        {
            return TotalInstitution(mkt, aMkt, convCcy.Ccy, name);
        }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset convCcy)
        {
            return GetTotalAccount(mkt, aMkt, convCcy, "Total");
        }

        public void ModifyAmount(FXMarket mkt, AssetMarket aMkt, string v, object valueAmount)
        {
            throw new NotImplementedException();
        }

        public void ModifyCcy(FXMarket mkt, AssetMarket aMkt, string v, ICcyAsset valueCcy, bool isLastRow)
        {
            if (isLastRow)
            {
                _Ccy = valueCcy.Ccy;
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

        public Category(string name, Currency ccy)
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

        public void AddInstitution(string name, Currency currency = null)
        {
            if (currency == null)
                currency = Ccy;
            Institution instit = new Institution(name, currency);
            _Institutions.Add(instit.InstitutionName, instit);
        }

        private void AddInstitution(string name, Institution instit)
        {
            _Institutions.Add(name, instit);
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

        public Category Copy()
        {
            Category res = new Category(_CategoryName, (Currency)_Ccy.Clone());
            foreach (string item in _Institutions.Keys)
            {
                res.AddInstitution(item, _Institutions[item].Copy());
            }
            return res;
        }
    }
}
