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
    // ----------------------------------------------------------------------------------------------
    protected ConstructedTypeEntity(TypeEntity embeddedType)
    {
      if (embeddedType == null )
      {
        throw new ArgumentNullException("embeddedType");
      }

      EmbeddedType = embeddedType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the embedded type that this type build upon.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity EmbeddedType { get; private set; }
  }
}
