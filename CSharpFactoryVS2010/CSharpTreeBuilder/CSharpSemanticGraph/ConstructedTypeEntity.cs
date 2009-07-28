using System;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a type entity that is constructed from other types:
  /// array, pointer, nullable types, and constructed generic types.
  /// </summary>
  // ================================================================================================
  public abstract class ConstructedTypeEntity : TypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructedTypeEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected ConstructedTypeEntity(TypeEntity embeddedType)
    {
      if (embeddedType == null )
      {
        throw new ArgumentNullException("embeddedType");
      }

      EmbeddedType = embeddedType;
      Parent = embeddedType.Parent; 
      Name = embeddedType.Name;

      // Type tulajdonságok
      // BaseTypes = embeddedType.BaseTypes;
      // - array: System.Array
      // - nullable: System.Nullable<T> base type-jai
      // - pointer: nincs base type
      // _Members = (List<MemberEntity>)embeddedType.Members;
      // - array: örökli a System.Array-t
      // - nullable: System.Nullable<T> memberei
      // - pointer: nincs membere
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the embedded type that this type build upon.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity EmbeddedType { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the declaration space of the entity. 
    /// For a constructed type, the declaration space is the embedded type's declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override DeclarationSpace DeclarationSpace
    {
      get
      {
        return EmbeddedType.DeclarationSpace;
      }
    }
  }
}
