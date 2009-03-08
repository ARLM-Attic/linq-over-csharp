using System;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an Int32 constant.
  /// </summary>
  // ==================================================================================
  public sealed class Int32Constant : IntegerConstant
  {
    private readonly Int32 _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new Int32 constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="value">Value to initiate with.</param>
    // --------------------------------------------------------------------------------
    public Int32Constant(Token token, CSharpSyntaxParser parser, Int32 value)
      : base(token, parser)
    {
      _Value = value;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int Value
    {
      get { return _Value; }
    }
  }
}