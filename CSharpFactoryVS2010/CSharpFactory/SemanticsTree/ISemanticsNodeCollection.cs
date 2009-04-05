// ================================================================================================
// ISemanticsNodeCollection.cs
//
// Created: 2009.04.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Syntax;

namespace CSharpFactory.Semantics
{
  // ================================================================================================
  /// <summary>
  /// This interface sings a type as one that manages a collection of SemanticsNode instances.
  /// </summary>
  // ================================================================================================
  public interface ISemanticsNodeCollection
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes all nodes from the collection that belong to the specified source file node.
    /// </summary>
    /// <param name="sourceFileNode">The source file node.</param>
    // ----------------------------------------------------------------------------------------------
    void RemoveNodes(SourceFileNode sourceFileNode);
  }
}