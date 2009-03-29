// ================================================================================================
// ControlSegments.cs
//
// Created: 2009.03.27, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a control segment that can control the state of an 
  /// OutputItemSerializer.
  /// </summary>
  // ================================================================================================
  public abstract class ControlSegment
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Controls the state of the specified OutputItemSerializer.
    /// </summary>
    /// <param name="serializer">The serializer to control.</param>
    // ----------------------------------------------------------------------------------------------
    public abstract void Control(OutputItemSerializer serializer);
  }

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