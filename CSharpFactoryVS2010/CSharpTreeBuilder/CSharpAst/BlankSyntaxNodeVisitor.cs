//
// WARNING! This file is generated, do not modify it manually!
//
// Generated on: 2011. 04. 24. 13:53:12
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
    public virtual bool Visit(ISyntaxNode node) { return true; }
    public virtual bool Visit(LocalVariableNode node) { return true; }
    public virtual bool Visit(LocalVariableTagNode node) { return true; }
    public virtual bool Visit(ArraySizeSpecifierNode node) { return true; }
    public virtual bool Visit(AttributedDeclarationNode node) { return true; }
    public virtual bool Visit(ClassDeclarationNode node) { return true; }
    public virtual bool Visit(DelegateDeclarationNode node) { return true; }
    public virtual bool Visit(EnumDeclarationNode node) { return true; }
    public virtual bool Visit(InterfaceDeclarationNode node) { return true; }
    public virtual bool Visit(ModifierNode node) { return true; }
    public virtual bool Visit(NamespaceOrTypeNameNode node) { return true; }
    public virtual bool Visit(NameTagNode node) { return true; }
    public virtual bool Visit(QualifiedAliasMemberNode node) { return true; }
    public virtual bool Visit(RankSpecifierNode node) { return true; }
    public virtual bool Visit(StructDeclarationNode node) { return true; }
    public virtual bool Visit(TypeDeclarationNode node) { return true; }
    public virtual bool Visit(TypeNode node) { return true; }
    public virtual bool Visit(TypeOrMemberDeclarationNode node) { return true; }
    public virtual bool Visit(TypeParameterConstraintNode node) { return true; }
    public virtual bool Visit(TypeParameterConstraintTagNode node) { return true; }
    public virtual bool Visit(TypeParameterNode node) { return true; }
    public virtual bool Visit(TypeTagNode node) { return true; }
    public virtual bool Visit(TypeWithBodyDeclarationNode node) { return true; }
    public virtual bool Visit(TypeWithMembersDeclarationNode node) { return true; }
    public virtual bool Visit(BlockStatementNode node) { return true; }
    public virtual bool Visit(BlockWrappingStatementNode node) { return true; }
    public virtual bool Visit(BreakStatementNode node) { return true; }
    public virtual bool Visit(CatchClauseNode node) { return true; }
    public virtual bool Visit(CheckedStatementNode node) { return true; }
    public virtual bool Visit(ConstStatementNode node) { return true; }
    public virtual bool Visit(ContinueStatementNode node) { return true; }
    public virtual bool Visit(DoWhileStatementNode node) { return true; }
    public virtual bool Visit(EmptyStatementNode node) { return true; }
    public virtual bool Visit(ExpressionStatementNode node) { return true; }
    public virtual bool Visit(FixedInitializerNode node) { return true; }
    public virtual bool Visit(FixedStatementNode node) { return true; }
    public virtual bool Visit(ForeachStatementNode node) { return true; }
    public virtual bool Visit(ForStatementNode node) { return true; }
    public virtual bool Visit(GotoStatementNode node) { return true; }
    public virtual bool Visit(IfStatementNode node) { return true; }
    public virtual bool Visit(LabelNode node) { return true; }
    public virtual bool Visit(LockStatementNode node) { return true; }
    public virtual bool Visit(ReturnStatementNode node) { return true; }
    public virtual bool Visit(StatementNode node) { return true; }
    public virtual bool Visit(SwitchLabelNode node) { return true; }
    public virtual bool Visit(SwitchSectionNode node) { return true; }
    public virtual bool Visit(SwitchStatementNode node) { return true; }
    public virtual bool Visit(ThrowStatementNode node) { return true; }
    public virtual bool Visit(TryStatementNode node) { return true; }
    public virtual bool Visit(UncheckedStatementNode node) { return true; }
    public virtual bool Visit(UnsafeStatementNode node) { return true; }
    public virtual bool Visit(UsingStatementNode node) { return true; }
    public virtual bool Visit(VariableDeclarationStatementNode node) { return true; }
    public virtual bool Visit(WhileStatementNode node) { return true; }
    public virtual bool Visit(YieldBreakStatementNode node) { return true; }
    public virtual bool Visit(YieldReturnStatementNode node) { return true; }
    public virtual bool Visit(AttributeArgumentNode node) { return true; }
    public virtual bool Visit(AttributeDecorationNode node) { return true; }
    public virtual bool Visit(AttributeNode node) { return true; }
    public virtual bool Visit(CompilationUnitNode node) { return true; }
    public virtual bool Visit(ExternAliasNode node) { return true; }
    public virtual bool Visit(NamespaceDeclarationNode node) { return true; }
    public virtual bool Visit(NamespaceScopeNode node) { return true; }
    public virtual bool Visit(UsingAliasNode node) { return true; }
    public virtual bool Visit(UsingNamespaceNode node) { return true; }
    public virtual bool Visit(ConditionalPragmaNode node) { return true; }
    public virtual bool Visit(DefinePragmaNode node) { return true; }
    public virtual bool Visit(ElseIfPragmaNode node) { return true; }
    public virtual bool Visit(ElsePragmaNode node) { return true; }
    public virtual bool Visit(EndIfPragmaNode node) { return true; }
    public virtual bool Visit(EndRegionPragmaNode node) { return true; }
    public virtual bool Visit(IfPragmaNode node) { return true; }
    public virtual bool Visit(PragmaNode node) { return true; }
    public virtual bool Visit(PragmaPragmaNode node) { return true; }
    public virtual bool Visit(RegionPragmaNode node) { return true; }
    public virtual bool Visit(UndefPragmaNode node) { return true; }
    public virtual bool Visit(AccessorNode node) { return true; }
    public virtual bool Visit(BaseConstructorInitializerNode node) { return true; }
    public virtual bool Visit(CastOperatorDeclarationNode node) { return true; }
    public virtual bool Visit(ConstDeclarationNode node) { return true; }
    public virtual bool Visit(ConstructorDeclarationNode node) { return true; }
    public virtual bool Visit(ConstructorInitializerNode node) { return true; }
    public virtual bool Visit(ConstTagNode node) { return true; }
    public virtual bool Visit(DestructorDeclarationNode node) { return true; }
    public virtual bool Visit(EnumValueNode node) { return true; }
    public virtual bool Visit(EventPropertyDeclarationNode node) { return true; }
    public virtual bool Visit(FieldDeclarationNode node) { return true; }
    public virtual bool Visit(FieldTagNode node) { return true; }
    public virtual bool Visit(FormalParameterNode node) { return true; }
    public virtual bool Visit(IndexerDeclarationNode node) { return true; }
    public virtual bool Visit(InterfaceEventDeclarationNode node) { return true; }
    public virtual bool Visit(MemberDeclarationNode node) { return true; }
    public virtual bool Visit(MemberWithBodyDeclarationNode node) { return true; }
    public virtual bool Visit(MethodDeclarationNode node) { return true; }
    public virtual bool Visit(OperatorDeclarationNode node) { return true; }
    public virtual bool Visit(PropertyDeclarationNode node) { return true; }
    public virtual bool Visit(ThisConstructorInitializerNode node) { return true; }
    public virtual bool Visit(ArrayInitializerNode node) { return true; }
    public virtual bool Visit(BaseMemberAccessMemberDeclaratorNode node) { return true; }
    public virtual bool Visit(ElementInitializerNode node) { return true; }
    public virtual bool Visit(ExpressionInitializerNode node) { return true; }
    public virtual bool Visit(IdentifierMemberDeclaratorNode node) { return true; }
    public virtual bool Visit(MemberAccessMemberDeclaratorNode node) { return true; }
    public virtual bool Visit(MemberDeclaratorNode node) { return true; }
    public virtual bool Visit(MemberInitializerNode node) { return true; }
    public virtual bool Visit(ObjectOrCollectionInitializerNode node) { return true; }
    public virtual bool Visit(SimpleNameMemberDeclaratorNode node) { return true; }
    public virtual bool Visit(StackAllocInitializerNode node) { return true; }
    public virtual bool Visit(VariableInitializerNode node) { return true; }
    public virtual bool Visit(ExpressionNode node) { return true; }
    public virtual bool Visit(CastExpressionNode node) { return true; }
    public virtual bool Visit(PostDecrementExpressionNode node) { return true; }
    public virtual bool Visit(PostIncrementExpressionNode node) { return true; }
    public virtual bool Visit(PreDecrementExpressionNode node) { return true; }
    public virtual bool Visit(PreIncrementExpressionNode node) { return true; }
    public virtual bool Visit(UnaryOperatorExpressionNode node) { return true; }
    public virtual bool Visit(FromClauseNode node) { return true; }
    public virtual bool Visit(GroupClauseNode node) { return true; }
    public virtual bool Visit(JoinClauseNode node) { return true; }
    public virtual bool Visit(JoinIntoClauseNode node) { return true; }
    public virtual bool Visit(LetClauseNode node) { return true; }
    public virtual bool Visit(OrderByClauseNode node) { return true; }
    public virtual bool Visit(OrderingClauseNode node) { return true; }
    public virtual bool Visit(QueryBodyClauseNode node) { return true; }
    public virtual bool Visit(QueryBodyNode node) { return true; }
    public virtual bool Visit(QueryContinuationNode node) { return true; }
    public virtual bool Visit(QueryExpressionNode node) { return true; }
    public virtual bool Visit(SelectClauseNode node) { return true; }
    public virtual bool Visit(WhereClauseNode node) { return true; }
    public virtual bool Visit(AnonymousMethodExpressionNode node) { return true; }
    public virtual bool Visit(AnonymousObjectCreationExpressionNode node) { return true; }
    public virtual bool Visit(ArgumentNode node) { return true; }
    public virtual bool Visit(ArrayCreationExpressionNode node) { return true; }
    public virtual bool Visit(BaseAccessNode node) { return true; }
    public virtual bool Visit(BaseElementAccessNode node) { return true; }
    public virtual bool Visit(BaseMemberAccessNode node) { return true; }
    public virtual bool Visit(CheckedExpressionNode node) { return true; }
    public virtual bool Visit(DefaultValueExpressionNode node) { return true; }
    public virtual bool Visit(ElementAccessNode node) { return true; }
    public virtual bool Visit(InvocationExpressionNode node) { return true; }
    public virtual bool Visit(MemberAccessNode node) { return true; }
    public virtual bool Visit(ObjectCreationExpressionNode node) { return true; }
    public virtual bool Visit(ParenthesizedExpressionNode node) { return true; }
    public virtual bool Visit(PointerMemberAccessNode node) { return true; }
    public virtual bool Visit(PredefinedTypeMemberAccessNode node) { return true; }
    public virtual bool Visit(PrimaryExpressionMemberAccessNode node) { return true; }
    public virtual bool Visit(PrimaryExpressionWithEmbeddedTypeNode node) { return true; }
    public virtual bool Visit(QualifiedAliasMemberAccessNode node) { return true; }
    public virtual bool Visit(SimpleNameNode node) { return true; }
    public virtual bool Visit(SizeofExpressionNode node) { return true; }
    public virtual bool Visit(ThisAccessNode node) { return true; }
    public virtual bool Visit(TypeofExpressionNode node) { return true; }
    public virtual bool Visit(UncheckedExpressionNode node) { return true; }
    public virtual bool Visit(BooleanLiteralNode node) { return true; }
    public virtual bool Visit(CharLiteralNode node) { return true; }
    public virtual bool Visit(DecimalLiteralNode node) { return true; }
    public virtual bool Visit(DoubleLiteralNode node) { return true; }
    public virtual bool Visit(FalseLiteralNode node) { return true; }
    public virtual bool Visit(Int32LiteralNode node) { return true; }
    public virtual bool Visit(Int64LiteralNode node) { return true; }
    public virtual bool Visit(IntegerLiteralNode node) { return true; }
    public virtual bool Visit(LiteralNode node) { return true; }
    public virtual bool Visit(NullLiteralNode node) { return true; }
    public virtual bool Visit(RealLiteralNode node) { return true; }
    public virtual bool Visit(SingleLiteralNode node) { return true; }
    public virtual bool Visit(StringLiteralNode node) { return true; }
    public virtual bool Visit(TrueLiteralNode node) { return true; }
    public virtual bool Visit(UInt32LiteralNode node) { return true; }
    public virtual bool Visit(UInt64LiteralNode node) { return true; }
    public virtual bool Visit(LambdaExpressionNode node) { return true; }
    public virtual bool Visit(ConditionalExpressionNode node) { return true; }
    public virtual bool Visit(AssignmentExpressionNode node) { return true; }
    public virtual bool Visit(BinaryExpressionNode node) { return true; }
    public virtual bool Visit(TypeTestingExpressionNode node) { return true; }
    public virtual bool Visit(BlockCommentNode node) { return true; }
    public virtual bool Visit(ICommentNode node) { return true; }
    public virtual bool Visit(LineCommentNode node) { return true; }
    public virtual bool Visit(MultiCommentNode node) { return true; }
  }
}
#pragma warning restore 1591