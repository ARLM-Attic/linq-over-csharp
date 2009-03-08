using System.Collections.Generic;
using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a "fixed" statement.
  /// </summary>
  // ==================================================================================
  public sealed class FixedStatement : BlockStatement
  {
    #region Private fields

    private readonly List<ValueAssignmentStatement> _Assignments = 
      new List<ValueAssignmentStatement>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "fixed" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parentBlock">Block owning this statement.</param>
    // --------------------------------------------------------------------------------
    public FixedStatement(Token token, CSharpSyntaxParser parser, IBlockOwner parentBlock)
      : base(token, parser, parentBlock)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of assignment statements belonging to this "fixed" statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<ValueAssignmentStatement> Assignments
    {
      get { return _Assignments; }
    }

    #endregion

    #region Iterator methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator enumerating this and all nested statements belonging to this one.
    /// </summary>
    /// <value>Returns recursively all nested statements.</value>
    /// <remarks>
    /// First the variable declaration statements are returned, then the nested 
    /// statements in the order of their declaration.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override IEnumerable<Statement> AllStatements
    {
      get
      {
        foreach (ValueAssignmentStatement stmt in _Assignments)
        {
          yield return stmt;
        }
        foreach (Statement stmt in NestedStatements)
        {
          yield return stmt;
        }
      }
    }

    #endregion

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      foreach (ValueAssignmentStatement stm in _Assignments)
      {
        stm.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }
}