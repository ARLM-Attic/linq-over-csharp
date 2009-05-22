// ================================================================================================
// ControlSegments.cs
//
// Created: 2009.03.27, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
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
}