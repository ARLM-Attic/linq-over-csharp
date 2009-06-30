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
    /// Tests the parsing of the expressions:
    ///     int i1 = 1;
    ///     long i2 = 2L;
    ///     uint i3 = 3U;
    ///     ulong i4 = 4UL;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void IntegerLiteral()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\IntegerLiteral.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var init1 = ((VariableDeclarationStatementNode)method.Body.Statements[0]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      ((Int32LiteralNode)init1.Expression).Value.ShouldEqual(1);

      var init2 = ((VariableDeclarationStatementNode)method.Body.Statements[1]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      ((Int64LiteralNode)init2.Expression).Value.ShouldEqual(2);

      var init3 = ((VariableDeclarationStatementNode)method.Body.Statements[2]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      ((UInt32LiteralNode)init3.Expression).Value.ShouldEqual<uint>(3);

      var init4 = ((VariableDeclarationStatementNode)method.Body.Statements[3]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      ((UInt64LiteralNode)init4.Expression).Value.ShouldEqual<ulong>(4);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expressions:
    ///     float f1 = 1F;
    ///     double f2 = 2D;
    ///     decimal f3 = 3M;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void RealLiteral()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\RealLiteral.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var init1 = ((VariableDeclarationStatementNode)method.Body.Statements[0]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      ((SingleLiteralNode)init1.Expression).Value.ShouldEqual(1);

      var init2 = ((VariableDeclarationStatementNode)method.Body.Statements[1]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      ((DoubleLiteralNode)init2.Expression).Value.ShouldEqual(2);

      var init3 = ((VariableDeclarationStatementNode)method.Body.Statements[2]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      ((DecimalLiteralNode)init3.Expression).Value.ShouldEqual(3);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expressions:
    ///     char c = 'a';
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CharLiteral()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\CharLiteral.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var init1 = ((VariableDeclarationStatementNode)method.Body.Statements[0]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      ((CharLiteralNode)init1.Expression).Value.ShouldEqual('a');
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expressions:
    ///     string s = "a";
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void StringLiteral()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\StringLiteral.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var init1 = ((VariableDeclarationStatementNode)method.Body.Statements[0]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      ((StringLiteralNode)init1.Expression).Value.ShouldEqual("a");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expressions:
    ///     int i = (1);
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ParenthesizedExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\ParenthesizedExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var parenthesizedExpressionNode = initializer.Expression as ParenthesizedExpressionNode;
      ((Int32LiteralNode)parenthesizedExpressionNode.Expression).Value.ShouldEqual(1);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expressions:
    ///         ++i;
    ///         --i;
    ///         i++;
    ///         i--;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrePostIncDecrementExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\PrePostIncDecrementExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var op1 = ((ExpressionStatementNode)method.Body.Statements[0]).Expression as PreIncrementOperatorNode;
      op1.ShouldNotBeNull();

      var op2 = ((ExpressionStatementNode)method.Body.Statements[1]).Expression as PreDecrementOperatorNode;
      op2.ShouldNotBeNull();

      var op3 = ((ExpressionStatementNode)method.Body.Statements[2]).Expression as PostIncrementOperatorNode;
      op3.ShouldNotBeNull();

      var op4 = ((ExpressionStatementNode)method.Body.Statements[3]).Expression as PostDecrementOperatorNode;
      op4.ShouldNotBeNull();
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
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

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as TypeTestingOperatorNode;
      expr.Operator.ShouldEqual(TypeTestingOperatorType.As);
      ((StringLiteralNode)expr.LeftOperand).Value.ShouldEqual("a");
      expr.RightOperand.TypeTags[0].Identifier.ShouldEqual("string");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    ///          Type t1 = typeof(int);
    ///          Type t2 = typeof(Generic<,>);
    ///          Type t3 = typeof(Generic<int, int>);
    ///          Type t4 = typeof(myAlias::IList<>);
    ///          Type t5 = typeof(Generic<,>.EmbeddedGeneric<>);
    ///          Type t6 = typeof(void);
    /// </summary>
    /// <remarks>
    /// Also tests unbound type names, because they are valid in typeof expressions only.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void TypeofExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\TypeofExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var init1 = ((VariableDeclarationStatementNode)method.Body.Statements[0]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var typeName1 = ((TypeofOperatorNode) init1.Expression).TypeName;
      typeName1.TypeTags[0].Identifier.ShouldEqual("int");
      typeName1.TypeTags[0].IsUnbound.ShouldBeFalse();
      typeName1.TypeTags[0].GenericDimensions.ShouldEqual(0);

      var init2 = ((VariableDeclarationStatementNode)method.Body.Statements[1]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var typeName2 = ((TypeofOperatorNode) init2.Expression).TypeName;
      typeName2.TypeTags[0].Identifier.ShouldEqual("Generic");
      typeName2.TypeTags[0].IsUnbound.ShouldBeTrue();
      typeName2.TypeTags[0].GenericDimensions.ShouldEqual(2);

      var init3 = ((VariableDeclarationStatementNode)method.Body.Statements[2]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var typeName3 = ((TypeofOperatorNode)init3.Expression).TypeName;
      typeName3.TypeTags[0].Identifier.ShouldEqual("Generic");
      typeName3.TypeTags[0].IsUnbound.ShouldBeFalse();
      typeName3.TypeTags[0].GenericDimensions.ShouldEqual(2);
      typeName3.TypeTags[0].Arguments[0].TypeTags[0].Identifier.ShouldEqual("int");
      typeName3.TypeTags[0].Arguments[1].TypeTags[0].Identifier.ShouldEqual("int");

      var init4 = ((VariableDeclarationStatementNode)method.Body.Statements[3]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var typeName4 = ((TypeofOperatorNode)init4.Expression).TypeName;
      typeName4.Qualifier.ShouldEqual("myAlias");
      typeName4.TypeTags[0].Identifier.ShouldEqual("IList");
      typeName4.TypeTags[0].IsUnbound.ShouldBeTrue();
      typeName4.TypeTags[0].GenericDimensions.ShouldEqual(1);

      var init5 = ((VariableDeclarationStatementNode)method.Body.Statements[4]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var typeName5 = ((TypeofOperatorNode)init5.Expression).TypeName;
      typeName5.Qualifier.ShouldBeNull();
      typeName5.TypeTags[0].Identifier.ShouldEqual("Generic");
      typeName5.TypeTags[0].IsUnbound.ShouldBeTrue();
      typeName5.TypeTags[0].GenericDimensions.ShouldEqual(2);
      typeName5.TypeTags[1].Identifier.ShouldEqual("EmbeddedGeneric");
      typeName5.TypeTags[1].IsUnbound.ShouldBeTrue();
      typeName5.TypeTags[1].GenericDimensions.ShouldEqual(1);

      var init6 = ((VariableDeclarationStatementNode)method.Body.Statements[5]).Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var typeName6 = ((TypeofOperatorNode)init6.Expression).TypeName;
      typeName6.TypeTags[0].Identifier.ShouldEqual("void");
      typeName6.TypeTags[0].IsUnbound.ShouldBeFalse();
      typeName6.TypeTags[0].GenericDimensions.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    ///  int i = sizeof (int);
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void SizeofExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\SizeofExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as SizeofOperatorNode;
      expr.TypeName.TypeTags[0].Identifier.ShouldEqual("int");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    ///  int i = default(int);
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void DefaultValueExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\DefaultValueExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expr = initializer.Expression as DefaultValueOperatorNode;
      expr.TypeName.TypeTags[0].Identifier.ShouldEqual("int");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    ///  int i = checked(p++);
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CheckedExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\CheckedExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expression = initializer.Expression as CheckedOperatorNode;
      var embeddedExpression = expression.Expression as PostIncrementOperatorNode;
      embeddedExpression.ShouldNotBeNull();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    ///      int i = unchecked(p++);
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UncheckedExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\UncheckedExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;

      var expression = initializer.Expression as UncheckedOperatorNode;
      var embeddedExpression = expression.Expression as PostIncrementOperatorNode;
      embeddedExpression.ShouldNotBeNull();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    ///    var i1 = +1;
    ///    var i2 = -2;
    ///    var i3 = !true;
    ///    var i4 = ~0;
    ///    var i5 = &i1;
    ///    var i6 = *i5;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UnaryOperatorExpressions()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\UnaryOperatorExpressions.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      var var1 = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var init1 = var1.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var op1 = init1.Expression as UnaryOperatorExpressionNode;
      op1.Operator.ShouldEqual(UnaryOperatorType.Identity);
      ((Int32LiteralNode) op1.Operand).Value.ShouldEqual(1);

      var var2 = method.Body.Statements[1] as VariableDeclarationStatementNode;
      var init2 = var2.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var op2 = init2.Expression as UnaryOperatorExpressionNode;
      op2.Operator.ShouldEqual(UnaryOperatorType.Negation);
      ((Int32LiteralNode)op2.Operand).Value.ShouldEqual(2);

      var var3 = method.Body.Statements[2] as VariableDeclarationStatementNode;
      var init3 = var3.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var op3 = init3.Expression as UnaryOperatorExpressionNode;
      op3.Operator.ShouldEqual(UnaryOperatorType.LogicalNegation);
      ((BooleanLiteralNode)op3.Operand).Value.ShouldEqual(true);

      var var4 = method.Body.Statements[3] as VariableDeclarationStatementNode;
      var init4 = var4.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var op4 = init4.Expression as UnaryOperatorExpressionNode;
      op4.Operator.ShouldEqual(UnaryOperatorType.BitwiseNegation);
      ((Int32LiteralNode)op4.Operand).Value.ShouldEqual(0);

      var var5 = method.Body.Statements[4] as VariableDeclarationStatementNode;
      var init5 = var5.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var op5 = init5.Expression as UnaryOperatorExpressionNode;
      op5.Operator.ShouldEqual(UnaryOperatorType.AddressOf);
      ((SimpleNameNode)op5.Operand).Identifier.ShouldEqual("i1");

      var var6 = method.Body.Statements[5] as VariableDeclarationStatementNode;
      var init6 = var6.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      var op6 = init6.Expression as UnaryOperatorExpressionNode;
      op6.Operator.ShouldEqual(UnaryOperatorType.PointerIndirection);
      ((SimpleNameNode)op6.Operand).Identifier.ShouldEqual("i5");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    ///    var i1 = (int)0;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CastExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\CastExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      
      var typecast = initializer.Expression as TypecastOperatorNode;
      typecast.TypeName.TypeTags[0].Identifier.ShouldEqual("int");
      ((Int32LiteralNode) typecast.Operand).Value.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    ///    var i1 = true ? 0 : 1;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ConditionalExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\ConditionalExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      
      var conditional = initializer.Expression as ConditionalOperatorNode;
      ((BooleanLiteralNode) conditional.Condition).Value.ShouldEqual(true);
      ((Int32LiteralNode) conditional.TrueExpression).Value.ShouldEqual(0);
      ((Int32LiteralNode) conditional.FalseExpression).Value.ShouldEqual(1);
    }
  }
}