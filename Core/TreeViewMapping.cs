using Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [Serializable]
    public class TreeViewMappingElement : IEquatable<TreeViewMappingElement>, ISerializable
    {
        [JsonProperty]
        public string Name;

        [JsonProperty]
        public List<TreeViewMappingElement> Nodes;

        public bool Expand; 

        public int Count { get { return 1 + Nodes.Select(x => x.Count).Sum(); } }

        #region IEnumerable

        public IEnumerator<TreeViewMappingElement> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return Nodes.GetEnumerator();
        //}

        #endregion

        #region IEquatable

        public bool Equals(TreeViewMappingElement tvme)
        {
            if (tvme == null)
                return false;
            if( Name == tvme.Name
                && Expand == tvme.Expand)
            {
                if (Nodes == null)
                {
                    if (tvme.Nodes == null)
                        return true;
                    return false;
                }
                else
                {
                    if (Nodes.Count != tvme.Nodes.Count)
                        return false;
                    for (int i = 0; i < Nodes.Count; i++)
                    {
                        if (Nodes[i] != tvme.Nodes[i])
                            return false;
                    }
                    return true;
                }
                
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as TreeViewMappingElement);
        }

        public override int GetHashCode()
        {
            int res = Name.GetHashCode() + Expand.GetHashCode();
            foreach (var item in Nodes)
                res += item.GetHashCode();
            return res;
        }

        public static bool operator ==(TreeViewMappingElement tvme1, TreeViewMappingElement tvme2)
        {
            if (tvme1 is null)
            {
                if (tvme2 is null) { return true; }
                return false;
            }
            return tvme1.Equals(tvme2);
        }

        public static bool operator !=(TreeViewMappingElement tvme1, TreeViewMappingElement tvme2)
        {
            return !(tvme1 == tvme2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("Nodes", Nodes, typeof(List<TreeViewMappingElement>));
        }

        public TreeViewMappingElement(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            Nodes = (List<TreeViewMappingElement>)info.GetValue("Nodes", typeof(List<TreeViewMappingElement>));
            Expand = false;
        }

        #endregion

        public TreeViewMappingElement(string name)
        {             
            Name = name;
            Nodes = new List<TreeViewMappingElement> { };
            Expand = false;
        }

        internal TreeViewMappingElement GetElement(string name)
        {
            if (Nodes == null)
                return null;
            return Nodes.Where(x => x.Name == name).First();
        }

        internal int GetElementIndex(string name)
        {
            return Nodes.FindIndex(x => x.Name == name);
        }

        public void AddElement(TreeViewMappingElement elmt)
        {
            if (Nodes == null)
                throw new InvalidProgramException("Nodes should not be null");
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
                throw new InvalidProgramException("Nodes should not be null");
            
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
            throw new NotImplementedException();
            //var res = new TreeViewMappingElement(iNewElmt.GetName());
            //foreach (IAccountingElement item in iNewElmt.GetTreeStructure())
            //{
            //    res.AddElement(CreateElement(item));
            //}
            //return res;
        }

        internal string Delete(string v)
        {
            if (Nodes.Count > 1)
            {
                Nodes.Remove(GetElement(v));
                return Nodes[0].Name;
            }
            return v;
        }

        internal void MoveNode(string v1, string v2)
        {
            int tvme1_i = GetElementIndex(v1);
            int tvme2_i = GetElementIndex(v2);
            TreeViewMappingElement memo = Nodes[tvme1_i];
            if (tvme1_i < tvme2_i)
            {
                for (int i = tvme1_i; i < tvme2_i; i++)
                {
                    Nodes[i] = Nodes[i + 1];
                }
                Nodes[tvme2_i] = memo;
            }
            else
            {
                for (int i = tvme1_i; i > tvme2_i; i--)
                {
                    Nodes[i] = Nodes[i - 1];
                }
                Nodes[tvme2_i + 1] = memo;
            }
        }

        internal void MoveNode(string v1)
        {
            TreeViewMappingElement memo = Nodes[0];
            int tvme_1 = GetElementIndex(v1);
            Nodes[0] = Nodes[tvme_1];
            for (int i = 0; i < tvme_1; i++)
            {
                var temp = Nodes[i + 1];
                Nodes[i + 1] = memo;
                memo = temp;
            }
        }

        public override string ToString()
        {
            string res = Name + "\n";
            foreach (var item in Nodes)
            {
                res += $"--> {item}";
            }
            return res;
        }

        public string Aux_ToString(int level = 0)
        {
            string res = Name + "\n";
            foreach (var item in Nodes)
            {
                res += $"{String.Join("-", new string[level])}> {item.Aux_ToString(level+1)}";
            }
            return res;
        }
    }

    [Serializable]
    public class TreeViewMapping : IEquatable<TreeViewMapping>, ISerializable
    {
        [JsonProperty]
        TreeViewMappingElement Map;

        #region IEnumerable

        public IEnumerator<TreeViewMappingElement> GetEnumerator()
        {
            return Map.GetEnumerator();
        }

        #endregion

        #region IEquatable

        public bool Equals(TreeViewMapping tvm)
        {
            if (tvm == null)
                return false;
            return Map == tvm.Map;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as TreeViewMapping);
        }

        public override int GetHashCode()
        {
            return Map.GetHashCode();
        }

        public static bool operator ==(TreeViewMapping tvm1, TreeViewMapping tvm2)
        {
            if (tvm1 is null)
            {
                if (tvm2 is null) { return true; }
                return false;
            }
            return tvm1.Equals(tvm2);
        }

        public static bool operator !=(TreeViewMapping tvm1, TreeViewMapping tvm2)
        {
            return !(tvm1 == tvm2);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Map", Map, typeof(TreeViewMappingElement));
        }

        public TreeViewMapping(SerializationInfo info, StreamingContext context)
        {
            Map = (TreeViewMappingElement)info.GetValue("Map", typeof(TreeViewMappingElement));
        }

        #endregion

        public int Count { get { return Map.Count - 1; } }

        public TreeViewMapping() { Map = new TreeViewMappingElement("Root"); }

        public TreeViewMapping(List<Category> data)
        {
            Map = new TreeViewMappingElement("Root");
            foreach (var itemC in data)
            {
                TreeViewMappingElement Map2 = Map.AddElement(itemC.CategoryName);
                foreach (var itemI in itemC.Institutions)
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
                    return Map  .GetElement(na.Address[0])
                                .GetElement(na.Address[1]);
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

        public IEnumerable<string> GetSubNodes(NodeAddress na)
        {
            var subElmt = GetElement(na);
            return subElmt.Nodes.Select(x => x.Name);
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

        internal void AddItem_Simple(string item_name)
        {
            Map.AddElement(item_name);
        }

        internal void AddItem_Simple(TreeViewMapping tvm, string sub_node = null)
        {
            
            if (sub_node == null)
                foreach (TreeViewMappingElement tvme in tvm.Map.Nodes) { Map.AddElement(tvme); }
            else
                foreach (TreeViewMappingElement tvme in tvm.Map.Nodes) { Map.GetElement(sub_node).AddElement(tvme); }
        }

        internal NodeAddress DeleteNode(NodeAddress na)
        {
            NodeAddress refNode = na.GetParent();
            TreeViewMappingElement tvme = GetElement(refNode);
            refNode.AddLast(tvme.Delete(na.GetLast()));
            return refNode;
        }

        public void MoveNode(NodeAddress draggedNode, NodeAddress refNode)
        {
            NodeAddress parent = draggedNode.GetParent();
            if (parent.IsEqual(refNode.GetParent()))
            {
                TreeViewMappingElement tvme = GetElement(parent);
                tvme.MoveNode(draggedNode.GetLast(), refNode.GetLast());
            }
            if (parent.IsEqual(refNode))
            {
                TreeViewMappingElement tvme = GetElement(parent);
                tvme.MoveNode(draggedNode.GetLast());
            }
        }

        public override string ToString()
        {
            return Map.Aux_ToString(1);
        }
    }
}
