// ================================================================================================
// StringNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;
using CSharpFactory.Utility;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines a System.Char literal.
  /// </summary>
  // ================================================================================================
  public class StringNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StringNode"/> class.
    /// </summary>
    /// <param name="token">The start token.</param>
    // ----------------------------------------------------------------------------------------------
    public StringNode(Token token)
      : base(token)
    {
      if (token.Value.EndsWith(@""""))
      {
        if (token.Value.StartsWith(@""""))
        {
          Value = StringHelper.StringFromCSharpLiteral(token.Value.Substring(1, token.Value.Length - 2));
          return;
        }
        if (token.Value.StartsWith(@"@"""))
        {
          Value = StringHelper.StringFromVerbatimLiteral(token.Value.Substring(2, token.Value.Length - 3));
          return;
        }
      }
      throw new ArgumentException("Invalid string literal is used.");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Value { get; internal set; }
  }
}