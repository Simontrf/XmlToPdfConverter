using System.Linq;

namespace XmlToPdfConverter.CLI
{
    public static class ArrayExtensions
    {
        public static bool Contains(this string[] array, string value)
        {
            return array.Any(arg => arg.Equals(value, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}