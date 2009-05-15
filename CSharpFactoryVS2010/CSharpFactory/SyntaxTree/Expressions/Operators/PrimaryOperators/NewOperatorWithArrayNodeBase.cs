// ================================================================================================
// NewOperatorWithArrayNodeBase.cs
//
// Created: 2009.05.14, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class is a base for "new" operator with array initializers
  /// </summary>
  // ================================================================================================
  public class NewOperatorWithArrayNodeBase : NewOperatorNode, IArrayDimensions
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NewOperatorWithImplicitArrayNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public NewOperatorWithArrayNodeBase(Token start)
      : base(start)
    {
      Commas = new ImmutableCollection<Token>();
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