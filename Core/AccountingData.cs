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

        public Institution GetInstitution(string catName, string institName)
        {
            return _Data[catName].GetInstitution(institName);
        }

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
    }
}
