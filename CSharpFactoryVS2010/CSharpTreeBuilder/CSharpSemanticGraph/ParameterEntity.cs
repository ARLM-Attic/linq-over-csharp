using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a formal parameter of a method, indexer, etc.
  /// </summary>
  // ================================================================================================
  public sealed class ParameterEntity : NonFieldVariableEntity
  {
    #region State

    /// <summary>Gets the parameter kind (eg. ref, out).</summary>
    public ParameterKind Kind { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="type">The type of the variable (a type entity reference).</param>
    /// <param name="kind">The parameter kind (eg. ref, out).</param>
    // ----------------------------------------------------------------------------------------------
    public ParameterEntity(string name, SemanticEntityReference<TypeEntity> type, ParameterKind kind)
      : base (name, type, null)
    {
      Kind = kind;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    /// <param name="resolveTypeParameters">True to resolve type parameters immediately, false to defer it.</param>
    // ----------------------------------------------------------------------------------------------
    private ParameterEntity(ParameterEntity template, TypeParameterMap typeParameterMap, bool resolveTypeParameters)
      : base(template, typeParameterMap, resolveTypeParameters)
    {
      Kind = template.Kind;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <param name="resolveTypeParameters">True to resolve type parameters during construction, 
    /// false to defer it to a later phase.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override SemanticEntity ConstructNew(TypeParameterMap typeParameterMap, bool resolveTypeParameters)
    {
      return new ParameterEntity(this, typeParameterMap, resolveTypeParameters);
    }
  }
}
