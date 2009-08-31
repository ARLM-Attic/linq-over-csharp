namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Enumerates the parameter kinds.
  /// </summary>
  // ================================================================================================
  public enum ParameterKind
  {
    /// <summary>Parameter passed by value.</summary>
    Value,

    /// <summary>Parameter passed by reference (ref).</summary>
    Reference,

    /// <summary>Output parameter (out).</summary>
    Output
  }
}
