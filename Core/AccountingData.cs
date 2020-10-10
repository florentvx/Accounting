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

        public void Reset()
        {
            _Data = new Dictionary<string, Category> { };
            Category cat = new Category("Category");
            cat.AddInstitution("Institution");
            cat.AddAccount("Account", "Institution");
            _Data.Add(cat.CategoryName, cat);
        }

        public void ChangeName(string before, string after, NodeType nodeTag)
        {
            // TODO: Check before that the new Name does not already exist
            if (nodeTag == NodeType.Category)
            {
                _Data[after] = _Data[before];
                _Data.Remove(before);
            }
            else
            {
                foreach (Category item in _Data.Values)
                {
                    if (item.ChangeName(before, after, nodeTag))
                        break;
                }
            }
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
