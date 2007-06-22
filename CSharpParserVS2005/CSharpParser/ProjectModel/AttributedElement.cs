using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class is the common base class of all language elements having attributes.
  /// </summary>
  // ==================================================================================
  public abstract class AttributedElement : LanguageElement
  {
    private AttributeCollection _Attributes = new AttributeCollection();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an attributed element descriptor according to the info provided by the
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    // --------------------------------------------------------------------------------
    protected AttributedElement(Token token) : base(token)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an attributed element descriptor according to the info provided by the
    /// specified token and name.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    /// <param name="name">Language element name</param>
    // --------------------------------------------------------------------------------
    protected AttributedElement(Token token, string name)
      : base(token, name)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an attributed element descriptor according to the info provided by the
    /// specified token.
    /// </summary>
    /// <param name="token">Token providing information about the element.</param>
    /// <param name="parser">Parser instance</param>
    // --------------------------------------------------------------------------------
    protected AttributedElement(Token token, Parser parser)
      : base(token, parser)
    {
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or the collection of attributes belonging to this type declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public AttributeCollection Attributes
    {
      get { return _Attributes; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets or the collection of attributes belonging to this type declaration.
    /// </summary>
    /// <param name="attrs">Attributes to assign</param>
    // --------------------------------------------------------------------------------
    public void AssignAttributes(AttributeCollection attrs)
    {
      if (attrs == null)
      {
        _Attributes.Clear();
      }
      else
      {
        _Attributes = attrs;
      }
    }
  }
}