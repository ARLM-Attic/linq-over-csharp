using System.Text;

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
    /// Gets the string representation of the object.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      return UnderlyingType.ToString() + "*";
    }
  }
}