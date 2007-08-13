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
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    protected IntegerConstant(Token token, CSharpSyntaxParser parser) : base(token, parser)
    {
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an IntegerConstant instance from the specified token value.
    /// </summary>
    /// <param name="t">Token holding the value.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <returns>
    /// Instance created from the token.
    /// </returns>
    // --------------------------------------------------------------------------------
    public static IntegerConstant Create(Token t, CSharpSyntaxParser parser)
    {
      string valueStr = t.val.ToLower();
      if (valueStr.StartsWith("0x"))
      {
        valueStr = valueStr.Substring(2);
        if (valueStr.EndsWith("ul") || valueStr.EndsWith("lu"))
          return new UInt64Constant(t, parser, 
            UInt64.Parse(valueStr, NumberStyles.HexNumber));
        else if (valueStr.EndsWith("u"))
          return new UInt32Constant(t, parser, UInt32.Parse(valueStr, NumberStyles.HexNumber));
        else if (valueStr.EndsWith("l"))
          return new Int64Constant(t, parser,
            Int64.Parse(valueStr, NumberStyles.HexNumber));
        else
        {
          try
          {
            return new Int32Constant(t, parser,
              Int32.Parse(valueStr, NumberStyles.HexNumber));
          }
          catch (OverflowException)
          {
            return new UInt32Constant(t, parser,
              UInt32.Parse(valueStr, NumberStyles.HexNumber));
          }
        }
      }
      else
      { 
        // Decimal
        if (valueStr.EndsWith("ul") || valueStr.EndsWith("lu"))
          return new UInt64Constant(t, parser, UInt64.Parse(valueStr));
        else if (valueStr.EndsWith("u"))
          return new UInt32Constant(t, parser, UInt32.Parse(valueStr));
        else if (valueStr.EndsWith("l"))
          return new Int64Constant(t, parser,
            Int64.Parse(valueStr));
        else
        {
          try
          {
            return new Int32Constant(t, parser, Int32.Parse(valueStr));
          }
          catch (OverflowException)
          {
            return new UInt32Constant(t, parser, UInt32.Parse(valueStr));
          }
        }
      }
    }
  }
}