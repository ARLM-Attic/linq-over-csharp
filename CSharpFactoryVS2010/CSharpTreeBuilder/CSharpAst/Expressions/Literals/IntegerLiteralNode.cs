// ================================================================================================
// IntegerLiteralNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Globalization;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines an integer literal.
  /// </summary>
  // ================================================================================================
  public abstract class IntegerLiteralNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="IntegerLiteralNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected IntegerLiteralNode(Token start)
      : base(start)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an IntegerConstant instance from the specified token value.
    /// </summary>
    /// <param name="t">Token holding the value.</param>
    /// <returns>
    /// Instance created from the token.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static IntegerLiteralNode Create(Token t)
    {
      string valueStr = t.Value.ToLower();
      if (valueStr.StartsWith("0x"))
      {
        valueStr = valueStr.Substring(2);
        if (valueStr.EndsWith("ul") || valueStr.EndsWith("lu"))
        {
          valueStr = valueStr.Remove(valueStr.Length - 2);
          return new UInt64LiteralNode(t, UInt64.Parse(valueStr, NumberStyles.HexNumber));
        }
        if (valueStr.EndsWith("u"))
        {
          valueStr = valueStr.Remove(valueStr.Length - 1);
          return new UInt32LiteralNode(t, UInt32.Parse(valueStr, NumberStyles.HexNumber));
        }
        if (valueStr.EndsWith("l"))
        {
          valueStr = valueStr.Remove(valueStr.Length - 1);
          return new Int64LiteralNode(t, Int64.Parse(valueStr, NumberStyles.HexNumber));
        }

        try
        {
          return new Int32LiteralNode(t, Int32.Parse(valueStr, NumberStyles.HexNumber));
        }
        catch (OverflowException)
        {
          return new UInt32LiteralNode(t, UInt32.Parse(valueStr, NumberStyles.HexNumber));
        }
      }

      if (valueStr.EndsWith("ul") || valueStr.EndsWith("lu"))
      {
        valueStr = valueStr.Remove(valueStr.Length - 2);
        return new UInt64LiteralNode(t, UInt64.Parse(valueStr));
      }
      if (valueStr.EndsWith("u"))
      {
        valueStr = valueStr.Remove(valueStr.Length - 1);
        return new UInt32LiteralNode(t, UInt32.Parse(valueStr));
      }
      if (valueStr.EndsWith("l"))
      {
        valueStr = valueStr.Remove(valueStr.Length - 1);
        return new Int64LiteralNode(t, Int64.Parse(valueStr));
      }
      
      try
      {
        return new Int32LiteralNode(t, Int32.Parse(valueStr));
      }
      catch (OverflowException)
      {
        return new UInt32LiteralNode(t, UInt32.Parse(valueStr));
      }
      
    }
  }
}