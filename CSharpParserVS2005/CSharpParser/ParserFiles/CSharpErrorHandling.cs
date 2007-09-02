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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new warning instance.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    // --------------------------------------------------------------------------------
    public void Warning(string code, Token errorPoint, string description)
    {
      _CompilationUnit.ErrorHandler.Warning(code, errorPoint, description, null);
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
    public void Warning(string code, Token errorPoint, string description,
      params object[] parameters)
    {
      _CompilationUnit.ErrorHandler.Warning(code, errorPoint, description, parameters);
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
    /// Error CS0106: The modifier '{0}' is not valid for this item.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="modifier">Modifier information</param>
    // --------------------------------------------------------------------------------
    public void Error0106(Token token, string modifier)
    {
      Error("CS0106", token, 
        string.Format("The modifier '{0}' is not valid for this item", modifier));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0107: More than one protection modifier.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0107(Token token)
    {
      Error("CS0107", token, "More than one protection modifier");
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
    /// Error CS0138: A using namespace directive can only be applied to namespaces; 
    /// '{0}' is a type not a namespace.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Mismatched name</param>
    // --------------------------------------------------------------------------------
    public void Error0138(Token token, string name)
    {
      Error("CS0138", token,
        string.Format("A using namespace directive can only be applied to namespaces; " +
        "'{0}' is a type not a namespace", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0234: The type or namespace name '{0}' does not exist in the namespace 
    /// '{1}' (are you missing an assembly reference?).
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    /// <param name="nameSpace">Namespace</param>
    // --------------------------------------------------------------------------------
    public void Error0234(Token token, string name, string nameSpace)
    {
      Error("CS0234", token,
        string.Format("The type or namespace name '{0}' does not exist in the "+
        "namespace '{1}' (are you missing an assembly reference?)", name, nameSpace));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0246: The type or namespace name '{0}' could not be found (are  you 
    /// missing a using directive or an assembly reference?)
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0246(Token token, string name)
    {
      Error("CS0246", token,
        string.Format("The type or namespace name '{0}' could not be found (are you "+
        "missing a using directive or an assembly reference?)", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0400: The type or namespace name '{0}' could not be found in the 
    /// global namespace (are you missing an assembly reference?)
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0400(Token token, string name)
    {
      Error("CS0400", token,
        string.Format("The type or namespace name '{0}' could not be found in the "+
        "global namespace (are you missing an assembly reference?)", name));
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
    /// Error CS0426: The type name '{0}' does not exist in the type '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    /// <param name="type">Type name</param>
    // --------------------------------------------------------------------------------
    public void Error0426(Token token, string name, string type)
    {
      Error("CS0426", token,
        string.Format("The type name '{0}' does not exist in the type '{1}'", 
        name, type));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0430: The extern alias '{0}' was not specified in a /reference option
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Missing alias</param>
    // --------------------------------------------------------------------------------
    public void Error0430(Token token, string name)
    {
      Error("CS0430", token,
        string.Format("The extern alias '{0}' was not specified in a /reference option", 
        name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0431: Cannot use alias '{0}' with '::' since the alias references a 
    /// type. Use '.' instead.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Missing alias</param>
    // --------------------------------------------------------------------------------
    public void Error0431(Token token, string name)
    {
      Error("CS0431", token, 
        string.Format("Cannot use alias '{0}' with '::' since the alias references "+
        "a type. Use '.' instead.", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0432: Alias '{0}' not found
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Missing alias</param>
    // --------------------------------------------------------------------------------
    public void Error0432(Token token, string name)
    {
      Error("CS0432", token, string.Format("Alias '{0}' not found", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0433: The type '{0}' exists in both '{1}' and '{2}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="type">Conflicting type</param>
    /// <param name="location1">First conflicting location</param>
    /// <param name="location2">Second conflicting location</param>
    // --------------------------------------------------------------------------------
    public void Error0433(Token token, string type, string location1, string location2)
    {
      Error("CS0433", token, 
        string.Format("The type '{0}' exists in both '{1}' and '{2}'", 
        type, location1, location2));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0434: The namespace '{0}' in '{1}' conflicts with the type '{2}' 
    /// in '{3}'
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="nameSpace">Conflicting namespace</param>
    /// <param name="location1">Location of the namespace</param>
    /// <param name="type">Conflicting type</param>
    /// <param name="location2">Location of the type</param>
    // --------------------------------------------------------------------------------
    public void Error0434(Token token, string nameSpace, string location1,
      string type, string location2)
    {
      Error("CS0434", token, string.Format("The namespace '{0}' in '{1}' conflicts "+
        "with the type '{2}' in '{3}'", nameSpace, location1, type, location2));
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
    /// Error CS0576: Namespace '{0}' contains a definition conflicting with alias '{1}'
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    /// <param name="alias">Alias name</param>
    // --------------------------------------------------------------------------------
    public void Error0576(Token token, string name, string alias)
    {
      Error("CS0576", token,
        string.Format("Namespace '{0}' contains a definition conflicting with "+
        "alias '{1}'", name, alias));
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
    /// Error CS1527: Namespace elements cannot be explicitly declared as private, 
    /// protected, or protected internal.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error1527(Token token)
    {
      Error("CS1527", token, "Namespace elements cannot be explicitly declared as " +
        "private, protected, or protected internal");
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

    #region Warning methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS0435: The namespace '{0}' in source code conflicts with the imported
    /// type in '{1}'. Using the namespace in the source code.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="nameSpace">Namespace name</param>
    /// <param name="location">The location of the imported type</param>
    // --------------------------------------------------------------------------------
    public void Warning0435(Token token, string nameSpace, string location)
    {
      Warning("CS0435", token,
        string.Format("The namespace '{0}' in source code conflicts with the imported "+
        "type in '{1}'. Using the namespace in the source code.",
          nameSpace, location));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS0436: The type '{0}' declared in source code conflicts with the 
    /// imported type '{1}'. Using the one in the source code.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="type">Type name</param>
    /// <param name="location">Location of imported type</param>
    // --------------------------------------------------------------------------------
    public void Warning0436(Token token, string type, string location)
    {
      Warning("CS0436", token,
        string.Format("The type '{0}' in source code conflicts with the imported type "+
        "in '{1}'. Using the one in the source code.",
          type, location));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS0437: The type '{0}' in source code conflicts with the imported 
    /// namespace '{1}'. Using the type.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="type">Type name</param>
    /// <param name="name">Type name</param>
    // --------------------------------------------------------------------------------
    public void Warning0437(Token token, string type, string name)
    {
      Warning("CS0437", token,
        string.Format("The type '{0}' in source code conflicts with the imported "+
        "namespace '{1}'. Using the type.",
          type, name));
    }

    #endregion
  }
}