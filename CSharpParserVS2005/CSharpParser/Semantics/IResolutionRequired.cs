namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This interface defines the behaviour of language elements that require further
  /// resolution.
  /// </summary>
  // ==================================================================================
  public interface IResolutionRequired
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    void ResolveTypeReferences(ResolutionContext contextType, 
      IResolutionRequired contextInstance);
  }
}
