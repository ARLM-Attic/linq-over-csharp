// ================================================================================================
// IndentationSegment.cs
//
// Created: 2009.03.27, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// Controls the indentation of the source code segments
  /// </summary>
  // ================================================================================================
  public class IndentationSegment : ControlSegment
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Apply the current indentation level.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static readonly IndentationSegment Apply = new IndentationSegment();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Increment the current indentation level.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static readonly IndentationSegment Increment = new IndentationSegment();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Decrement the current indentation level.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static readonly IndentationSegment Decrement = new IndentationSegment();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Avoid external instantiation
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private IndentationSegment() { }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Controls the indentation of the specified OutputItemSerializer.
    /// </summary>
    /// <param name="serializer">The serializer to control.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Control(OutputItemSerializer serializer)
    {
      if (this == Apply) serializer.ApplyIndentation();
      else if (this == Increment) serializer.IncrementIndentation();
      else if (this == Decrement) serializer.DecrementIndentation();
    }
  }
}