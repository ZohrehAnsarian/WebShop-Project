using System;
using System.IO;
using System.Reflection;

namespace CyberneticCode.Web.Mvc.Helpers
{


    public class ReflectionCall
    {
        string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
        string className { get; set; }
        string _methodName { get; set; }
        object _instance { get; set; }
        MethodInfo _method { get; set; }
        Type[] _parameterTypes { get; set; }
        Type _assemblyType { get; set; }
        object[] _parameterValues { get; set; }
        public ReflectionCall(string assemblyName, string typeName, string methodName, Type[] parameterTypes, object[] parameterValues)
        {
            _methodName = _methodName;
            _parameterTypes = parameterTypes;
            _parameterValues = parameterValues;

            var assemblyPath = AssemblyDirectory + "\\" + assemblyName;
            var assembly = Assembly.LoadFile(assemblyPath);
            _assemblyType = assembly.GetType(typeName);
            _instance = Activator.CreateInstance(_assemblyType);

        }
        public T Invoke<T>()
        {
            MethodInfo method = _assemblyType.GetMethod(_methodName, _parameterTypes);
            T result = (T)method.Invoke(_instance, _parameterValues);
            return result;
        }
    }
}
