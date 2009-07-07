//
// WARNING! This file is generated, do not modify it manually!
//
// Generated on: 2009. 07. 07. 14:31:46
//
// The template file is ISyntaxNodeVisitor.tt which uses SyntaxNode_filelist.txt as input.
// You can modify SyntaxNode_filelist.txt manually, 
// or you can regenerate it using generate_SyntaxNode_filelist.cmd.
//

// disable warnings about missing XML comments
#pragma warning disable 1591 

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// Interface for traversing the CSharpSyntaxTree with Visitor pattern.
  /// </summary>
  // ================================================================================================
  public interface ISyntaxNodeVisitor
  {
    void Visit(AccessorNode node);
    void Visit(AnonymousMethodNode node);
    void Visit(AnonymousObjectCreationExpressionNode node);
    void Visit(ArgumentNode node);
    void Visit(ArrayCreationExpressionNode node);
    void Visit(ArrayInitializerNode node);
    void Visit(ArrayItemInitializerNode node);
    void Visit(ArraySizeSpecifierNode node);
    void Visit(AssignmentOperatorNode node);
    void Visit(AttributeArgumentNode node);
    void Visit(AttributedDeclarationNode node);
    void Visit(AttributeDecorationNode node);
    void Visit(AttributeNode node);
    void Visit(BaseAccessNode node);
    void Visit(BaseConstructorInitializerNode node);
    void Visit(BaseElementAccessNode node);
    void Visit(BaseMemberAccessNode node);
    void Visit(BinaryOperatorNode node);
    void Visit(BlockCommentNode node);
    void Visit(BlockStatementNode node);
    void Visit(BlockWrappingStatementNode node);
    void Visit(BooleanLiteralNode node);
    void Visit(BreakStatementNode node);
    void Visit(CastOperatorDeclarationNode node);
    void Visit(CatchClauseNode node);
    void Visit(CharLiteralNode node);
    void Visit(CheckedOperatorNode node);
    void Visit(CheckedStatementNode node);
    void Visit(ClassDeclarationNode node);
    void Visit(CompilationUnitNode node);
    void Visit(ConditionalOperatorNode node);
    void Visit(ConditionalPragmaNode node);
    void Visit(ConstDeclarationNode node);
    void Visit(ConstructorDeclarationNode node);
    void Visit(ConstructorInitializerNode node);
    void Visit(ConstStatementNode node);
    void Visit(ConstTagNode node);
    void Visit(ContinueStatementNode node);
    void Visit(DecimalLiteralNode node);
    void Visit(DefaultValueOperatorNode node);
    void Visit(DefinePragmaNode node);
    void Visit(DelegateDeclarationNode node);
    void Visit(DoubleLiteralNode node);
    void Visit(DoWhileStatementNode node);
    void Visit(ElementAccessNode node);
    void Visit(ElementInitializerNode node);
    void Visit(ElseIfPragmaNode node);
    void Visit(ElsePragmaNode node);
    void Visit(EmbeddedTypeOperatorNode node);
    void Visit(EmptyStatementNode node);
    void Visit(EndIfPragmaNode node);
    void Visit(EndRegionPragmaNode node);
    void Visit(EnumDeclarationNode node);
    void Visit(EnumValueNode node);
    void Visit(EventPropertyDeclarationNode node);
    void Visit(ExpressionInitializerNode node);
    void Visit(ExpressionNode node);
    void Visit(ExpressionStatementNode node);
    void Visit(ExternAliasNode node);
    void Visit(FalseLiteralNode node);
    void Visit(FieldDeclarationNode node);
    void Visit(FieldTagNode node);
    void Visit(FinalizerDeclarationNode node);
    void Visit(FixedInitializerNode node);
    void Visit(FixedStatementNode node);
    void Visit(ForeachStatementNode node);
    void Visit(FormalParameterListNode node);
    void Visit(FormalParameterNode node);
    void Visit(ForStatementNode node);
    void Visit(FromClauseNode node);
    void Visit(GotoStatementNode node);
    void Visit(GroupByClauseNode node);
    void Visit(ICommentNode node);
    void Visit(IdentifierMemberDeclaratorNode node);
    void Visit(IfPragmaNode node);
    void Visit(IfStatementNode node);
    void Visit(IndexerDeclarationNode node);
    void Visit(Int32LiteralNode node);
    void Visit(Int64LiteralNode node);
    void Visit(IntegerLiteralNode node);
    void Visit(InterfaceDeclarationNode node);
    void Visit(InterfaceEventDeclarationNode node);
    void Visit(IntoClauseNode node);
    void Visit(InvocationOperatorNode node);
    void Visit(ISyntaxNode node);
    void Visit(JoinClauseNode node);
    void Visit(LabelNode node);
    void Visit(LambdaExpressionNode node);
    void Visit(LetClauseNode node);
    void Visit(LineCommentNode node);
    void Visit(LiteralNode node);
    void Visit(LocalVariableNode node);
    void Visit(LocalVariableTagNode node);
    void Visit(LockStatementNode node);
    void Visit(MemberAccessMemberDeclaratorNode node);
    void Visit(MemberDeclarationNode node);
    void Visit(MemberDeclaratorNode node);
    void Visit(MemberInitializerNode node);
    void Visit(MemberWithBodyDeclarationNode node);
    void Visit(MethodDeclarationNode node);
    void Visit(ModifierNode node);
    void Visit(MultiCommentNode node);
    void Visit(NamespaceDeclarationNode node);
    void Visit(NamespaceScopeNode node);
    void Visit(NameTagNode node);
    void Visit(NewOperatorNode node);
    void Visit(NullLiteralNode node);
    void Visit(ObjectCreationExpressionNode node);
    void Visit(ObjectOrCollectionInitializerNode node);
    void Visit(OperatorDeclarationNode node);
    void Visit(OperatorNode node);
    void Visit(OrderByClauseNode node);
    void Visit(OrderingClauseNode node);
    void Visit(ParenthesizedExpressionNode node);
    void Visit(PointerMemberAccessOperatorNode node);
    void Visit(PostDecrementOperatorNode node);
    void Visit(PostIncrementOperatorNode node);
    void Visit(PragmaNode node);
    void Visit(PragmaPragmaNode node);
    void Visit(PreDecrementOperatorNode node);
    void Visit(PredefinedTypeMemberAccessNode node);
    void Visit(PreIncrementOperatorNode node);
    void Visit(PrimaryMemberAccessOperatorNode node);
    void Visit(PrimaryOperatorNode node);
    void Visit(PropertyDeclarationNode node);
    void Visit(QualifiedAliasMemberAccessNode node);
    void Visit(QualifiedAliasMemberNode node);
    void Visit(QueryBodyClauseNode node);
    void Visit(QueryBodyNode node);
    void Visit(QueryExpressionNode node);
    void Visit(RankSpecifierNode node);
    void Visit(RealLiteralNode node);
    void Visit(RegionPragmaNode node);
    void Visit(ReturnStatementNode node);
    void Visit(SelectClauseNode node);
    void Visit(SimpleNameMemberDeclaratorNode node);
    void Visit(SimpleNameNode node);
    void Visit(SingleLiteralNode node);
    void Visit(SizeofOperatorNode node);
    void Visit(StackAllocInitializerNode node);
    void Visit(StatementNode node);
    void Visit(StringLiteralNode node);
    void Visit(StructDeclarationNode node);
    void Visit(SwitchLabelNode node);
    void Visit(SwitchSectionNode node);
    void Visit(SwitchStatementNode node);
    void Visit(ThisAccessNode node);
    void Visit(ThisConstructorInitializerNode node);
    void Visit(ThrowStatementNode node);
    void Visit(TrueLiteralNode node);
    void Visit(TryStatementNode node);
    void Visit(TypecastOperatorNode node);
    void Visit(TypeDeclarationNode node);
    void Visit(TypeofOperatorNode node);
    void Visit(TypeOrMemberDeclarationNode node);
    void Visit(TypeOrNamespaceNode node);
    void Visit(TypeParameterConstraintNode node);
    void Visit(TypeParameterConstraintTagNode node);
    void Visit(TypeParameterListNode node);
    void Visit(TypeParameterNode node);
    void Visit(TypeTagNode node);
    void Visit(TypeTestingOperatorNode node);
    void Visit(TypeWithBodyDeclarationNode node);
    void Visit(TypeWithMembersDeclarationNode node);
    void Visit(UInt32LiteralNode node);
    void Visit(UInt64LiteralNode node);
    void Visit(UnaryOperatorExpressionNode node);
    void Visit(UnaryOperatorNode node);
    void Visit(UncheckedOperatorNode node);
    void Visit(UncheckedStatementNode node);
    void Visit(UndefPragmaNode node);
    void Visit(UnsafeStatementNode node);
    void Visit(UsingAliasNode node);
    void Visit(UsingNamespaceNode node);
    void Visit(UsingStatementNode node);
    void Visit(VariableDeclarationStatementNode node);
    void Visit(VariableInitializerNode node);
    void Visit(WhereClauseNode node);
    void Visit(WhileStatementNode node);
    void Visit(YieldBreakStatementNode node);
    void Visit(YieldReturnStatementNode node);
  }
}
#pragma warning restore 1591