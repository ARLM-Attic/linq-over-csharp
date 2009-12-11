using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of resolvers where the source object is a reflected metadata.
  /// </summary>
  /// <typeparam name="TTargetType">The type of the target object. Any class.</typeparam>
  /// <typeparam name="TMetadataType">The type of the source object. A subclass of System.Reflection.MemberInfo.</typeparam>
  // ================================================================================================
  public abstract class MetadataResolver<TTargetType, TMetadataType> : Resolver<TTargetType>
    where TTargetType : class
    where TMetadataType : System.Reflection.MemberInfo
  {
    #region State

    /// <summary>Gets the source object to be resolved.</summary>
    public TMetadataType Metadata { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MetadataResolver{TTargetType,TMetadataType}"/> class.
    /// </summary>
    /// <param name="metadata">The source object to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    protected MetadataResolver(TMetadataType metadata)
    {
      Metadata = metadata;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MetadataResolver{TTargetType,TMetadataType}"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected MetadataResolver(MetadataResolver<TTargetType, TMetadataType> template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      Metadata = template.Metadata;
    }
  }
}
