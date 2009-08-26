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
    /// <param name="mode">The parameter mode.</param>
    // ----------------------------------------------------------------------------------------------
    public ParameterEntity(string name, SemanticEntityReference<TypeEntity> type, ParameterMode mode)
      : base (name, type)
    {
      Mode = mode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parameter mode.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ParameterMode Mode { get; private set; }
  }
}
