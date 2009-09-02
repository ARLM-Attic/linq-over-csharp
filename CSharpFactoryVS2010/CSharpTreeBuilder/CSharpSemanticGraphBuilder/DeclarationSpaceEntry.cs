using System;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents an entry in a declaration space. 
  /// It associates a named semantic entity with a name, type parameter count and parameters.
  /// </summary>
  // ================================================================================================
  public sealed class DeclarationSpaceEntry
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclarationSpaceEntry"/> type.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <param name="name">The name of the semantic entity.</param>
    /// <param name="typeParameterCount">The number of type parameters of the semantic entity.</param>
    /// <param name="parameters">The parameters of the semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public DeclarationSpaceEntry(INamedEntity entity, string name, int typeParameterCount, IEnumerable<ParameterEntity> parameters)
    {
      if (entity == null)
      {
        throw new ArgumentNullException("entity");
      }

      if (name == null)
      {
        throw new ArgumentNullException("name");
      }

      Entity = entity;
      Name = name;
      TypeParameterCount = typeParameterCount;
      Parameters = parameters;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclarationSpaceEntry"/> type.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <param name="name">The name of the semantic entity.</param>
    /// <param name="typeParameterCount">The number of type parameters of the semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public DeclarationSpaceEntry(INamedEntity entity, string name, int typeParameterCount)
      : this (entity, name, typeParameterCount, null)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclarationSpaceEntry"/> type.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <param name="name">The name of the semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public DeclarationSpaceEntry(INamedEntity entity, string name)
      : this(entity, name, 0, null)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclarationSpaceEntry"/> type.
    /// </summary>
    /// <param name="entity">A named entity.</param>
    /// <param name="signature">The signature of the semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public DeclarationSpaceEntry(INamedEntity entity, Signature signature)
      : this(entity, signature.Name, signature.TypeParameterCount, signature.Parameters)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the declared named entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public INamedEntity Entity { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the semantic entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters of semantic entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int TypeParameterCount { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parameters of the semantic entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<ParameterEntity> Parameters { get; private set; }
  }
}
