using System;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a nullable type entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class NullableTypeEntity : ConstructedTypeEntity, IAliasType
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NullableTypeEntity"/> class.
    /// </summary>
    /// <param name="embeddedType">The underlying type.</param>
    // ----------------------------------------------------------------------------------------------
    public NullableTypeEntity(TypeEntity embeddedType)
      : base(embeddedType)
    {
      if ((embeddedType is NullableTypeEntity) || !(embeddedType.IsValueType))
      {
        throw new ArgumentException(
          string.Format("Non-nullable value type expected, but received {0}", embeddedType.GetType()),
          "embeddedType");
      }

      AliasToType = new ReflectedTypeBasedTypeEntityReference(typeof(System.Nullable<>));
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the distinctive name of the entity, which is unique for all entities in a declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string DistinctiveName
    {
      get
      {
        return Name + '?';
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the reference to the alias type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> AliasToType { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the declaration space of the entity. 
    /// </summary>
    /// <remarks>    
    /// If the aliased type is already resolved then returns that type's declaration space.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public override DeclarationSpace DeclarationSpace
    {
      get
      {
        return (AliasToType.ResolutionState == ResolutionState.Resolved)
                 ? AliasToType.TargetEntity.DeclarationSpace
                 : base.DeclarationSpace;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of base types.
    /// </summary>
    /// <remarks>    
    /// If the aliased type is already resolved then returns that type's base types.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public override IEnumerable<SemanticEntityReference<TypeEntity>> BaseTypes
    {
      get
      {
        return (AliasToType.ResolutionState == ResolutionState.Resolved)
                 ? AliasToType.TargetEntity.BaseTypes
                 : base.BaseTypes;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of members.
    /// </summary>
    /// <remarks>    
    /// If the aliased type is already resolved then returns that type's members.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public override IEnumerable<MemberEntity> Members
    {
      get
      {
        return (AliasToType.ResolutionState == ResolutionState.Resolved)
                 ? AliasToType.TargetEntity.Members
                 : base.Members;
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
      base.AcceptVisitor(visitor);

      visitor.Visit(this);
    }

    #endregion

  }
}