// ================================================================================================
// ObjectOrCollectionInitializerNode.cs
//
// Created: 2009.06.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// 
  /// </summary>
  // ================================================================================================
  public class ObjectOrCollectionInitializerNode : SyntaxNode<ISyntaxNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectOrCollectionInitializerNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ObjectOrCollectionInitializerNode(Token start)
      : base(start)
    {
      MemberInitializers = new MemberInitializerNodeCollection {ParentNode = this};
      ElementInitializers = new ElementInitializerNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the member initializers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public MemberInitializerNodeCollection MemberInitializers { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the element initializers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ElementInitializerNodeCollection ElementInitializers { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional orphan comma separator separator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OrphanSeparator { get; internal set; }

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

      foreach (var memberInitializer in MemberInitializers)
      {
        memberInitializer.AcceptVisitor(visitor);
      }

      foreach (var elementInitializer in ElementInitializers)
      {
        elementInitializer.AcceptVisitor(visitor);
      }
    }

    #endregion

  }
}