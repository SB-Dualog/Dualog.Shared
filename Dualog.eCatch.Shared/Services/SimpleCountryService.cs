using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dualog.eCatch.Shared.Utilities;

namespace Dualog.eCatch.Shared.Services
{
    public static class SimpleCountryService
    {
        private static List<string> _countries { get; set; }

        public static IEnumerable<string> GetAll()
        {
            if (_countries != null) return _countries;
            var result = new List<string>();
            using (var stream = ResourceLoader.GetEmbeddedResourceStream(typeof(SimpleCountryService).GetTypeInfo().Assembly, "Countries.txt"))
            using (var streamReader = new StreamReader(stream))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    result.Add(line);
                }
            }
            return result;
        }
    }
}
