// ================================================================================================
// MandatoryWhiteSpaceSegment.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
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
  public sealed class MandatoryWhiteSpaceSegment : ControlSegment
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Default (singleton) instance of this whitespace segment.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static readonly MandatoryWhiteSpaceSegment Default = 
      new MandatoryWhiteSpaceSegment();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Avoid external instantiation
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private MandatoryWhiteSpaceSegment() { }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signs that a mandatory whitespace is required for the serializer.
    /// </summary>
    /// <param name="serializer">The serializer to control.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Control(OutputItemSerializer serializer)
    {
      serializer.SignMandatoryWhiteSpace();
    }
  }
}