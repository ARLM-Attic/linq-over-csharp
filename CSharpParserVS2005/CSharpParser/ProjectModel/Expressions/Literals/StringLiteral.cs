using System;
using CSharpParser.ParserFiles;
using CSharpParser.Utility;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents a string literal.
  /// </summary>
  // ==================================================================================
  public sealed class StringLiteral : Literal
  {
    private readonly string _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new string constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public StringLiteral(Token token)
      : base(token)
    {
      if (token.val.EndsWith(@""""))
      {
        if (token.val.StartsWith(@""""))
        {
          _Value = StringHelper.StringFromCSharpLiteral(token.val.Substring(1, token.val.Length - 2));
          return;
        }
        else if (token.val.StartsWith(@"@"""))
        {
          _Value = StringHelper.StringFromVerbatimLiteral(token.val.Substring(2, token.val.Length - 3));
          return;
        }
      }
      throw new ArgumentException("Invalid string literal is used.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Value
    {
      get { return _Value; }
    }
  }
}