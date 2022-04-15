using Core.Finance;
using Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [Serializable]
    public class Category : ICategory, IEquatable<Category>, ISerializable
    {
        [JsonProperty]
        private string _CategoryName;

        [JsonProperty]
        Currency _CcyRef;

        [JsonProperty]
        public List<Institution> _Institutions;

        public Currency CcyRef { get { return _CcyRef; } }
           
        public Dictionary<string, Institution> InstitutionsDictionary
        {
            get { return _Institutions.ToDictionary(x => x.InstitutionName, x => x); }
        }

        #region IAccountingElement

        public string GetName() { return _CategoryName; }

        public ICcyAsset Ccy { get { return CcyRef; } }

        public IEnumerable<IAccountingElement> GetItemList()
        {
            return _Institutions.ToList<IAccountingElement>();
        }

        public IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme)
        {
            return GetInstitutions(tvme);
        }

        public NodeType GetNodeType() { return NodeType.Category; }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, Currency convCcy, string name)
        {
            return TotalInstitution(mkt, aMkt, convCcy.Ccy, name);
        }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, Currency convCcy)
        {
            return GetTotalAccount(mkt, aMkt, convCcy, "Total");
        }

        public void Delete(string name)
        {
            if (GetItemList().Count() > 1)
            {
                _Institutions = _Institutions.Where(x => x.InstitutionName != name).Select(x => x).ToList();
            }
        }

        public SummaryReport GetSummary()
        {
            SummaryReport sr = new SummaryReport();
            foreach (var item in Institutions)
            {
                sr.Add(item.GetSummary());
            }
            return sr;
        }

        public object Clone()
        {
            Category res = new Category(CategoryName, (Currency)Ccy.Clone());
            foreach (var item in _Institutions)
            {
                res.AddInstitution((Institution)item.Clone());
            }
            return res;
        }

        #endregion

        #region ICategory

        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        public IEnumerable<IInstitution> GetInstitutions(TreeViewMappingElement tvme)
        {
            return tvme.Nodes.Select(x => InstitutionsDictionary[x.Name]);
        }

        public IAccount TotalInstitution(FXMarket mkt, AssetMarket aMkt, Currency ccy, string overrideName)
        {
            Price total = new Price(0, ccy);
            foreach (var item in Institutions)
                total += item.GetTotalAccount(mkt, aMkt, ccy).Value;
            Account acc = new Account(overrideName, total);
            return acc;
        }

        public IAccount TotalInstitution(FXMarket mkt, AssetMarket aMkt, Currency convCcy)
        {
            return TotalInstitution(mkt, aMkt, convCcy, "Total");
        }

        #endregion

        #region IEquatable

        public bool Equals(Category cat)
        {
            if (cat == null)
                return false;
            if (_CategoryName == cat._CategoryName
                && _CcyRef == cat._CcyRef
                && _Institutions.Count == cat._Institutions.Count)
            {
                for (int i = 0; i < _Institutions.Count; i++)
                {
                    if (_Institutions[i] != cat._Institutions[i])
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Category);
        }

        public override int GetHashCode()
        {
            int res = _CategoryName.GetHashCode() + _CcyRef.GetHashCode();
            foreach (Institution item in _Institutions)
                res += item.GetHashCode();
            return res;
        }

        public static bool operator ==(Category cat1, Category cat2)
        {
            if (cat1 is null)
            {
                if (cat2 is null) { return true; }
                return false;
            }
            return cat1.Equals(cat2);
        }

        public static bool operator !=(Category cat1, Category cat2)
        {
            return !(cat1 == cat2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", _CategoryName, typeof(string));
            info.AddValue("Currency", _CcyRef, typeof(Currency));
            info.AddValue("Institutions", _Institutions, typeof(List<Institution>));
        }

        public Category(SerializationInfo info, StreamingContext context)
        {
            _CategoryName = (string)info.GetValue("Name", typeof(string));
            _CcyRef = (Currency)info.GetValue("Currency", typeof(Currency));
            _Institutions = (List<Institution>)info.GetValue("Institutions", typeof(List<Institution>));
        }

        #endregion

        public Category(string name, Currency ccy)
        {
            _CategoryName = name;
            _CcyRef = ccy;
            _Institutions = new List<Institution> { };
        }

        public IEnumerable<IInstitution> Institutions
        {
            get { return _Institutions.ToList<IInstitution>(); }
        }

        public Institution GetInstitution(string name)
        {
            return InstitutionsDictionary[name];
        }

        public void AddInstitution(string name, Currency currency = null)
        {
            if (currency == null)
                currency = CcyRef;
            Institution instit = new Institution(name, currency);
            _Institutions.Add(instit);
            //instit.ModifyAmountEventHandler += this.UpdateTotalAmount;
        }

        public void AddInstitution(Institution instit)
        {
            _Institutions.Add(instit);
            //instit.ModifyAmountEventHandler += this.UpdateTotalAmount;
        }

        public Institution AddNewInstitution()
        {
            int i = 0;
            string newNameRef = "New Institution";
            string newName = newNameRef;
            while (InstitutionsDictionary.ContainsKey(newName))
            {
                i++;
                newName = $"{newNameRef} - {i}";
            }
            AddInstitution(newName);
            Institution newInstit = GetInstitution(newName);
            newInstit.AddAccount("New Account");
            //newInstit.ModifyAmountEventHandler += this.UpdateTotalAmount;
            return newInstit;
        }

        public bool ChangeName(string before, string after, NodeAddress nodeTag)
        {
            bool test = false;
            switch (nodeTag.NodeType)
            {
                case NodeType.Institution:
                    if (InstitutionsDictionary.ContainsKey(before) && !InstitutionsDictionary.ContainsKey(after))
                    {
                        Institution instit_before = GetInstitution(before);
                        instit_before.InstitutionName = after;
                        test = true;
                    }
                    break;
                case NodeType.Account:
                    test = GetInstitution(nodeTag.Address[1]).ChangeName(before, after);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return test;
        }

        //public event EventHandler<ModifyAmountEventArgs> ModifyAmountEventHandler;

        internal void ReorgItems(IEnumerable<string> enumerable)
        {
            List<Institution> res = new List<Institution> { };
            foreach (string item in enumerable)
            {
                res.Add((Institution)GetInstitution(item).Clone());
            }
            _Institutions = res;
        }
    }
}
