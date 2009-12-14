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
    private LocalVariableNode _Declaration;

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
    public LocalVariableNode Declaration
    {
      get
      {
        return _Declaration;
      }
      internal set
      {
        _Declaration = value;
        if (_Declaration != null)
        {
          _Declaration.ParentNode = this;
        }
      }
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      if (!visitor.Visit(this)) { return; }

      base.AcceptVisitor(visitor);

      if (Declaration!=null)
      {
        Declaration.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}