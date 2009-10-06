using System.Text;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an array type entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class ArrayTypeEntity: ConstructedTypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayTypeEntity"/> class.
    /// </summary>
    /// <param name="elementType">The element type.</param>
    /// <param name="rank">The rank of the array.</param>
    // ----------------------------------------------------------------------------------------------
    public ArrayTypeEntity(TypeEntity elementType, int rank)
      : base(elementType)
    {
      Rank = rank;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the rank of the array (the number of array dimensions).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Rank { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the base type of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override TypeEntity BaseType
    {
      get { return SemanticGraph.SystemArray; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a reference type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsReferenceType
    {
      get { return true; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is an array type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsArrayType
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
        var stringBuilder = new StringBuilder(UnderlyingType.ToString());
        stringBuilder.Append('[');
        stringBuilder.Append(',',Rank-1);
        stringBuilder.Append(']');
        return stringBuilder.ToString();
    }
  }
}