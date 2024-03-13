using System.IO;
using System.Reflection;
using ChainSafe.Gaming.Web3;

namespace ChainSafe.Gaming.Evm.Utils
{
    public static class AbiHelper
    {
        public static string ReadAbiFromResources(Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new Web3Exception($"Resource {resourceName} not found in the provided assembly.");
                }

                // Read the stream content
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}