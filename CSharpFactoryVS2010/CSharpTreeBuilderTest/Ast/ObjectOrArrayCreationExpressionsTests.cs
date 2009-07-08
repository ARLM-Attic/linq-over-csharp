using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilderTest
{
  // ================================================================================================
  /// <summary>
  /// Tests the parsing of object or array creation expressions
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class ObjectOrArrayCreationExpressionTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expressions:
    /// <code>
    ///     var a1 = new ObjectCreationExpression();
    ///     var a2 = new ObjectCreationExpression(1, 2);
    ///     var a3 = new ObjectCreationExpression() { A = 3 };
    ///     var a4 = new ObjectCreationExpression(4, 5) { A = 6 };
    ///     var a5 = new ObjectCreationExpression { A = 7, B = 8 };
    /// </code>
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ObjectCreationExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ObjectOrArrayCreationExpressions\ObjectCreationExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var init1 = ((VariableDeclarationStatementNode)method.Body.Statements[0]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp1 = init1.Expression as ObjectCreationExpressionNode;
      exp1.TypeName.TypeTags[0].Identifier.ShouldEqual("ObjectCreationExpression");
      exp1.HasConstructorArguments.ShouldBeFalse();
      exp1.HasObjectInitializer.ShouldBeFalse();

      var init2 = ((VariableDeclarationStatementNode)method.Body.Statements[1]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp2 = init2.Expression as ObjectCreationExpressionNode;
      exp2.TypeName.TypeTags[0].Identifier.ShouldEqual("ObjectCreationExpression");
      exp2.HasConstructorArguments.ShouldBeTrue();
      ((Int32LiteralNode) exp2.Arguments[0].Expression).Value.ShouldEqual(1);
      ((Int32LiteralNode) exp2.Arguments[1].Expression).Value.ShouldEqual(2);
      exp2.HasObjectInitializer.ShouldBeFalse();

      var init3 = ((VariableDeclarationStatementNode)method.Body.Statements[2]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp3 = init3.Expression as ObjectCreationExpressionNode;
      exp3.TypeName.TypeTags[0].Identifier.ShouldEqual("ObjectCreationExpression");
      exp3.HasConstructorArguments.ShouldBeFalse();
      exp3.HasObjectInitializer.ShouldBeTrue();
      var memberInit3 = exp3.ObjectOrCollectionInitializer.MemberInitializers[0];
      memberInit3.Identifier.ShouldEqual("A");
      ((Int32LiteralNode) memberInit3.Expression).Value.ShouldEqual(3);

      var init4 = ((VariableDeclarationStatementNode)method.Body.Statements[3]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp4 = init4.Expression as ObjectCreationExpressionNode;
      exp4.TypeName.TypeTags[0].Identifier.ShouldEqual("ObjectCreationExpression");
      exp4.HasConstructorArguments.ShouldBeTrue();
      ((Int32LiteralNode)exp4.Arguments[0].Expression).Value.ShouldEqual(4);
      ((Int32LiteralNode)exp4.Arguments[1].Expression).Value.ShouldEqual(5);
      exp4.HasObjectInitializer.ShouldBeTrue();
      var memberInit4 = exp4.ObjectOrCollectionInitializer.MemberInitializers[0];
      memberInit4.Identifier.ShouldEqual("A");
      ((Int32LiteralNode)memberInit4.Expression).Value.ShouldEqual(6);

      var init5 = ((VariableDeclarationStatementNode)method.Body.Statements[4]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp5 = init5.Expression as ObjectCreationExpressionNode;
      exp5.TypeName.TypeTags[0].Identifier.ShouldEqual("ObjectCreationExpression");
      exp5.HasConstructorArguments.ShouldBeFalse();
      exp5.HasObjectInitializer.ShouldBeTrue();
      var memberInit5 = exp5.ObjectOrCollectionInitializer.MemberInitializers[0];
      memberInit5.Identifier.ShouldEqual("A");
      ((Int32LiteralNode)memberInit5.Expression).Value.ShouldEqual(7);
      var memberInit6 = exp5.ObjectOrCollectionInitializer.MemberInitializers[1];
      memberInit6.Identifier.ShouldEqual("B");
      ((Int32LiteralNode)memberInit6.Expression).Value.ShouldEqual(8);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expressions:
    /// <code>
    ///    var a1 = new List<int>() {1, 2*3};
    ///    var a2 = new Dictionary<int, string> { { 4, "a" }, { 5, "b" } };
    /// </code>
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CollectionInitializer()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ObjectOrArrayCreationExpressions\CollectionInitializer.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var init1 = ((VariableDeclarationStatementNode) method.Body.Statements[0]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp1 = init1.Expression as ObjectCreationExpressionNode;
      exp1.TypeName.TypeTags[0].Identifier.ShouldEqual("List");
      exp1.TypeName.TypeTags[0].GenericDimensions.ShouldEqual(1);
      exp1.TypeName.TypeTags[0].Arguments[0].TypeTags[0].Identifier.ShouldEqual("int");
      exp1.HasConstructorArguments.ShouldBeFalse();
      exp1.HasObjectInitializer.ShouldBeTrue();
      var elementInit1 = exp1.ObjectOrCollectionInitializer.ElementInitializers[0].NonAssignmentExpression as Int32LiteralNode;
      elementInit1.Value.ShouldEqual(1);
      var elementInit2 = exp1.ObjectOrCollectionInitializer.ElementInitializers[1].NonAssignmentExpression as BinaryExpressionNode;
      ((Int32LiteralNode) elementInit2.LeftOperand).Value.ShouldEqual(2);
      elementInit2.Operator.ShouldEqual(BinaryOperator.Multiplication);
      ((Int32LiteralNode) elementInit2.RightOperand).Value.ShouldEqual(3);

      var init2 = ((VariableDeclarationStatementNode)method.Body.Statements[1]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp2 = init2.Expression as ObjectCreationExpressionNode;
      exp2.TypeName.TypeTags[0].Identifier.ShouldEqual("Dictionary");
      exp2.TypeName.TypeTags[0].GenericDimensions.ShouldEqual(2);
      exp2.TypeName.TypeTags[0].Arguments[0].TypeTags[0].Identifier.ShouldEqual("int");
      exp2.TypeName.TypeTags[0].Arguments[1].TypeTags[0].Identifier.ShouldEqual("string");
      exp2.HasConstructorArguments.ShouldBeFalse();
      exp2.HasObjectInitializer.ShouldBeTrue();
      ((Int32LiteralNode)exp2.ObjectOrCollectionInitializer.ElementInitializers[0].ExpressionList[0]).Value.ShouldEqual(4);
      ((StringLiteralNode)exp2.ObjectOrCollectionInitializer.ElementInitializers[0].ExpressionList[1]).Value.ShouldEqual("a");
      ((Int32LiteralNode)exp2.ObjectOrCollectionInitializer.ElementInitializers[1].ExpressionList[0]).Value.ShouldEqual(5);
      ((StringLiteralNode)exp2.ObjectOrCollectionInitializer.ElementInitializers[1].ExpressionList[1]).Value.ShouldEqual("b");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the erroneous expression:
    /// <code>
    ///    var a1 = new List<int>() {1, Capacity = 2};
    /// </code>
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CollectionInitializer_Error()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ObjectOrArrayCreationExpressions\CollectionInitializer_Error.cs");
      InvokeParser(project).ShouldBeFalse();
      project.Errors[0].Code.ShouldEqual("CS0747");
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expressions:
    /// <code>
    ///     var myDelegate = new MyDelegate(MyTarget);
    /// </code>
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void DelegateCreationExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ObjectOrArrayCreationExpressions\DelegateCreationExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var init1 = ((VariableDeclarationStatementNode) method.Body.Statements[0]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp1 = init1.Expression as ObjectCreationExpressionNode;
      exp1.TypeName.TypeTags[0].Identifier.ShouldEqual("MyDelegate");
      exp1.HasConstructorArguments.ShouldBeTrue();
      ((SimpleNameNode)exp1.Arguments[0].Expression).Identifier.ShouldEqual("MyTarget");
      exp1.HasObjectInitializer.ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expressions:
    /// <code>
    ///    var a1 = new { x, AnonymousObjectCreationExpression.xs1, global::AnonymousObjectCreationExpression.xs2, A = 1 };
    ///    var a2 = new { };
    /// </code>
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void AnonymousObjectCreationExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ObjectOrArrayCreationExpressions\AnonymousObjectCreationExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var init1 = ((VariableDeclarationStatementNode)method.Body.Statements[0]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp1 = init1.Expression as AnonymousObjectCreationExpressionNode;
      exp1.Declarators.Count.ShouldEqual(4);

      var memberDeclarator1 = exp1.Declarators[0] as SimpleNameMemberDeclaratorNode;
      memberDeclarator1.SimpleName.Identifier.ShouldEqual("x");

      var memberDeclarator2 = exp1.Declarators[1] as MemberAccessMemberDeclaratorNode;
      var memberAccess2 = memberDeclarator2.MemberAccess as PrimaryExpressionMemberAccessNode;
      ((SimpleNameNode) memberAccess2.PrimaryExpression).Identifier.ShouldEqual("AnonymousObjectCreationExpression");
      memberAccess2.MemberName.Identifier.ShouldEqual("xs1");

      var memberDeclarator3 = exp1.Declarators[2] as MemberAccessMemberDeclaratorNode;
      var memberAccess3 = memberDeclarator3.MemberAccess as QualifiedAliasMemberAccessNode;
      memberAccess3.QualifiedAliasMember.Qualifier.ShouldEqual("global");
      memberAccess3.QualifiedAliasMember.Identifier.ShouldEqual("AnonymousObjectCreationExpression");
      memberAccess3.MemberName.Identifier.ShouldEqual("xs2");

      var memberDeclarator4 = exp1.Declarators[3] as IdentifierMemberDeclaratorNode;
      memberDeclarator4.Identifier.ShouldEqual("A");
      ((Int32LiteralNode)memberDeclarator4.Expression).Value.ShouldEqual(1);

      var init2 = ((VariableDeclarationStatementNode)method.Body.Statements[1]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp2 = init2.Expression as AnonymousObjectCreationExpressionNode;
      exp2.Declarators.Count.ShouldEqual(0);

    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the erroneous expressions:
    /// <code>
    ///   var a = new { 1 };
    /// </code>
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void AnonymousObjectCreationExpression_Error()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ObjectOrArrayCreationExpressions\AnonymousObjectCreationExpression_Error.cs");
      InvokeParser(project).ShouldBeFalse();
      project.Errors[0].Code.ShouldEqual("CS0746");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expressions:
    /// <code>
    ///   var a1 = new int[1, 2][][,] { { null, null } };
    ///   var a2 = new int**[5, 6];
    ///   var a3 = new[,] { { a = 7, 8 }, { 9, 10 } };
    ///   var a4 = new int[][,] { null, null };
    /// </code>
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ArrayCreationExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ObjectOrArrayCreationExpressions\ArrayCreationExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var init1 = ((VariableDeclarationStatementNode) method.Body.Statements[0]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp1 = init1.Expression as ArrayCreationExpressionNode;
      exp1.TypeName.TypeTags[0].Identifier.ShouldEqual("int");
      exp1.TypeName.IsArray.ShouldBeFalse(); // note that the type is NOT an array type!
      exp1.TypeName.IsPointer.ShouldBeFalse();
      exp1.HasSpecifiedArraySizes.ShouldBeTrue();
      exp1.ArraySizeSpecifier.Rank.ShouldEqual(2);
      ((Int32LiteralNode)exp1.ArraySizeSpecifier.Expressions[0]).Value.ShouldEqual(1);
      ((Int32LiteralNode) exp1.ArraySizeSpecifier.Expressions[1]).Value.ShouldEqual(2);
      exp1.HasRankSpecifiers.ShouldBeTrue();
      exp1.RankSpecifiers[0].Rank.ShouldEqual(1);
      exp1.RankSpecifiers[1].Rank.ShouldEqual(2);
      exp1.HasInitializer.ShouldBeTrue();
      var init1_1 = exp1.Initializer.Items[0].Initializer as ArrayInitializerNode;
      (((ExpressionInitializerNode)init1_1.Items[0].Initializer).Expression as NullLiteralNode).ShouldNotBeNull();
      (((ExpressionInitializerNode)init1_1.Items[1].Initializer).Expression as NullLiteralNode).ShouldNotBeNull();

      var init2 = ((VariableDeclarationStatementNode)method.Body.Statements[1]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp2 = init2.Expression as ArrayCreationExpressionNode;
      exp2.TypeName.TypeTags[0].Identifier.ShouldEqual("int");
      exp2.TypeName.IsPointer.ShouldBeTrue();
      exp2.TypeName.PointerSpecifierCount.ShouldEqual(2);
      exp2.HasSpecifiedArraySizes.ShouldBeTrue();
      exp2.ArraySizeSpecifier.Rank.ShouldEqual(2);
      ((Int32LiteralNode)exp2.ArraySizeSpecifier.Expressions[0]).Value.ShouldEqual(5);
      ((Int32LiteralNode)exp2.ArraySizeSpecifier.Expressions[1]).Value.ShouldEqual(6);
      exp2.HasInitializer.ShouldBeFalse();

      var init3 = ((VariableDeclarationStatementNode)method.Body.Statements[2]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp3 = init3.Expression as ArrayCreationExpressionNode;
      exp3.TypeName.IsEmpty.ShouldBeTrue();
      exp3.HasSpecifiedArraySizes.ShouldBeFalse();
      exp3.HasRankSpecifiers.ShouldBeTrue();
      exp3.RankSpecifiers.Count.ShouldEqual(1);
      exp3.RankSpecifiers[0].Rank.ShouldEqual(2);
      exp3.HasInitializer.ShouldBeTrue();
      exp3.Initializer.Items.Count.ShouldEqual(2);
      ((ArrayInitializerNode) exp3.Initializer.Items[0].Initializer).Items.Count.ShouldEqual(2);
      var init3_1 = ((ArrayInitializerNode) exp3.Initializer.Items[0].Initializer).Items[0].Initializer as ExpressionInitializerNode;
      var assignment = init3_1.Expression as AssignmentExpressionNode;
      ((SimpleNameNode) assignment.LeftOperand).Identifier.ShouldEqual("a");
      assignment.Operator.ShouldEqual(AssignmentOperator.SimpleAssignment);
      ((Int32LiteralNode)assignment.RightOperand).Value.ShouldEqual(7);
      var init3_2 = ((ArrayInitializerNode)exp3.Initializer.Items[0].Initializer).Items[1].Initializer as ExpressionInitializerNode;
      ((Int32LiteralNode)init3_2.Expression).Value.ShouldEqual(8);
      var init3_3 = ((ArrayInitializerNode)exp3.Initializer.Items[1].Initializer).Items[0].Initializer as ExpressionInitializerNode;
      ((Int32LiteralNode)init3_3.Expression).Value.ShouldEqual(9);
      var init3_4 = ((ArrayInitializerNode)exp3.Initializer.Items[1].Initializer).Items[1].Initializer as ExpressionInitializerNode;
      ((Int32LiteralNode)init3_4.Expression).Value.ShouldEqual(10);

      var init4 = ((VariableDeclarationStatementNode)method.Body.Statements[3]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var exp4 = init4.Expression as ArrayCreationExpressionNode;
      exp4.TypeName.TypeTags[0].Identifier.ShouldEqual("int");
      exp4.HasSpecifiedArraySizes.ShouldBeFalse();
      exp4.HasRankSpecifiers.ShouldBeTrue();
      exp4.RankSpecifiers[0].Rank.ShouldEqual(1);
      exp4.RankSpecifiers[1].Rank.ShouldEqual(2);
      exp4.HasInitializer.ShouldBeTrue();
      var init4_1 = exp1.Initializer.Items[0].Initializer as ArrayInitializerNode;
      (((ExpressionInitializerNode)init4_1.Items[0].Initializer).Expression as NullLiteralNode).ShouldNotBeNull();
      (((ExpressionInitializerNode)init4_1.Items[1].Initializer).Expression as NullLiteralNode).ShouldNotBeNull();

    }
  }
}
