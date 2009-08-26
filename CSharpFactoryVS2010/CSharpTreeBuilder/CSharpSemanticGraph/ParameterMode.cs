namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Enumerates the parameter modes.
  /// </summary>
  // ================================================================================================
  public enum ParameterMode
  {
    /// <summary>Parameter passed by value.</summary>
    Value,

    /// <summary>Parameter passed by reference (ref).</summary>
    Reference,

    /// <summary>Output parameter (out).</summary>
    Output
  }
}
