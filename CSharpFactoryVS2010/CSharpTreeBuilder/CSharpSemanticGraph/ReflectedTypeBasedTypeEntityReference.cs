namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a type entity based on a System.Type object.
  /// </summary>
  // ================================================================================================
  public sealed class ReflectedTypeBasedTypeEntityReference: MetadataBasedSemanticEntityReference<TypeEntity, System.Type>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ReflectedTypeBasedTypeEntityReference"/> class.
    /// </summary>
    /// <param name="metadata">A metadata object that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public ReflectedTypeBasedTypeEntityReference(System.Type metadata)
      : base(metadata)
    {
    }
  }
}
