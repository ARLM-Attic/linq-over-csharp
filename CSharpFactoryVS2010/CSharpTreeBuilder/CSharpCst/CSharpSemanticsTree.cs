// ================================================================================================
// CSharpSemanticsTree.cs
//
// Created: 2009.05.22, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.Cst
{
  // ================================================================================================
  /// <summary>
  /// This class represents the semantics tree of a compilation unit.
  /// </summary>
  // ================================================================================================
  public class CSharpSemanticsTree : 
    ICSharpSemanticsTree,
    ISemanticsNodeCollection
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpSemanticsTree"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CSharpSemanticsTree()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Removes all nodes from the collection that belong to the specified source file node.
    /// </summary>
    /// <param name="sourceFileNode">The source file node.</param>
    // ----------------------------------------------------------------------------------------------
    public void RemoveNodes(SourceFileNode sourceFileNode)
    {
    }
  }
}