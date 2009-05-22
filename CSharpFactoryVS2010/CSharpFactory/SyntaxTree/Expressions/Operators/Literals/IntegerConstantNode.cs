// ================================================================================================
// IntegerConstantNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Globalization;
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines an integer constant.
  /// </summary>
  // ================================================================================================
  public abstract class IntegerConstantNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="IntegerConstantNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected IntegerConstantNode(Token start)
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
    public static IntegerConstantNode Create(Token t)
    {
      string valueStr = t.val.ToLower();
      if (valueStr.StartsWith("0x"))
      {
        valueStr = valueStr.Substring(2);
        if (valueStr.EndsWith("ul") || valueStr.EndsWith("lu"))
          return new UInt64Node(t, UInt64.Parse(valueStr, NumberStyles.HexNumber));
        if (valueStr.EndsWith("u"))
          return new UInt32Node(t, UInt32.Parse(valueStr, NumberStyles.HexNumber));
        if (valueStr.EndsWith("l"))
          return new Int64Node(t, Int64.Parse(valueStr, NumberStyles.HexNumber));
        {
          try
          {
            return new Int32Node(t, Int32.Parse(valueStr, NumberStyles.HexNumber));
          }
          catch (OverflowException)
          {
            return new UInt32Node(t, UInt32.Parse(valueStr, NumberStyles.HexNumber));
          }
        }
      }
      if (valueStr.EndsWith("ul") || valueStr.EndsWith("lu"))
        return new UInt64Node(t, UInt64.Parse(valueStr));
      if (valueStr.EndsWith("u"))
        return new UInt32Node(t, UInt32.Parse(valueStr));
      if (valueStr.EndsWith("l"))
        return new Int64Node(t, Int64.Parse(valueStr));
      {
        try
        {
          return new Int32Node(t, Int32.Parse(valueStr));
        }
        catch (OverflowException)
        {
          return new UInt32Node(t, UInt32.Parse(valueStr));
        }
      }
    }
  }
}