// ================================================================================================
// RealConstantNode.cs
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
  /// This abstract class defines a real literal.
  /// </summary>
  // ================================================================================================
  public abstract class RealLiteralNode : LiteralNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="RealLiteralNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected RealLiteralNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an RealConstantNode instance from the specified token value.
    /// </summary>
    /// <param name="t">Token holding the value.</param>
    /// <returns>
    /// Instance created from the token.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static RealLiteralNode Create(Token t)
    {
      string valueSrt = t.Value.ToLower();
      if (valueSrt.EndsWith("f"))
        return new SingleLiteralNode(t, Single.Parse(valueSrt.Substring(0, valueSrt.Length - 1),
                                              NumberStyles.Float, CultureInfo.InvariantCulture));
      if (valueSrt.EndsWith("m"))
        return new DecimalLiteralNode(t, Decimal.Parse(valueSrt.Substring(0, valueSrt.Length - 1),
                                                NumberStyles.Float, CultureInfo.InvariantCulture));
      if (valueSrt.EndsWith("d"))
        return new DoubleLiteralNode(t, Double.Parse(valueSrt.Substring(0, valueSrt.Length - 1),
                                              NumberStyles.Float, CultureInfo.InvariantCulture));
      return new DoubleLiteralNode(t, Double.Parse(valueSrt, NumberStyles.Float,
                                            CultureInfo.InvariantCulture));
    }
  }
}