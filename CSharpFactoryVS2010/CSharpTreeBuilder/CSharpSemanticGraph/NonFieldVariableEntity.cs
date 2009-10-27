﻿using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
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
    #region State

    /// <summary>Gets the name of the variable.</summary>
    public string Name { get; private set; }

    /// <summary>Gets the type reference of the variable.</summary>
    public SemanticEntityReference<TypeEntity> TypeReference { get; private set; }

    /// <summary>Gets the initializer of the variable.</summary>
    public VariableInitializer Initializer { get; private set; }

    #endregion

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
    /// Initializes a new instance of the <see cref="NonFieldVariableEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    /// <param name="resolveTypeParameters">True to resolve type parameters immediately, false to defer it.</param>
    // ----------------------------------------------------------------------------------------------
    protected NonFieldVariableEntity(NonFieldVariableEntity template, TypeParameterMap typeParameterMap, bool resolveTypeParameters)
      : base(template, typeParameterMap, resolveTypeParameters)
    {
      Name = template.Name;
      TypeReference = template.TypeReference;
      Initializer = template.Initializer;
    }

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

  }
}
