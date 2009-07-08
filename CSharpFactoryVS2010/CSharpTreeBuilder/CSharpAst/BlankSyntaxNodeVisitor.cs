//
// WARNING! This file is generated, do not modify it manually!
//
// Generated on: 2009. 07. 08. 9:48:29
//
// The template file is BlankSyntaxNodeVisitor.tt 
//

// disable warnings about missing XML comments
#pragma warning disable 1591 

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// An implementation of ISyntaxNodeVisitor that implements every method as an empty virtual function.
  /// Subclass this class if you want to create a SyntaxNodeVisitor that only handles some syntax node types.
  /// This way you can avoid creating lots af method implementations that do nothing.
  /// </summary>
  // ================================================================================================
  public class BlankSyntaxNodeVisitor : ISyntaxNodeVisitor
  {
    public virtual void Visit(AccessorNode node) { }
    public virtual void Visit(AnonymousMethodNode node) { }
    public virtual void Visit(AnonymousObjectCreationExpressionNode node) { }
    public virtual void Visit(ArgumentNode node) { }
    public virtual void Visit(ArrayCreationExpressionNode node) { }
    public virtual void Visit(ArrayInitializerNode node) { }
    public virtual void Visit(ArrayItemInitializerNode node) { }
    public virtual void Visit(ArraySizeSpecifierNode node) { }
    public virtual void Visit(AssignmentOperatorNode node) { }
    public virtual void Visit(AttributeArgumentNode node) { }
    public virtual void Visit(AttributedDeclarationNode node) { }
    public virtual void Visit(AttributeDecorationNode node) { }
    public virtual void Visit(AttributeNode node) { }
    public virtual void Visit(BaseAccessNode node) { }
    public virtual void Visit(BaseConstructorInitializerNode node) { }
    public virtual void Visit(BaseElementAccessNode node) { }
    public virtual void Visit(BaseMemberAccessNode node) { }
    public virtual void Visit(BinaryOperatorNode node) { }
    public virtual void Visit(BlockCommentNode node) { }
    public virtual void Visit(BlockStatementNode node) { }
    public virtual void Visit(BlockWrappingStatementNode node) { }
    public virtual void Visit(BooleanLiteralNode node) { }
    public virtual void Visit(BreakStatementNode node) { }
    public virtual void Visit(CastOperatorDeclarationNode node) { }
    public virtual void Visit(CatchClauseNode node) { }
    public virtual void Visit(CharLiteralNode node) { }
    public virtual void Visit(CheckedOperatorNode node) { }
    public virtual void Visit(CheckedStatementNode node) { }
    public virtual void Visit(ClassDeclarationNode node) { }
    public virtual void Visit(CompilationUnitNode node) { }
    public virtual void Visit(ConditionalOperatorNode node) { }
    public virtual void Visit(ConditionalPragmaNode node) { }
    public virtual void Visit(ConstDeclarationNode node) { }
    public virtual void Visit(ConstructorDeclarationNode node) { }
    public virtual void Visit(ConstructorInitializerNode node) { }
    public virtual void Visit(ConstStatementNode node) { }
    public virtual void Visit(ConstTagNode node) { }
    public virtual void Visit(ContinueStatementNode node) { }
    public virtual void Visit(DecimalLiteralNode node) { }
    public virtual void Visit(DefaultValueOperatorNode node) { }
    public virtual void Visit(DefinePragmaNode node) { }
    public virtual void Visit(DelegateDeclarationNode node) { }
    public virtual void Visit(DoubleLiteralNode node) { }
    public virtual void Visit(DoWhileStatementNode node) { }
    public virtual void Visit(ElementAccessNode node) { }
    public virtual void Visit(ElementInitializerNode node) { }
    public virtual void Visit(ElseIfPragmaNode node) { }
    public virtual void Visit(ElsePragmaNode node) { }
    public virtual void Visit(EmbeddedTypeOperatorNode node) { }
    public virtual void Visit(EmptyStatementNode node) { }
    public virtual void Visit(EndIfPragmaNode node) { }
    public virtual void Visit(EndRegionPragmaNode node) { }
    public virtual void Visit(EnumDeclarationNode node) { }
    public virtual void Visit(EnumValueNode node) { }
    public virtual void Visit(EventPropertyDeclarationNode node) { }
    public virtual void Visit(ExpressionInitializerNode node) { }
    public virtual void Visit(ExpressionNode node) { }
    public virtual void Visit(ExpressionStatementNode node) { }
    public virtual void Visit(ExternAliasNode node) { }
    public virtual void Visit(FalseLiteralNode node) { }
    public virtual void Visit(FieldDeclarationNode node) { }
    public virtual void Visit(FieldTagNode node) { }
    public virtual void Visit(FinalizerDeclarationNode node) { }
    public virtual void Visit(FixedInitializerNode node) { }
    public virtual void Visit(FixedStatementNode node) { }
    public virtual void Visit(ForeachStatementNode node) { }
    public virtual void Visit(FormalParameterListNode node) { }
    public virtual void Visit(FormalParameterNode node) { }
    public virtual void Visit(ForStatementNode node) { }
    public virtual void Visit(FromClauseNode node) { }
    public virtual void Visit(GotoStatementNode node) { }
    public virtual void Visit(GroupByClauseNode node) { }
    public virtual void Visit(ICommentNode node) { }
    public virtual void Visit(IdentifierMemberDeclaratorNode node) { }
    public virtual void Visit(IfPragmaNode node) { }
    public virtual void Visit(IfStatementNode node) { }
    public virtual void Visit(IndexerDeclarationNode node) { }
    public virtual void Visit(Int32LiteralNode node) { }
    public virtual void Visit(Int64LiteralNode node) { }
    public virtual void Visit(IntegerLiteralNode node) { }
    public virtual void Visit(InterfaceDeclarationNode node) { }
    public virtual void Visit(InterfaceEventDeclarationNode node) { }
    public virtual void Visit(IntoClauseNode node) { }
    public virtual void Visit(InvocationOperatorNode node) { }
    public virtual void Visit(ISyntaxNode node) { }
    public virtual void Visit(JoinClauseNode node) { }
    public virtual void Visit(JoinIntoClauseNode node) { }
    public virtual void Visit(LabelNode node) { }
    public virtual void Visit(LambdaExpressionNode node) { }
    public virtual void Visit(LetClauseNode node) { }
    public virtual void Visit(LineCommentNode node) { }
    public virtual void Visit(LiteralNode node) { }
    public virtual void Visit(LocalVariableNode node) { }
    public virtual void Visit(LocalVariableTagNode node) { }
    public virtual void Visit(LockStatementNode node) { }
    public virtual void Visit(MemberAccessMemberDeclaratorNode node) { }
    public virtual void Visit(MemberDeclarationNode node) { }
    public virtual void Visit(MemberDeclaratorNode node) { }
    public virtual void Visit(MemberInitializerNode node) { }
    public virtual void Visit(MemberWithBodyDeclarationNode node) { }
    public virtual void Visit(MethodDeclarationNode node) { }
    public virtual void Visit(ModifierNode node) { }
    public virtual void Visit(MultiCommentNode node) { }
    public virtual void Visit(NamespaceDeclarationNode node) { }
    public virtual void Visit(NamespaceScopeNode node) { }
    public virtual void Visit(NameTagNode node) { }
    public virtual void Visit(NewOperatorNode node) { }
    public virtual void Visit(NullLiteralNode node) { }
    public virtual void Visit(ObjectCreationExpressionNode node) { }
    public virtual void Visit(ObjectOrCollectionInitializerNode node) { }
    public virtual void Visit(OperatorDeclarationNode node) { }
    public virtual void Visit(OperatorNode node) { }
    public virtual void Visit(OrderByClauseNode node) { }
    public virtual void Visit(OrderingClauseNode node) { }
    public virtual void Visit(ParenthesizedExpressionNode node) { }
    public virtual void Visit(PointerMemberAccessOperatorNode node) { }
    public virtual void Visit(PostDecrementOperatorNode node) { }
    public virtual void Visit(PostIncrementOperatorNode node) { }
    public virtual void Visit(PragmaNode node) { }
    public virtual void Visit(PragmaPragmaNode node) { }
    public virtual void Visit(PreDecrementOperatorNode node) { }
    public virtual void Visit(PredefinedTypeMemberAccessNode node) { }
    public virtual void Visit(PreIncrementOperatorNode node) { }
    public virtual void Visit(PrimaryMemberAccessOperatorNode node) { }
    public virtual void Visit(PrimaryOperatorNode node) { }
    public virtual void Visit(PropertyDeclarationNode node) { }
    public virtual void Visit(QualifiedAliasMemberAccessNode node) { }
    public virtual void Visit(QualifiedAliasMemberNode node) { }
    public virtual void Visit(QueryBodyClauseNode node) { }
    public virtual void Visit(QueryBodyNode node) { }
    public virtual void Visit(QueryExpressionNode node) { }
    public virtual void Visit(RankSpecifierNode node) { }
    public virtual void Visit(RealLiteralNode node) { }
    public virtual void Visit(RegionPragmaNode node) { }
    public virtual void Visit(ReturnStatementNode node) { }
    public virtual void Visit(SelectClauseNode node) { }
    public virtual void Visit(SimpleNameMemberDeclaratorNode node) { }
    public virtual void Visit(SimpleNameNode node) { }
    public virtual void Visit(SingleLiteralNode node) { }
    public virtual void Visit(SizeofOperatorNode node) { }
    public virtual void Visit(StackAllocInitializerNode node) { }
    public virtual void Visit(StatementNode node) { }
    public virtual void Visit(StringLiteralNode node) { }
    public virtual void Visit(StructDeclarationNode node) { }
    public virtual void Visit(SwitchLabelNode node) { }
    public virtual void Visit(SwitchSectionNode node) { }
    public virtual void Visit(SwitchStatementNode node) { }
    public virtual void Visit(ThisAccessNode node) { }
    public virtual void Visit(ThisConstructorInitializerNode node) { }
    public virtual void Visit(ThrowStatementNode node) { }
    public virtual void Visit(TrueLiteralNode node) { }
    public virtual void Visit(TryStatementNode node) { }
    public virtual void Visit(TypecastOperatorNode node) { }
    public virtual void Visit(TypeDeclarationNode node) { }
    public virtual void Visit(TypeofOperatorNode node) { }
    public virtual void Visit(TypeOrMemberDeclarationNode node) { }
    public virtual void Visit(TypeOrNamespaceNode node) { }
    public virtual void Visit(TypeParameterConstraintNode node) { }
    public virtual void Visit(TypeParameterConstraintTagNode node) { }
    public virtual void Visit(TypeParameterListNode node) { }
    public virtual void Visit(TypeParameterNode node) { }
    public virtual void Visit(TypeTagNode node) { }
    public virtual void Visit(TypeTestingOperatorNode node) { }
    public virtual void Visit(TypeWithBodyDeclarationNode node) { }
    public virtual void Visit(TypeWithMembersDeclarationNode node) { }
    public virtual void Visit(UInt32LiteralNode node) { }
    public virtual void Visit(UInt64LiteralNode node) { }
    public virtual void Visit(UnaryOperatorExpressionNode node) { }
    public virtual void Visit(UnaryOperatorNode node) { }
    public virtual void Visit(UncheckedOperatorNode node) { }
    public virtual void Visit(UncheckedStatementNode node) { }
    public virtual void Visit(UndefPragmaNode node) { }
    public virtual void Visit(UnsafeStatementNode node) { }
    public virtual void Visit(UsingAliasNode node) { }
    public virtual void Visit(UsingNamespaceNode node) { }
    public virtual void Visit(UsingStatementNode node) { }
    public virtual void Visit(VariableDeclarationStatementNode node) { }
    public virtual void Visit(VariableInitializerNode node) { }
    public virtual void Visit(WhereClauseNode node) { }
    public virtual void Visit(WhileStatementNode node) { }
    public virtual void Visit(YieldBreakStatementNode node) { }
    public virtual void Visit(YieldReturnStatementNode node) { }
  }
}
#pragma warning restore 1591