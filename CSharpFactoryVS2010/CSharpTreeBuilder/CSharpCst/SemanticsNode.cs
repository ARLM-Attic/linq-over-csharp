// ================================================================================================
// SemanticsNode.cs
//
// Created: 2009.04.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.Cst
{
  // ================================================================================================
  /// <summary>
  /// This class is intended to be the common root class for all nodes of a semantics tree.
  /// </summary>
  // ================================================================================================
  public abstract class SemanticsNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticsNode"/> class.
    /// </summary>
    /// <param name="compilationUnitNode">The compilation unit node.</param>
    // ----------------------------------------------------------------------------------------------
    protected SemanticsNode(CompilationUnitNode compilationUnitNode)
    {
      CompilationUnitNode = compilationUnitNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit node belonging to this semantics node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CompilationUnitNode CompilationUnitNode { get; private set; }
  }
}