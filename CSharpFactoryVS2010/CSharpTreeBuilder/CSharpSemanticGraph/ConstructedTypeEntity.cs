using System;

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
      : base(null, underlyingType.Name)
    {
      if (underlyingType == null )
      {
        throw new ArgumentNullException("underlyingType");
      }

      UnderlyingType = underlyingType;
      _Parent = underlyingType.Parent; 
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the underlying type that this type build upon.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity UnderlyingType { get; private set; }
  }
}
