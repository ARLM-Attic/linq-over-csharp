// ================================================================================================
// BracingStyle.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This enumeration defines the output styles used for bracing.
  /// </summary>
  // ================================================================================================
  public enum BracingStyle
  {
    /// <summary>
    /// At next line with the current indentation position.
    /// </summary>
    NextLineBsd,
    /// <summary>
    /// At next line indented with one position.
    /// </summary>
    NextLineGnu,
    /// <summary>
    /// At next line indented with one position, and child items are indented together with the brace.
    /// </summary>
    NextLineWhitesmiths,
    /// <summary>
    /// Directly at the end of the line with no space.
    /// </summary>
    EndOfLineNoSpace,
    /// <summary>
    /// Directly at the end of the line with one space.
    /// </summary>
    EndOfLineKAndR
  }
}