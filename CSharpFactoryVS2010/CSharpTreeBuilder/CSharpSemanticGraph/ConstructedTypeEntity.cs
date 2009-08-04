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
    /// <param name="underlyingType">A type that this constructed type builds upon.</param>
    // ----------------------------------------------------------------------------------------------
    protected ConstructedTypeEntity(TypeEntity underlyingType)
    {
      if (underlyingType == null )
      {
        throw new ArgumentNullException("underlyingType");
      }

      UnderlyingType = underlyingType;
      Parent = underlyingType.Parent; 
      Name = underlyingType.Name;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the underlying type that this type build upon.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity UnderlyingType { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the declaration space of the entity. 
    /// For a constructed type, the declaration space is the underlying type's declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override DeclarationSpace DeclarationSpace
    {
      get
      {
        return UnderlyingType.DeclarationSpace;
      }
    }
  }
}
