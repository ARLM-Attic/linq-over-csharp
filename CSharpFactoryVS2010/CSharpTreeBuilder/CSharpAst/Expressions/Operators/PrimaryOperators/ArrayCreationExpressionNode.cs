// ================================================================================================
// ArrayCreationExpressionNode.cs
//
// Created: 2009.05.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Collections;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an array creation expression.
  /// </summary>
  // ================================================================================================
  public sealed class ArrayCreationExpressionNode : NewOperatorNode, IArrayDimensions
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
      Commas = new ImmutableCollection<Token>();
      SizedDimensions = new SizedArrayDimensionNode {ParentNode = this};
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
    /// Gets or sets the sized dimensions.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SizedArrayDimensionNode SizedDimensions { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has sized dimensions.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasSizedDimensions
    {
      get { return SizedDimensions.Count > 0; }
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
    /// Gets or sets the opening square bracket.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenSquareBracket { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of comma tokens.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<Token> Commas { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the closing square bracket.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseSquareBracket { get; internal set; }
  }
}