// ================================================================================================
// VariableDeclarationStatementNode.cs
//
// Created: 2009.05.08, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a "const" statement.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   ConstStatementNode:
  ///     "const" TypeOrNamespaceNode ConstTagNode { ConstMemberTagContinuationNode } ";"
  /// </remarks>
  // ================================================================================================
  public sealed class VariableDeclarationStatementNode : StatementNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="VariableDeclarationStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public VariableDeclarationStatementNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the declaration belonging to this statement.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public LocalVariableNode Declaration { get; internal set; }
  }
}