using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a variable that is not a field (eg. parameter, array element).
  /// </summary>
  // ================================================================================================
  public abstract class NonFieldVariableEntity : SemanticEntity, IVariableEntity, INamedEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NonFieldVariableEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="type">The type of the variable (a type entity reference).</param>
    // ----------------------------------------------------------------------------------------------
    protected NonFieldVariableEntity(string name, SemanticEntityReference<TypeEntity> type)
    {
      Name = name;
      TypeReference = type;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the distinctive name of the entity, which is unique for all entities in a declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string DistinctiveName
    {
      get { return Name; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> TypeReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get { return TypeReference == null ? null : TypeReference.TargetEntity; }
    }
  }
}
