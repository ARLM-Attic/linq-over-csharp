namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Enumerates the build states of a semantic graph.
  /// </summary>
  // ================================================================================================
  public enum SemanticGraphBuildState
  {
    /// <summary>The semantic graph was just created.</summary>
    Created,
    /// <summary>Mscorlib was imported into the semantic graph.</summary>
    MscorlibImported,
    /// <summary>Referenced units were imported into the semantic graph.</summary>
    ReferencedUnitsImported,
    /// <summary>Syntax trees were imported into the semantic graph.</summary>
    SyntaxTreesImported,
    /// <summary>Partial types were merged.</summary>
    PartialTypesMerged,
    /// <summary>Type declarations were resolved.</summary>
    TypeDeclarationsResolved,
    /// <summary>Type bodies were resolved.</summary>
    TypeBodiesResolved,
    /// <summary>Expressions were evaluated.</summary>
    ExpressionsEvaluated
  }
}
