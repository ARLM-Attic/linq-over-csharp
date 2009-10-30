using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
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

    /// <summary>Gets or sets the reference to the type of the field.</summary>
    public SemanticEntityReference<TypeEntity> TypeReference { get; private set; }

    /// <summary>Gets the initializer of the variable.</summary>
    public VariableInitializer Initializer { get; private set; }
    
    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="isStatic">True, if the field is static, false otherwise.</param>
    /// <param name="type">The type of the field (a type entity reference).</param>
    /// <param name="name">The name of the member.</param>
    /// <param name="initializer">The initializer of the field.</param>
    // ----------------------------------------------------------------------------------------------
    public FieldEntity(
      bool isDeclaredInSource,
      AccessibilityKind? accessibility,
      bool isStatic,
      SemanticEntityReference<TypeEntity> type,
      string name, 
      VariableInitializer initializer)
      : 
      base(isDeclaredInSource, accessibility, name)
    {
      _IsStatic = isStatic;
      TypeReference = type;
      Initializer = initializer;

      if (Initializer != null)
      {
        Initializer.Parent = this;
      }
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
      TypeReference = template.TypeReference;
      
      // TODO: clone initializer
      Initializer = template.Initializer;
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
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get 
      {
        return TypeReference != null && TypeReference.TargetEntity != null
          ? TypeReference.TargetEntity.GetMappedType(TypeParameterMap)
          : null;
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
