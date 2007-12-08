using System;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an Single constant.
  /// </summary>
  // ==================================================================================
  public sealed class SingleConstant : RealConstant
  {
    private readonly Single _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new Single constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="value">Value to initiate with.</param>
    // --------------------------------------------------------------------------------
    public SingleConstant(Token token, CSharpSyntaxParser parser, Single value)
      : base(token, parser)
    {
      _Value = value;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Single Value
    {
      get { return _Value; }
    }
  }
}