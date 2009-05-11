// ================================================================================================
// VariableDeclarationStatementNode.cs
//
// Created: 2009.05.08, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a "const" statement.
  /// </summary>
  /// <remarks>
  /// Syntax:
  ///   ConstStatementNode:
  ///     "const" TypeOrNamespaceNode ConstMemberTagNode { ConstMemberTagContinuationNode } ";"
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
      ConstTags = new ConstMemberTagNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type belonging to this const statement.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the const tags.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ConstMemberTagNodeCollection ConstTags { get; private set; }
  }
}