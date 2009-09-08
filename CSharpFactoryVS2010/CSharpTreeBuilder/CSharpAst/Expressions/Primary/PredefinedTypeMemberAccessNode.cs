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
    /// <param name="typeName">The node representing the predefined type.</param>
    // ----------------------------------------------------------------------------------------------
    public PredefinedTypeMemberAccessNode(Token start, NamespaceOrTypeNameNode typeName)
      : base(start)
    {
      if (typeName == null)
      {
        throw new ArgumentNullException("typeName");
      }
      TypeName = typeName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the predefined type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNode TypeName { get; private set; }

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

      if (TypeName != null)
      {
        TypeName.AcceptVisitor(visitor);
      }

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
