//
// WARNING! This file is generated, do not modify it manually!
//
// Generated on: 2009. 06. 10. 22:18:08
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
    void Visit(AddOperatorNode node);
    void Visit(AndAssignmentOperatorNode node);
    void Visit(AnonymousDelegateNode node);
    void Visit(ArgumentNode node);
    void Visit(ArrayIndexerInvocationOperatorNode node);
    void Visit(ArrayInitializerNode node);
    void Visit(ArrayItemInitializerNode node);
    void Visit(ArrayModifierNode node);
    void Visit(AsOperatorNode node);
    void Visit(AssignmentOperatorNode node);
    void Visit(AttributeArgumentNode node);
    void Visit(AttributedDeclarationNode node);
    void Visit(AttributeDecorationNode node);
    void Visit(AttributeNode node);
    void Visit(BaseNode node);
    void Visit(BinaryOperatorNode node);
    void Visit(BitwiseAndOperatorNode node);
    void Visit(BitwiseNotOperatorNode node);
    void Visit(BitwiseOrOperatorNode node);
    void Visit(BitwiseXorOperatorNode node);
    void Visit(BlockStatementNode node);
    void Visit(BlockWrappingStatementNode node);
    void Visit(BooleanNode node);
    void Visit(BreakStatementNode node);
    void Visit(CatchClauseNode node);
    void Visit(CharNode node);
    void Visit(CheckedOperatorNode node);
    void Visit(CheckedStatementNode node);
    void Visit(ClassDeclarationNode node);
    void Visit(ConditionalOperatorNode node);
    void Visit(ConstDeclarationNode node);
    void Visit(ConstructorDeclarationNode node);
    void Visit(ConstStatementNode node);
    void Visit(ConstTagNode node);
    void Visit(ContinueStatementNode node);
    void Visit(CTypeMemberAccessOperatorNode node);
    void Visit(DecimalNode node);
    void Visit(DefaultOperatorNode node);
    void Visit(DelegateDeclarationNode node);
    void Visit(DivideAssignmentOperatorNode node);
    void Visit(DivideOperatorNode node);
    void Visit(DoubleNode node);
    void Visit(DoWhileStatementNode node);
    void Visit(ElementInitializerNode node);
    void Visit(EmbeddedExpressionNode node);
    void Visit(EmbeddedTypeOperatorNode node);
    void Visit(EmptyStatementNode node);
    void Visit(EnumDeclarationNode node);
    void Visit(EnumValueNode node);
    void Visit(EqualOperatorNode node);
    void Visit(EventPropertyDeclarationNode node);
    void Visit(ExpressionInitializerNode node);
    void Visit(ExpressionNode node);
    void Visit(ExpressionStatementNode node);
    void Visit(ExternAliasNode node);
    void Visit(FalseNode node);
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
    void Visit(GreaterThanOperatorNode node);
    void Visit(GreaterThanOrEqualOperatorNode node);
    void Visit(GroupByClauseNode node);
    void Visit(IfStatementNode node);
    void Visit(Int32Node node);
    void Visit(Int64Node node);
    void Visit(IntegerConstantNode node);
    void Visit(InterfaceDeclarationNode node);
    void Visit(IntoClauseNode node);
    void Visit(InvocationOperatorNode node);
    void Visit(IsOperatorNode node);
    void Visit(ISyntaxNode node);
    void Visit(JoinClauseNode node);
    void Visit(LabelNode node);
    void Visit(LambdaExpressionNode node);
    void Visit(LeftShiftAssignmentOperatorNode node);
    void Visit(LeftShiftOperatorNode node);
    void Visit(LessThanOperatorNode node);
    void Visit(LessThanOrEqualOperatorNode node);
    void Visit(LetClauseNode node);
    void Visit(LiteralNode node);
    void Visit(LocalVariableNode node);
    void Visit(LocalVariableTagNode node);
    void Visit(LockStatementNode node);
    void Visit(LogicalAndOperatorNode node);
    void Visit(LogicalOrOperatorNode node);
    void Visit(MemberAccessOperatorNode node);
    void Visit(MemberDeclarationNode node);
    void Visit(MemberDeclaratorNode node);
    void Visit(MemberInitializerNode node);
    void Visit(MemberWithBodyDeclarationNode node);
    void Visit(MethodDeclarationNode node);
    void Visit(MethodInvocationOperatorNode node);
    void Visit(MinusAssignmentOperatorNode node);
    void Visit(ModifierNode node);
    void Visit(ModuloAssignmentOperatorNode node);
    void Visit(ModuloOperatorNode node);
    void Visit(MultiplyAssignmentOperatorNode node);
    void Visit(MultiplyOperatorNode node);
    void Visit(NamespaceDeclarationNode node);
    void Visit(NamespaceScopeNode node);
    void Visit(NameTagNode node);
    void Visit(NewOperatorNode node);
    void Visit(NewOperatorWithAnonymousTypeNode node);
    void Visit(NewOperatorWithArrayNode node);
    void Visit(NewOperatorWithConstructorNode node);
    void Visit(NotEqualOperatorNode node);
    void Visit(NullCoalescingOperatorNode node);
    void Visit(NullNode node);
    void Visit(ObjectOrCollectionInitializerNode node);
    void Visit(OperatorNode node);
    void Visit(OrAssignmentOperatorNode node);
    void Visit(OrderByClauseNode node);
    void Visit(OrderingClauseNode node);
    void Visit(ParenthesisExpressionNode node);
    void Visit(PlusAssignmentOperatorNode node);
    void Visit(PointerModifierNode node);
    void Visit(PointerOperatorNode node);
    void Visit(PostDecrementOperatorNode node);
    void Visit(PostIncrementOperatorNode node);
    void Visit(PreDecrementOperatorNode node);
    void Visit(PreIncrementOperatorNode node);
    void Visit(PrimaryOperatorNode node);
    void Visit(PrimitiveNamedNode node);
    void Visit(PropertyDeclarationNode node);
    void Visit(QueryBodyClauseNode node);
    void Visit(QueryBodyNode node);
    void Visit(QueryExpressionNode node);
    void Visit(RealConstantNode node);
    void Visit(ReferenceOperatorNode node);
    void Visit(ReturnStatementNode node);
    void Visit(RightShiftAssignmentOperatorNode node);
    void Visit(RightShiftOperatorNode node);
    void Visit(ScopedNameNode node);
    void Visit(SelectClauseNode node);
    void Visit(SimpleNameNode node);
    void Visit(SingleNode node);
    void Visit(SizedArrayDimensionNode node);
    void Visit(SizeofOperatorNode node);
    void Visit(SourceFileNode node);
    void Visit(StackAllocInitializerNode node);
    void Visit(StatementNode node);
    void Visit(StringNode node);
    void Visit(StructDeclarationNode node);
    void Visit(SubtractOperatorNode node);
    void Visit(SwitchLabelNode node);
    void Visit(SwitchSectionNode node);
    void Visit(SwitchStatementNode node);
    void Visit(ThisNode node);
    void Visit(ThrowStatementNode node);
    void Visit(TrueNode node);
    void Visit(TryStatementNode node);
    void Visit(TypecastOperatorNode node);
    void Visit(TypeDeclarationNode node);
    void Visit(TypeModifierNode node);
    void Visit(TypeofOperatorNode node);
    void Visit(TypeOperatorNode node);
    void Visit(TypeOrMemberDeclarationNode node);
    void Visit(TypeOrNamespaceNode node);
    void Visit(TypeParameterConstraintNode node);
    void Visit(TypeParameterConstraintTagNode node);
    void Visit(TypeParameterListNode node);
    void Visit(TypeParameterNode node);
    void Visit(TypeTagNode node);
    void Visit(TypeWithBodyDeclarationNode node);
    void Visit(TypeWithMembersDeclarationNode node);
    void Visit(UInt32Node node);
    void Visit(UInt64Node node);
    void Visit(UnaryMinusOperatorNode node);
    void Visit(UnaryNotOperatorNode node);
    void Visit(UnaryOperatorNode node);
    void Visit(UnaryPlusOperatorNode node);
    void Visit(UncheckedOperatorNode node);
    void Visit(UncheckedStatementNode node);
    void Visit(UnsafeStatementNode node);
    void Visit(UsingAliasNode node);
    void Visit(UsingNamespaceNode node);
    void Visit(UsingStatementNode node);
    void Visit(VariableDeclarationStatementNode node);
    void Visit(VariableInitializerNode node);
    void Visit(WhereClauseNode node);
    void Visit(WhileStatementNode node);
    void Visit(XorAssignmentOperatorNode node);
    void Visit(YieldBreakStatementNode node);
    void Visit(YieldReturnStatementNode node);
  }
}
#pragma warning restore 1591