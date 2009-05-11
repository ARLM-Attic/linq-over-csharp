// ================================================================================================
// LocalVariableContinuationTagNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a continuation tag in a local variable declaration.
  /// </summary>
  // ================================================================================================
  public class LocalVariableContinuationTagNode : LocalVariableTagNode, IContinuationTag
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalVariableContinuationTagNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public LocalVariableContinuationTagNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token separating the continuation tag from the preceding tag.
    /// </summary>
    /// <value></value>
    // ----------------------------------------------------------------------------------------------
    public Token SeparatorToken { get; internal set; }
  }
}