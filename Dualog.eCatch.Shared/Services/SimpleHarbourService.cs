using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dualog.eCatch.Shared.Models;
using Dualog.eCatch.Shared.Utilities;

namespace Dualog.eCatch.Shared.Services
{
    public static class SimpleHarbourService
    {
        private static List<Harbour> _harbours { get; set; }

        public static IEnumerable<Harbour> GetAll()
        {
            if (_harbours != null) return _harbours;
            var result = new List<Harbour>();
            using (var stream = ResourceLoader.GetEmbeddedResourceStream(typeof(SimpleHarbourService).GetTypeInfo().Assembly, "Harbours.txt"))
            using (var streamReader = new StreamReader(stream))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var parts = line.Split(new[] { '\t' }, 4);
                    var code = parts[0].Trim();
                    var harbour  = new Harbour(code.Substring(0, 2), code.Substring(2, code.Length - 2), parts[1].Trim(), parts[2].Trim(), parts[3].Trim());
                    result.Add(harbour);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the harbour by passing in the code, eg: NOTOS
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Option<Harbour> GetByCode(string code)
        {
            if (_harbours == null) GetAll();
            var result = _harbours.First(x => x.Id == code);
            return result == null ? Option.None<Harbour>() : Option.Some<Harbour>(result);
        }

        public static void ClearAllHarbours()
        {
            _harbours = null;
        }
    }
}
