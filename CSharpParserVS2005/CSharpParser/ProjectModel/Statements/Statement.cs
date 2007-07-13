using System.Collections.Generic;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an abstract statement.
  /// </summary>
  // ==================================================================================
  public abstract class Statement : LanguageElement
  {
    #region Private fields

    private int _Depth;
    private string _Label;
    private Statement _Parent;

    #endregion

    #region Lifecycle methods 

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    protected Statement(Token token)
      : base(token)
    {
      _Depth = 0;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the depth of this statemenet in the statement hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int Depth
    {
      get { return _Depth; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the label of this statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Label
    {
      get { return _Label; }
      set { _Label = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent block of this statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Statement Parent
    {
      get { return _Parent; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this statemenet has a parent or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasParent
    {
      get { return _Parent != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the leftmost name in this statement.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string LeftmostName
    {
      get
      {
        ExpressionStatement expStm = this as ExpressionStatement;
        if (expStm != null) return expStm.Expression.LeftmostExpression.Name;
        return Name;
      }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the parent block of this statement.
    /// </summary>
    /// <param name="parent">Parent block.</param>
    // --------------------------------------------------------------------------------
    public void SetParent(Statement parent)
    {
      _Parent = parent;
      _Depth = _Parent.Depth + 1;
    }

    #endregion

    #region Iterator methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator enumerating all nested statements belonging to this one.
    /// </summary>
    /// <value>Returns recursively all nested statements.</value>
    /// <remarks>Statements are returned in the order of their declaration.</remarks>
    // --------------------------------------------------------------------------------
    public virtual IEnumerable<Statement> NestedStatements
    {
      get { yield break; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator enumerating all direclty nested statements belonging to this one.
    /// </summary>
    /// <value>Returns only the directly nested statements, does not do recursion.</value>
    /// <remarks>Statements are returned in the order of their declaration.</remarks>
    // --------------------------------------------------------------------------------
    public virtual IEnumerable<Statement> DirectNestedStatements
    {
      get { yield break; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Iterator enumerating this and all nested statements belonging to this one.
    /// </summary>
    /// <value>Returns recursively all nested statements.</value>
    /// <remarks>
    /// First this statement is returned than the nested statements in the order of 
    /// their declaration.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public virtual IEnumerable<Statement> AllStatements
    {
      get
      {
        yield return this;
        foreach (Statement stmt in NestedStatements)
        {
          yield return stmt;
        }
      }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a list of statements.
  /// </summary>
  // ==================================================================================
  public class StatementCollection : List<Statement>
  {
    private readonly Statement _Parent;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a list of statements indicating the parent of the collection.
    /// </summary>
    /// <param name="parent"></param>
    // --------------------------------------------------------------------------------
    public StatementCollection(Statement parent)
    {
      _Parent = parent;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent statement of the collection.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Statement Parent
    {
      get { return _Parent; }
    }
  }
}
