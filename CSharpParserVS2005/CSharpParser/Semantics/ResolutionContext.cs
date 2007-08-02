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

    /// <summary>Type declaration in a method declaration.</summary>
    MethodDeclaration,

    /// <summary>Type declaration in an accessor declaration.</summary>
    AccessorDeclaration
  }
}
