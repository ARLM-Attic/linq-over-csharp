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
    public virtual void Visit(ExternAliasEntity entity) { }
    public virtual void Visit(UsingNamespaceEntity entity) { }
    public virtual void Visit(UsingAliasEntity entity) { }

    public virtual void Visit(EnumEntity entity) { }
    public virtual void Visit(ClassEntity entity) { }
    public virtual void Visit(StructEntity entity) { }
    public virtual void Visit(DelegateEntity entity) { }
    public virtual void Visit(InterfaceEntity entity) { }
    public virtual void Visit(BuiltInTypeEntity entity) { }

    public virtual void Visit(FieldEntity entity) { }
    public virtual void Visit(PropertyEntity entity) { }
    public virtual void Visit(MethodEntity entity) { }

    #pragma warning restore 1591

  }
}
