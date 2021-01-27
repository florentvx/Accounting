using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class Tools
    {
        public static bool CompareList<X>(List<X> list1, List<X> list2)
            where X: IEquatable<X>
        {
            if (list1.Count != list2.Count)
                return false;
            for (int i = 0; i < list1.Count; i++)
            {
                X elmt1_i = list1[i];
                X elmt2_i = list2[i];
                if (!elmt1_i.Equals(elmt2_i))
                    return false;
            }
            return true;
        }
        public static bool CompareDictionary<X, Y>(Dictionary<X, Y> dict1, Dictionary<X, Y> dict2)
            where  X: IEquatable<X>
            where Y: IEquatable<Y>
        {
            if (dict1.Keys.Count != dict2.Keys.Count)
                return false;
            foreach (X key in dict1.Keys)
            {
                if (!dict2.ContainsKey(key))
                    return false;
                Y y1 = dict1[key];
                Y y2 = dict2[key];
                if (!y1.Equals(y2))
                    return false;
            }
            return true;
        }
    }
}
