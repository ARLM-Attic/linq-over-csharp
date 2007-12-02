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
    private bool _IsImplicitArray;

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
    /// Gets the flag indicating if this operator is used to an implicit array
    /// creation or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsImplicitArray
    {
      get { return _IsImplicitArray; }
      set { _IsImplicitArray = value; }
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
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      if (_Type != null)
      {
        _Type.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
      if (_Initializer != null)
      {
        _Initializer.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
      foreach (Argument arg in _Arguments)
      {
        arg.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
      foreach (Expression expr in _Dimensions)
      {
        expr.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion
  }
}