namespace CSharpFactory.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This interface defines the behaviour of language elements that require further
  /// resolution.
  /// </summary>
  // ==================================================================================
  public interface IUsesResolutionContext
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope);
  }
}
