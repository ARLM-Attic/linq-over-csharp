using System;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an Double constant.
  /// </summary>
  // ==================================================================================
  public sealed class DoubleConstant : RealConstant
  {
    private Double _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new Double constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="value">Value to initiate with.</param>
    // --------------------------------------------------------------------------------
    public DoubleConstant(Token token, Double value)
      : base(token)
    {
      _Value = value;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Double Value
    {
      get { return _Value; }
    }
  }
}