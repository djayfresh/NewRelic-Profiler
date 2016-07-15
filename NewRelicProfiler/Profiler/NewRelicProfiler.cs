namespace NewRelicProfiler.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class NewRelicProfiler
    {
        private List<MethodInfo> _methods;
        private List<string> _loadedAssemblies;

        public NewRelicProfiler()
        {
            _methods = new List<MethodInfo>();
            _loadedAssemblies = new List<string>();
        }

        public void Load(Assembly a)
        {
            if (_loadedAssemblies.Contains(a.GetName().Name))
                return;

            _loadedAssemblies.Add(a.GetName().Name);

            _methods.AddRange(a.GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(NewRelicProfileAttribute), false).Length > 0));

            //foreach (var method in _methods)
            //{
            //    Console.WriteLine(String.Format("Assembly: {0},\nClass: {1},\nMethod: {2}", a.GetName().Name, method.DeclaringType.Name, method.Name));
            //}
        }

        public List<MethodInfo> GetProfiledMethods()
        {
            return _methods;
        }
    }
}
