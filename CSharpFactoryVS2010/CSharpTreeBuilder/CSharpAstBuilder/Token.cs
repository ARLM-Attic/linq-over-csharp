// ================================================================================================
// Token.cs
//
// Created: 2009.05.21, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.CSharpAstBuilder
{

  // ==================================================================================
  /// <summary>
  /// This class defines a Token used by the scanner and the parser.
  /// </summary>
  /// <remarks>
  /// This class defines public fields that are against the convention not using public
  /// fields. Because wrapping this fields into properties would affect the performance
  /// we live together to violating the convention.
  /// </remarks>
  // ==================================================================================
  public class Token
  {
    // --- We use these fields because CoCo generates these names into the Scanner.cs and Parser.cs.
    // --- Removing these fields is subject of refactoring.
    
    // ReSharper disable InconsistentNaming
    internal int kind;
    internal string val;
    internal int pos;
    internal int col;
    internal int line;
    internal Token next;
    // ReSharper restore InconsistentNaming

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Kind of token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Kind 
    { 
      get { return kind; }
      internal set { kind = value; }
    }


    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Token position in the source text (starting at 0).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Position
    {
      get { return pos; } 
      internal set { pos = value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Token column (starting at 0).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Column
    {
      get { return col; } 
      internal set { col = value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Token line (starting at 1).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Line
    {
      get { return line; } 
      internal set { line = value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Token value.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Value
    {
      get { return val; } 
      internal set { val = value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Points to the previous token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token Previous { get; private set; }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the next token.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public Token Next
    {
      get { return next; }
      set
      {
        next = value;
        if (value != null) next.Previous = this;
      }
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the previous token.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public Token prev
    {
      get { return Previous; }
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of the token.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public override string ToString()
    {
      return Value ?? "<<null>>";
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a blank token object. Used by the parser.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public Token()
    {
    }

    //-----------------------------------------------------------------------------------
    /// <summary>
    /// Creates a token object with the specified value. All other fields are blank.
    /// </summary>
    /// <param name="tokenValue">The text value if the token.</param>
    //-----------------------------------------------------------------------------------
    public Token(string tokenValue)
    {
      val = tokenValue;
    }
  }
}