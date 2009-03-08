using System;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type represents the usage of a language element.
  /// </summary>
  // ==================================================================================
  public sealed class ElementUsage
  {
    #region private fields

    private readonly SourceFile _File;
    private readonly LanguageElement _Element;
    private readonly LanguageElement _ContextElement;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a usage element.
    /// </summary>
    /// <param name="file">Source file where the element is used.</param>
    /// <param name="element">Element used</param>
    // --------------------------------------------------------------------------------
    public ElementUsage(SourceFile file, LanguageElement element):
      this(file, element, null)
    {
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a usage element.
    /// </summary>
    /// <param name="file">Source file where the element is used.</param>
    /// <param name="element">Element used</param>
    /// <param name="contextElement">
    /// Element that uses the referenced element.
    /// </param>
    // --------------------------------------------------------------------------------
    public ElementUsage(SourceFile file, LanguageElement element, 
      LanguageElement contextElement)
    {
      if (file == null) throw new ArgumentNullException("file");
      if (element == null) throw new ArgumentNullException("element");
      _File = file;
      _Element = element;
      _ContextElement = contextElement;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file where the element is used.
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFile File
    {
      get { return _File; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the element referenced by this usage instance.
    /// </summary>
    // --------------------------------------------------------------------------------
    public LanguageElement Element
    {
      get { return _Element; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the context element that refers to this usage element.
    /// </summary>
    // --------------------------------------------------------------------------------
    public LanguageElement ContextElement
    {
      get { return _ContextElement; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the line number of the language element
    /// </summary>
    // --------------------------------------------------------------------------------
    public int Line
    {
      get { return _Element.Token.line; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the column number of the language element
    /// </summary>
    // --------------------------------------------------------------------------------
    public int Column
    {
      get { return _Element.Token.col; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the position of the language element within the source file.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int Position
    {
      get { return _Element.Token.pos;  }
    }

    #endregion
  }
}