namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ==================================================================================
  /// <summary>
  /// This partial class declares predefined Tokens.
  /// </summary>
  // ==================================================================================
  public partial class Token
  {
    /// <summary>
    /// Returns a "using" token.
    /// </summary>
    public static Token Using { get { return new Token("using"); } }

    /// <summary>
    /// Returns a ";" (semicolon) token.
    /// </summary>
    public static Token Semicolon { get { return new Token(";"); } }

    /// <summary>
    /// Returns a "::" (double colon) token.
    /// </summary>
    public static Token DoubleColon { get { return new Token("::"); } }

    /// <summary>
    /// Returns a "." (dot) token.
    /// </summary>
    public static Token Dot { get { return new Token("."); } }

    /// <summary>
    /// Returns a "," (comma) token.
    /// </summary>
    public static Token Comma { get { return new Token(","); } }

    /// <summary>
    /// Returns a "=" (equal) token.
    /// </summary>
    public static Token Equal { get { return new Token("="); } }

    /// <summary>
    /// Returns a "&lt;" (less than) token.
    /// </summary>
    public static Token LessThan { get { return new Token("<"); } }

    /// <summary>
    /// Returns a "&gt;" (greater than) token.
    /// </summary>
    public static Token GreatherThan { get { return new Token(">"); } }
  }
}
