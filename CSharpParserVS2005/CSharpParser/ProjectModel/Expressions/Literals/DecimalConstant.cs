using System;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an Decimal constant.
  /// </summary>
  // ==================================================================================
  public sealed class DecimalConstant : RealConstant
  {
    private readonly Decimal _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new Decimal constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="value">Value to initiate with.</param>
    // --------------------------------------------------------------------------------
    public DecimalConstant(Token token, CSharpSyntaxParser parser, Decimal value)
      : base(token, parser)
    {
      _Value = value;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Decimal Value
    {
      get { return _Value; }
    }
  }
}