// ================================================================================================
// TypeParameters.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This interface represents syntax tree nodes that can hold type parameters
  /// </summary>
  // ================================================================================================
  public interface ITypeParameterHolder
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has type parameters.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has type parameters; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    bool HasTypeParameters { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the open sign token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Token OpenSign { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the open sign token.
    /// </summary>
    /// <param name="token">The token.</param>
    // ----------------------------------------------------------------------------------------------
    void SetOpenSign(Token token);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the close sign token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Token CloseSign { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the close sign token.
    /// </summary>
    /// <param name="token">The token.</param>
    // ----------------------------------------------------------------------------------------------
    void SetCloseSign(Token token);

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of type parameters.
    /// </summary>
    /// <value>The type parameters.</value>
    // ----------------------------------------------------------------------------------------------
    ImmutableCollection<TypeParameterNode> TypeParameters { get; }
  }

  // ================================================================================================
  /// <summary>
  /// This node represents a type parameter with its attributes.
  /// </summary>
  // ================================================================================================
  public class TypeParameterNode : NameTagNode, IAttributedDeclaration
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="attrNodes">Attributes of the type parameter.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterNode(Token start, AttributeDecorationNodeCollection attrNodes)
      : base(start)
    {
      AttributeDecorations = attrNodes;
    }

    #region Implementation of IAttributedDeclaration

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the attribute decorations belonging to this declaration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AttributeDecorationNodeCollection AttributeDecorations { get; internal set; }

    #endregion
  }

  // ================================================================================================
  /// <summary>
  /// This type represents a type parameter continuation node.
  /// </summary>
  // ================================================================================================
  public class TypeParameterContinuationNode : TypeParameterNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterContinuationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="identifier">The identifier.</param>
    /// <param name="attrNodes">Attributes of the type parameter.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterContinuationNode(Token start, Token identifier,
      AttributeDecorationNodeCollection attrNodes)
      : base(start, attrNodes)
    {
      SeparatorToken = start;
      IdentifierToken = identifier;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the separator token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; private set; }
  }
}