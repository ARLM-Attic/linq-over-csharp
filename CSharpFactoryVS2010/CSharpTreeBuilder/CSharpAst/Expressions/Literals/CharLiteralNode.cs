// ================================================================================================
// CharLiteralNode.cs
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
  /// This class defines a System.Char literal.
  /// </summary>
  // ================================================================================================
  public class CharLiteralNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CharLiteralNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    // ----------------------------------------------------------------------------------------------
    public CharLiteralNode(Token start)
      : base(start)
    {
      if (start.Value.StartsWith("'") && start.Value.EndsWith("'"))
        Value = StringHelper.CharFromCSharpLiteral(start.Value.Substring(1, start.Value.Length - 2));
      else
        throw new ArgumentException("Invalid char literal is used.");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public char Value { get; internal set; }
  }
}