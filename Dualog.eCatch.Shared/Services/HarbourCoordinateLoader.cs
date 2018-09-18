using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dualog.eCatch.Shared.Models;
using Dualog.eCatch.Shared.Utilities;

namespace Dualog.eCatch.Shared.Services
{
    public static class HarbourCoordinateLoader
    {
        public static Dictionary<string, DmCoordinate> Load()
        {
            var result = new Dictionary<string, DmCoordinate>();
            using (var stream = ResourceLoader.GetEmbeddedResourceStream(typeof(KeyValueReferenceTableLoader).GetTypeInfo().Assembly, "Harbours.txt"))
            using (var streamReader = new StreamReader(stream))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var items = line.Split('\t');
                    //items[1] is the Harbour name, might need it later.
                    result.Add(items[0], new DmCoordinate
                    {
                        Latitude = items[2],
                        Longitude = items[3]
                    });
                }
            }

            return result;
        } 
    }
}
