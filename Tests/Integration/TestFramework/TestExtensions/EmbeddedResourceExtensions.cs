using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TestFramework.TestExtensions
{
    [ExcludeFromCodeCoverage]
    public static class EmbeddedResourceExtensions
    {
        public static string ReadAllText(string filename)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(f => f.EndsWith($".{filename}"));

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }

}
