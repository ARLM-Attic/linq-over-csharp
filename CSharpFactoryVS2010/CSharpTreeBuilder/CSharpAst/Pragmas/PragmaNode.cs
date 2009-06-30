// ================================================================================================
// PragmaNode.cs
//
// Created: 2009.06.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class is intended to be the base class of all nodes representing pragmas.
  /// </summary>
  /// <remarks>
  /// Pragmas are represented by a simple token, properties of this class can be used to access
  /// parts of pragma definitions.
  /// </remarks>
  // ================================================================================================
  public abstract class PragmaNode : SyntaxNode<CompilationUnitNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PragmaNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected PragmaNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the first symbol after the preprocessor directive.
    /// </summary>
    /// <returns>
    /// Preprocessor tag
    /// </returns>
    /// <remarks>
    /// input:        "#" {ws} directive ws {ws} {not-newline} {newline}
    /// valid input:  "#" {ws} directive ws {ws} {non-ws} {ws} {newline}
    /// output:       {non-ws}
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public string PreprocessorSymbol
    {
      get
      {
        var symbol = StartToken.Value;
        var start = 1;
        start = EndOf(symbol, start, true);    // --- Skip {ws}
        start = EndOf(symbol, start, false);   // --- Skip directive  
        start = EndOf(symbol, start, true);    // --- Skip ws {ws}
        var end = EndOf(symbol, start, false); // --- Search end of symbol
        return symbol.Substring(start, end - start).Trim();
      }
    }
  
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the text after the preprocessor directive.
    /// </summary>
    /// <returns>
    /// Preprocessor tag
    /// </returns>
    /// <remarks>
    /// input:        "#" {ws} directive ws {ws} {not-newline} {newline}
    /// valid input:  "#" {ws} directive ws {ws} {non-ws} {ws} {newline}
    /// output:       {non-ws}
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public string PreprocessorText
    {
      get
      {
        var symbol = StartToken.Value;
        var start = 1;
        start = EndOf(symbol, start, true);  // --- Skip {ws}
        start = EndOf(symbol, start, false); // --- Skip directive  
        start = EndOf(symbol, start, true);  // --- Skip ws {ws}
        var end = symbol.Length - 1;
        return end <= start ? string.Empty : symbol.Substring(start, end - start);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the end of the whitespaces in the given string.
    /// </summary>
    /// <param name="symbol">Symbol to search for white spaces</param>
    /// <param name="start">Starting index of the string representing the symbol</param>
    /// <param name="whitespaces">
    /// Flag indicating if we look for whitespace or non-whitespace
    /// </param>
    /// <returns>
    /// The end of the whitespaces in the given string if whitespaces is true;
    /// otherwise returns the end of the non-whitespaces.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static int EndOf(string symbol, int start, bool whitespaces)
    {
      while ((start < symbol.Length) && (char.IsWhiteSpace(symbol[start]) ^ !whitespaces)) ++start;
      return start;
    }

    
  }
}