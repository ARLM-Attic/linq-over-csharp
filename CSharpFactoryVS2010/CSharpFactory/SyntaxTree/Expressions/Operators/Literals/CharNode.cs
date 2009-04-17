// ================================================================================================
// CharNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpFactory.ParserFiles;
using CSharpFactory.Utility;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines a System.Char literal.
  /// </summary>
  // ================================================================================================
  public class CharNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CharNode"/> class.
    /// </summary>
    /// <param name="start">The start token.</param>
    // ----------------------------------------------------------------------------------------------
    public CharNode(Token start)
      : base(start)
    {
      if (start.val.StartsWith("'") && start.val.EndsWith("'"))
        Value = StringHelper.CharFromCSharpLiteral(start.val.Substring(1, start.val.Length - 2));
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