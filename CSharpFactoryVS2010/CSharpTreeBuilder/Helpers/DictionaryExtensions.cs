using System.Collections.Generic;

namespace CSharpTreeBuilder.Helpers
{
  // ==================================================================================
  /// <summary>
  /// This static class implements extensions methods for the Dictionary`2 class.
  /// </summary>
  // ==================================================================================
  public static class DictionaryExtensions
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds the content of a Dictionary to another dictionary.
    /// </summary>
    /// <param name="target">Elements will be added to this dictionary.</param>
    /// <param name="source">Elements from this dictionary will be copied to the target.</param>
    // --------------------------------------------------------------------------------
    public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> target, Dictionary<TKey, TValue> source)
    {
      if (source != null)
      {
        foreach (var key in source.Keys)
        {
          target.Add(key, source[key]);
        }
      }
    }
  }
}