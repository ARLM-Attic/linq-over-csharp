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
    private TypeNode _Type;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayCreationExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ArrayCreationExpressionNode(Token start)
      : base(start)
    {
      _Type = new TypeNode(null);
      ArraySizeSpecifier = new ArraySizeSpecifierNode();
      RankSpecifiers = new RankSpecifierNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the array.
    /// </summary>
    /// <value>The type of the array.</value>
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
    /// Gets a value indicating whether this new operator is implicit (now explicit type used)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsImplicit
    {
      get { return Type == null; }
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
      if (!visitor.Visit(this))
      {
        return;
      }

      if (Type!=null)
      {
        Type.AcceptVisitor(visitor);
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