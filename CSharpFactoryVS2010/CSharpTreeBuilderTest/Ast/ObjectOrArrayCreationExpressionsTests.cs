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
      var elementInit1 = exp1.ObjectOrCollectionInitializer.ElementInitializers[0].Expression as Int32LiteralNode;
      elementInit1.Value.ShouldEqual(1);
      var elementInit2 = exp1.ObjectOrCollectionInitializer.ElementInitializers[1].Expression as BinaryOperatorNode;
      ((Int32LiteralNode) elementInit2.LeftOperand).Value.ShouldEqual(2);
      elementInit2.Operator.ShouldEqual(BinaryOperatorType.Multiplication);
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
  }
}
