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

    public class NodeAddress
    {
        public static char Separator = '/';
        public NodeType _NodeType;
        public List<string> Address;

        public NodeType NodeType { get { return _NodeType; } }

        public NodeAddress(NodeType nt, string path)
        {
            _NodeType = nt;
            Address = new List<string> { };
            var res = path.Split(Separator);
            for (int i = 0; i < res.Length; i++)
                Address.Add(res[i]);
        }

        public void ChangeAddress(string label)
        {
            Address[Address.Count() - 1] = label;
        }

        public string GetLabelText()
        {
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
                    return new NodeAddress(NodeType.All, "");
                case NodeType.Institution:
                    return new NodeAddress(NodeType.Category, Address[0]);
                case NodeType.Account:
                    return new NodeAddress(NodeType.Institution, Address[0] + Separator + Address[1]);
                default:
                    return null;
            }
        }
    }
}
