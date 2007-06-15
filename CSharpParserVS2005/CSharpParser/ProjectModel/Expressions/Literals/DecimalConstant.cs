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
    private Decimal _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new Decimal constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="value">Value to initiate with.</param>
    // --------------------------------------------------------------------------------
    public DecimalConstant(Token token, Decimal value)
      : base(token)
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