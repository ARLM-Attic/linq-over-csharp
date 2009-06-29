using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilderTest
{
  // ================================================================================================
  /// <summary>
  /// Tests the parsing of simple expressions
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class SimpleExpressionTests : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// int x = 6;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void IntegerLiteral()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\IntegerLiteral.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var literalNode = initializer.Expression as Int32LiteralNode;
      literalNode.Value.ShouldEqual(6);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// int y = x = 8;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void SimpleAssignment()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\SimpleAssignment.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var assignment = initializer.Expression as AssignmentOperatorNode;
      assignment.Operator.ShouldEqual(AssignmentOperatorType.SimpleAssignment);
      ((SimpleNameNode) assignment.LeftOperand).Identifier.ShouldEqual("x");
      ((Int32LiteralNode) assignment.RightOperand).Value.ShouldEqual(8);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// int a1 = 2 * 3 * 4;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Multiplications()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\Multiplications.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var binary1 = initializer.Expression as BinaryOperatorNode;
      binary1.Operator.ShouldEqual(BinaryOperatorType.Multiplication);

      var binary1RightOperand = binary1.RightOperand as Int32LiteralNode;
      binary1RightOperand.Value.ShouldEqual(4);

      var binary2 = binary1.LeftOperand as BinaryOperatorNode;
      binary2.Operator.ShouldEqual(BinaryOperatorType.Multiplication);

      var binary2RightOperand = binary2.RightOperand as Int32LiteralNode;
      binary2RightOperand.Value.ShouldEqual(3);

      var binary2LeftOperand = binary2.LeftOperand as Int32LiteralNode;
      binary2LeftOperand.Value.ShouldEqual(2);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// int a2 = 2 + 3 + 4;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Additions()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\Additions.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var binary1 = initializer.Expression as BinaryOperatorNode;
      binary1.Operator.ShouldEqual(BinaryOperatorType.Addition);

      var binary1RightOperand = binary1.RightOperand as Int32LiteralNode;
      binary1RightOperand.Value.ShouldEqual(4);

      var binary2 = binary1.LeftOperand as BinaryOperatorNode;
      binary2.Operator.ShouldEqual(BinaryOperatorType.Addition);

      var binary2RightOperand = binary2.RightOperand as Int32LiteralNode;
      binary2RightOperand.Value.ShouldEqual(3);

      var binary2LeftOperand = binary2.LeftOperand as Int32LiteralNode;
      binary2LeftOperand.Value.ShouldEqual(2);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// int a3 = 2 + 3 * 4 + 5;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void AddAndMultiplyPrecedence()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\AddAndMultiplyPrecedence.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var binary1 = initializer.Expression as BinaryOperatorNode;
      binary1.Operator.ShouldEqual(BinaryOperatorType.Addition);

      var binary1RightOperand = binary1.RightOperand as Int32LiteralNode;
      binary1RightOperand.Value.ShouldEqual(5);

      var binary2 = binary1.LeftOperand as BinaryOperatorNode;
      binary2.Operator.ShouldEqual(BinaryOperatorType.Addition);

      var binary2LeftOperand = binary2.LeftOperand as Int32LiteralNode;
      binary2LeftOperand.Value.ShouldEqual(2);

      var binary3 = binary2.RightOperand as BinaryOperatorNode;
      binary3.Operator.ShouldEqual(BinaryOperatorType.Multiplication);

      var binary3LeftOperand = binary3.LeftOperand as Int32LiteralNode;
      binary3LeftOperand.Value.ShouldEqual(3);

      var binary3RightOperand = binary3.RightOperand as Int32LiteralNode;
      binary3RightOperand.Value.ShouldEqual(4);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// int a = x += x -= x *= x /= 1;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void AssignmentExpressions()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\AssignmentExpressions.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      // Assignment is right-associative: (x += (x -= (x *= (x /= 1))))
      // The topmost expression is (x += assignment-expression)
      var op1 = initializer.Expression as AssignmentOperatorNode;
      op1.Operator.ShouldEqual(AssignmentOperatorType.AdditionAssignment);
      ((SimpleNameNode) (op1.LeftOperand)).Identifier.ShouldEqual("x");

      // The next expression is (x -= assignment-expression)
      var op2 = op1.RightOperand as AssignmentOperatorNode;
      op2.Operator.ShouldEqual(AssignmentOperatorType.SubtractionAssignment);
      ((SimpleNameNode)(op2.LeftOperand)).Identifier.ShouldEqual("x");

      // The next expression is (x *= assignment-expression)
      var op3 = op2.RightOperand as AssignmentOperatorNode;
      op3.Operator.ShouldEqual(AssignmentOperatorType.MultiplicationAssignment);
      ((SimpleNameNode)(op3.LeftOperand)).Identifier.ShouldEqual("x");

      // The next expression is (x /= x)
      var op4 = op3.RightOperand as AssignmentOperatorNode;
      op4.Operator.ShouldEqual(AssignmentOperatorType.DivisionAssignment);
      ((SimpleNameNode)(op4.LeftOperand)).Identifier.ShouldEqual("x");
      ((Int32LiteralNode)(op4.RightOperand)).Value.ShouldEqual(1);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// string s1 = "a" ?? "b";
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NullCoalescingExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\NullCoalescingExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var binary1 = initializer.Expression as BinaryOperatorNode;
      binary1.Operator.ShouldEqual(BinaryOperatorType.NullCoalescing);
      ((StringLiteralNode) (binary1.LeftOperand)).Value.ShouldEqual("a");
      ((StringLiteralNode) (binary1.RightOperand)).Value.ShouldEqual("b");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// bool t = true;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void TrueLiteral()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\TrueLiteral.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var trueLiteral = ((TrueLiteralNode)(initializer.Expression));
      trueLiteral.Value.ShouldEqual(true);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// bool f = false;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void FalseLiteral()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\FalseLiteral.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      
      var falseLiteral = ((FalseLiteralNode)(initializer.Expression));
      falseLiteral.Value.ShouldEqual(false);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// int i1 = "a".Length;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccessExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\PrimaryMemberAccessExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as PrimaryMemberAccessOperatorNode;
      expr.MemberName.Identifier.ShouldEqual("Length");
      ((StringLiteralNode) expr.PrimaryExpression).Value.ShouldEqual("a");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// string s2 = bool.FalseString;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PredefinedTypeMemberAccessExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\PredefinedTypeMemberAccessExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as PredefinedTypeMemberAccessNode;
      expr.MemberName.Identifier.ShouldEqual("FalseString");
      expr.TypeName.TypeTags[0].Identifier.ShouldEqual("bool");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// string s3 = myAlias::Boolean.FalseString;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void QualifiedAliasMemberAccessExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\QualifiedAliasMemberAccessExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as QualifiedAliasMemberAccessNode;
      expr.MemberName.Identifier.ShouldEqual("FalseString");
      expr.QualifiedAliasMember.Qualifier.ShouldEqual("myAlias");
      expr.QualifiedAliasMember.Identifier.ShouldEqual("Boolean");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// int i2 = myAlias::Boolean.FalseString.Length;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void MultipleMemberAccessExpressions()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\MultipleMemberAccessExpressions.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as PrimaryMemberAccessOperatorNode;
      expr.MemberName.Identifier.ShouldEqual("Length");

      var embeddedExpr = expr.PrimaryExpression as QualifiedAliasMemberAccessNode;
      embeddedExpr.MemberName.Identifier.ShouldEqual("FalseString");
      embeddedExpr.QualifiedAliasMember.Qualifier.ShouldEqual("myAlias");
      embeddedExpr.QualifiedAliasMember.Identifier.ShouldEqual("Boolean");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// p->x = 1; (inside a fixed statement)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PointerMemberAccessExpressions()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\PointerMemberAccessExpressions.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var fixedStatementNode = method.Body.Statements[0] as FixedStatementNode;
      var bodyBlock = fixedStatementNode.Statement as BlockStatementNode;
      var exprStatement = bodyBlock.Statements[0] as ExpressionStatementNode;
      var expression = exprStatement.Expression as AssignmentOperatorNode;
      var pointerExpr = expression.LeftOperand as PointerMemberAccessOperatorNode;
      ((SimpleNameNode)pointerExpr.PrimaryExpression).Identifier.ShouldEqual("p");
      pointerExpr.MemberName.Identifier.ShouldEqual("x");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// var e1 = this;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ThisAccessExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\ThisAccessExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var thisNode = initializer.Expression as ThisAccessNode;
      thisNode.ShouldNotBeNull();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// int i3 = base.Field;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BaseMemberAccessExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\BaseMemberAccessExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as BaseMemberAccessNode;
      expr.MemberName.Identifier.ShouldEqual("Field");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// int i4 = base[1];
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BaseElementAccessExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\BaseElementAccessExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as BaseElementAccessNode;
      ((Int32LiteralNode) expr.Expressions[0]).Value.ShouldEqual(1);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// DummyMethod();
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void InvocationExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\InvocationExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var invocation = ((ExpressionStatementNode) method.Body.Statements[0]).Expression as InvocationOperatorNode;
      ((SimpleNameNode) invocation.PrimaryExpression).Identifier.ShouldEqual("DummyMethod");
      invocation.Arguments.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// Equals(null); 
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void InvocationExpressionWithArgument()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\InvocationExpressionWithArgument.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var invocation = ((ExpressionStatementNode)method.Body.Statements[0]).Expression as InvocationOperatorNode;
      ((SimpleNameNode)invocation.PrimaryExpression).Identifier.ShouldEqual("Equals");
      var nullLiteral = invocation.Arguments[0].Expression as NullLiteralNode;
      nullLiteral.ShouldNotBeNull();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// char c1 = "a"[0]; 
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ElementAccessExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\ElementAccessExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as ElementAccessNode;
      ((StringLiteralNode)expr.PrimaryExpression).Value.ShouldEqual("a");
      ((Int32LiteralNode)expr.Expressions[0]).Value.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// bool b1 = true is bool;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void IsExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\IsExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as TypeTestingOperatorNode;
      expr.Operator.ShouldEqual(TypeTestingOperatorType.Is);
      ((TrueLiteralNode) expr.LeftOperand).Value.ShouldEqual(true);
      expr.RightOperand.TypeTags[0].Identifier.ShouldEqual("bool");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// bool b2 = b1 && x + y is bool;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void EmbeddedIsExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\EmbeddedIsExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr1 = initializer.Expression as BinaryOperatorNode;
      expr1.Operator.ShouldEqual(BinaryOperatorType.ConditionalAnd);
      ((TrueLiteralNode) expr1.LeftOperand).Value.ShouldEqual(true);

      var expr2 = expr1.RightOperand as TypeTestingOperatorNode;
      expr2.Operator.ShouldEqual(TypeTestingOperatorType.Is);
      expr2.RightOperand.TypeTags[0].Identifier.ShouldEqual("bool");

      var expr3 = expr2.LeftOperand as BinaryOperatorNode;
      expr3.Operator.ShouldEqual(BinaryOperatorType.Addition);
      ((Int32LiteralNode)expr3.LeftOperand).Value.ShouldEqual(1);
      ((Int32LiteralNode)expr3.RightOperand).Value.ShouldEqual(2);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// string s4 = s1 as string;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void AsExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\AsExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.SourceFileNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as TypeTestingOperatorNode;
      expr.Operator.ShouldEqual(TypeTestingOperatorType.As);
      ((StringLiteralNode)expr.LeftOperand).Value.ShouldEqual("a");
      expr.RightOperand.TypeTags[0].Identifier.ShouldEqual("string");
    }
  }
}
