using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core
{

    public class AccountingData : IAccountingData
    {
        Currency _Ccy;
        Dictionary<string, Category> _Data = new Dictionary<string, Category> { };

        public Currency Ccy
        {
            get { return _Ccy; }
            set { _Ccy = value; }
        }

        public void ModifyCcy(object valueCcy)
        {
            _Ccy = CurrencyFunctions.ToCurrency(valueCcy);
        }

        public IEnumerable<ICategory> Categories
        {
            get { return _Data.Values.ToList<ICategory>(); }
        }

        public AccountingData(List<Category> input)
        {
            _Ccy = Currency.USD;
            _Data = input.ToDictionary(x => x.CategoryName, x => x);
        }

        public Dictionary<string, Dictionary<string, List<string>>> GetSummary()
        {
            return _Data.ToDictionary(x => x.Key, x => x.Value.GetCategorySummary());
        }

        public Category GetCategory(string catName)
        {
            return _Data[catName];
        }

        public IInstitution GetInstitution(string catName, string institName)
        {
            return _Data[catName].GetInstitution(institName);
        }

        public ICategory GetFirstCategory()
        {
            return _Data[_Data.Keys.First().ToString()];
        }

        public IAccount Total()
        {
            double total = 0;
            foreach (var item in _Data)
                total += item.Value.TotalInstitution().Amount;
            return new Account("Total", Ccy, total);
        }

        public string AddNewCategory()
        {
            int i = 0;
            string newNameRef = "New Category";
            string newName = newNameRef;
            while (_Data.ContainsKey(newName))
            {
                i++;
                newName = $"{newNameRef} - {i}";
            }
            Category cat = new Category(newName);
            cat.AddInstitution("New Institution");
            cat.AddAccount("New Account", "New Institution");
            _Data.Add(cat.CategoryName, cat);
            return newName;
        }

        public void Reset()
        {
            _Data = new Dictionary<string, Category> { };
            AddNewCategory();
            
        }

        public void ChangeName(string before, string after, NodeAddress nodeTag)
        {
            if (nodeTag.NodeType == NodeType.Category)
            {
                if (!_Data.ContainsKey(after))
                {
                    _Data[after] = _Data[before];
                    _Data.Remove(before);
                }
            }
            else
            {
                _Data[nodeTag.Address[0]].ChangeName(before, after, nodeTag);
            }
        }

        public NodeAddress AddItem(NodeAddress nodeAddress)
        {
            if (nodeAddress.NodeType == NodeType.Category)
            {
                string newName = AddNewCategory();
                return new NodeAddress(NodeType.Category, newName);
            }
            else
                return _Data[nodeAddress.Address[0]].AddItem(nodeAddress);
        }

        #region IEnumerable

        public IEnumerator<Category> GetEnumerator()
        {
            return _Data.Values.GetEnumerator();
        }

        #endregion
    }
}
