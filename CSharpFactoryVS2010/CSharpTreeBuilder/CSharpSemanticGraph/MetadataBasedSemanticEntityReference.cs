namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a semantic entity that is based on metadata imported from a referenced assembly.
  /// </summary>
  /// <typeparam name="TTargetEntity">The type of the target entity, must be a subclass of SemanticEntity.</typeparam>
  /// <typeparam name="TMetadata">The type of the metadata, must be a subclass of System.Reflection.MemberInfo,</typeparam>
  // ================================================================================================
  public abstract class MetadataBasedSemanticEntityReference<TTargetEntity, TMetadata> : SemanticEntityReference<TTargetEntity>
    where TTargetEntity : SemanticEntity
    where TMetadata : System.Reflection.MemberInfo
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MetadataBasedSemanticEntityReference{TTargetEntity,TMetadata}"/> class.
    /// </summary>
    /// <param name="metadata">A metadata object that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public MetadataBasedSemanticEntityReference(TMetadata metadata)
    {
      Metadata = metadata;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the metadata that represents the referenced entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TMetadata Metadata { get; private set; }


  }
}
