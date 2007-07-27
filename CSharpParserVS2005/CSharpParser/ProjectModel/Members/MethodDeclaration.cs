using System;
using System.Collections.Generic;
using System.Text;
using CSharpParser.Collections;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a method declaration.
  /// </summary>
  // ==================================================================================
  public class MethodDeclaration : MemberDeclaration, IBlockOwner, ITypeParameterOwner
  {
    #region Private fields

    private readonly TypeParameterCollection _TypeParameters = new TypeParameterCollection();
    private readonly FormalParameterCollection _FormalParameters = new FormalParameterCollection();
    private readonly List<TypeParameterConstraint> _ParameterConstraints
      = new List<TypeParameterConstraint>();
    private readonly StatementCollection _Statements = new StatementCollection(null);
    private bool _HasBody;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new method declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public MethodDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of formal parameters belonging to the method.
    /// </summary>
    // --------------------------------------------------------------------------------
    public FormalParameterCollection FormalParameters
    {
      get { return _FormalParameters; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of type parameters belonging to the method.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeParameterCollection TypeParameters
    {
      get { return _TypeParameters; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of type parameter constraints belonging to the method.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<TypeParameterConstraint> ParameterConstraints
    {
      get { return _ParameterConstraints; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of statements in the method body.
    /// </summary>
    // --------------------------------------------------------------------------------
    public StatementCollection Statements
    {
      get { return _Statements; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this method has a body or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasBody
    {
      get { return _HasBody; }
      set { _HasBody = value; }
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the signature of this method.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Signature
    {
      get
      {
        StringBuilder sb = new StringBuilder(100);
        bool isFirst;
        sb.Append(FullName);
        if (_TypeParameters.Count != 0)
        {
          sb.Append('<');
          isFirst = true;
          foreach (TypeParameter par in _TypeParameters)
          {
            if (!isFirst)
            {
              sb.Append(", ");
            }
            sb.Append(par.Name);
            isFirst = false;
          }
          sb.Append('>');
        }
        sb.Append('(');
        isFirst = true;
        foreach (FormalParameter par in _FormalParameters)
        {
          if (!isFirst)
          {
            sb.Append(", ");
          }
          if (par.Kind != FormalParameterKind.In)
          {
            sb.Append(par.Kind.ToString().ToLower());
            sb.Append(' ');
          }
          sb.Append(par.Type.FullName);
          isFirst = false;
        }
        sb.Append(')');
        return sb.ToString();
      }
    }

    #endregion

    #region IBlockOwner Members

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the element owning the block;
    /// </summary>
    // --------------------------------------------------------------------------------
    public LanguageElement Owner
    {
      get { return this; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new statement to the method body.
    /// </summary>
    /// <param name="statement">Statement to add.</param>
    // --------------------------------------------------------------------------------
    public void Add(Statement statement)
    {
      Statements.Add(statement);
    }

    #endregion

    #region ITypeParameterOwner members

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new type parameter to the method declaration
    /// </summary>
    /// <param name="parameter">Type parameter</param>
    // --------------------------------------------------------------------------------
    public void AddTypeParameter(TypeParameter parameter)
    {
      try
      {
        _TypeParameters.Add(parameter);
      }
      catch (ArgumentException)
      {
        Parser.Error("CS0692", parameter.Token,
          String.Format("Duplicate type parameter '{0}'", parameter.Name));
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new type parameter constraint to the type declaration
    /// </summary>
    /// <param name="constraint">Type parameter constraint</param>
    // --------------------------------------------------------------------------------
    public void AddTypeParameterConstraint(TypeParameterConstraint constraint)
    {
      try
      {
        _ParameterConstraints.Add(constraint);
      }
      catch (ArgumentException)
      {
        Parser.Error("CS0409", constraint.Token,
          String.Format("A constraint clause has already been specified for type " +
          "parameter '{0}'. All of the constraints for a type parameter must be " +
          "specified in a single where clause.", constraint.Name));
      }
    }

    #endregion

    #region IResolutionRequired implementation

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
      Statement.ResolveTypeReferences(this, contextType, contextInstance);
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of method declarations that can be indexed by the
  /// signature of the method.
  /// </summary>
  // ==================================================================================
  public class MethodDeclarationCollection : RestrictedIndexedCollection<MethodDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">MethodDeclaration item.</param>
    /// <returns>
    /// Signature of the member declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(MethodDeclaration item)
    {
      return item.Signature;
    }
  }
}
