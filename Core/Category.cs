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
        Currency _Ccy;

        [JsonProperty]
        public List<Institution> _Institutions;

        Currency _TotalCcy = new Currency("NONE");
        double _TotalAmount = 0;
        public double TotalAmount { get { return _TotalAmount; } }

        public Dictionary<string, Institution> InstitutionsDictionary
        {
            get { return _Institutions.ToDictionary(x => x.InstitutionName, x => x); }
        }

        #region ICategory

        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        public Currency Ccy { get { return _Ccy; } }

        public IEnumerable<IInstitution> GetInstitutions(TreeViewMappingElement tvme)
        {
            return tvme.Nodes.Select(x => InstitutionsDictionary[x.Name]);
        }

        public IAccount TotalInstitution(FXMarket mkt, AssetMarket aMkt, Currency convCcy, string overrideName, Price lastAmount = null)
        {
            double total = 0;
            foreach (var item in Institutions)
                total += item.TotalAccount(mkt, aMkt, Ccy).ConvertedAmount;
            Account acc = new Account(overrideName, Ccy, total, true, lastAmount);
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
            return _Institutions.ToList<IAccountingElement>();
        }

        public IEnumerable<IAccountingElement> GetItemList(TreeViewMappingElement tvme)
        {
            return (IEnumerable<IAccountingElement>)GetInstitutions(tvme);
        }

        public NodeType GetNodeType() { return NodeType.Category; }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset convCcy, string name, Price lastAmount)
        {
            return TotalInstitution(mkt, aMkt, convCcy.Ccy, name, lastAmount);
        }

        public IAccount GetTotalAccount(FXMarket mkt, AssetMarket aMkt, ICcyAsset convCcy)
        {
            return GetTotalAccount(mkt, aMkt, convCcy, "Total", null);
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

        public void ModifyTotalCcy(FXMarket mkt, AssetMarket aMkt, Currency ccy)
        {
            if (_TotalCcy != ccy)
            {
                _TotalCcy = ccy;
                _TotalAmount = 0;
                foreach (Institution instit in Institutions)
                {
                    instit.ModifyTotalCcy(mkt, aMkt, ccy);
                    _TotalAmount += instit.TotalAmount;
                }
            }
        }

        public void Delete(string v)
        {
            if (GetItemList().Count() > 1)
            {
                _Institutions = _Institutions.Where(x => x.InstitutionName != v).Select(x => x).ToList();
                //_Institutions.Remove(v);
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

        public Price GetTotalAmount(Currency ccy, FXMarket fxMkt)
        {
            double value = TotalAmount * fxMkt.GetQuote(new CurrencyPair(_TotalCcy, ccy));
            return new Price(value, ccy);
        }

        #endregion

        #region IEquatable

        public bool Equals(Category cat)
        {
            if (cat == null)
                return false;
            if (_CategoryName == cat._CategoryName
                && _Ccy == cat._Ccy
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
            int res = _CategoryName.GetHashCode() + _Ccy.GetHashCode();
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
            info.AddValue("Currency", _Ccy, typeof(Currency));
            info.AddValue("Institutions", _Institutions, typeof(List<Institution>));
        }

        public Category(SerializationInfo info, StreamingContext context)
        {
            _CategoryName = (string)info.GetValue("Name", typeof(string));
            _Ccy = (Currency)info.GetValue("Currency", typeof(Currency));
            _Institutions = (List<Institution>)info.GetValue("Institutions", typeof(List<Institution>));
        }

        #endregion

        public Category(string name, Currency ccy)
        {
            _CategoryName = name;
            _Ccy = ccy;
            _TotalCcy = ccy;
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
                currency = Ccy;
            Institution instit = new Institution(name, currency);
            _Institutions.Add(instit);
            instit.ModifyAmountEventHandler += this.UpdateTotalAmount;
        }

        public void AddInstitution(Institution instit)
        {
            _Institutions.Add(instit);
            instit.ModifyAmountEventHandler += this.UpdateTotalAmount;
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
            newInstit.ModifyAmountEventHandler += this.UpdateTotalAmount;
            return newInstit;
        }

        public void AddAccount(string name, string institutionName)
        {
            Institution instit = GetInstitution(institutionName);
            instit.AddAccount(name, instit.Ccy);
        }

        public bool ChangeName(string before, string after, NodeAddress nodeTag)
        {
            bool test = false;
            if (nodeTag.NodeType == NodeType.Institution)
            {
                if (InstitutionsDictionary.ContainsKey(before) && !InstitutionsDictionary.ContainsKey(after))
                {
                    Institution instit_before = GetInstitution(before);
                    instit_before.InstitutionName = after;
                    test = true;
                }
            }
            else
            {
                test = GetInstitution(nodeTag.Address[1]).ChangeName(before, after, nodeTag);
            }
            return test;
        }

        public event EventHandler<ModifyAmountEventArgs> ModifyAmountEventHandler;

        public void UpdateTotalAmount(object sender, ModifyAmountEventArgs e)
        {
            _TotalAmount = 0;
            foreach (Institution item in Institutions)
            {
                _TotalAmount += item.TotalAmount;
            }
            ModifyAmountEventHandler?.Invoke(this, e);
        }

        internal void RefreshTotalAmount(FXMarket fXMarket, AssetMarket assetMarket)
        {
            _TotalAmount = 0;
            foreach (Institution item in Institutions)
            {
                item.RefreshTotalAmount(fXMarket, assetMarket);
                _TotalAmount += item.TotalAmount;
            }
        }
        
        internal void PrepareForLoading(Currency ccy, FXMarket fXMarket, AssetMarket assetMarket)
        {
            _TotalCcy = ccy;
            _TotalAmount = 0;
            foreach (Institution item in Institutions)
            {
                item.PrepareForLoading(_TotalCcy, fXMarket, assetMarket);
                item.ModifyAmountEventHandler += this.UpdateTotalAmount;
                _TotalAmount += item.TotalAmount;
            }
        }

        internal void ReorgItems(IEnumerable<string> enumerable)
        {
            List<Institution> res = new List<Institution> { };
            foreach (string item in enumerable)
            {
                res.Add(GetInstitution(item).Copy());
            }
            _Institutions = res;
        }

        public Category Copy()
        {
            Category res = new Category(_CategoryName, (Currency)_Ccy.Clone());
            foreach (var item in _Institutions)
            {
                res.AddInstitution(item.Copy());
            }
            return res;
        }
    }
}
