using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dualog.eCatch.Shared.Models;
using Dualog.eCatch.Shared.Utilities;

namespace Dualog.eCatch.Shared.Services
{
    public static class SimpleErrorService
    {
        private static List<ReturnMessageError> _errors { get; set; }
        public static IEnumerable<ReturnMessageError> GetAll()
        {
            if (_errors != null) return _errors;
            var result = new List<ReturnMessageError>();
            using (var stream = ResourceLoader.GetEmbeddedResourceStream(typeof(SimpleHarbourService).GetTypeInfo().Assembly, "ErrorCodes.txt"))
            using (var streamReader = new StreamReader(stream))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var parts = line.Split(new[] { '\t' }, 4);
                    
                    var error = new ReturnMessageError(parts[0].Trim(), parts[1].Trim(), parts[2].Trim());
                    result.Add(error);
                }
            }
            _errors = result;
            return result;
        }

        public static void ClearAll()
        {
            _errors = null;
        }
    }
}
