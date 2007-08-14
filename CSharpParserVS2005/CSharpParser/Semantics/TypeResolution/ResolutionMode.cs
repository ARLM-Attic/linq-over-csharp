namespace CSharpParser
{
  // ==================================================================================
  /// <summary>
  /// This enumeration defines the result how a referenced type can be resolved.
  /// </summary>
  // ==================================================================================
  public enum ResolutionMode
  {
    /// <summary>
    /// The type has not been resolved.
    /// </summary>
    Unresolved = 0,

    /// <summary>
    /// The type cannot be resolved (sematic error).
    /// </summary>
    CannotResolve,
    
    /// <summary>
    /// The type has been resolved as a standard .NET runtime type.
    /// </summary>
    RuntimeType,
    
    /// <summary>
    /// The type has been resolved as a source type defined in this compilation unit.
    /// </summary>
    SourceType,
    
    /// <summary>
    /// The type has been resolved as a source type defined in a referenced compilation unit.
    /// </summary>
    ReferencedUnit
  }
}
