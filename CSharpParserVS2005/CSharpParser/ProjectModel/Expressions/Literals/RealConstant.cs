using System;
using System.Globalization;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a real constant.
  /// </summary>
  // ==================================================================================
  public abstract class RealConstant : Literal
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new real constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected RealConstant(Token token)
      : base(token)
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
    public static RealConstant Create(Token t)
    {
      string valueSrt = t.val.ToLower();
      if (valueSrt.EndsWith("f"))
        return new SingleConstant(t, Single.Parse(valueSrt.Substring(0, valueSrt.Length-1), NumberStyles.Float, CultureInfo.InvariantCulture));
      else if (valueSrt.EndsWith("m"))
        return new DecimalConstant(t, Decimal.Parse(valueSrt.Substring(0, valueSrt.Length - 1), NumberStyles.Float, CultureInfo.InvariantCulture));
      else if (valueSrt.EndsWith("d"))
        return new DoubleConstant(t, Double.Parse(valueSrt.Substring(0, valueSrt.Length - 1), NumberStyles.Float, CultureInfo.InvariantCulture));
      else
        return new DoubleConstant(t, Double.Parse(valueSrt, NumberStyles.Float, CultureInfo.InvariantCulture ));
    }
  }
}