using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public enum NodeType
    {
        Category, Institution, Account
    }

    public class NodeAddress
    {
        public static char Separator = '/';
        public NodeType NodeType;
        public List<string> Address;

        public NodeAddress(NodeType nt, string path)
        {
            NodeType = nt;
            Address = new List<string> { };
            var res = path.Split(Separator);
            for (int i = 0; i < res.Length; i++)
                Address.Add(res[i]);
        }

        public void ChangeAddress(string label)
        {
            Address[Address.Count() - 1] = label;
        }
    }
}
