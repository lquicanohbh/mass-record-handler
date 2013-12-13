using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassRecord.Helpers
{
    public static class ClassHelper
    {
        public static void InvokeMethod(string ClassName, string MethodName)
        {
            Type myType = Type.GetType(ClassName);
            MethodInfo method = myType.GetMethod(MethodName);
            method.Invoke(null, null);
        }
    }
}
