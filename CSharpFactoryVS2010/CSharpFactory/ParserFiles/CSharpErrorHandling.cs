using CSharpFactory.ProjectModel;

namespace CSharpFactory.ParserFiles
{
  // ==================================================================================
  /// <summary>
  /// This part of the CSharpSyntaxParser class adds error handling to the CoCo/R
  /// generated parser.
  /// </summary>
  // ==================================================================================
  public partial class CSharpSyntaxParser
  {
    #region Private fields

    private bool _SuppressErrors = false;

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if error emission is suppressed or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool SuppressErrors
    {
      get { return _SuppressErrors; }
      set { _SuppressErrors = value; }
    }

    #endregion

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
      if (!_SuppressErrors)
      {
        CompilationUnit.ErrorHandler.Error(code, errorPoint, description, null);
      }
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
      if (!_SuppressErrors)
      {
        CompilationUnit.ErrorHandler.Error(code, errorPoint, description, parameters);
      }
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
      if (!_SuppressErrors)
      {
        CompilationUnit.ErrorHandler.Warning(code, errorPoint, description, null);
      }
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
      if (!_SuppressErrors)
      {
        CompilationUnit.ErrorHandler.Warning(code, errorPoint, description, parameters);
      }
    }

    #endregion

    #region Error methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0052: Inconsistent accessibility: field type '{0}' is less accessible 
    /// than field '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="field">filed name</param>
    /// <param name="type">Type name</param>
    // --------------------------------------------------------------------------------
    public void Error0052(Token token, string field, string type)
    {
      Error("CS0060", token,
        string.Format("Inconsistent accessibility: field type '{0}' is less " +
        "accessible than field '{1}'", type, field));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0060: Inconsistent accessibility: base class '{0}' is less 
    /// accessible than class '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    /// <param name="baseName">Base class name</param>
    // --------------------------------------------------------------------------------
    public void Error0060(Token token, string name, string baseName)
    {
      Error("CS0060", token,
        string.Format("Inconsistent accessibility: base class '{0}' is less " +
        "accessible than class '{1}'", name, baseName));
    }

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
    /// Error CS0112: A static member '{0}' cannot be marked as override, virtual, 
    /// or abstract.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Mismatched name</param>
    // --------------------------------------------------------------------------------
    public void Error0112(Token token, string name)
    {
      Error("CS0112", token,
        string.Format("A static member '{0}' cannot be marked as override, virtual, " +
        "or abstract", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0113: A static member '{0}' cannot be marked as override, virtual, 
    /// or abstract.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Mismatched name</param>
    // --------------------------------------------------------------------------------
    public void Error0113(Token token, string name)
    {
      Error("CS0113", token,
        string.Format("A member '{0}' marked as override cannot be marked as " +
        "new or virtual", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0116: A namespace does not directly contain members such as fields 
    /// or methods.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0116(Token token)
    {
      Error("CS0116", token, 
        "A namespace does not directly contain members such as fields or methods");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0118: '{0}' is a '{1}' but is used like a '{2}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Construct name</param>
    /// <param name="construct1">Real construct</param>
    /// <param name="construct2">Construct used as</param>
    // --------------------------------------------------------------------------------
    public void Error0118(Token token, string name, string construct1, string construct2)
    {
      Error("CS0118", token,
        string.Format("'{0}' is a '{1}' but is used like a '{2}'", name, construct1, construct2));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0128: A local variable named '{0}' is already defined in this scope.
    /// </summary>
    /// <param name="localVariable">Local variable already declared</param>
    // --------------------------------------------------------------------------------
    public void Error0128(LocalVariable localVariable)
    {
      Error((string) "CS0128", (Token) localVariable.Token,
        string.Format("A local variable named '{0}' is already defined in this scope",
        localVariable.Name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0132: '{0}': a static constructor must be parameterless.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Mismatched name</param>
    // --------------------------------------------------------------------------------
    public void Error0132(Token token, string name)
    {
      Error("CS0132", token,"'{0}': a static constructor must be parameterless", name);
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
      Error((string) "CS0136", (Token) localVariable.Token,
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
    /// Error CS0146: Circular base class dependency involving '{0}' and '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    /// <param name="baseName">Base class name</param>
    // --------------------------------------------------------------------------------
    public void Error0146(Token token, string name, string baseName)
    {
      Error("CS0146", token,
        string.Format("Circular base class dependency involving '{0}' and '{1}'", 
        name, baseName));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0180: '{0}' cannot be both extern and abstract.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0180(Token token, string name)
    {
      Error("CS0180", token, "'{0}' cannot be both extern and abstract", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0208: Cannot take the address of, get the size of, or declare a 
    /// pointer to a managed type ('{0}').
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Mismatched name</param>
    // --------------------------------------------------------------------------------
    public void Error0208(Token token, string name)
    {
      Error("CS0208", token,
        string.Format("Cannot take the address of, get the size of, or declare a " +
        "pointer to a managed type ('{0}')", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0215: The return type of operator True or False must be bool.
    /// constraints.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0215(Token token)
    {
      Error("CS0215", token, "The return type of operator True or False must be bool");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0216: The operator '{0}' requires a matching operator '{1}' to also 
    /// be defined.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="opName">Operator name</param>
    /// <param name="opPair">Missing operator name</param>
    // --------------------------------------------------------------------------------
    public void Error0216(Token token, string opName, string opPair)
    {
      Error("CS0216", token, 
        "The operator '{0}' requires a matching operator '{1}' to also be defined",
        opName, opPair);
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
    /// Error CS0238: '{0}' cannot be both extern and abstract.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0238(Token token, string name)
    {
      Error("CS0238", token, "'{0}' cannot be sealed because it is not " +
        "an override", name);
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
    /// Error CS0260: Missing partial modifier on declaration of type '{0}'; another 
    /// partial declaration of this type exists
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0260(Token token, string name)
    {
      Error("CS0260", token,
        string.Format("Missing partial modifier on declaration of type '{0}'; " +
        "another partial declaration of this type exists", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0261: Partial declarations of '{0}' must be all classes, all 
    /// structs, or all interfaces
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0261(Token token, string name)
    {
      Error("CS0261", token,
        string.Format("Partial declarations of '{0}' must be all classes, all " +
        "structs, or all interfaces", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0262: Partial declarations of '{0}' have conflicting accessibility
    ///  modifiers.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0262(Token token, string name)
    {
      Error("CS0262", token,
        string.Format("Partial declarations of '{0}' have conflicting " +
        "accessibility modifiers", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0263: Partial declarations of '{0}' must not specify different 
    /// base classes.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0263(Token token, string name)
    {
      Error("CS0263", token,
        string.Format("Partial declarations of '{0}' must not specify different " +
        "base classes", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0273: The accessibility modifier of the '{0}' accessor must be more 
    /// restrictive than the property or indexer '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="prop">Property name</param>
    /// <param name="name">Accessor name</param>
    // --------------------------------------------------------------------------------
    public void Error0273(Token token, string prop, string name)
    {
      Error("CS0273", token,
        "The accessibility modifier of the '{0}' accessor must be more restrictive " +
        "than the property or indexer '{1}'", name, prop);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0274: Cannot specify accessibility modifiers for both accessors of 
    /// the property or indexer '{0}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Accessor name</param>
    // --------------------------------------------------------------------------------
    public void Error0274(Token token, string name)
    {
      Error("CS0274", token,
        "Cannot specify accessibility modifiers for both accessors of the property" +
        " or indexer '{0}'", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0275: '{0}': accessibility modifiers may not be used on accessors in 
    /// an interface.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Accessor name</param>
    // --------------------------------------------------------------------------------
    public void Error0275(Token token, string name)
    {
      Error("CS0275", token,
        "'{0}': accessibility modifiers may not be used on accessors in an interface", 
        name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0276: '{0}': accessibility modifiers on accessors may only be used if 
    /// the property or indexer has both a get and a set accessor.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Accessor name</param>
    // --------------------------------------------------------------------------------
    public void Error0276(Token token, string name)
    {
      Error("CS0276", token,
        "'{0}': accessibility modifiers on accessors may only be used if the " +
        "property or indexer has both a get and a set accessor", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0283: The type '{0}' cannot be declared const.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0283(Token token, string name)
    {
      Error("CS0283", token,
        string.Format("The type '{0}' cannot be declared const", name));
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
    /// Error CS0401: The new() constraint must be the last constraint specified.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0401(Token token)
    {
      Error("CS0401", token,
        "The new() constraint must be the last constraint specified");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0405: Duplicate constraint '{0}' for type parameter '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    /// <param name="constraint">Constraint name</param>
    // --------------------------------------------------------------------------------
    public void Error0405(Token token, string name, string constraint)
    {
      Error("CS0405", token,
        string.Format("Duplicate constraint '{0}' for type parameter '{1}'", 
        name, constraint));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0406: The class type constraint '{0}' must come before any other 
    /// constraints.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Special class name</param>
    // --------------------------------------------------------------------------------
    public void Error0406(Token token, string name)
    {
      Error("CS0406", token, string.Format("The class type constraint '{0}' must " +
        "come before any other constraints", name));
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
    /// Error CS0418: '{0}': an abstract class cannot be sealed or static.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Missing alias</param>
    // --------------------------------------------------------------------------------
    public void Error0418(Token token, string name)
    {
      Error("CS0418", token,
        string.Format("'{0}': an abstract class cannot be sealed or static", name));
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
    /// Error CS0441: '{0}': a class cannot be both static and sealed.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Missing alias</param>
    // --------------------------------------------------------------------------------
    public void Error0441(Token token, string name)
    {
      Error("CS0441", token,
        string.Format("'{0}': a class cannot be both static and sealed",
        name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0448: The return type for ++ or -- operator must be the containing 
    /// type or derived from the containing type.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0448(Token token)
    {
      Error("CS0448", token,
        "The return type for ++ or -- operator must be the containing type or " +
        "derived from the containing type");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0449: The 'class' or 'struct' constraint must come before any other 
    /// constraints.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0449(Token token)
    {
      Error("CS0449", token, 
        "The 'class' or 'struct' constraint must come before any other constraints");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0451: The 'new()' constraint cannot be used with the 'struct' 
    /// constraint.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0451(Token token)
    {
      Error("CS0451", token,
        "The 'new()' constraint cannot be used with the 'struct' constraint");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0454: Circular constraint dependency involving '{0}' and '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name1">First name</param>
    /// <param name="name2">Second name</param>
    // --------------------------------------------------------------------------------
    public void Error0454(Token token, string name1, string name2)
    {
      Error("CS0454", token,
        string.Format("Circular constraint dependency involving '{0}' and '{1}'",
        name1, name2));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0455: Type parameter '{0}' inherits conflicting constraints '{1}' 
    /// and '{2}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name1">First name</param>
    /// <param name="name2">Second name</param>
    /// <param name="name3">Third name</param>
    // --------------------------------------------------------------------------------
    public void Error0455(Token token, string name1, string name2, string name3)
    {
      Error("CS0455", token, 
        string.Format("Type parameter '{0}' inherits conflicting constraints '{1}' " +
        "and '{2}'",
        name1, name2, name3));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0456: Circular constraint dependency involving '{0}' and '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name1">First name</param>
    /// <param name="name2">Second name</param>
    // --------------------------------------------------------------------------------
    public void Error0456(Token token, string name1, string name2)
    {
      Error("CS0456", token, "Type parameter '{0}' has the 'struct' constraint so " +
        "'{0}' cannot be used as a constraint for '{1}'",
        name1, name2);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0502: '{0}' cannot be both abstract and sealed.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0502(Token token, string name)
    {
      Error("CS0502", token, "'{0}' cannot be both abstract and sealed", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0503: The abstract method '{0}' cannot be marked virtual.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0503(Token token, string name)
    {
      Error("CS0503", token, "The abstract method '{0}' cannot be marked virtual", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0509: '{0}': cannot derive from sealed type '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    /// <param name="sealedName">Name of sealed type</param>
    // --------------------------------------------------------------------------------
    public void Error0509(Token token, string name, string sealedName)
    {
      Error("CS0509", token,
        string.Format("'{0}': cannot derive from sealed type '{1}'", name, sealedName));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0515: '{0}': access modifiers are not allowed on static constructors.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Method name</param>
    // --------------------------------------------------------------------------------
    public void Error0515(Token token, string name)
    {
      Error("CS0515", token,
        "'{0}': access modifiers are not allowed on static constructors", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0527: Type '{0}' in interface list is not an interface.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    // --------------------------------------------------------------------------------
    public void Error0527(Token token, string name)
    {
      Error("CS0527", token,
        string.Format("Type '{0}' in interface list is not an interface", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0528: '{0}' is already listed in interface list.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    // --------------------------------------------------------------------------------
    public void Error0528(Token token, string name)
    {
      Error("CS0528", token,
        string.Format("'{0}' is already listed in interface list", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0529: Circular base class dependency involving '{0}' and '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    /// <param name="baseName">Base class name</param>
    // --------------------------------------------------------------------------------
    public void Error0529(Token token, string name, string baseName)
    {
      Error("CS0529", token,
        string.Format("Inherited interface '{0}' causes a cycle in the interface " +
        "hierarchy of '{1}'", name, baseName));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0542: '{0}': member names cannot be the same as their enclosing type.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0542(Token token, string name)
    {
      Error("CS0542", token,
        string.Format("'{0}': member names cannot be the same as their enclosing type", 
        name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0547: '{0}' is not an attribute class.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0547(Token token, string name)
    {
      Error("CS0547", token, "'{0}': property or indexer cannot have void type", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0552: '{0}': user-defined conversion to/from interface.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0552(Token token, string name)
    {
      Error("CS0552", token, "'{0}': user-defined conversion to/from interface", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0555: User-defined operator cannot take an object of the enclosing 
    /// type and convert to an object of the enclosing type.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0555(Token token)
    {
      Error("CS0555", token,
        "User-defined operator cannot take an object of the enclosing type and " +
        "convert to an object of the enclosing type");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0556: User-defined conversion must convert to or from the enclosing 
    /// type.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0556(Token token)
    {
      Error("CS0556", token,
        "User-defined conversion must convert to or from the enclosing type");
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
    /// Error CS0558: User-defined operator '{0}' must be declared static and public.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    // --------------------------------------------------------------------------------
    public void Error0558(Token token, string name)
    {
      Error("CS0558", token, 
        "User-defined operator '{0}' must be declared static and public", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0559: The parameter type for ++ or -- operator must be the 
    /// containing type.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0559(Token token)
    {
      Error("CS0559", token,
        "The parameter type for ++ or -- operator must be the containing type");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0562: The parameter of a unary operator must be the containing type.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0562(Token token)
    {
      Error("CS0562", token, 
        "The parameter of a unary operator must be the containing type");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0563: One of the parameters of a binary operator must be the 
    /// containing type.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0563(Token token)
    {
      Error("CS0563", token,
        "One of the parameters of a binary operator must be the containing type");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0573: '{0}': cannot have instance field initializers in structs.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0573(Token token, string name)
    {
      Error("CS0573", token,
        string.Format("'{0}': cannot have instance field initializers in structs", name));
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
    /// Error CS0590: User-defined operators cannot return void
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0590(Token token)
    {
      Error("CS0590", token, "User-defined operators cannot return void");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0616: '{0}' is not an attribute class.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0616(Token token, string name)
    {
      Error("CS0616", token,
        string.Format("'{0}' is not an attribute class", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0620: Indexers cannot have void type
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0620(Token token)
    {
      Error("CS0620", token, "Indexers cannot have void type");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0621: '{0}': virtual or abstract members cannot be private.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error0621(Token token, string name)
    {
      Error("CS0621", token, "ref and out are not valid in this context", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0631: ref and out are not valid in this context.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0631(Token token)
    {
      Error("CS0631", token, "ref and out are not valid in this context");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0644: '{0}' cannot derive from special class '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    /// <param name="specialName">Special class name</param>
    // --------------------------------------------------------------------------------
    public void Error0644(Token token, string name, string specialName)
    {
      Error("CS0644", token,
        string.Format("'{0}' cannot derive from special class '{1}'", name, specialName));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0663: '{0}': virtual or abstract members cannot be private.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    // --------------------------------------------------------------------------------
    public void Error0663(Token token, string name)
    {
      Error("CS0663", token,
        "'{0}' cannot define overloaded methods that differ only on ref and out", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0670: Field cannot have void type.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0670(Token token)
    {
      Error("CS0670", token, "Field cannot have void type");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0677: '{0}': a volatile field cannot be of the type '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    /// <param name="typeName">Type name</param>
    // --------------------------------------------------------------------------------
    public void Error0677(Token token, string name, string typeName)
    {
      Error("CS0677", token,
        string.Format("'{0}': a volatile field cannot be of the type '{1}'", 
        name, typeName));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0678: '{0}': a field cannot be both volatile and readonly.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Field name</param>
    // --------------------------------------------------------------------------------
    public void Error0678(Token token, string name)
    {
      Error("CS0678", token,
        string.Format("'{0}': a field cannot be both volatile and readonly", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0681: The modifier 'abstract' is not valid on fields. Try using a 
    /// property instead.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error0681(Token token)
    {
      Error("CS0681", token, "The modifier 'abstract' is not valid on fields. " +
      "Try using a property instead.");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0689: Cannot derive from '{0}' because it is a type parameter
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0689(Token token, string name)
    {
      Error("CS0689", token, 
        string.Format("Cannot derive from '{0}' because it is a type parameter", name));
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
    /// Error CS0694: Type parameter '{0}' has the same name as the containing type, 
    /// or method.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0694(Token token, string name)
    {
      Error("CS0694", token, "Type parameter '{0}' has the same name as the " +
        "containing type, or method", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0699: '{0}' does not define type parameter '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Type name</param>
    /// <param name="paramName">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0699(Token token, string name, string paramName)
    {
      Error("CS0699", token, 
        string.Format("'{0}' does not define type parameter '{1}'", name, paramName));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0701: '{0}' is not a valid constraint. A type used as a constraint
    /// must be an interface, a non-sealed class or a type parameter.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0701(Token token, string name)
    {
      Error("CS0701", token,
        string.Format("'{0}' is not a valid constraint. A type used as a constraint " +
        "must be an interface, a non-sealed class or a type parameter.", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0702: TConstraint cannot be special class '{0}'
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Special class name</param>
    // --------------------------------------------------------------------------------
    public void Error0702(Token token, string name)
    {
      Error("CS0702", token,
        string.Format("Constraint cannot be special class '{0}'", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0709: '{0}': cannot derive from static class '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    /// <param name="baseName">Base class name</param>
    // --------------------------------------------------------------------------------
    public void Error0709(Token token, string name, string baseName)
    {
      Error("CS0709", token,
        string.Format("'{0}': cannot derive from static class '{1}'", name, baseName));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0713: Static class '{0}' cannot derive from type '{1}'. Static classes
    /// must derive from object.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    /// <param name="baseName">Base class name</param>
    // --------------------------------------------------------------------------------
    public void Error0713(Token token, string name, string baseName)
    {
      Error("CS0713", token,
        string.Format("Static class '{0}' cannot derive from type '{1}'. Static classes "+
        "must derive from object.", name, baseName));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0714: '{0}': static classes cannot implement interfaces
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0714(Token token, string name)
    {
      Error("CS0714", token,
        string.Format("'{0}': static classes cannot implement interfaces", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0723: Cannot declare variable of static type '{0}'
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error0723(Token token, string name)
    {
      Error("CS0723", token,
        string.Format("Cannot declare variable of static type '{0}'", name));
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
    /// Error CS1004: Duplicate '{0}' modifier.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="modifier">Modifier symbol</param>
    // --------------------------------------------------------------------------------
    public void Error1004(Token token, string modifier)
    {
      Error("CS1004", token, string.Format("Duplicate '{0}' modifier", modifier));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1008: Type byte, sbyte, short, ushort, int, uint, long, or 
    /// ulong expected.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error1008(Token token)
    {
      Error("CS1008", token, 
        "Type byte, sbyte, short, ushort, int, uint, long, or ulong expected");
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
    /// Error CS1530: Keyword new not allowed on namespace elements.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error1530(Token token)
    {
      Error("CS1530", token, "Keyword new not allowed on namespace elements");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1534: Overloaded binary operator '{0}' takes two parameters.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error1534(Token token, string name)
    {
      Error("CS1534", token, 
        "Overloaded binary operator '{0}' takes two parameters", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1535: Overloaded unary operator '{0}' takes one parameter.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error1535(Token token, string name)
    {
      Error("CS1535", token,
        "Overloaded unary operator '{0}' takes one parameter", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1547: Keyword '{0}' cannot be used in this context.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Error1547(Token token, string name)
    {
      Error("CS1547", token, "Keyword '{0}' cannot be used in this context", name);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1551: Indexers must have at least one parameter
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Error1551(Token token)
    {
      Error("CS1551", token, "Indexers must have at least one parameter");
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1614: '{0}' is ambiguous between '{0}' and '{0}Attribute'; use either 
    /// '@X' or 'XAttribute'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="name">Member name</param>
    // --------------------------------------------------------------------------------
    public void Error1614(Token token, string name)
    {
      Error("CS1614", token,
        string.Format("'{0}' is ambiguous between '{0}' and '{0}Attribute'; " +
        "use either '@X' or 'XAttribute'", name));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1721: Class '{0}' cannot have multiple base classes: '{1}' and '{2}'
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="type">Conflicting type</param>
    /// <param name="base1">First base type</param>
    /// <param name="base2">Second base type</param>
    // --------------------------------------------------------------------------------
    public void Error1721(Token token, string type, string base1, string base2)
    {
      Error("CS1721", token,
        string.Format("Class '{0}' cannot have multiple base classes: '{1}' and '{2}'",
        type, base1, base2));
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Error CS1721: Base class '{0}' must come before any interfaces.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="type">Conflicting type</param>
    // --------------------------------------------------------------------------------
    public void Error1722(Token token, string type)
    {
      Error("CS1722", token,
        string.Format("Base class '{0}' must come before any interfaces", type));
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS1570: XML comment on '{0}' has badly formed XML -- '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="member">Member name</param>
    /// <param name="reason">Reason text</param>
    // --------------------------------------------------------------------------------
    public void Warning1570(Token token, string member, string reason)
    {
      Warning("CS1570", token,
        "XML comment on '{0}' has badly formed XML -- '{1}'", member, reason);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS1571: XML comment on '{0}' has a duplicate param tag for '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="member">Member name</param>
    /// <param name="parameter">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Warning1571(Token token, string member, string parameter)
    {
      Warning("CS1571", token,
        "XML comment on '{0}' has a duplicate param tag for '{1}'", member, parameter);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS1572: XML comment on '{0}' has a param tag for '{1}', but there is 
    /// no parameter by that name.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="member">Member name</param>
    /// <param name="parameter">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Warning1572(Token token, string member, string parameter)
    {
      Warning("CS1572", token,
        "XML comment on '{0}' has a param tag for '{1}', but there is no parameter " +
        "by that name", member, parameter);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS1573: Parameter '{0}' has no matching param tag in the XML comment 
    /// for '{1}' (but other parameters do).
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="member">Member name</param>
    /// <param name="parameter">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Warning1573(Token token, string member, string parameter)
    {
      Warning("CS1573", token,
        "Parameter '{0}' has no matching param tag in the XML comment for '{1}' " +
        "(but other parameters do)", parameter, member);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS1587: XML comment is not placed on a valid language element.
    /// </summary>
    /// <param name="token">Error point</param>
    // --------------------------------------------------------------------------------
    public void Warning1587(Token token)
    {
      Warning("CS1587", token, "XML comment is not placed on a valid language element");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS1591: Missing XML comment for publicly visible type or member '{0}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="type">Type name</param>
    // --------------------------------------------------------------------------------
    public void Warning1591(Token token, string type)
    {
      Warning("CS1591", token, 
        "Missing XML comment for publicly visible type or member '{0}'", type);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS1710: XML comment on '{0}' has a duplicate typeparam tag for '{1}'.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="member">Member name</param>
    /// <param name="parameter">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Warning1710(Token token, string member, string parameter)
    {
      Warning("CS1710", token,
        "XML comment on '{0}' has a duplicate typeparam tag for '{1}'", 
        member, parameter);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS1711: XML comment on '{0}' has a typeparam tag for '{1}', but there 
    /// is no type parameter by that name.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="member">Member name</param>
    /// <param name="parameter">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Warning1711(Token token, string member, string parameter)
    {
      Warning("CS1711", token,
        "XML comment on '{0}' has a typeparam tag for '{1}', but there is no type " +
        "parameter by that name", member, parameter);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Warning CS1712: Type parameter '{0}' has no matching typeparam tag in the XML 
    /// comment on '{1}.
    /// </summary>
    /// <param name="token">Error point</param>
    /// <param name="member">Member name</param>
    /// <param name="parameter">Parameter name</param>
    // --------------------------------------------------------------------------------
    public void Warning1712(Token token, string member, string parameter)
    {
      Warning("CS1712", token,
        "Type parameter '{0}' has no matching typeparam tag in the XML comment on '{1}", 
        parameter, member);
    }

    #endregion
  }
}