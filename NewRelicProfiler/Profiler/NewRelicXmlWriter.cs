namespace NewRelicProfiler.Profiler
{
    using System.IO;
    using System.Xml.Linq;
    public class NewRelicXmlWriter
    {
        public void WriteToFile(XElement file, string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            file.Save(fileName);
        }

        public XElement BuildXML(NewRelicProfiler profiler)
        {
            XElement instrumentation = new XElement("instrumentation");

            foreach (var method in profiler.GetProfiledMethods())
            {
                var methodElement = Method(method.Name);
                var match = Match(method.DeclaringType.Assembly.GetName().Name, method.DeclaringType.Name);
                match.Add(methodElement);
                var factory = TracerFactory(method.DeclaringType.Name + "-" + method.Name);
                factory.Add(match);

                instrumentation.Add(factory);
            }

            XName name = XName.Get("extenstion", "urn:newrelic-extension");
            XElement document = new XElement(name);
            document.Add(instrumentation);
            return document;
        }

        private XElement TracerFactory(string name)
        {
            var factory = new XElement("tracerFactory");
            factory.SetAttributeValue("metricName", "Custom/" + name);
            return factory;
        }

        private XElement Match(string assmeblyName, string className)
        {
            var match = new XElement("match");
            match.SetAttributeValue("assemblyName", assmeblyName);
            match.SetAttributeValue("classname", className);
            return match;
        }

        private XElement Method(string methodName)
        {
            var match = new XElement("exactMethodMatcher");
            match.SetAttributeValue("methodName", methodName);
            return match;
        }
    }
}
