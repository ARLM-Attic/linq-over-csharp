using System;

namespace CSharpTreeBuilder.Helpers
{
  // ================================================================================================
  /// <summary>
  /// This class implements extension methods for Enum types.
  /// </summary>
  // ================================================================================================
  public static class EnumExtensions
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether a flag is set in a flag-type enum value.
    /// </summary>
    /// <param name="input">An enum value.</param>
    /// <param name="matchTo">A flag.</param>
    /// <returns>True if the enum value contains the flag, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public static bool IsSet(this Enum input, Enum matchTo)
    {
      return (Convert.ToUInt32(input) & Convert.ToUInt32(matchTo)) != 0;
    }
  }
}
