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
    /// by deep copying from another instance.
    /// </summary>
    /// <param name="source">The object whose state will be copied to the new object.</param>
    // ----------------------------------------------------------------------------------------------
    public ParameterEntity(ParameterEntity source)
      : base(source)
    {
      Kind = source.Kind;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a deep copy of the semantic subtree starting at this entity.
    /// </summary>
    /// <returns>The deep clone of this entity and its semantic subtree.</returns>
    // ----------------------------------------------------------------------------------------------
    public override object Clone()
    {
      return new ParameterEntity(this);
    }
  }
}
