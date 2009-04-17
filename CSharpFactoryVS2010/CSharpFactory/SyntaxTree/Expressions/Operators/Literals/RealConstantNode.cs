// ================================================================================================
// RealConstantNode.cs
//
// Created: 2009.04.15, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Globalization;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines a real value constant.
  /// </summary>
  // ================================================================================================
  public abstract class RealConstantNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="RealConstantNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected RealConstantNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an RealConstantNode instance from the specified token value.
    /// </summary>
    /// <param name="t">Token holding the value.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <returns>
    /// Instance created from the token.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static RealConstantNode Create(Token t)
    {
      var valueSrt = t.val.ToLower();
      if (valueSrt.EndsWith("f"))
        return new SingleNode(t, Single.Parse(valueSrt.Substring(0, valueSrt.Length - 1), 
          NumberStyles.Float, CultureInfo.InvariantCulture));
      if (valueSrt.EndsWith("m"))
        return new DecimalNode(t, Decimal.Parse(valueSrt.Substring(0, valueSrt.Length - 1), 
          NumberStyles.Float, CultureInfo.InvariantCulture));
      if (valueSrt.EndsWith("d"))
        return new DoubleNode(t, Double.Parse(valueSrt.Substring(0, valueSrt.Length - 1), 
          NumberStyles.Float, CultureInfo.InvariantCulture));
      return new DoubleNode(t, Double.Parse(valueSrt, NumberStyles.Float, 
        CultureInfo.InvariantCulture));
    }
  }
}