//
// WARNING! This file is generated, do not modify it manually!
//
// Generated on: 2009. 12. 10. 10:56:25
//
// The template file is SemanticGraphVisitor.tt 
//

// disable warnings about missing XML comments
#pragma warning disable 1591 

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
    public virtual void Visit(AccessorEntity entity) { }
    public virtual void Visit(ArgumentEntity entity) { }
    public virtual void Visit(ArrayInitializerEntity entity) { }
    public virtual void Visit(ArrayTypeEntity entity) { }
    public virtual void Visit(AssignmentExpressionEntity entity) { }
    public virtual void Visit(BlockStatementEntity entity) { }
    public virtual void Visit(ChildTypeCapableTypeEntity entity) { }
    public virtual void Visit(ClassEntity entity) { }
    public virtual void Visit(ConstantMemberEntity entity) { }
    public virtual void Visit(ConstructedTypeEntity entity) { }
    public virtual void Visit(DeclarationSpaceDefiningStatementEntity entity) { }
    public virtual void Visit(DefaultValueExpressionEntity entity) { }
    public virtual void Visit(DelegateEntity entity) { }
    public virtual void Visit(EnumEntity entity) { }
    public virtual void Visit(EnumMemberEntity entity) { }
    public virtual void Visit(ExpressionEntity entity) { }
    public virtual void Visit(ExpressionStatementEntity entity) { }
    public virtual void Visit(ExternAliasEntity entity) { }
    public virtual void Visit(FieldEntity entity) { }
    public virtual void Visit(FunctionMemberEntity entity) { }
    public virtual void Visit(FunctionMemberWithAccessorsEntity entity) { }
    public virtual void Visit(FunctionMemberWithBodyEntity entity) { }
    public virtual void Visit(GenericCapableTypeEntity entity) { }
    public virtual void Visit(IMemberEntity entity) { }
    public virtual void Visit(INamedEntity entity) { }
    public virtual void Visit(InterfaceEntity entity) { }
    public virtual void Visit(InvocationExpressionEntity entity) { }
    public virtual void Visit(IOverloadableEntity entity) { }
    public virtual void Visit(ISemanticEntity entity) { }
    public virtual void Visit(IVariableEntity entity) { }
    public virtual void Visit(LiteralExpressionEntity entity) { }
    public virtual void Visit(LocalVariableDeclarationStatementEntity entity) { }
    public virtual void Visit(LocalVariableEntity entity) { }
    public virtual void Visit(MemberAccessExpressionEntity entity) { }
    public virtual void Visit(MethodEntity entity) { }
    public virtual void Visit(NamespaceEntity entity) { }
    public virtual void Visit(NamespaceOrTypeEntity entity) { }
    public virtual void Visit(NonFieldVariableEntity entity) { }
    public virtual void Visit(NonTypeMemberEntity entity) { }
    public virtual void Visit(NullLiteralExpressionEntity entity) { }
    public virtual void Visit(ParameterEntity entity) { }
    public virtual void Visit(PointerTypeEntity entity) { }
    public virtual void Visit(PredefinedTypeMemberAccessExpressionEntity entity) { }
    public virtual void Visit(PrimaryMemberAccessExpressionEntity entity) { }
    public virtual void Visit(PropertyEntity entity) { }
    public virtual void Visit(QualifiedAliasMemberAccessExpressionEntity entity) { }
    public virtual void Visit(ReturnStatementEntity entity) { }
    public virtual void Visit(RootNamespaceEntity entity) { }
    public virtual void Visit(ScalarInitializerEntity entity) { }
    public virtual void Visit(SemanticEntity entity) { }
    public virtual void Visit(SimpleNameExpressionEntity entity) { }
    public virtual void Visit(StatementEntity entity) { }
    public virtual void Visit(StructEntity entity) { }
    public virtual void Visit(ThisAccessExpressionEntity entity) { }
    public virtual void Visit(TypedLiteralExpressionEntity entity) { }
    public virtual void Visit(TypeEntity entity) { }
    public virtual void Visit(TypeParameterEntity entity) { }
    public virtual void Visit(UsingAliasEntity entity) { }
    public virtual void Visit(UsingEntity entity) { }
    public virtual void Visit(UsingNamespaceEntity entity) { }
  }
}
#pragma warning restore 1591