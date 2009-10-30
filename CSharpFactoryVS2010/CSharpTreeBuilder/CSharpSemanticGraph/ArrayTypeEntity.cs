using System.Text;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

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
    /// Gets the base class of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override ClassEntity BaseClass
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
    /// Gets a value indicating whether this is an open type
    /// (ie. is a type that involves type parameters).
    /// </summary>
    /// <remarks>
    /// A type parameter defines an open type.
    /// An array type is an open type if and only if its element type is an open type.
    /// A constructed type is an open type if and only if one or more of its type arguments is 
    /// an open type. A constructed nested type is an open type if and only if one or more of 
    /// its type arguments or the type arguments of its containing type(s) is an open type.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public override bool IsOpen
    {
      get
      {
        return UnderlyingType.IsOpen;
      }
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
      return ConstructedTypeHelper.GetConstructedArrayType(UnderlyingType.GetMappedType(typeParameterMap), Rank);
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