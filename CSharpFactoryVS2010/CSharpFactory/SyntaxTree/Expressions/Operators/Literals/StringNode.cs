// ================================================================================================
// StringNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpFactory.ParserFiles;
using CSharpFactory.Utility;

namespace CSharpTreeBuilder.Ast
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
      if (token.val.EndsWith(@""""))
      {
        if (token.val.StartsWith(@""""))
        {
          Value = StringHelper.StringFromCSharpLiteral(token.val.Substring(1, token.val.Length - 2));
          return;
        }
        if (token.val.StartsWith(@"@"""))
        {
          Value = StringHelper.StringFromVerbatimLiteral(token.val.Substring(2, token.val.Length - 3));
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