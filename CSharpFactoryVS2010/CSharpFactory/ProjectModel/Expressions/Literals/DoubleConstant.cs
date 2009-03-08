using System;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an Double constant.
  /// </summary>
  // ==================================================================================
  public sealed class DoubleConstant : RealConstant
  {
    private readonly Double _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new Double constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="value">Value to initiate with.</param>
    // --------------------------------------------------------------------------------
    public DoubleConstant(Token token, CSharpSyntaxParser parser, Double value)
      : base(token, parser)
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