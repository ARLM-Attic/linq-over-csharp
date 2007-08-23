using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a "new" operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class NewOperator : PrimaryOperator
  {
    #region Private fields

    private TypeReference _Type;
    private ArrayInitializer _Initializer;
    private readonly ArgumentList _Arguments = new ArgumentList();
    private readonly List<Expression> _Dimensions = new List<Expression>();
    private int _RunningDimensions;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a "new" operator.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    public NewOperator(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
      _RunningDimensions = 0;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the new operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Type
    {
      get { return _Type; }
      set { _Type = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the array initialization expression.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ArrayInitializer Initializer
    {
      get { return _Initializer; }
      set { _Initializer = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the number of running dimensions.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int RunningDimensions
    {
      get { return _RunningDimensions; }
      set { _RunningDimensions = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of constructor arguments.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ArgumentList Arguments
    {
      get { return _Arguments; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of dimension expressions.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<Expression> Dimensions
    {
      get { return _Dimensions; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this new operator is a simple constructor call.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsSimpleConstructor
    {
      get { return _Dimensions.Count == 0 && _Initializer == null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this new operator is a new array operation.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNewArray
    {
      get { return _Dimensions.Count > 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this new operator is an array initialization.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsArrayInitialization
    {
      get { return _Dimensions.Count == 0 && _Initializer != null; }
    }

    #endregion

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType,
      IUsesResolutionContext contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      if (_Type != null)
      {
        _Type.ResolveTypeReferences(contextType, contextInstance);
      }
      if (_Initializer != null)
      {
        _Initializer.ResolveTypeReferences(contextType, contextInstance);
      }
      foreach (Argument arg in _Arguments)
      {
        arg.ResolveTypeReferences(contextType, contextInstance);
      }
      foreach (Expression expr in _Dimensions)
      {
        expr.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion
  }
}