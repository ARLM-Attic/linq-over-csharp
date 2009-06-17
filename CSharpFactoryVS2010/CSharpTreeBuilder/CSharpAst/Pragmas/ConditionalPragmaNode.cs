// ================================================================================================
// ConditionalPragmaNode.cs
//
// Created: 2009.06.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class is a base class for conditional pragma nodes, like "#if", "#elif" and "#else".
  /// </summary>
  // ================================================================================================
  public abstract class ConditionalPragmaNode : PragmaNode
  {
    // --- Backing fields
    private Token _FirstSkippedToken;
    private Token _LastSkippedToken;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConditionalPragmaNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected ConditionalPragmaNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether the condition belonging to this pragma evaluates 
    /// to true.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool EvaluatesToTrue { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the tokens skipped when the conditional pragma is evaluated to false.
    /// </summary>
    /// <value>The skipped tokens.</value>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<Token> SkippedTokens 
    {
      get
      {
        if (_FirstSkippedToken == null) yield break;
        var currentToken = _FirstSkippedToken;
        do
        {
          yield return currentToken;
          if (currentToken == _LastSkippedToken) break;
          currentToken = currentToken.Next;
        } while (true);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the first skipped token.
    /// </summary>
    /// <param name="token">The token that is the first on the list of skipped ones.</param>
    // ----------------------------------------------------------------------------------------------
    internal void SetFirstSkippedToken(Token token)
    {
      _FirstSkippedToken = token;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the first skipped token.
    /// </summary>
    /// <param name="token">The token that is the last on the list of skipped ones.</param>
    // ----------------------------------------------------------------------------------------------
    internal void SetLastSkippedToken(Token token)
    {
      _LastSkippedToken = token;
    }
  }
}