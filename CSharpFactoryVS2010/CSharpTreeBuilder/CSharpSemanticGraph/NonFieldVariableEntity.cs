using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using System;

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
    /// <param name="typeReference">A reference to the type of the variable.</param>
    // ----------------------------------------------------------------------------------------------
    protected NonFieldVariableEntity(string name, SemanticEntityReference<TypeEntity> typeReference)
    {
      if (name == null)
      {
        throw new ArgumentNullException("name");
      }
      if (typeReference == null)
      {
        throw new ArgumentNullException("typeReference");
      }

      Name = name;
      TypeReference = typeReference;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type reference of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> TypeReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get { return TypeReference == null ? null : TypeReference.TargetEntity; }
    }
  }
}
