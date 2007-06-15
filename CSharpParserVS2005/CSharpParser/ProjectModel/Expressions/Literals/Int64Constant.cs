using System;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an UInt64 constant.
  /// </summary>
  // ==================================================================================
  public sealed class Int64Constant : IntegerConstant
  {
    private Int64 _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new UInt64 constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="value">Value to initiate with.</param>
    // --------------------------------------------------------------------------------
    public Int64Constant(Token token, Int64 value)
      : base(token)
    {
      _Value = value;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Int64 Value
    {
      get { return _Value; }
    }
  }
}