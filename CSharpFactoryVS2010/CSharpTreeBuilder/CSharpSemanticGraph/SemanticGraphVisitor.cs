namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of SemanticGraph visitor classes.
  /// </summary>
  /// <remarks>
  /// Preorder, depth-first traversal. 
  /// All base classes are visited from the most-derived to the less derived.
  /// </remarks>
  // ================================================================================================
  public abstract class SemanticGraphVisitor
  {
    // disable warnings about missing XML comments
    #pragma warning disable 1591 

    public virtual void Visit(SemanticEntity entity) { }

    public virtual void Visit(NamespaceEntity entity) { }
    public virtual void Visit(ExternAliasEntity entity) { }
    public virtual void Visit(UsingNamespaceEntity entity) { }
    public virtual void Visit(UsingAliasEntity entity) { }

    public virtual void Visit(TypeEntity entity) { }

    public virtual void Visit(EnumEntity entity) { }
    public virtual void Visit(ClassEntity entity) { }
    public virtual void Visit(StructEntity entity) { }
    public virtual void Visit(DelegateEntity entity) { }
    public virtual void Visit(InterfaceEntity entity) { }
    public virtual void Visit(TypeParameterEntity entity) { }

    public virtual void Visit(ConstantMemberEntity entity) { }
    public virtual void Visit(FieldEntity entity) { }
    public virtual void Visit(PropertyEntity entity) { }
    public virtual void Visit(MethodEntity entity) { }

    public virtual void Visit(ScalarInitializerEntity entity) { }
    public virtual void Visit(ArrayInitializerEntity entity) { }

    public virtual void Visit(ExpressionEntity entity) { }

    public virtual void Visit(NullLiteralExpressionEntity entity) { }
    public virtual void Visit(TypedLiteralExpressionEntity entity) { }
    public virtual void Visit(SimpleNameExpressionEntity entity) { }
    public virtual void Visit(DefaultValueExpressionEntity entity) { }

    #pragma warning restore 1591

  }
}
