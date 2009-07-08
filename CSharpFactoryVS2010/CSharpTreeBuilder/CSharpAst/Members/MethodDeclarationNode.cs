// ================================================================================================
// MethodDeclarationNode.cs
//
// Created: 2009.04.19, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a method declaration.
  /// </summary>
  // ================================================================================================
  public class MethodDeclarationNode : MemberWithBodyDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public MethodDeclarationNode(Token start)
      : base(start)
    {
      FormalParameters = new FormalParameterNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the node representing formal parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FormalParameterNodeCollection FormalParameters { get; internal set; }

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

      foreach (var attributeDecoration in AttributeDecorations)
      {
        attributeDecoration.AcceptVisitor(visitor);
      }

      if (TypeName != null)
      {
        TypeName.AcceptVisitor(visitor);
      }

      foreach (var typeParameter in TypeParameters)
      {
        typeParameter.AcceptVisitor(visitor);
      }

      foreach (var typeParameterConstraint in TypeParameterConstraints)
      {
        typeParameterConstraint.AcceptVisitor(visitor);
      }

      foreach (var formalParameter in FormalParameters)
      {
        formalParameter.AcceptVisitor(visitor);
      }

      if (Body != null)
      {
        Body.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}