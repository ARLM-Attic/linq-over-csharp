// ================================================================================================
// ConstStatementNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
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
  public sealed class ConstStatementNode : StatementNode
  {
    // --- Backing fields
    private TypeNode _Type;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstStatementNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstStatementNode(Token start)
      : base(start)
    {
      ConstTags = new ConstTagNodeCollection { ParentNode = this };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type belonging to this const statement.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeNode Type
    {
      get { return _Type; }
      internal set
      {
        _Type = value;
        if (_Type != null) _Type.ParentNode = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the const tags.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ConstTagNodeCollection ConstTags { get; private set; }

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

      if (Type != null)
      {
        Type.AcceptVisitor(visitor);
      }

      foreach (var constTag in ConstTags)
      {
        constTag.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}