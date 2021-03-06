// ================================================================================================
// ISemanticsTree.cs
//
// Created: 2009.04.05, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpFactory.Semantics
{
  // ================================================================================================
  /// <summary>
  /// This interface represents the C# semantics tree.
  /// </summary>
  // ================================================================================================
  public interface ISemanticsTree
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of errors belonging to the semantics tree.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    CompilationMessageCollection Errors { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of warnings belonging to the semantics tree.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    CompilationMessageCollection Warnings { get; }
  }
}