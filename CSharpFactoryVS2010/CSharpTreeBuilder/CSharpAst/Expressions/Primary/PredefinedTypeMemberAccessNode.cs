using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type represents the member access on a predefined type.
  /// </summary>
  // ================================================================================================
  public sealed class PredefinedTypeMemberAccessNode : MemberAccessNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PredefinedTypeMemberAccessNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="typeNode">The node representing the predefined type.</param>
    // ----------------------------------------------------------------------------------------------
    public PredefinedTypeMemberAccessNode(Token start, TypeOrNamespaceNode typeNode)
      : base(start)
    {
      if (typeNode == null)
      {
        throw new ArgumentNullException("typeNode");
      }
      TypeName = typeNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the predefined type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; private set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      visitor.Visit(this);

      if (TypeName != null)
      {
        TypeName.AcceptVisitor(visitor);
      }

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
