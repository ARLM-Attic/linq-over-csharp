using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a local variable.
  /// </summary>
  // ================================================================================================
  public sealed class LocalVariableEntity : NonFieldVariableEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalVariableEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="type">The type of the variable (a type entity reference).</param>
    /// <param name="isConstant">A value indicating whether this is a constant.</param>
    // ----------------------------------------------------------------------------------------------
    public LocalVariableEntity(string name, SemanticEntityReference<TypeEntity> type, bool isConstant)
      : base (name, type, null)
    {
      IsConstant = isConstant;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a constant.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsConstant { get; private set; }
  }
}
