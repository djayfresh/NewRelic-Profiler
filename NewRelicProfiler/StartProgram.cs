namespace NewRelicProfiler
{
    using NewRelicProfiler.Profiler;
    using System.Reflection;
    class StartProgram
    {
        static void Main(string[] args)
        {
            NewRelicProfiler profiler = new NewRelicProfiler();

            string assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            string fileName = assemblyName + "-newrelic.xml";

            if (args.Length > 0)
            {
                assemblyName = args[0];

                if (args.Length > 1)
                {
                    fileName = args[1];
                }

                if (args.Length > 2)
                {
                    fileName = args[0];

                    for (int arg = 1; arg < args.Length; arg++)
                    {
                        assemblyName = args[arg];
                        profiler.Load(Assembly.Load(assemblyName));
                    }
                }
                else
                {
                    profiler.Load(Assembly.Load(assemblyName));
                }
            }
            else
            {
                profiler.Load(Assembly.Load(assemblyName));
            }

            NewRelicXmlWriter writer = new NewRelicXmlWriter();

            var file = writer.BuildXML(profiler);
            writer.WriteToFile(file, fileName);
        }
    }
}
