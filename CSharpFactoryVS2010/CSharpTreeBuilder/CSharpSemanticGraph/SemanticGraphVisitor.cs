namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of SemanticGraph visitor classes.
  /// </summary>
  // ================================================================================================
  public abstract class SemanticGraphVisitor
  {
    // disable warnings about missing XML comments
    #pragma warning disable 1591 

    public virtual void Visit(NamespaceEntity entity) { }
    public virtual void Visit(TypeEntity entity) { }
    public virtual void Visit(BuiltInTypeEntity entity) { }
    public virtual void Visit(FieldEntity entity) { }

    #pragma warning restore 1591

  }
}
