// ================================================================================================
// ConstructorDeclarationNode.cs
//
// Created: 2009.05.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a constructor declaration.
  /// </summary>
  // ================================================================================================
  public class ConstructorDeclarationNode : MethodDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructorDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstructorDeclarationNode(Token start)
      : base(start)
    {
      IdentifierToken = start;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the colon token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token Colon { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the initializer token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode Initializer { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has initializer.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has initializer; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasInitializer { get { return Initializer != null; } }

  }
}