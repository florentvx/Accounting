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
        TreeViewMapping _Map;

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
            get
            {
                IEnumerable<string> nameList = _Map.GetList(new NodeAddress(NodeType.All, ""));
                return nameList.Select(x => _Data[x]);
            }
        }

        public TreeViewMapping Map { get { return _Map; } }

        public AccountingData(List<Category> input)
        {
            _Ccy = Currency.USD;
            _Data = input.ToDictionary(x => x.CategoryName, x => x);
            _Map = new TreeViewMapping(_Data);
        }

        public Dictionary<string, Dictionary<string, List<string>>> GetSummary()
        {
            return _Data.ToDictionary(x => x.Key, x => x.Value.GetCategorySummary());
        }

        public Category GetCategory(string catName)
        {
            return _Data[catName];
        }

        public Category GetCategory(NodeAddress na)
        {
            return GetCategory(na.Address[0]);
        }

        public IInstitution GetInstitution(string catName, string institName)
        {
            return GetCategory(catName).GetInstitution(institName);
        }

        public IInstitution GetInstitution(NodeAddress na)
        {
            return GetInstitution(na.Address[0], na.Address[1]);
        }

        public ICategory GetFirstCategory()
        {
            return _Data[_Data.Keys.First().ToString()];
        }

        public ICategory GetElement(NodeAddress na)
        {
            switch (na.NodeType)
            {
                case NodeType.All:
                    break;
                case NodeType.Category:
                    GetCategory(na.Address[0]);
                    break;
                case NodeType.Institution:
                    GetInstitution(na.Address[0], na.Address[1]);
                    break;
                case NodeType.Account:
                    break;
                default:
                    break;
            }
            throw new Exception("ERROR");
        }

        public IAccount Total()
        {
            double total = 0;
            foreach (var item in _Data)
                total += item.Value.TotalInstitution().Amount;
            return new Account("Total", Ccy, total);
        }

        public Category AddNewCategory()
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
            return cat;
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
                    _Data[after].CategoryName = after;
                }
            }
            else
            {
                _Data[nodeTag.Address[0]].ChangeName(before, after, nodeTag);
            }
            _Map.ChangeName(nodeTag, after);
        }

        public NodeAddress AddItem(NodeAddress nodeAddress)
        {
            switch (nodeAddress.NodeType)
            {
                case NodeType.All:
                    break;
                case NodeType.Category:
                    Category newCat = AddNewCategory();
                    Map.AddItem(nodeAddress, newCat);
                    nodeAddress.ChangeAddress(newCat.CategoryName);
                    return nodeAddress;
                case NodeType.Institution:
                    Institution newInstit = _Data[nodeAddress.Address[0]].AddNewInstitution();
                    Map.AddItem(nodeAddress, newInstit);
                    nodeAddress.ChangeAddress(newInstit.InstitutionName);
                    return nodeAddress;
                case NodeType.Account:
                    Account newAccount = GetCategory(nodeAddress.Address[0]).GetInstitution(nodeAddress.Address[1]).AddNewAccount();
                    Map.AddItem(nodeAddress, newAccount);
                    nodeAddress.ChangeAddress(newAccount.AccountName);
                    return nodeAddress;
                default:
                    break;
            }
            throw new Exception("Issue");
        }

        #region IEnumerable

        public IEnumerator<Category> GetEnumerator()
        {
            return _Data.Values.GetEnumerator();
        }

        #endregion
    }
}
