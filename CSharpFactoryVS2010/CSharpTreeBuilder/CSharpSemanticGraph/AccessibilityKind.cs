namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Enumerates the different kinds of accessibility.
  /// </summary>
  // ================================================================================================
  public enum AccessibilityKind
  {
    /// <summary>
    /// Access limited to the containing type. Same as C# private.
    /// </summary>
    Private,

    /// <summary>
    /// Access is not limited. Same as C# public.
    /// </summary>
    Public,

    /// <summary>
    /// Access limited to the containing class or types derived from the containing class. Same as C# protected.
    /// </summary>
    Family,

    /// <summary>
    /// Access limited to the containing assembly. Same as C# internal.
    /// </summary>
    Assembly,

    /// <summary>
    /// Access limited to the containing assembly or types derived from the containing class. Same as C# protected internal.
    /// </summary>
    FamilyOrAssembly,

    /// <summary>
    /// Accessible only to referents that qualify for both family and assembly access. No equivalent in C#.
    /// </summary>
    FamilyAndAssembly
  }
}
