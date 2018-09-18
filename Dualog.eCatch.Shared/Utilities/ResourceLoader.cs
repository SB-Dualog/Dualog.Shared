using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dualog.eCatch.Shared.Utilities
{
	public static class ResourceLoader
	{
		/// <summary>
		/// Attempts to find and return the given resource from within the specified assembly.
		/// </summary>
		/// <returns>The embedded resource stream.</returns>
		/// <param name="assembly">Assembly.</param>
		/// <param name="resourceFileName">Resource file name.</param>
		public static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
		{
			var resourceNames = assembly.GetManifestResourceNames();

			var resourcePaths = resourceNames
				.Where(x => x.EndsWith(resourceFileName, StringComparison.CurrentCultureIgnoreCase))
				.ToArray();

			if (!resourcePaths.Any())
			{
				throw new Exception($"Resource ending with {resourceFileName} not found.");
			}

			if (resourcePaths.Count() > 1)
			{
				throw new Exception($"Multiple resources ending with {resourceFileName} found: {Environment.NewLine}{string.Join(Environment.NewLine, resourcePaths)}");
			}

			return assembly.GetManifestResourceStream(resourcePaths.Single());
		}

		/// <summary>
		/// Attempts to find and return the given resource from within the specified assembly.
		/// </summary>
		/// <returns>The embedded resource as a byte array.</returns>
		/// <param name="assembly">Assembly.</param>
		/// <param name="resourceFileName">Resource file name.</param>
		public static byte[] GetEmbeddedResourceBytes(Assembly assembly, string resourceFileName)
		{
			var stream = GetEmbeddedResourceStream(assembly, resourceFileName);

			using (var memoryStream = new MemoryStream())
			{
				stream.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
		}

		/// <summary>
		/// Attempts to find and return the given resource from within the specified assembly.
		/// </summary>
		/// <returns>The embedded resource as a string.</returns>
		/// <param name="assembly">Assembly.</param>
		/// <param name="resourceFileName">Resource file name.</param>
		public static string GetEmbeddedResourceString(Assembly assembly, string resourceFileName)
		{
			var stream = GetEmbeddedResourceStream(assembly, resourceFileName);

			using (var streamReader = new StreamReader(stream))
			{
				return streamReader.ReadToEnd();
			}
		}
	}
}