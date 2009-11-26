﻿using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an enum entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class EnumEntity : TypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumEntity"/> class.
    /// </summary>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    public EnumEntity(AccessibilityKind? accessibility, string name)
      : base(accessibility, name)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the reference to the underlying type of the enum.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Resolver<TypeEntity> UnderlyingTypeReference { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the underlying type of the enum.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity UnderlyingType
    {
      get
      {
        return UnderlyingTypeReference != null && UnderlyingTypeReference.Target != null
          ? UnderlyingTypeReference.Target.GetMappedType(TypeParameterMap)
          : null;
      }
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a value type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsValueType
    {
      get { return true; }
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
    }

    #endregion
  }
}