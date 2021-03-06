﻿using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a field of a type.
  /// </summary>
  // ================================================================================================
  public sealed class FieldEntity : NonTypeMemberEntity, IVariableEntity
  {
    #region State

    /// <summary>Gets a value indicating whether this field is readonly.</summary>
    public bool IsReadOnly { get; private set; }

    /// <summary>Gets or sets the reference to the type of the field.</summary>
    public Resolver<TypeEntity> TypeReference { get; private set; }

    /// <summary>Backing field for Initializer property.</summary>
    private VariableInitializer _Initializer;


    
    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="isStatic">True, if the field is static, false otherwise.</param>
    /// <param name="isReadOnly">True, if the field is readonly, false otherwise.</param>
    /// <param name="type">The type of the field (a type entity reference).</param>
    /// <param name="name">The name of the member.</param>
    // ----------------------------------------------------------------------------------------------
    public FieldEntity(
      bool isDeclaredInSource,
      AccessibilityKind? accessibility,
      bool isStatic,
      bool isReadOnly,
      Resolver<TypeEntity> type,
      string name)
      : 
      base(isDeclaredInSource, accessibility, name)
    {
      _IsStatic = isStatic;
      IsReadOnly = isReadOnly;
      TypeReference = type;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private FieldEntity(FieldEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      IsReadOnly = template.IsReadOnly;

      if (template.TypeReference != null)
      {
        TypeReference = (Resolver<TypeEntity>)template.TypeReference.GetGenericClone(typeParameterMap);
      }

      if (template.Initializer != null)
      {
        Initializer = (VariableInitializer)template.Initializer.GetGenericClone(typeParameterMap);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override SemanticEntity ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new FieldEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child entity.
    /// </summary>
    /// <param name="entity">A child entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void AddChild(ISemanticEntity entity)
    {
      if (entity is VariableInitializer)
      {
        Initializer = entity as VariableInitializer;
      }
      else
      {
        base.AddChild(entity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the initializer of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public VariableInitializer Initializer
    {
      get
      {
        return _Initializer;
      }

      private set
      {
        _Initializer = value;
        if (_Initializer != null)
        {
          _Initializer.Parent = this;
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get 
      {
        return TypeReference != null ? TypeReference.Target : null;
      }
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
    /// Gets a value indicating whether this member is invocable.
    /// </summary>
    /// <remarks>A member is invocable if it's a method or event, 
    /// or if it is a constant, field or property of a delegate type.</remarks>
    // ----------------------------------------------------------------------------------------------
    public override bool IsInvocable
    {
      get 
      { 
        return (Type != null) && Type is DelegateEntity; 
      }
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      visitor.Visit(this);
      base.AcceptVisitor(visitor);

      if (Initializer != null)
      {
        Initializer.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
