using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Helpers
{
    static public class ConvertHelper
    {
        static public long ConvertStringToLong(string value)
        {
            try
            {
                var returnValue = Convert.ToInt64(value);
                return returnValue;
            }
            catch
            {
                throw new InvalidCastException(String.Format("Unable to parse {0}.", value));
            }
        }
    }
}
