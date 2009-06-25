// ================================================================================================
// StringLiteralNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpFactory.Utility;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines a System.Char literal.
  /// </summary>
  // ================================================================================================
  public class StringLiteralNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="StringLiteralNode"/> class.
    /// </summary>
    /// <param name="token">The start token.</param>
    // ----------------------------------------------------------------------------------------------
    public StringLiteralNode(Token token)
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