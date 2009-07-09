// ================================================================================================
// ArrayCreationExpressionNode.cs
//
// Created: 2009.05.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an array creation expression.
  /// </summary>
  // ================================================================================================
  public sealed class ArrayCreationExpressionNode : PrimaryExpressionNodeBase
  {
    // --- Backing fields
    private TypeOrNamespaceNode _TypeName;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayCreationExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ArrayCreationExpressionNode(Token start)
      : base(start)
    {
      _TypeName = TypeOrNamespaceNode.CreateEmptyTypeNode(null);
      ArraySizeSpecifier = new ArraySizeSpecifierNode();
      RankSpecifiers = new RankSpecifierNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
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
    /// Gets a value indicating whether this new operator is implicit (now explicit type used)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsImplicit
    {
      get { return TypeName == null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the array size specifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ArraySizeSpecifierNode ArraySizeSpecifier { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has specified array sizes.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasSpecifiedArraySizes
    {
      get { return ArraySizeSpecifier.Expressions.Count > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the rank specifier collection.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public RankSpecifierNodeCollection RankSpecifiers { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has any rank specifiers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasRankSpecifiers
    {
      get { return RankSpecifiers.Count > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the initializer used to implicit array initialization.
    /// </summary>
    /// <value>The initializer.</value>
    // ----------------------------------------------------------------------------------------------
    public ArrayInitializerNode Initializer { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has an initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasInitializer
    {
      get { return Initializer != null; }
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
      visitor.Visit(this);

      if (TypeName!=null)
      {
        TypeName.AcceptVisitor(visitor);
      }

      if (ArraySizeSpecifier!=null)
      {
        ArraySizeSpecifier.AcceptVisitor(visitor);
      }

      if (Initializer!=null)
      {
        Initializer.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}