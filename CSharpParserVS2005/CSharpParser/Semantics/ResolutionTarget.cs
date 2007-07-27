namespace CSharpParser
{
  // ==================================================================================
  /// <summary>
  /// This enumeration defines the target of a successful type resolution.
  /// </summary>
  // ==================================================================================
  public enum ResolutionTarget
  {
    /// <summary>The name has not been resolved.</summary>
    Unresolved = 0,
    /// <summary>The name cannot be resolved unambiguously.</summary>
    Ambiguous,
    /// <summary>The name has been resolved as a namespace.</summary>
    Namespace,
    /// <summary>The name has been resolved as a type.</summary>
    Type
  }
}
