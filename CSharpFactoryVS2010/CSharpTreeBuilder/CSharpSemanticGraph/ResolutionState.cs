namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Enumerates the different resolution states of a namespace or type entity reference.
  /// </summary>
  // ================================================================================================
  public enum ResolutionState
  {
    /// <summary>Reference is not yet resolved.</summary>
    NotYetResolved,

    /// <summary>Reference is successfully resolved.</summary>
    Resolved,

    /// <summary>Could not resolve reference.</summary>
    Unresolvable
  }
}
