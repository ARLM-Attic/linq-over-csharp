//
// WARNING! This file is generated, do not modify it manually!
//
// Generated on: 2009. 09. 07. 14:40:17
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
    bool Visit(AccessorNode node);
    bool Visit(AnonymousMethodExpressionNode node);
    bool Visit(AnonymousObjectCreationExpressionNode node);
    bool Visit(ArgumentNode node);
    bool Visit(ArrayCreationExpressionNode node);
    bool Visit(ArrayInitializerNode node);
    bool Visit(ArraySizeSpecifierNode node);
    bool Visit(AssignmentExpressionNode node);
    bool Visit(AttributeArgumentNode node);
    bool Visit(AttributedDeclarationNode node);
    bool Visit(AttributeDecorationNode node);
    bool Visit(AttributeNode node);
    bool Visit(BaseAccessNode node);
    bool Visit(BaseConstructorInitializerNode node);
    bool Visit(BaseElementAccessNode node);
    bool Visit(BaseMemberAccessMemberDeclaratorNode node);
    bool Visit(BaseMemberAccessNode node);
    bool Visit(BinaryExpressionNode node);
    bool Visit(BlockCommentNode node);
    bool Visit(BlockStatementNode node);
    bool Visit(BlockWrappingStatementNode node);
    bool Visit(BooleanLiteralNode node);
    bool Visit(BreakStatementNode node);
    bool Visit(CastExpressionNode node);
    bool Visit(CastOperatorDeclarationNode node);
    bool Visit(CatchClauseNode node);
    bool Visit(CharLiteralNode node);
    bool Visit(CheckedExpressionNode node);
    bool Visit(CheckedStatementNode node);
    bool Visit(ClassDeclarationNode node);
    bool Visit(CompilationUnitNode node);
    bool Visit(ConditionalExpressionNode node);
    bool Visit(ConditionalPragmaNode node);
    bool Visit(ConstDeclarationNode node);
    bool Visit(ConstructorDeclarationNode node);
    bool Visit(ConstructorInitializerNode node);
    bool Visit(ConstStatementNode node);
    bool Visit(ConstTagNode node);
    bool Visit(ContinueStatementNode node);
    bool Visit(DecimalLiteralNode node);
    bool Visit(DefaultValueExpressionNode node);
    bool Visit(DefinePragmaNode node);
    bool Visit(DelegateDeclarationNode node);
    bool Visit(DestructorDeclarationNode node);
    bool Visit(DoubleLiteralNode node);
    bool Visit(DoWhileStatementNode node);
    bool Visit(ElementAccessNode node);
    bool Visit(ElementInitializerNode node);
    bool Visit(ElseIfPragmaNode node);
    bool Visit(ElsePragmaNode node);
    bool Visit(EmptyStatementNode node);
    bool Visit(EndIfPragmaNode node);
    bool Visit(EndRegionPragmaNode node);
    bool Visit(EnumDeclarationNode node);
    bool Visit(EnumValueNode node);
    bool Visit(EventPropertyDeclarationNode node);
    bool Visit(ExpressionInitializerNode node);
    bool Visit(ExpressionNode node);
    bool Visit(ExpressionStatementNode node);
    bool Visit(ExternAliasNode node);
    bool Visit(FalseLiteralNode node);
    bool Visit(FieldDeclarationNode node);
    bool Visit(FieldTagNode node);
    bool Visit(FixedInitializerNode node);
    bool Visit(FixedStatementNode node);
    bool Visit(ForeachStatementNode node);
    bool Visit(FormalParameterNode node);
    bool Visit(ForStatementNode node);
    bool Visit(FromClauseNode node);
    bool Visit(GotoStatementNode node);
    bool Visit(GroupClauseNode node);
    bool Visit(ICommentNode node);
    bool Visit(IdentifierMemberDeclaratorNode node);
    bool Visit(IfPragmaNode node);
    bool Visit(IfStatementNode node);
    bool Visit(IndexerDeclarationNode node);
    bool Visit(Int32LiteralNode node);
    bool Visit(Int64LiteralNode node);
    bool Visit(IntegerLiteralNode node);
    bool Visit(InterfaceDeclarationNode node);
    bool Visit(InterfaceEventDeclarationNode node);
    bool Visit(InvocationExpressionNode node);
    bool Visit(ISyntaxNode node);
    bool Visit(JoinClauseNode node);
    bool Visit(JoinIntoClauseNode node);
    bool Visit(LabelNode node);
    bool Visit(LambdaExpressionNode node);
    bool Visit(LetClauseNode node);
    bool Visit(LineCommentNode node);
    bool Visit(LiteralNode node);
    bool Visit(LocalVariableNode node);
    bool Visit(LocalVariableTagNode node);
    bool Visit(LockStatementNode node);
    bool Visit(MemberAccessMemberDeclaratorNode node);
    bool Visit(MemberAccessNode node);
    bool Visit(MemberDeclarationNode node);
    bool Visit(MemberDeclaratorNode node);
    bool Visit(MemberInitializerNode node);
    bool Visit(MemberWithBodyDeclarationNode node);
    bool Visit(MethodDeclarationNode node);
    bool Visit(ModifierNode node);
    bool Visit(MultiCommentNode node);
    bool Visit(NamespaceDeclarationNode node);
    bool Visit(NamespaceOrTypeNameNode node);
    bool Visit(NamespaceScopeNode node);
    bool Visit(NameTagNode node);
    bool Visit(NullLiteralNode node);
    bool Visit(ObjectCreationExpressionNode node);
    bool Visit(ObjectOrCollectionInitializerNode node);
    bool Visit(OperatorDeclarationNode node);
    bool Visit(OrderByClauseNode node);
    bool Visit(OrderingClauseNode node);
    bool Visit(ParenthesizedExpressionNode node);
    bool Visit(PointerMemberAccessNode node);
    bool Visit(PostDecrementExpressionNode node);
    bool Visit(PostIncrementExpressionNode node);
    bool Visit(PragmaNode node);
    bool Visit(PragmaPragmaNode node);
    bool Visit(PreDecrementExpressionNode node);
    bool Visit(PredefinedTypeMemberAccessNode node);
    bool Visit(PreIncrementExpressionNode node);
    bool Visit(PrimaryExpressionMemberAccessNode node);
    bool Visit(PrimaryExpressionWithEmbeddedTypeNode node);
    bool Visit(PropertyDeclarationNode node);
    bool Visit(QualifiedAliasMemberAccessNode node);
    bool Visit(QualifiedAliasMemberNode node);
    bool Visit(QueryBodyClauseNode node);
    bool Visit(QueryBodyNode node);
    bool Visit(QueryContinuationNode node);
    bool Visit(QueryExpressionNode node);
    bool Visit(RankSpecifierNode node);
    bool Visit(RealLiteralNode node);
    bool Visit(RegionPragmaNode node);
    bool Visit(ReturnStatementNode node);
    bool Visit(SelectClauseNode node);
    bool Visit(SimpleNameMemberDeclaratorNode node);
    bool Visit(SimpleNameNode node);
    bool Visit(SingleLiteralNode node);
    bool Visit(SizeofExpressionNode node);
    bool Visit(StackAllocInitializerNode node);
    bool Visit(StatementNode node);
    bool Visit(StringLiteralNode node);
    bool Visit(StructDeclarationNode node);
    bool Visit(SwitchLabelNode node);
    bool Visit(SwitchSectionNode node);
    bool Visit(SwitchStatementNode node);
    bool Visit(ThisAccessNode node);
    bool Visit(ThisConstructorInitializerNode node);
    bool Visit(ThrowStatementNode node);
    bool Visit(TrueLiteralNode node);
    bool Visit(TryStatementNode node);
    bool Visit(TypeDeclarationNode node);
    bool Visit(TypeNode node);
    bool Visit(TypeofExpressionNode node);
    bool Visit(TypeOrMemberDeclarationNode node);
    bool Visit(TypeParameterConstraintNode node);
    bool Visit(TypeParameterConstraintTagNode node);
    bool Visit(TypeParameterNode node);
    bool Visit(TypeTagNode node);
    bool Visit(TypeTestingExpressionNode node);
    bool Visit(TypeWithBodyDeclarationNode node);
    bool Visit(TypeWithMembersDeclarationNode node);
    bool Visit(UInt32LiteralNode node);
    bool Visit(UInt64LiteralNode node);
    bool Visit(UnaryOperatorExpressionNode node);
    bool Visit(UncheckedExpressionNode node);
    bool Visit(UncheckedStatementNode node);
    bool Visit(UndefPragmaNode node);
    bool Visit(UnsafeStatementNode node);
    bool Visit(UsingAliasNode node);
    bool Visit(UsingNamespaceNode node);
    bool Visit(UsingStatementNode node);
    bool Visit(VariableDeclarationStatementNode node);
    bool Visit(VariableInitializerNode node);
    bool Visit(WhereClauseNode node);
    bool Visit(WhileStatementNode node);
    bool Visit(YieldBreakStatementNode node);
    bool Visit(YieldReturnStatementNode node);
  }
}
#pragma warning restore 1591