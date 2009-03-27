// ================================================================================================
// MandatoryWhiteSpaceSegment.cs
//
// Created: 2009.03.24, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This segment represents a mandatory whitespace between two output segments.
  /// </summary>
  /// <remarks>
  /// There are language constructs where a mandatory whitespace should be used between two tokens. 
  /// For example in using directives there is a mandatory whitespace bewtween the "using" token and
  /// the identifier (or identifier fragment) following it.
  /// </remarks>
  // ================================================================================================
  public sealed class MandatoryWhiteSpaceSegment : OutputSegment
  {
    public static readonly MandatoryWhiteSpaceSegment Default = 
      new MandatoryWhiteSpaceSegment();
  }

  // ================================================================================================
  /// <summary>
  /// This segment represents a new line
  /// </summary>
  // ================================================================================================
  public sealed class ForceNewLineSegment : OutputSegment
  {
  }
}