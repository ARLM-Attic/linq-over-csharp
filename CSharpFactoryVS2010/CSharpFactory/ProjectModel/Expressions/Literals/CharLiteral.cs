using System;
using CSharpFactory.ParserFiles;
using CSharpFactory.Utility;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents an char literal.
  /// </summary>
  // ==================================================================================
  public sealed class CharLiteral : Literal
  {
    private readonly char _Value;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new char constant.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public CharLiteral(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
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