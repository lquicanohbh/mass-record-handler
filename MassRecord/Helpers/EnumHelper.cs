
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MassRecord.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<ValueName> GetItems<TEnum>(List<TEnum> FilteredList)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an Enumeration type.");
            var res = from e in FilteredList
                      select new ValueName()
                      {
                          Value = Convert.ToInt32(e),
                          Name = e.ToString()
                      };
            return res;
        }
    }
}
