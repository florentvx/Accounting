using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public enum NodeType
    {
        Account = 4,
        Institution = 3,
        Category = 2,
        All = 1,
    }

    static class NodeTypeProperties
    {
        public static NodeType GetNodeType(int level)
        {
            foreach (NodeType nt in Enum.GetValues(typeof(NodeType)))
            if ((int) nt == level) return nt;
            throw new NotSupportedException($"Level not recognised: {level}");
        }

        public static NodeType GetNext(this NodeType nt)
        {
            switch (nt)
            {
                case NodeType.All:
                    return NodeType.Category;
                case NodeType.Category:
                    return NodeType.Institution;
                case NodeType.Institution:
                    return NodeType.Account;
                case NodeType.Account:
                default:
                    throw new Exception("Impossible");
            }
        }
    }

    public class NodeAddress: ICloneable
    {
        public static char Separator = '\\';
        public List<string> Address;

        public NodeType NodeType { get { return NodeTypeProperties.GetNodeType(Address.Count()); } }

        public string Path { 
            get 
            {
                string res = "";
                foreach (string item in Address)
                {
                    res += item + Separator;
                }
                if (res.Length == 0)
                    throw new NotSupportedException($"Path cannot be null");
                return res.Substring(0, res.Length - 1);
            } 
        }

        public NodeAddress(string path)
        {
            Address = new List<string> { };
            if (path != null && path != "")
            {
                var res = path.Split(Separator);
                for (int i = 0; i < res.Length; i++)
                    Address.Add(res[i]);
            }   
        }

        internal NodeAddress(IEnumerable<string> list)
        {
            Address = list.ToList();
        }

        public void ChangeAddress(string label)
        {
            Address[Address.Count() - 1] = label;
        }

        public string GetLabelText()
        {
            NodeType used_nt = NodeType;
            if (NodeType == NodeType.All)
                return "Root";
            else if (NodeType == NodeType.Account)
                used_nt = NodeType.Institution;
            string res = Enum.GetName(typeof(NodeType), used_nt) + " : ";
            res += Address[1];
            if (used_nt == NodeType.Institution)
                res += $" -> {Address[1]}";
            return res;
        }

        public NodeAddress GetParent()
        {
            return new NodeAddress(
                Address .Select((value, index) => new Tuple<int, string>(index, value))
                        .Where(x => x.Item1 < (int)NodeType - 1)
                        .Select(x => x.Item2)
            );
        }

        public string GetLast()
        {
            return Address.Last();
        }

        public string GetLabel(NodeType nt)
        {
            if ((int)NodeType < (int)nt) return null;
            else if ((int)NodeType == (int)nt)
                return GetLast();
            else
                return Address[Address.Count - ((int)NodeType - (int)nt) - 1];
        }

        internal bool IsEqual(NodeAddress na)
        {
            if (NodeType == na.NodeType && Address.Count == na.Address.Count)
            {
                bool test = true;
                for (int i = 0; i < na.Address.Count; i++)
                    test = Address[i] == na.Address[i];
                return test;
            }
            return false;
        }

        internal void AddLast(string v)
        {
            Address.Add(v);
        }

        public object Clone()
        {
            return new NodeAddress((string)Path.Clone());
        }
    }
}
