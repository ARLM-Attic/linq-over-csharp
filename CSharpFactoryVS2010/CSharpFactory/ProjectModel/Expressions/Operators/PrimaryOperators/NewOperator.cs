using System.Collections.Generic;
using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract class represents a "new" operator expression.
  /// </summary>
  // ==================================================================================
  public sealed class NewOperator : PrimaryOperator
  {
    #region Private fields

    private readonly ArgumentList _Arguments = new ArgumentList();
    private readonly List<Expression> _Dimensions = new List<Expression>();

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
      RunningDimensions = 0;
      Kind = NewOperatorKind.TypedObjectCreation;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the kind of this new operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NewOperatorKind Kind { get; internal set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the new operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Type { get; internal set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the array initialization expression.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Initializer Initializer { get; internal set;}

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the number of running dimensions.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int RunningDimensions { get; internal set;}

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
      get { return _Dimensions.Count == 0 && Initializer == null; }
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
    public bool IsImplicitArray { get; internal set;}

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this new operator is an array initialization.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsArrayInitialization
    {
      get { return _Dimensions.Count == 0 && Initializer != null; }
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
      if (Type != null)
      {
        Type.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
      if (Initializer != null)
      {
        Initializer.ResolveTypeReferences(contextType, declarationScope, parameterScope);
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


  // ==================================================================================
  /// <summary>
  /// This enumeration defines the types of new operator.
  /// </summary>
  // ==================================================================================
  public enum NewOperatorKind
  {
    /// <summary>A new typed object creation.</summary>
    TypedObjectCreation,
    /// <summary>A new typed object creation and initialization.</summary>
    TypedObjectInitialization,
    /// <summary>A new typed array creation.</summary>
    TypedArrayCreation,
    /// <summary>A new typed array creation and initialization.</summary>
    TypedArrayInitialization,
    /// <summary>A new untyped array initialization.</summary>
    UntypedArrayInitialization,
    /// <summary>A new anonymous type creation.</summary>
    AnonymousTypeCreation,
  }
}