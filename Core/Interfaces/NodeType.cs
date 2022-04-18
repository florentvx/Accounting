using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public enum NodeType
    {
        All, Category, Institution, Account
    }

    static class NodeTypeProperties
    {
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
        public NodeType _NodeType;
        public List<string> Address;

        public NodeType NodeType { get { return _NodeType; } }

        public string Path { 
            get 
            {
                string res = "";
                foreach (string item in Address)
                {
                    res += item + Separator;
                }
                if (res.Length == 0)
                    return res;
                return res.Substring(0, res.Length - 1);
            } 
        }

        public NodeAddress(NodeType nt, string path)
        {
            _NodeType = nt;
            Address = new List<string> { };
            if (path != null && path != "")
            {
                var res = path.Split(Separator);
                for (int i = 0; i < res.Length; i++)
                    Address.Add(res[i]);
            }   
        }

        public void ChangeAddress(string label)
        {
            Address[Address.Count() - 1] = label;
        }

        public string GetLabelText()
        {
            if (NodeType == NodeType.All)
                return "Root";
            NodeType nt = NodeType;
            if (_NodeType == NodeType.Account)
                nt = NodeType.Institution;
            string res = Enum.GetName(typeof(NodeType), nt) + " : ";
            res += Address[0];
            if (nt == NodeType.Institution)
                res += $" -> {Address[1]}";
            return res;
        }

        public NodeAddress GetParent()
        {
            switch (NodeType)
            {
                case NodeType.All:
                    return null;
                case NodeType.Category:
                    return new NodeAddress(NodeType.All, null);
                case NodeType.Institution:
                    return new NodeAddress(NodeType.Category, Address[0]);
                case NodeType.Account:
                    return new NodeAddress(NodeType.Institution, Address[0] + Separator + Address[1]);
                default:
                    return null;
            }
        }

        public string GetLast()
        {
            return Address.Last();
        }

        public string GetLabel(NodeType nt)
        {
            if (NodeType == nt)
                return GetLast();
            switch (NodeType)
            {
                case NodeType.Institution:
                    if (nt == NodeType.Category)
                        return GetParent().GetLast();
                    break;
                case NodeType.Account:
                    if (nt == NodeType.Category)
                        return GetParent().GetParent().GetLast();
                    if (nt == NodeType.Institution)
                        return GetParent().GetLast();
                    break;
            }
            return null;
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
            _NodeType = NodeType.GetNext();
            Address.Add(v);
        }

        public object Clone()
        {
            return new NodeAddress(_NodeType, (string)Path.Clone());
        }
    }
}
