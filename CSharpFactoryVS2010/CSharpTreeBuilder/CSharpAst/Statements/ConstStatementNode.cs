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
    private TypeOrNamespaceNode _TypeName;

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
    public TypeOrNamespaceNode TypeName
    {
      get { return _TypeName; }
      internal set
      {
        _TypeName = value;
        if (_TypeName != null) _TypeName.ParentNode = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the const tags.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ConstTagNodeCollection ConstTags { get; private set; }
  }
}