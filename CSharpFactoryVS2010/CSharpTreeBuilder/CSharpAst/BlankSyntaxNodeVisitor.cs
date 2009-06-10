//
// WARNING! This file is generated, do not modify it manually!
//
// Generated on: 2009. 06. 10. 22:18:08
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
    public virtual void Visit(AddOperatorNode node) { }
    public virtual void Visit(AndAssignmentOperatorNode node) { }
    public virtual void Visit(AnonymousDelegateNode node) { }
    public virtual void Visit(ArgumentNode node) { }
    public virtual void Visit(ArrayIndexerInvocationOperatorNode node) { }
    public virtual void Visit(ArrayInitializerNode node) { }
    public virtual void Visit(ArrayItemInitializerNode node) { }
    public virtual void Visit(ArrayModifierNode node) { }
    public virtual void Visit(AsOperatorNode node) { }
    public virtual void Visit(AssignmentOperatorNode node) { }
    public virtual void Visit(AttributeArgumentNode node) { }
    public virtual void Visit(AttributedDeclarationNode node) { }
    public virtual void Visit(AttributeDecorationNode node) { }
    public virtual void Visit(AttributeNode node) { }
    public virtual void Visit(BaseNode node) { }
    public virtual void Visit(BinaryOperatorNode node) { }
    public virtual void Visit(BitwiseAndOperatorNode node) { }
    public virtual void Visit(BitwiseNotOperatorNode node) { }
    public virtual void Visit(BitwiseOrOperatorNode node) { }
    public virtual void Visit(BitwiseXorOperatorNode node) { }
    public virtual void Visit(BlockStatementNode node) { }
    public virtual void Visit(BlockWrappingStatementNode node) { }
    public virtual void Visit(BooleanNode node) { }
    public virtual void Visit(BreakStatementNode node) { }
    public virtual void Visit(CatchClauseNode node) { }
    public virtual void Visit(CharNode node) { }
    public virtual void Visit(CheckedOperatorNode node) { }
    public virtual void Visit(CheckedStatementNode node) { }
    public virtual void Visit(ClassDeclarationNode node) { }
    public virtual void Visit(ConditionalOperatorNode node) { }
    public virtual void Visit(ConstDeclarationNode node) { }
    public virtual void Visit(ConstructorDeclarationNode node) { }
    public virtual void Visit(ConstStatementNode node) { }
    public virtual void Visit(ConstTagNode node) { }
    public virtual void Visit(ContinueStatementNode node) { }
    public virtual void Visit(CTypeMemberAccessOperatorNode node) { }
    public virtual void Visit(DecimalNode node) { }
    public virtual void Visit(DefaultOperatorNode node) { }
    public virtual void Visit(DelegateDeclarationNode node) { }
    public virtual void Visit(DivideAssignmentOperatorNode node) { }
    public virtual void Visit(DivideOperatorNode node) { }
    public virtual void Visit(DoubleNode node) { }
    public virtual void Visit(DoWhileStatementNode node) { }
    public virtual void Visit(ElementInitializerNode node) { }
    public virtual void Visit(EmbeddedExpressionNode node) { }
    public virtual void Visit(EmbeddedTypeOperatorNode node) { }
    public virtual void Visit(EmptyStatementNode node) { }
    public virtual void Visit(EnumDeclarationNode node) { }
    public virtual void Visit(EnumValueNode node) { }
    public virtual void Visit(EqualOperatorNode node) { }
    public virtual void Visit(EventPropertyDeclarationNode node) { }
    public virtual void Visit(ExpressionInitializerNode node) { }
    public virtual void Visit(ExpressionNode node) { }
    public virtual void Visit(ExpressionStatementNode node) { }
    public virtual void Visit(ExternAliasNode node) { }
    public virtual void Visit(FalseNode node) { }
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
    public virtual void Visit(GreaterThanOperatorNode node) { }
    public virtual void Visit(GreaterThanOrEqualOperatorNode node) { }
    public virtual void Visit(GroupByClauseNode node) { }
    public virtual void Visit(IfStatementNode node) { }
    public virtual void Visit(Int32Node node) { }
    public virtual void Visit(Int64Node node) { }
    public virtual void Visit(IntegerConstantNode node) { }
    public virtual void Visit(InterfaceDeclarationNode node) { }
    public virtual void Visit(IntoClauseNode node) { }
    public virtual void Visit(InvocationOperatorNode node) { }
    public virtual void Visit(IsOperatorNode node) { }
    public virtual void Visit(ISyntaxNode node) { }
    public virtual void Visit(JoinClauseNode node) { }
    public virtual void Visit(LabelNode node) { }
    public virtual void Visit(LambdaExpressionNode node) { }
    public virtual void Visit(LeftShiftAssignmentOperatorNode node) { }
    public virtual void Visit(LeftShiftOperatorNode node) { }
    public virtual void Visit(LessThanOperatorNode node) { }
    public virtual void Visit(LessThanOrEqualOperatorNode node) { }
    public virtual void Visit(LetClauseNode node) { }
    public virtual void Visit(LiteralNode node) { }
    public virtual void Visit(LocalVariableNode node) { }
    public virtual void Visit(LocalVariableTagNode node) { }
    public virtual void Visit(LockStatementNode node) { }
    public virtual void Visit(LogicalAndOperatorNode node) { }
    public virtual void Visit(LogicalOrOperatorNode node) { }
    public virtual void Visit(MemberAccessOperatorNode node) { }
    public virtual void Visit(MemberDeclarationNode node) { }
    public virtual void Visit(MemberDeclaratorNode node) { }
    public virtual void Visit(MemberInitializerNode node) { }
    public virtual void Visit(MemberWithBodyDeclarationNode node) { }
    public virtual void Visit(MethodDeclarationNode node) { }
    public virtual void Visit(MethodInvocationOperatorNode node) { }
    public virtual void Visit(MinusAssignmentOperatorNode node) { }
    public virtual void Visit(ModifierNode node) { }
    public virtual void Visit(ModuloAssignmentOperatorNode node) { }
    public virtual void Visit(ModuloOperatorNode node) { }
    public virtual void Visit(MultiplyAssignmentOperatorNode node) { }
    public virtual void Visit(MultiplyOperatorNode node) { }
    public virtual void Visit(NamespaceDeclarationNode node) { }
    public virtual void Visit(NamespaceScopeNode node) { }
    public virtual void Visit(NameTagNode node) { }
    public virtual void Visit(NewOperatorNode node) { }
    public virtual void Visit(NewOperatorWithAnonymousTypeNode node) { }
    public virtual void Visit(NewOperatorWithArrayNode node) { }
    public virtual void Visit(NewOperatorWithConstructorNode node) { }
    public virtual void Visit(NotEqualOperatorNode node) { }
    public virtual void Visit(NullCoalescingOperatorNode node) { }
    public virtual void Visit(NullNode node) { }
    public virtual void Visit(ObjectOrCollectionInitializerNode node) { }
    public virtual void Visit(OperatorNode node) { }
    public virtual void Visit(OrAssignmentOperatorNode node) { }
    public virtual void Visit(OrderByClauseNode node) { }
    public virtual void Visit(OrderingClauseNode node) { }
    public virtual void Visit(ParenthesisExpressionNode node) { }
    public virtual void Visit(PlusAssignmentOperatorNode node) { }
    public virtual void Visit(PointerModifierNode node) { }
    public virtual void Visit(PointerOperatorNode node) { }
    public virtual void Visit(PostDecrementOperatorNode node) { }
    public virtual void Visit(PostIncrementOperatorNode node) { }
    public virtual void Visit(PreDecrementOperatorNode node) { }
    public virtual void Visit(PreIncrementOperatorNode node) { }
    public virtual void Visit(PrimaryOperatorNode node) { }
    public virtual void Visit(PrimitiveNamedNode node) { }
    public virtual void Visit(PropertyDeclarationNode node) { }
    public virtual void Visit(QueryBodyClauseNode node) { }
    public virtual void Visit(QueryBodyNode node) { }
    public virtual void Visit(QueryExpressionNode node) { }
    public virtual void Visit(RealConstantNode node) { }
    public virtual void Visit(ReferenceOperatorNode node) { }
    public virtual void Visit(ReturnStatementNode node) { }
    public virtual void Visit(RightShiftAssignmentOperatorNode node) { }
    public virtual void Visit(RightShiftOperatorNode node) { }
    public virtual void Visit(ScopedNameNode node) { }
    public virtual void Visit(SelectClauseNode node) { }
    public virtual void Visit(SimpleNameNode node) { }
    public virtual void Visit(SingleNode node) { }
    public virtual void Visit(SizedArrayDimensionNode node) { }
    public virtual void Visit(SizeofOperatorNode node) { }
    public virtual void Visit(SourceFileNode node) { }
    public virtual void Visit(StackAllocInitializerNode node) { }
    public virtual void Visit(StatementNode node) { }
    public virtual void Visit(StringNode node) { }
    public virtual void Visit(StructDeclarationNode node) { }
    public virtual void Visit(SubtractOperatorNode node) { }
    public virtual void Visit(SwitchLabelNode node) { }
    public virtual void Visit(SwitchSectionNode node) { }
    public virtual void Visit(SwitchStatementNode node) { }
    public virtual void Visit(ThisNode node) { }
    public virtual void Visit(ThrowStatementNode node) { }
    public virtual void Visit(TrueNode node) { }
    public virtual void Visit(TryStatementNode node) { }
    public virtual void Visit(TypecastOperatorNode node) { }
    public virtual void Visit(TypeDeclarationNode node) { }
    public virtual void Visit(TypeModifierNode node) { }
    public virtual void Visit(TypeofOperatorNode node) { }
    public virtual void Visit(TypeOperatorNode node) { }
    public virtual void Visit(TypeOrMemberDeclarationNode node) { }
    public virtual void Visit(TypeOrNamespaceNode node) { }
    public virtual void Visit(TypeParameterConstraintNode node) { }
    public virtual void Visit(TypeParameterConstraintTagNode node) { }
    public virtual void Visit(TypeParameterListNode node) { }
    public virtual void Visit(TypeParameterNode node) { }
    public virtual void Visit(TypeTagNode node) { }
    public virtual void Visit(TypeWithBodyDeclarationNode node) { }
    public virtual void Visit(TypeWithMembersDeclarationNode node) { }
    public virtual void Visit(UInt32Node node) { }
    public virtual void Visit(UInt64Node node) { }
    public virtual void Visit(UnaryMinusOperatorNode node) { }
    public virtual void Visit(UnaryNotOperatorNode node) { }
    public virtual void Visit(UnaryOperatorNode node) { }
    public virtual void Visit(UnaryPlusOperatorNode node) { }
    public virtual void Visit(UncheckedOperatorNode node) { }
    public virtual void Visit(UncheckedStatementNode node) { }
    public virtual void Visit(UnsafeStatementNode node) { }
    public virtual void Visit(UsingAliasNode node) { }
    public virtual void Visit(UsingNamespaceNode node) { }
    public virtual void Visit(UsingStatementNode node) { }
    public virtual void Visit(VariableDeclarationStatementNode node) { }
    public virtual void Visit(VariableInitializerNode node) { }
    public virtual void Visit(WhereClauseNode node) { }
    public virtual void Visit(WhileStatementNode node) { }
    public virtual void Visit(XorAssignmentOperatorNode node) { }
    public virtual void Visit(YieldBreakStatementNode node) { }
    public virtual void Visit(YieldReturnStatementNode node) { }
  }
}
#pragma warning restore 1591