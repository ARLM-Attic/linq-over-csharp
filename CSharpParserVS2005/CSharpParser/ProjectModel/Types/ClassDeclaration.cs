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

    private bool _IsStatic;
    private FinalizerDeclaration _Finalizer;

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
    /// Gets the flag indicating if this type has a 'static' modifier or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsStatic
    {
      get { return _IsStatic; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the finalizer of this type declarations
    /// </summary>
    // --------------------------------------------------------------------------------
    public FinalizerDeclaration Finalizer
    {
      get { return _Finalizer; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the type has a finalizer or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasFinalizer
    {
      get { return _Finalizer != null; }
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
      if (IsAbstract)
      {
        if (sb.Length > 0) sb.Append(' ');
        sb.Append("abstract");
      }

      if (IsSealed)
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

    #region Internal methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if this class has already a finalizer or not.
    /// </summary>
    /// <param name="finalizer">Finalizer declared in the class.</param>
    /// <remarks>
    /// Raises a compilation error, if the class already has a finalizer.
    /// </remarks>
    // --------------------------------------------------------------------------------
    internal bool HasAlreadyFinalizer(FinalizerDeclaration finalizer)
    {
      if (HasFinalizer)
      {
        Parser.Error0111(finalizer.Token, Name, finalizer.Signature);
        return true;
      }
      _Finalizer = finalizer;
      return false;
    }

    #endregion
  }
}
