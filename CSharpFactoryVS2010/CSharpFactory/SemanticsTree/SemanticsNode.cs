// ================================================================================================
// SemanticsNode.cs
//
// Created: 2009.04.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Syntax;

namespace CSharpFactory.Semantics
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
    /// <param name="sourceFileNode">The source file node.</param>
    // ----------------------------------------------------------------------------------------------
    protected SemanticsNode(SourceFileNode sourceFileNode)
    {
      SourceFileNode = sourceFileNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file node belonging to this semantics node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SourceFileNode SourceFileNode { get; private set; }
  }
}