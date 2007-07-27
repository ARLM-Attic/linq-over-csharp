namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This enumeration defines the context where type resolutions are required.
  /// </summary>
  // ==================================================================================
  public enum ResolutionContext
  {
    /// <summary>Source file, out of any namespace declarations.</summary>
    SourceFile,

    /// <summary>Namespace, out of any type declarations.</summary>
    Namespace,

    /// <summary>Type declaration, out of any members.</summary>
    TypeDeclaration,
  }
}
