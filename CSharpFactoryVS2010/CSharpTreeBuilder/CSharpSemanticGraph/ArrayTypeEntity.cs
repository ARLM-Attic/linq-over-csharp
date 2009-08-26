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
    /// <summary>Backing field for BaseType property.</summary>
    private readonly ClassEntity _BaseType;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayTypeEntity"/> class.
    /// </summary>
    /// <param name="elementType">The element type.</param>
    /// <param name="rank">The rank of the array.</param>
    /// <param name="baseType">The base type.</param>
    // ----------------------------------------------------------------------------------------------
    public ArrayTypeEntity(TypeEntity elementType, int rank, ClassEntity baseType)
      : base(elementType)
    {
      Rank = rank;
      _BaseType = baseType;
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
      get { return _BaseType; }
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
    /// Gets the distinctive name of the entity, which is unique for all entities in a declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string DistinctiveName
    {
      get
      {
        var stringBuilder = new StringBuilder(UnderlyingType.DistinctiveName);
        stringBuilder.Append('[');
        stringBuilder.Append(',',Rank-1);
        stringBuilder.Append(']');
        return stringBuilder.ToString();
      }
    }
  }
}