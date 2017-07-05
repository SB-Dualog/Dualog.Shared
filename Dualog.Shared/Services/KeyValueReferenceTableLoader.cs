using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dualog.Shared.Utilities;

namespace Dualog.Shared.Services
{
    public static class KeyValueReferenceTableLoader
    {
        public static Dictionary<string, string> Load(string filename, int valueIndex = 1)
        {
            var result = new Dictionary<string, string>();

            using (var stream = ResourceLoader.GetEmbeddedResourceStream(typeof(KeyValueReferenceTableLoader).GetTypeInfo().Assembly, filename))
            using (var streamReader = new StreamReader(stream))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var items = line.Split('\t');
                    result.Add(items[0], items[valueIndex]);
                }
            }

            return result;
        }

        public static Dictionary<string, string> LoadFishingActivities(string fileName, int languageIndex = 2)
        {
            var result = new Dictionary<string, string>();

            using (var stream = ResourceLoader.GetEmbeddedResourceStream(typeof(KeyValueReferenceTableLoader).GetTypeInfo().Assembly, fileName))
            using (var streamReader = new StreamReader(stream))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var items = line.Split('\t');
                    //The norwegian text is the third
                    result.Add(items[0], items[languageIndex]);
                }
            }

            return result;
        }
    }
}
