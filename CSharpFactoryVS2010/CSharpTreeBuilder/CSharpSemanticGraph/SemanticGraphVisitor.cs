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

    public virtual bool Visit(NamespaceEntity entity) { return true; }
    public virtual bool Visit(ExternAliasEntity entity) { return true; }
    public virtual bool Visit(UsingNamespaceEntity entity) { return true; }
    public virtual bool Visit(UsingAliasEntity entity) { return true; }

    public virtual bool Visit(EnumEntity entity) { return true; }
    public virtual bool Visit(ClassEntity entity) { return true; }
    public virtual bool Visit(StructEntity entity) { return true; }
    public virtual bool Visit(DelegateEntity entity) { return true; }
    public virtual bool Visit(InterfaceEntity entity) { return true; }
    public virtual bool Visit(TypeParameterEntity entity) { return true; }

    public virtual bool Visit(ConstantMemberEntity entity) { return true; }
    public virtual bool Visit(FieldEntity entity) { return true; }
    public virtual bool Visit(PropertyEntity entity) { return true; }
    public virtual bool Visit(MethodEntity entity) { return true; }

    public virtual bool Visit(ScalarInitializerEntity entity) { return true; }
    public virtual bool Visit(ArrayInitializerEntity entity) { return true; }

    public virtual bool Visit(NullLiteralExpressionEntity entity) { return true; }
    public virtual bool Visit(TypedLiteralExpressionEntity entity) { return true; }
    public virtual bool Visit(SimpleNameExpressionEntity entity) { return true; }
    public virtual bool Visit(DefaultValueExpressionEntity entity) { return true; }

    #pragma warning restore 1591

  }
}
