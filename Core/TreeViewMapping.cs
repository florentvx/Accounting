using Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class TreeViewMappingElement : IEnumerable<TreeViewMappingElement>
    {
        public string Name;
        public List<TreeViewMappingElement> Nodes;

        public TreeViewMappingElement(string name)
        {             
            Name = name;
            Nodes = null;
        }

        public TreeViewMappingElement AddElement(string name)
        {
            if (Nodes == null)
                Nodes = new List<TreeViewMappingElement> { };
            TreeViewMappingElement newElmt = new TreeViewMappingElement(name);
            Nodes.Add(newElmt);
            return newElmt;
        }

        public void AddElement(TreeViewMappingElement elmt)
        {
            if (Nodes == null)
                Nodes = new List<TreeViewMappingElement> { };
            Nodes.Add(elmt);
        }

        public IEnumerator<TreeViewMappingElement> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        internal TreeViewMappingElement GetElement(string name)
        {
            if (Nodes == null)
                return null;
            foreach (var item in Nodes)
            {
                if (name == item.Name)
                    return item;
            }
            return null;
        }

        public static TreeViewMappingElement CreateElement(Account newInst)
        {
            var res = new TreeViewMappingElement(newInst.AccountName);
            return res;
        }

        public static TreeViewMappingElement CreateElement(Institution newInst)
        {
            var res = new TreeViewMappingElement(newInst.InstitutionName);
            foreach (Account item in newInst.Accounts)
            {
                res.AddElement(CreateElement(item));
            }
            return res;
        }

        public static TreeViewMappingElement CreateElement(Category newCat)
        {
            var res = new TreeViewMappingElement(newCat.CategoryName);
            foreach (Institution item in newCat.Institutions)
            {
                res.AddElement(CreateElement(item));
            }
            return res;
        }

        internal TreeViewMappingElement AddElement(string stringRef, TreeViewMappingElement elmt)
        {
            TreeViewMappingElement last;
            int i = 0;
            while (Nodes[i].Name != stringRef)
                i++;
            i++;
            if (i == Nodes.Count)
            {
                last = elmt;
            }
            else
            {
                last = Nodes[i];
                Nodes[i] = elmt;
                for (int j = i + 1; j < Nodes.Count; j++)
                {
                    var temp = Nodes[j];
                    Nodes[j] = last;
                    last = temp;
                }
            }
            Nodes.Add(last);
            return Nodes[i];
        }


    }

    public class TreeViewMapping : IEnumerable<TreeViewMappingElement>
    {
        TreeViewMappingElement Map;

        public IEnumerator<TreeViewMappingElement> GetEnumerator()
        {
            return Map.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Map.GetEnumerator();
        }

        public TreeViewMapping(Dictionary<string, Category> data)
        {
            Map = new TreeViewMappingElement("Root");
            foreach (var itemC in data)
            {
                TreeViewMappingElement Map2 = Map.AddElement(itemC.Key);
                foreach (var itemI in data[itemC.Key].Institutions)
                {
                    TreeViewMappingElement Map3 = Map2.AddElement(itemI.InstitutionName);
                    foreach (var itemA in itemI.Accounts)
                        Map3.AddElement(itemA.AccountName);
                }
            }
        }

        public TreeViewMappingElement GetElement(NodeAddress na)
        {
            switch (na.NodeType)
            {
                case NodeType.All:
                    return Map;
                case NodeType.Category:
                    return Map  .GetElement(na.Address[0]);
                case NodeType.Institution:
                    return Map  .GetElement(na.Address[0]).GetElement(na.Address[1]);
                case NodeType.Account:
                    return Map  .GetElement(na.Address[0])
                                .GetElement(na.Address[1])
                                .GetElement(na.Address[2]);
                default:
                    return null;
            }
        }

        internal void ChangeName(NodeAddress nodeTag, string after)
        {
            GetElement(nodeTag).Name = after;
        }

        internal void AddItem(NodeAddress nodeAddress, Account newAcc)
        {
            TreeViewMappingElement elmt = GetElement(nodeAddress.GetParent());
            TreeViewMappingElement newElmt = elmt.AddElement(nodeAddress.Address.Last(), TreeViewMappingElement.CreateElement(newAcc));
        }

        internal void AddItem(NodeAddress nodeAddress, Institution newInstit)
        {
            TreeViewMappingElement elmt = GetElement(nodeAddress.GetParent());
            TreeViewMappingElement newElmt = elmt.AddElement(nodeAddress.Address.Last(), TreeViewMappingElement.CreateElement(newInstit));
        }

        internal void AddItem(NodeAddress nodeAddress, Category newCat)
        {
            TreeViewMappingElement elmt = GetElement(nodeAddress.GetParent());
            TreeViewMappingElement newElmt = elmt.AddElement(nodeAddress.Address.Last(), TreeViewMappingElement.CreateElement(newCat));
        }

        internal IEnumerable<string> GetList(NodeAddress nodeAddress)
        {
            return GetElement(nodeAddress).Nodes.Select(x => x.Name);
        }
    }
}
