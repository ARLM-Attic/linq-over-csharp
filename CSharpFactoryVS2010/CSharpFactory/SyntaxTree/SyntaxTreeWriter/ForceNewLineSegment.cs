// ================================================================================================
// ForceNewLineSegment.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This segment represents a new line
  /// </summary>
  // ================================================================================================
  public sealed class ForceNewLineSegment : ControlSegment
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Default (singleton) instance of this whitespace segment.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static readonly ForceNewLineSegment Default = new ForceNewLineSegment();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Avoid external instantiation
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private ForceNewLineSegment() {}

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Forces a new line in the serializer.
    /// </summary>
    /// <param name="serializer">The serializer to control.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Control(OutputItemSerializer serializer)
    {
      serializer.ForceNewLine();
    }
  }
}