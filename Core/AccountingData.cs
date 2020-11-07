using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Finance;
using Core.Interfaces;

namespace Core
{

    public class AccountingData : IAccountingData
    {
        Currency _Ccy;
        Dictionary<string, Category> _Data = new Dictionary<string, Category> { };
        Market _Market;
        TreeViewMapping _Map;

        public Currency Ccy
        {
            get { return _Ccy; }
            set { _Ccy = value; }
        }

        private Category GetCategory(string catName)
        {
            return _Data[catName];
        }

        private Institution GetInstitution(string catName, string institName)
        {
            return GetCategory(catName).GetInstitution(institName);
        }

        #region IAccountingData

        public TreeViewMapping Map { get { return _Map; } }

        public Market Market { get { return _Market; } }

        public void ModifyCcy(object valueCcy)
        {
            _Ccy = CurrencyFunctions.ToCurrency(valueCcy);
        }

        public ICategory GetFirstCategory()
        {
            return _Data[_Data.Keys.First().ToString()];
        }

        public IEnumerable<ICategory> Categories
        {
            get
            {
                IEnumerable<string> nameList = _Map.GetList(new NodeAddress(NodeType.All, ""));
                return nameList.Select(x => _Data[x]);
            }
        }

        public IAccount Total()
        {
            double total = 0;
            foreach (var item in _Data)
                total += item.Value.TotalInstitution(_Market, Ccy).ConvertedAmount;
            return new Account("Total", Ccy, total);
        }

        public bool ChangeName(string before, string after, NodeAddress nodeTag)
        {
            bool test = false;
            if (nodeTag.NodeType == NodeType.Category)
            {
                if (!_Data.ContainsKey(after))
                {
                    _Data[after] = _Data[before];
                    _Data.Remove(before);
                    _Data[after].CategoryName = after;
                    test = true;
                }
            }
            else
            {
                test = _Data[nodeTag.Address[0]].ChangeName(before, after, nodeTag);
            }
            if (test)
                _Map.ChangeName(nodeTag, after);
            return test;
        }

        #endregion

        public AccountingData(List<Category> input, Market mkt)
        {
            _Ccy = Currency.USD;
            _Data = input.ToDictionary(x => x.CategoryName, x => x);
            _Market = mkt;
            _Map = new TreeViewMapping(_Data);
        }

        public IAccountingElement GetInstitution(NodeAddress na)
        {
            return GetInstitution(na.Address[0], na.Address[1]);
        }

        public Category GetCategory(NodeAddress na)
        {
            return GetCategory(na.Address[0]);
        }

        public void Reset()
        {
            _Data = new Dictionary<string, Category> { };
            Map.Reset();
            AddItem(new NodeAddress(NodeType.Category, "TEMP"));
        }

        public IAccountingElement GetElement(NodeAddress na)
        {
            switch (na.NodeType)
            {
                case NodeType.All:
                    break;
                case NodeType.Category:
                    return GetCategory(na);
                case NodeType.Institution:
                    return GetInstitution(na);
                case NodeType.Account:
                    break;
                default:
                    break;
            }
            throw new Exception("ERROR");
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

        public NodeAddress DeleteItem(NodeAddress na)
        {
            NodeAddress res;
            if (na.NodeType == NodeType.Category)
            {
                if (_Data.Count > 1)
                {
                    _Data.Remove(na.Address[0]);
                    res = _Map.DeleteNode(na);
                }
                else
                    res = na;
            }
            else
            {
                GetElement(na.GetParent()).Delete(na.GetLast());
                res= _Map.DeleteNode(na);
            }
            if (res.IsEqual(na))
                MessageBox.Show($"You cannot delete {na.GetLast()} as it is the last element of [{na.GetParent().GetLabelText()}]!");
            return res;
        }
    }
}
