using System;
using CSharpParser.ParserFiles;
using CSharpParser.Utility;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an char literal.
  /// </summary>
  // ==================================================================================
  public sealed class CharLiteral : Literal
  {
    private char _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new char constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public CharLiteral(Token token)
      : base(token)
    {
      if (token.val.StartsWith("'") && token.val.EndsWith("'"))
        _Value = StringHelper.CharFromCSharpLiteral(token.val.Substring(1, token.val.Length - 2));
      else
        throw new ArgumentException("Invalid char literal is used.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of the constant.
    /// </summary>
    // --------------------------------------------------------------------------------
    public char Value
    {
      get { return _Value; }
    }
  }
}