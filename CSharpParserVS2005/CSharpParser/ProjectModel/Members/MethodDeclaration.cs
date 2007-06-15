using System.Collections.Generic;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a method declaration.
  /// </summary>
  // ==================================================================================
  public class MethodDeclaration : MemberDeclaration, IBlockOwner
  {
    #region Private fields

    private TypeParameterCollection _TypeParameters = new TypeParameterCollection();
    private FormalParameterCollection _FormalParameters = new FormalParameterCollection();
    private List<TypeParameterConstraint> _ParameterConstraints
      = new List<TypeParameterConstraint>();
    private List<Statement> _Statements = new List<Statement>();
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
    public List<Statement> Statements
    {
      get { return _Statements; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets or the collection of attributes belonging to this method declaration.
    /// </summary>
    /// <param name="pars">Expression parameters to assign.</param>
    // --------------------------------------------------------------------------------
    public void AssignTypeParameters(TypeParameterCollection pars)
    {
      if (pars == null)
      {
        _TypeParameters.Clear();
      }
      else
      {
        _TypeParameters = pars;
      }
    }

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

    #region IBlockOwner Members

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
  }
}
