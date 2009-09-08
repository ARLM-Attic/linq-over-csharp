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
    /// Gets the parameter kind (eg. ref, out).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ParameterKind Kind { get; private set; }
  }
}
