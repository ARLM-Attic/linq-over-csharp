using System;
using System.Globalization;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an integer constant.
  /// </summary>
  // ==================================================================================
  public abstract class IntegerConstant : Literal
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new integer constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected IntegerConstant(Token token)
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
    public static IntegerConstant Create(Token t)
    {
      Console.WriteLine("{0}: {1}", t.val, t.line);
      string valueStr = t.val.ToLower();
      if (valueStr.StartsWith("0x"))
      {
        valueStr = valueStr.Substring(2);
        if (valueStr.EndsWith("ul") || valueStr.EndsWith("lu"))
          return new UInt64Constant(t, UInt64.Parse(valueStr, NumberStyles.HexNumber));
        else if (valueStr.EndsWith("u")) 
          return new UInt32Constant(t, UInt32.Parse(valueStr, NumberStyles.HexNumber));
        else if (valueStr.EndsWith("l"))
          return new Int64Constant(t, Int64.Parse(valueStr, NumberStyles.HexNumber));
        else return new Int32Constant(t, Int32.Parse(valueStr, NumberStyles.HexNumber));
      }
      else
      { 
        // Decimal
        if (valueStr.EndsWith("ul") || valueStr.EndsWith("lu"))
          return new UInt64Constant(t, UInt64.Parse(valueStr));
        else if (valueStr.EndsWith("u"))
          return new UInt32Constant(t, UInt32.Parse(valueStr));
        else if (valueStr.EndsWith("l"))
          return new Int64Constant(t, Int64.Parse(valueStr));
        else return new Int32Constant(t, Int32.Parse(valueStr));
      }
    }
  }
}