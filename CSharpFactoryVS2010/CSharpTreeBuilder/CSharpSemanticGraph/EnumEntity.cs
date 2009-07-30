namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an enum entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class EnumEntity : TypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a value type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsValueType
    {
      get { return true; }
    }
  }
}