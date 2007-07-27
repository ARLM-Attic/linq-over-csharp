using CSharpParser.Collections;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an operator member declaration.
  /// </summary>
  // ==================================================================================
  public class OperatorDeclaration : MethodDeclaration
  {
    #region Private fields

    private Operator _Operator;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public OperatorDeclaration(Token token)
      : base(token)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the operator code.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Operator Operator
    {
      get { return _Operator; }
      set { _Operator = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this operator is a unary operator
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsUnary
    {
      get { return FormalParameters.Count == 1; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this operator is a binary operator
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsBinary
    {
      get { return FormalParameters.Count == 2; }
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of operator declarations that can be indexed by the
  /// signature of the method.
  /// </summary>
  // ==================================================================================
  public class OperatorDeclarationCollection : RestrictedIndexedCollection<OperatorDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">OperatorDeclaration item.</param>
    /// <returns>
    /// Signature of the operator declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(OperatorDeclaration item)
    {
      return item.Signature;
    }
  }
}
