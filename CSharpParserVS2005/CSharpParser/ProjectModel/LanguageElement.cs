using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class is the common base class of all language element descriptors.
  /// </summary>
  // ==================================================================================
  public abstract class LanguageElement
  {
    private string _Name;
    private int _StartLine;
    private int _StartColumn;
    private int _StartPosition;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a language element descriptor according to the info provided by the
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    // --------------------------------------------------------------------------------
    protected LanguageElement(Token token)
    {
      _StartLine = token.line;
      _StartColumn = token.col;
      _StartPosition = token.pos;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual string Name
    {
      get { return _Name; }
      set { _Name = value; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the line number of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartLine
    {
      get { return _StartLine; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column number of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartColumn
    {
      get { return _StartColumn; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the start position of the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartPosition
    {
      get { return _StartPosition; }
    }
  }
}
