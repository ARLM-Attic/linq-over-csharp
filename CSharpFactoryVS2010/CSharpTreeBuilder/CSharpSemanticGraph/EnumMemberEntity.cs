using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an enum member (a named constant).
  /// </summary>
  // ================================================================================================
  public sealed class EnumMemberEntity : ConstantMemberEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumMemberEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="name">The name of the member.</param>
    /// <param name="type">The type of the field (a type entity reference).</param>
    // ----------------------------------------------------------------------------------------------
    public EnumMemberEntity(bool isDeclaredInSource, string name, SemanticEntityReference<TypeEntity> type)
      : base(isDeclaredInSource, null, type, name)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumMemberEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    /// <param name="resolveTypeParameters">True to resolve type parameters immediately, false to defer it.</param>
    // ----------------------------------------------------------------------------------------------
    private EnumMemberEntity(EnumMemberEntity template, TypeParameterMap typeParameterMap, bool resolveTypeParameters)
      : base(template, typeParameterMap, resolveTypeParameters)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <param name="resolveTypeParameters">True to resolve type parameters during construction, 
    /// false to defer it to a later phase.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override SemanticEntity ConstructNew(TypeParameterMap typeParameterMap, bool resolveTypeParameters)
    {
      return new EnumMemberEntity(this, typeParameterMap, resolveTypeParameters);
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
      get { return false; }
    }
  }
}
