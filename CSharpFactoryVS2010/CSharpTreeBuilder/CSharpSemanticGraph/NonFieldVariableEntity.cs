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
    /// <param name="initializer">The initializer of the variable.</param>
    // ----------------------------------------------------------------------------------------------
    protected NonFieldVariableEntity(
      string name, 
      SemanticEntityReference<TypeEntity> typeReference, 
      VariableInitializer initializer)
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
      Initializer = initializer;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string FullyQualifiedName
    {
      get
      {
        return Name;
      }
    }

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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this variable is an array. 
    /// Null if the type of the variable is not yet resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool? IsArray
    {
      get
      {
        return Type == null ? null : Type.IsArrayType as bool?;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the initializer of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public VariableInitializer Initializer { get; private set; }

  }
}
