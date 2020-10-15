using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core
{

    public class AccountingData : IAccountingData, IEnumerator<Category>
    {
        Dictionary<string, Category> _Data = new Dictionary<string, Category> { };
        int position = 0;

        Category IEnumerator<Category>.Current => throw new NotImplementedException();

        object IEnumerator.Current => throw new NotImplementedException();

        public AccountingData(List<Category> input)
        {
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

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        bool IEnumerator.MoveNext()
        {
            position++;
            return position < _Data.Count;
        }

        void IEnumerator.Reset()
        {
            position = -1;
        }

        #endregion
    }
}
