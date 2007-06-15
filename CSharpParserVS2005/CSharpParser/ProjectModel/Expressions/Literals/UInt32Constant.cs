using System;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an UInt32 constant.
  /// </summary>
  // ==================================================================================
  public sealed class UInt32Constant : IntegerConstant
  {
    private UInt32 _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new UInt32 constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="value">Value to initiate with.</param>
    // --------------------------------------------------------------------------------
    public UInt32Constant(Token token, UInt32 value)
      : base(token)
    {
      _Value = value;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // --------------------------------------------------------------------------------
    public UInt32 Value
    {
      get { return _Value; }
    }
  }
}