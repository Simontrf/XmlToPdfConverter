using System.Linq;

namespace XmlToPdfConverter.CLI
{
    public static class ArrayExtensions
    {
        // Vérifie si le tableau contient une valeur spécifique (insensible à la casse).
        // <param name="array">Tableau à parcourir</param>
        // <param name="value">Valeur recherchée</param>
        // <returns>True si trouvé, sinon False</returns>
        public static bool Contains(this string[] array, string value)
        {
            return array.Any(arg => arg.Equals(value, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}