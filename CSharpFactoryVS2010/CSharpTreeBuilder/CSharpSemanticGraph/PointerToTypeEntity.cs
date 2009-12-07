using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a pointer to a type.
  /// </summary>
  // ================================================================================================
  public sealed class PointerToTypeEntity : ConstructedTypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PointerToTypeEntity"/> class.
    /// </summary>
    /// <param name="underlyingType">The referent type.</param>
    // ----------------------------------------------------------------------------------------------
    public PointerToTypeEntity(TypeEntity underlyingType)
      : base(underlyingType)
    {
    }

    // Constructed types are not generic-cloned so no copy constructor here.

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a pointer type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsPointerType
    {
      get { return true; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns the result of mapping this type with a type parameter map.
    /// </summary>
    /// <param name="typeParameterMap">A map of type parameters and corresponding type arguments.</param>
    /// <returns>A TypeEntity, the result of the mapping.</returns>
    // ----------------------------------------------------------------------------------------------
    public override TypeEntity GetMappedType(TypeParameterMap typeParameterMap)
    {
      return ConstructedTypeHelper.GetConstructedPointerType(UnderlyingType.GetMappedType(typeParameterMap));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of the object.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      return UnderlyingType.ToString() + "*";
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