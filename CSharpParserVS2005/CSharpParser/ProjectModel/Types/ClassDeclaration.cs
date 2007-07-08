using System.Text;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a C# class declaration
  /// </summary>
  // ==================================================================================
  public sealed class ClassDeclaration: ClasslikeTypeDeclaration
  {
    #region Private fields

    private bool _IsAbstract;
    private bool _IsStatic;
    private bool _IsSealed;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new class declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance</param>
    // --------------------------------------------------------------------------------
    public ClassDeclaration(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type has an 'abstract' modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsAbstract
    {
      get { return _IsAbstract; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type has a 'static' modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsStatic
    {
      get { return _IsStatic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type has an 'sealed' modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsSealed
    {
      get { return _IsSealed; }
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the properties using the modifiers.
    /// </summary>
    /// <param name="mod">Modifier enumeration value.</param>
    // --------------------------------------------------------------------------------
    public override void SetModifiers(Modifier mod)
    {
      base.SetModifiers(mod);
      _IsAbstract = (mod & Modifier.@abstract) != 0;
      _IsSealed = (mod & Modifier.@sealed) != 0;
      _IsStatic = (mod & Modifier.@static) != 0;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representing the modifiers of this class definition.
    /// </summary>
    /// <returns>
    /// String representation of modifiers.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override string GetModifiersText()
    {
      StringBuilder sb = new StringBuilder(base.GetModifiersText());
      if (_IsAbstract)
      {
        if (sb.Length > 0) sb.Append(' ');
        sb.Append("abstract");
      }

      if (_IsSealed)
      {
        if (sb.Length > 0) sb.Append(' ');
        sb.Append("sealed");
      }

      if (_IsStatic)
      {
        if (sb.Length > 0) sb.Append(' ');
        sb.Append("static");
      }

      return sb.ToString();
    }

    #endregion
  }
}
