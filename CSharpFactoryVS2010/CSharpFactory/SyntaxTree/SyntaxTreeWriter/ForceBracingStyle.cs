// ================================================================================================
// ForceBracingStyle.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This enumeration defines how braces should be forced.
  /// </summary>
  // ================================================================================================
  public enum ForceBracingStyle
  {
    /// <summary>
    /// Remove braces if possible
    /// </summary>
    Remove,
    /// <summary>
    /// Keep braces as those are used in the original source
    /// </summary>
    DoNotChange,
    /// <summary>
    /// Add braces even if those are not used in the original source
    /// </summary>
    Add,
    /// <summary>
    /// Use braces only if single-statement block spread across multiple lines
    /// </summary>
    UseForMultiline
  }
}