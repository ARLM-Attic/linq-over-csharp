using CSharpParser.ProjectModel;

namespace CSharpParser.ParserFiles
{
  // ==================================================================================
  /// <summary>
  /// This part of the CSharpSyntaxParser class adds error handling to the CoCo/R
  /// generated parser.
  /// </summary>
  // ==================================================================================
  public partial class CSharpSyntaxParser
  {
    #region Error handling

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new error instance.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    // --------------------------------------------------------------------------------
    public void Error(string code, Token errorPoint, string description)
    {
      _CompilationUnit.ErrorHandler.Error(code, errorPoint, description, null);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new error instance.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    /// <param name="parameters">Error parameters.</param>
    // --------------------------------------------------------------------------------
    public void Error(string code, Token errorPoint, string description,
      params object[] parameters)
    {
      _CompilationUnit.ErrorHandler.Error(code, errorPoint, description, parameters);
    }

    #endregion

    #region Error methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0101: The namespace '{0}' already contains a definition for '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="nameSpace">Namespace name</param>
    /// <param name="name">Type name</param>
    // --------------------------------------------------------------------------------
    public void Error0101(Token token, string nameSpace, string name)
    {
      Error("CS0101", token,
        string.Format("The namespace '{0}' already contains a definition for '{1}'", 
          nameSpace, name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0102: The type '{0}' already contains a definition for '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="type">Type name</param>
    /// <param name="member">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0102(Token token, string type, string member)
    {
      Error("CS0102", token,
        string.Format("The type '{0}' already contains a definition for '{1}'",
          type, member));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0111: Type '{0}' already defines a member called '{1}' with the
    /// same parameter types.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="type">Type name</param>
    /// <param name="member">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0111(Token token, string type, string member)
    {
      Error("CS0111", token,
        string.Format("Type '{0}' already defines a member called '{1}' with the " + 
        "same parameter types", type, member));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0128: A local variable named '{0}' is already defined in this scope.
    /// </summary>
    /// <param name="localVariable">Local variable already declared</param>
    // --------------------------------------------------------------------------------
    public void Error0128(LocalVariable localVariable)
    {
      Error("CS0128", localVariable.Token,
        string.Format("A local variable named '{0}' is already defined in this scope",
        localVariable.Name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0136: A local variable named '{0}' cannot be declared in this scope
    /// because it would give a different meaning to '{0}', which is already used
    /// in a '{1}' scope to denote something else.
    /// </summary>
    /// <param name="localVariable">Local variable already declared</param>
    /// <param name="scope">Declaration scope</param>
    // --------------------------------------------------------------------------------
    public void Error0136(LocalVariable localVariable, string scope)
    {
      Error("CS0136", localVariable.Token,
        string.Format("A local variable named '{0}' cannot be declared in this scope " +
        "because it would give a different meaning to '{0}', which is already used " +
        "in a '{1}' scope to denote something else",
        localVariable.Name, scope));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0409: A constraint clause has already been specified for type
    /// parameter '{0}'. All of the constraints for a type parameter must be
    /// specified in a single where clause.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0409(Token token, string name)
    {
      Error("CS0409", token,
        string.Format("A constraint clause has already been specified for type " +
          "parameter '{0}'. All of the constraints for a type parameter must be " +
          "specified in a single where clause.", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0557: Duplicate user-defined conversion in type '{0}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0557(Token token, string name)
    {
      Error("CS0557", token,
        string.Format("Duplicate user-defined conversion in type '{0}'", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0692: Duplicate type parameter '{0}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0692(Token token, string name)
    {
      Error("CS0692", token, string.Format("Duplicate type parameter '{0}'", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1003: Syntax error, '{0}' expected.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="missingSymbol">Missing symbol</param>
    // --------------------------------------------------------------------------------
    public void Error1003(Token token, string missingSymbol)
    {
      Error("CS1003", token,
        string.Format("Syntax error, '{0}' expected", missingSymbol));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1028: Unexpected preprocessor directive.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error1028(Token token)
    {
      Error("CS1028", token, "Unexpected preprocessor directive.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1029: #error: {0}.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="pragmaText">Missing symbol</param>
    // --------------------------------------------------------------------------------
    public void Error1029(Token token, string pragmaText)
    {
      Error("CS1029", token,
        string.Format("#error: {0}", pragmaText));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1032: Cannot define/undefine preprocessor symbols after first token 
    /// in file.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error1032(Token token)
    {
      Error("CS1032", token, 
        "Cannot define/undefine preprocessor symbols after first token in file.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1038: #endregion directive expected.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error1038(Token token)
    {
      Error("CS1038", token, "#endregion directive expected.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1040: Preprocessor directives must appear as the first non-whitespace 
    /// character on a line.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error1040(Token token)
    {
      Error("CS1040", token, "Preprocessor directives must appear as the first " + 
        "non-whitespace character on a line.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1517: #endregion directive expected.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error1517(Token token)
    {
      Error("CS1517", token, "Invalid preprocessor expression");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1576: #endregion directive expected.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error1576(Token token)
    {
      Error("CS1576", token, 
        "The line number specified for #line directive is missing or invalid.");
    }

    #endregion
  }
}