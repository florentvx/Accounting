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
        public bool Expand; 

        #region IEnumerable

        public IEnumerator<TreeViewMappingElement> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        #endregion

        public TreeViewMappingElement(string name)
        {             
            Name = name;
            Nodes = null;
            Expand = false;
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

        internal void AddElement(TreeViewMappingElement elmt)
        {
            if (Nodes == null)
                Nodes = new List<TreeViewMappingElement> { };
            Nodes.Add(elmt);
        }

        internal TreeViewMappingElement AddElement(string name)
        {
            TreeViewMappingElement newElmt = new TreeViewMappingElement(name);
            AddElement(newElmt);
            return newElmt;
        }

        internal TreeViewMappingElement AddElement(string stringRef, TreeViewMappingElement elmt)
        {
            if (Nodes == null)
            {
                AddElement(elmt);
                return elmt;
            }
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

        public static TreeViewMappingElement CreateElement(IAccountingElement iNewElmt)
        {
            var res = new TreeViewMappingElement(iNewElmt.GetName());
            foreach (IAccountingElement item in iNewElmt.GetItemList())
            {
                res.AddElement(CreateElement(item));
            }
            return res;
        }
    }

    public class TreeViewMapping : IEnumerable<TreeViewMappingElement>
    {
        TreeViewMappingElement Map;

        #region IEnumerable

        public IEnumerator<TreeViewMappingElement> GetEnumerator()
        {
            return Map.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Map.GetEnumerator();
        }

        #endregion

        internal void Reset()
        {
            Map = new TreeViewMappingElement("Root");
        }

        public TreeViewMapping(Dictionary<string, Category> data)
        {
            Reset();
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

        internal IEnumerable<string> GetList(NodeAddress nodeAddress)
        {
            return GetElement(nodeAddress).Nodes.Select(x => x.Name);
        }

        internal void ChangeName(NodeAddress nodeTag, string after)
        {
            GetElement(nodeTag).Name = after;
        }

        internal void AddItem(NodeAddress nodeAddress, IAccountingElement iNewAcc)
        {
            TreeViewMappingElement elmt = GetElement(nodeAddress.GetParent());
            TreeViewMappingElement newElmt = elmt.AddElement(nodeAddress.Address.Last(), TreeViewMappingElement.CreateElement(iNewAcc));
        }

    }
}
