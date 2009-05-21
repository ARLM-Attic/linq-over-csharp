// ================================================================================================
// ITypeArguments.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the behavior of syntax nodes with type arguments.
  /// </summary>
  // ================================================================================================
  public interface ITypeArguments
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the node providing type arguments.
    /// </summary>
    /// <value>The arguments.</value>
    // ----------------------------------------------------------------------------------------------
    TypeArgumentListNode Arguments { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has type arguments.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has type arguments; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    bool HasTypeArguments { get; }
  }
}