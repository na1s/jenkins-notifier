using System.IO;
using System.Net.Mime;
using System.Reflection;
using NUnit.Framework;

namespace JenkinsRestClient.Tests
{
    [TestFixture]
    public class JsonTests
    {
        [Test] 
        public void TestSample()
        {
            GetJson("json1.json");
        }

        private static string GetJson(string name)
        {
            Assembly thisExe = Assembly.GetExecutingAssembly();
            System.IO.Stream stream = thisExe.GetManifestResourceStream("JenkinsRestClient.Tests.JsonResults." + name);
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }
}