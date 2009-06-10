// ================================================================================================
// CastOperatorDeclarationNode.cs
//
// Created: 2009.05.18, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.AstFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a cast operator.
  /// </summary>
  // ================================================================================================
  public class CastOperatorDeclarationNode : MethodDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CastOperatorDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public CastOperatorDeclarationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is an implicit operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsImplicit { get { return StartToken.val == "implicit"; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is an explicit operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsExplicit { get { return StartToken.val == "explicit"; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "operator" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OperatorToken { get; internal set; }
  }
}