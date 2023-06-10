using System;
using System.Collections.Generic;

namespace Sorting
{
    public class LineComparer : IComparer<string>
    {
        public int Compare(string x = "", string y = "")
        {
            int dotIndexX = x.IndexOf('.');
            int dotIndexY = y.IndexOf('.');

            string strX = x.Substring(dotIndexX + 2);
            string strY = y.Substring(dotIndexY + 2);

            int result = string.Compare(strX, strY, StringComparison.Ordinal);
            if (result == 0)
            {
                int numberX = int.Parse(x.Substring(0, dotIndexX));
                int numberY = int.Parse(y.Substring(0, dotIndexY));
                result = numberX.CompareTo(numberY);
            }

            return result;
        }
    }
}
