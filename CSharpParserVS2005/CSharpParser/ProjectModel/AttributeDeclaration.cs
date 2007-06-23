using System.Collections.Generic;
using CSharpParser.Collections;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represents an attribute declaration
  /// </summary>
  // ==================================================================================
  public sealed class AttributeDeclaration: LanguageElement
  {
    #region Private fields

    private string _Scope;
    private TypeReference _TypeReference;
    private readonly List<AttributeArgument> _Arguments = new List<AttributeArgument>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new attribute declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public AttributeDeclaration(Token token): base (token)
    {
    }

    #endregion

    #region Public Properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the scope of the attribute
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Scope
    {
      get { return _Scope; }
      set { _Scope = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type referenced in the attribute
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference TypeReference
    {
      get { return _TypeReference; }
      set
      {
        _TypeReference = value;
        Name = value.Name;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of arguments belonging to this attribute.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<AttributeArgument> Arguments
    {
      get { return _Arguments; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This class represents a collection of attribute declarations.
  /// </summary>
  // ==================================================================================
  public class AttributeCollection : RestrictedCollection<AttributeDeclaration>
  {
  }
}
