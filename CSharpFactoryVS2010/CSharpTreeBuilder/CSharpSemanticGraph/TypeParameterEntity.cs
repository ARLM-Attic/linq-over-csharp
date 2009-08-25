namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a type parameter (aka generic parameter) entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class TypeParameterEntity : TypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParameterEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the type parameter.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterEntity(string name)
      : base(name)
    {
    }
  }
}
