// ================================================================================================
// SemanticsTree.cs
//
// Created: 2009.04.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;

namespace CSharpFactory.Semantics
{
  // ================================================================================================
  /// <summary>
  /// This class represents the semantics tree of a compilation unit.
  /// </summary>
  // ================================================================================================
  public class SemanticsTree : ISemanticsTree,
    ISemanticsNodeCollection
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticsTree"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticsTree()
    {
      Errors = new CompilationMessageCollection();
      Warnings = new CompilationMessageCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of errors belonging to the semantics tree.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CompilationMessageCollection Errors { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of warnings belonging to the semantics tree.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CompilationMessageCollection Warnings { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes all nodes from the collection that belong to the specified source file node.
    /// </summary>
    /// <param name="sourceFileNode">The source file node.</param>
    // ----------------------------------------------------------------------------------------------
    public void RemoveNodes(SourceFileNode sourceFileNode)
    {
      Errors.RemoveNodes(sourceFileNode);
      Warnings.RemoveNodes(sourceFileNode);
    }
  }
}