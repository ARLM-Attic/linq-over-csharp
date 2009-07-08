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

      {
        var decl = method.Body.Statements[0] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        ((Int32LiteralNode)init.Expression).Value.ShouldEqual(1);
      }
      {
        var decl = method.Body.Statements[1] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        ((Int64LiteralNode)init.Expression).Value.ShouldEqual(2);
      }
      {
        var decl = method.Body.Statements[2] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        ((UInt32LiteralNode)init.Expression).Value.ShouldEqual<uint>(3);
      }
      {
        var decl = method.Body.Statements[3] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        ((UInt64LiteralNode)init.Expression).Value.ShouldEqual<ulong>(4);
      }
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

      {
        var decl = method.Body.Statements[0] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        ((SingleLiteralNode)init.Expression).Value.ShouldEqual(1);
      }
      {
        var decl = method.Body.Statements[1] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        ((DoubleLiteralNode)init.Expression).Value.ShouldEqual(2);
      }
      {
        var decl = method.Body.Statements[2] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        ((DecimalLiteralNode)init.Expression).Value.ShouldEqual(3);
      }
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

      var decl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      ((CharLiteralNode)init.Expression).Value.ShouldEqual('a');
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

      var decl = method.Body.Statements[0] as VariableDeclarationStatementNode;
      var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
      ((StringLiteralNode)init.Expression).Value.ShouldEqual("a");
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

      {
        var op = ((ExpressionStatementNode) method.Body.Statements[0]).Expression as PreIncrementExpressionNode;
        op.ShouldNotBeNull();
      }
      {
        var op = ((ExpressionStatementNode) method.Body.Statements[1]).Expression as PreDecrementExpressionNode;
        op.ShouldNotBeNull();
      }
      {
        var op = ((ExpressionStatementNode) method.Body.Statements[2]).Expression as PostIncrementExpressionNode;
        op.ShouldNotBeNull();
      }
      {
        var op = ((ExpressionStatementNode) method.Body.Statements[3]).Expression as PostDecrementExpressionNode;
        op.ShouldNotBeNull();
      }
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

      var assignment = initializer.Expression as AssignmentExpressionNode;
      assignment.Operator.ShouldEqual(AssignmentOperator.SimpleAssignment);
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

      var binary1 = initializer.Expression as BinaryExpressionNode;
      binary1.Operator.ShouldEqual(BinaryOperator.Multiplication);

      var binary1RightOperand = binary1.RightOperand as Int32LiteralNode;
      binary1RightOperand.Value.ShouldEqual(4);

      var binary2 = binary1.LeftOperand as BinaryExpressionNode;
      binary2.Operator.ShouldEqual(BinaryOperator.Multiplication);

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

      var binary1 = initializer.Expression as BinaryExpressionNode;
      binary1.Operator.ShouldEqual(BinaryOperator.Addition);

      var binary1RightOperand = binary1.RightOperand as Int32LiteralNode;
      binary1RightOperand.Value.ShouldEqual(4);

      var binary2 = binary1.LeftOperand as BinaryExpressionNode;
      binary2.Operator.ShouldEqual(BinaryOperator.Addition);

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

      var binary1 = initializer.Expression as BinaryExpressionNode;
      binary1.Operator.ShouldEqual(BinaryOperator.Addition);

      var binary1RightOperand = binary1.RightOperand as Int32LiteralNode;
      binary1RightOperand.Value.ShouldEqual(5);

      var binary2 = binary1.LeftOperand as BinaryExpressionNode;
      binary2.Operator.ShouldEqual(BinaryOperator.Addition);

      var binary2LeftOperand = binary2.LeftOperand as Int32LiteralNode;
      binary2LeftOperand.Value.ShouldEqual(2);

      var binary3 = binary2.RightOperand as BinaryExpressionNode;
      binary3.Operator.ShouldEqual(BinaryOperator.Multiplication);

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
      var op1 = initializer.Expression as AssignmentExpressionNode;
      op1.Operator.ShouldEqual(AssignmentOperator.AdditionAssignment);
      ((SimpleNameNode) (op1.LeftOperand)).Identifier.ShouldEqual("x");

      // The next expression is (x -= assignment-expression)
      var op2 = op1.RightOperand as AssignmentExpressionNode;
      op2.Operator.ShouldEqual(AssignmentOperator.SubtractionAssignment);
      ((SimpleNameNode)(op2.LeftOperand)).Identifier.ShouldEqual("x");

      // The next expression is (x *= assignment-expression)
      var op3 = op2.RightOperand as AssignmentExpressionNode;
      op3.Operator.ShouldEqual(AssignmentOperator.MultiplicationAssignment);
      ((SimpleNameNode)(op3.LeftOperand)).Identifier.ShouldEqual("x");

      // The next expression is (x /= x)
      var op4 = op3.RightOperand as AssignmentExpressionNode;
      op4.Operator.ShouldEqual(AssignmentOperator.DivisionAssignment);
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

      var binary = initializer.Expression as BinaryExpressionNode;
      binary.Operator.ShouldEqual(BinaryOperator.NullCoalescing);
      ((StringLiteralNode) (binary.LeftOperand)).Value.ShouldEqual("a");
      ((StringLiteralNode) (binary.RightOperand)).Value.ShouldEqual("b");
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

      var expr = initializer.Expression as PrimaryExpressionMemberAccessNode;
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

      var expr = initializer.Expression as PrimaryExpressionMemberAccessNode;
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
      var expression = exprStatement.Expression as AssignmentExpressionNode;
      var pointerExpr = expression.LeftOperand as PointerMemberAccessNode;
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

      var invocation = ((ExpressionStatementNode) method.Body.Statements[0]).Expression as InvocationExpressionNode;
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

      var invocation = ((ExpressionStatementNode)method.Body.Statements[0]).Expression as InvocationExpressionNode;
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

      var expr = initializer.Expression as TypeTestingExpressionNode;
      expr.Operator.ShouldEqual(TypeTestingOperator.Is);
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

      var expr1 = initializer.Expression as BinaryExpressionNode;
      expr1.Operator.ShouldEqual(BinaryOperator.ConditionalAnd);
      ((TrueLiteralNode) expr1.LeftOperand).Value.ShouldEqual(true);

      var expr2 = expr1.RightOperand as TypeTestingExpressionNode;
      expr2.Operator.ShouldEqual(TypeTestingOperator.Is);
      expr2.RightOperand.TypeTags[0].Identifier.ShouldEqual("bool");

      var expr3 = expr2.LeftOperand as BinaryExpressionNode;
      expr3.Operator.ShouldEqual(BinaryOperator.Addition);
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

      var expr = initializer.Expression as TypeTestingExpressionNode;
      expr.Operator.ShouldEqual(TypeTestingOperator.As);
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

      {
        var decl = method.Body.Statements[0] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var typeName = ((TypeofExpressionNode)init.Expression).TypeName;
        typeName.TypeTags[0].Identifier.ShouldEqual("int");
        typeName.TypeTags[0].IsUnbound.ShouldBeFalse();
        typeName.TypeTags[0].GenericDimensions.ShouldEqual(0);
      }
      {
        var decl = method.Body.Statements[1] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var typeName = ((TypeofExpressionNode)init.Expression).TypeName;
        typeName.TypeTags[0].Identifier.ShouldEqual("Generic");
        typeName.TypeTags[0].IsUnbound.ShouldBeTrue();
        typeName.TypeTags[0].GenericDimensions.ShouldEqual(2);
      }
      {
        var decl = method.Body.Statements[2] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var typeName = ((TypeofExpressionNode)init.Expression).TypeName;
        typeName.TypeTags[0].Identifier.ShouldEqual("Generic");
        typeName.TypeTags[0].IsUnbound.ShouldBeFalse();
        typeName.TypeTags[0].GenericDimensions.ShouldEqual(2);
        typeName.TypeTags[0].Arguments[0].TypeTags[0].Identifier.ShouldEqual("int");
        typeName.TypeTags[0].Arguments[1].TypeTags[0].Identifier.ShouldEqual("int");
      }
      {
        var decl = method.Body.Statements[3] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var typeName = ((TypeofExpressionNode)init.Expression).TypeName;
        typeName.Qualifier.ShouldEqual("myAlias");
        typeName.TypeTags[0].Identifier.ShouldEqual("IList");
        typeName.TypeTags[0].IsUnbound.ShouldBeTrue();
        typeName.TypeTags[0].GenericDimensions.ShouldEqual(1);
      }
      {
        var decl = method.Body.Statements[4] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var typeName = ((TypeofExpressionNode)init.Expression).TypeName;
        typeName.Qualifier.ShouldBeNull();
        typeName.TypeTags[0].Identifier.ShouldEqual("Generic");
        typeName.TypeTags[0].IsUnbound.ShouldBeTrue();
        typeName.TypeTags[0].GenericDimensions.ShouldEqual(2);
        typeName.TypeTags[1].Identifier.ShouldEqual("EmbeddedGeneric");
        typeName.TypeTags[1].IsUnbound.ShouldBeTrue();
        typeName.TypeTags[1].GenericDimensions.ShouldEqual(1);
      }
      {
        var decl = method.Body.Statements[5] as VariableDeclarationStatementNode;
        var init = decl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var typeName = ((TypeofExpressionNode)init.Expression).TypeName;
        typeName.TypeTags[0].Identifier.ShouldEqual("void");
        typeName.TypeTags[0].IsUnbound.ShouldBeFalse();
        typeName.TypeTags[0].GenericDimensions.ShouldEqual(0);
      }
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

      var expr = initializer.Expression as SizeofExpressionNode;
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

      var expr = initializer.Expression as DefaultValueExpressionNode;
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

      var expression = initializer.Expression as CheckedExpressionNode;
      var embeddedExpression = expression.Expression as PostIncrementExpressionNode;
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

      var expression = initializer.Expression as UncheckedExpressionNode;
      var embeddedExpression = expression.Expression as PostIncrementExpressionNode;
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

      {
        var var = method.Body.Statements[0] as VariableDeclarationStatementNode;
        var init = var.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var op = init.Expression as UnaryOperatorExpressionNode;
        op.Operator.ShouldEqual(UnaryOperator.Identity);
        ((Int32LiteralNode)op.Operand).Value.ShouldEqual(1);
      }
      {
        var var = method.Body.Statements[1] as VariableDeclarationStatementNode;
        var init = var.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var op = init.Expression as UnaryOperatorExpressionNode;
        op.Operator.ShouldEqual(UnaryOperator.Negation);
        ((Int32LiteralNode)op.Operand).Value.ShouldEqual(2);
      }
      {
        var var = method.Body.Statements[2] as VariableDeclarationStatementNode;
        var init = var.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var op = init.Expression as UnaryOperatorExpressionNode;
        op.Operator.ShouldEqual(UnaryOperator.LogicalNegation);
        ((BooleanLiteralNode)op.Operand).Value.ShouldEqual(true);
      }
      {
        var var = method.Body.Statements[3] as VariableDeclarationStatementNode;
        var init = var.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var op = init.Expression as UnaryOperatorExpressionNode;
        op.Operator.ShouldEqual(UnaryOperator.BitwiseNegation);
        ((Int32LiteralNode)op.Operand).Value.ShouldEqual(0);
      }
      {
        var var = method.Body.Statements[4] as VariableDeclarationStatementNode;
        var init = var.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var op = init.Expression as UnaryOperatorExpressionNode;
        op.Operator.ShouldEqual(UnaryOperator.AddressOf);
        ((SimpleNameNode)op.Operand).Identifier.ShouldEqual("i1");
      }
      {
        var var = method.Body.Statements[5] as VariableDeclarationStatementNode;
        var init = var.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var op = init.Expression as UnaryOperatorExpressionNode;
        op.Operator.ShouldEqual(UnaryOperator.PointerIndirection);
        ((SimpleNameNode) op.Operand).Identifier.ShouldEqual("i5");
      }
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
      
      var typecast = initializer.Expression as CastExpressionNode;
      typecast.TypeName.TypeTags[0].Identifier.ShouldEqual("int");
      ((Int32LiteralNode) typecast.Operand).Value.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    ///    var i1 = true ? 0 : 1;
    ///
    ///    // right-associativity test
    ///    var i2 = true ? 2 : false ? 3 : 4;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ConditionalExpression()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\ConditionalExpression.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      {
        var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var conditional = initializer.Expression as ConditionalExpressionNode;
        ((BooleanLiteralNode)conditional.Condition).Value.ShouldEqual(true);
        ((Int32LiteralNode)conditional.TrueExpression).Value.ShouldEqual(0);
        ((Int32LiteralNode)conditional.FalseExpression).Value.ShouldEqual(1);
      }
      {
        var varDecl = method.Body.Statements[1] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var conditional = initializer.Expression as ConditionalExpressionNode;
        ((BooleanLiteralNode) conditional.Condition).Value.ShouldEqual(true);
        ((Int32LiteralNode) conditional.TrueExpression).Value.ShouldEqual(2);
        var embeddedConditional = conditional.FalseExpression as ConditionalExpressionNode;
        ((Int32LiteralNode) embeddedConditional.TrueExpression).Value.ShouldEqual(3);
        ((Int32LiteralNode) embeddedConditional.FalseExpression).Value.ShouldEqual(4);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// <code>
    ///   // Lamba expression with block body, no parameters
    ///   Dvoid d1 = () =&gt; { };
    ///
    ///   // Lambda expression with expression body, no parameters
    ///   Expression&lt;Func&lt;int&gt;&gt; d2 = () =&gt; 1;
    ///
    ///   // Lambda expression with block body, explicit-anonymous-function-signature
    ///   Dint d3 = (int i, ref int j, out int k) =&gt; { k = 1; return i; };
    ///
    ///   // Lambda expression with expression body, implicit-anonymous-function-signature
    ///   Expression&lt;Func&lt;int, int, int&gt;&gt; d4 = (i, j) =&gt; i + j;
    ///
    ///   // Lambda expression are right-associative
    ///   Expression&lt;Func&lt;Func&lt;int&gt;&gt;&gt; d5 = () =&gt; () =&gt; 2;
    /// </code>
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void LambdaExpressions()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\LambdaExpressions.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;
      {
        var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var lambda = initializer.Expression as LambdaExpressionNode;
        lambda.FormalParameters.Count.ShouldEqual(0);
        lambda.IsSimpleExpression.ShouldBeFalse();
        lambda.Expression.ShouldBeNull();
        lambda.Block.Statements.Count.ShouldEqual(0);
      }
      {
        var varDecl = method.Body.Statements[1] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var lambda = initializer.Expression as LambdaExpressionNode;
        lambda.FormalParameters.Count.ShouldEqual(0);
        lambda.IsSimpleExpression.ShouldBeTrue();
        ((Int32LiteralNode)lambda.Expression).Value.ShouldEqual(1);
        lambda.Block.ShouldBeNull();
      }
      {
        var varDecl = method.Body.Statements[2] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var lambda = initializer.Expression as LambdaExpressionNode;
        lambda.FormalParameters.Count.ShouldEqual(3);
        lambda.FormalParameters[0].TypeName.TypeTags[0].Identifier.ShouldEqual("int");
        lambda.FormalParameters[0].Identifier.ShouldEqual("i");
        lambda.FormalParameters[0].Modifier.ShouldEqual(FormalParameterModifier.In);
        lambda.FormalParameters[1].TypeName.TypeTags[0].Identifier.ShouldEqual("int");
        lambda.FormalParameters[1].Identifier.ShouldEqual("j");
        lambda.FormalParameters[1].Modifier.ShouldEqual(FormalParameterModifier.Ref);
        lambda.FormalParameters[2].TypeName.TypeTags[0].Identifier.ShouldEqual("int");
        lambda.FormalParameters[2].Identifier.ShouldEqual("k");
        lambda.FormalParameters[2].Modifier.ShouldEqual(FormalParameterModifier.Out);
        lambda.IsSimpleExpression.ShouldBeFalse();
        lambda.Expression.ShouldBeNull();
        lambda.Block.Statements.Count.ShouldEqual(2);
      }
      {
        var varDecl = method.Body.Statements[3] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var lambda = initializer.Expression as LambdaExpressionNode;
        lambda.FormalParameters.Count.ShouldEqual(2);
        lambda.FormalParameters[0].TypeName.IsEmpty.ShouldBeTrue();
        lambda.FormalParameters[0].Identifier.ShouldEqual("i");
        lambda.FormalParameters[1].TypeName.IsEmpty.ShouldBeTrue();
        lambda.FormalParameters[1].Identifier.ShouldEqual("j");
        lambda.IsSimpleExpression.ShouldBeTrue();
        ((BinaryExpressionNode)lambda.Expression).Operator.ShouldEqual(BinaryOperator.Addition);
        ((SimpleNameNode)((BinaryExpressionNode)lambda.Expression).LeftOperand).Identifier.ShouldEqual("i");
        ((SimpleNameNode)((BinaryExpressionNode)lambda.Expression).RightOperand).Identifier.ShouldEqual("j");
        lambda.Block.ShouldBeNull();
      }
      {
        var varDecl = method.Body.Statements[4] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var lambda = initializer.Expression as LambdaExpressionNode;
        lambda.FormalParameters.Count.ShouldEqual(0);
        var embeddedLambda = lambda.Expression as LambdaExpressionNode;
        embeddedLambda.FormalParameters.Count.ShouldEqual(0);
        ((Int32LiteralNode) embeddedLambda.Expression).Value.ShouldEqual(2);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// <code>
    ///   // Anonymous method without signature
    ///   Dvoid d1 = delegate { };
    ///   // Anonymous method with empty signature
    ///   Dvoid d2 = delegate() { };
    ///   // Anonymous method with signature
    ///   Dint d3 = delegate(int i, ref int j, out int k) { k = 1; return i; };
    /// </code>
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void AnonymousMethodExpressions()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\AnonymousMethodExpressions.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      {
        var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var anonym = initializer.Expression as AnonymousMethodExpressionNode;
        anonym.FormalParameters.Count.ShouldEqual(0);
        anonym.Body.Statements.Count.ShouldEqual(0);
      }
      {
        var varDecl = method.Body.Statements[1] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var anonym = initializer.Expression as AnonymousMethodExpressionNode;
        anonym.FormalParameters.Count.ShouldEqual(0);
        anonym.Body.Statements.Count.ShouldEqual(0);
      }
      {
        var varDecl = method.Body.Statements[2] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var anonym = initializer.Expression as AnonymousMethodExpressionNode;
        anonym.FormalParameters.Count.ShouldEqual(3);
        anonym.FormalParameters[0].TypeName.TypeTags[0].Identifier.ShouldEqual("int");
        anonym.FormalParameters[0].Identifier.ShouldEqual("i");
        anonym.FormalParameters[0].Modifier.ShouldEqual(FormalParameterModifier.In);
        anonym.FormalParameters[1].TypeName.TypeTags[0].Identifier.ShouldEqual("int");
        anonym.FormalParameters[1].Identifier.ShouldEqual("j");
        anonym.FormalParameters[1].Modifier.ShouldEqual(FormalParameterModifier.Ref);
        anonym.FormalParameters[2].TypeName.TypeTags[0].Identifier.ShouldEqual("int");
        anonym.FormalParameters[2].Identifier.ShouldEqual("k");
        anonym.FormalParameters[2].Modifier.ShouldEqual(FormalParameterModifier.Out);
        anonym.Body.Statements.Count.ShouldEqual(2);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the parsing of the expression:
    /// <code>
    ///    // simplest query with select: from-clause select-clause
    ///    var a1 = from i in array
    ///             select i;
    ///    
    ///    // simplest query with group: from-clause group-clause
    ///    var a2 = from int i in array
    ///             group i by i;
    ///    
    ///    // query with all possible clauses 
    ///    var a3 = from i in array
    ///             from int j in array
    ///             let k = j
    ///             where true
    ///             join l in array on i equals l
    ///             join int p in array on i equals p
    ///             join n in array on i equals n into o
    ///             join int q in array on i equals q into r
    ///             orderby k ascending , j descending
    ///             select i
    ///             into m
    ///               select m;
    /// </code>
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void QueryExpressions()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Expressions\QueryExpressions.cs");
      InvokeParser(project).ShouldBeTrue();

      var method = project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0].MemberDeclarations[0] as MethodDeclarationNode;

      {
        var varDecl = method.Body.Statements[0] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var query = initializer.Expression as QueryExpressionNode;
        query.FromClause.TypeName.IsEmpty.ShouldBeTrue();
        query.FromClause.Identifier.ShouldEqual("i1");
        ((SimpleNameNode)query.FromClause.Expression).Identifier.ShouldEqual("array");
        query.QueryBody.BodyClauses.Count.ShouldEqual(0);
        ((SimpleNameNode)query.QueryBody.SelectClause.Expression).Identifier.ShouldEqual("i1");
        query.QueryBody.GroupClause.ShouldBeNull();
        query.QueryBody.QueryContinuation.ShouldBeNull();
      }
      {
        var varDecl = method.Body.Statements[1] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var query = initializer.Expression as QueryExpressionNode;
        query.FromClause.TypeName.IsEmpty.ShouldBeFalse();
        query.FromClause.TypeName.TypeTags[0].Identifier.ShouldEqual("int");
        query.FromClause.Identifier.ShouldEqual("i2");
        ((SimpleNameNode)query.FromClause.Expression).Identifier.ShouldEqual("array");
        query.QueryBody.BodyClauses.Count.ShouldEqual(0);
        query.QueryBody.SelectClause.ShouldBeNull();
        ((SimpleNameNode)query.QueryBody.GroupClause.GroupExpression).Identifier.ShouldEqual("i2");
        var unaryNode = query.QueryBody.GroupClause.ByExpression as UnaryOperatorExpressionNode;
        unaryNode.Operator.ShouldEqual(UnaryOperator.Negation);
        ((SimpleNameNode)unaryNode.Operand).Identifier.ShouldEqual("i2");
        query.QueryBody.QueryContinuation.ShouldBeNull();
      }
      {
        var varDecl = method.Body.Statements[2] as VariableDeclarationStatementNode;
        var initializer = varDecl.Declaration.VariableTags[0].Initializer as ExpressionInitializerNode;
        var query = initializer.Expression as QueryExpressionNode;
        
        query.FromClause.TypeName.IsEmpty.ShouldBeTrue();
        query.FromClause.Identifier.ShouldEqual("i");
        ((SimpleNameNode)query.FromClause.Expression).Identifier.ShouldEqual("array");

        query.QueryBody.BodyClauses.Count.ShouldEqual(8);
        
        var fromClause = query.QueryBody.BodyClauses[0] as FromClauseNode;
        fromClause.TypeName.IsEmpty.ShouldBeFalse();
        fromClause.TypeName.TypeTags[0].Identifier.ShouldEqual("int"); 
        fromClause.Identifier.ShouldEqual("j");
        ((SimpleNameNode) fromClause.Expression).Identifier.ShouldEqual("array");

        var letClause = query.QueryBody.BodyClauses[1] as LetClauseNode;
        letClause.Identifier.ShouldEqual("k");
        ((SimpleNameNode) letClause.Expression).Identifier.ShouldEqual("j");

        var whereClause = query.QueryBody.BodyClauses[2] as WhereClauseNode;
        ((TrueLiteralNode)whereClause.Expression).Value.ShouldEqual(true);

        var joinClause = query.QueryBody.BodyClauses[3] as JoinClauseNode;
        joinClause.TypeName.IsEmpty.ShouldBeTrue();
        joinClause.Identifier.ShouldEqual("l");
        ((SimpleNameNode) joinClause.InExpression).Identifier.ShouldEqual("array");
        ((SimpleNameNode)joinClause.OnExpression).Identifier.ShouldEqual("i");
        ((SimpleNameNode)joinClause.EqualsExpression).Identifier.ShouldEqual("l");

        var joinClauseWithTypeName = query.QueryBody.BodyClauses[4] as JoinClauseNode;
        joinClauseWithTypeName.TypeName.IsEmpty.ShouldBeFalse();
        joinClauseWithTypeName.TypeName.TypeTags[0].Identifier.ShouldEqual("int");
        joinClauseWithTypeName.Identifier.ShouldEqual("p");
        ((SimpleNameNode)joinClauseWithTypeName.InExpression).Identifier.ShouldEqual("array");
        ((SimpleNameNode)joinClauseWithTypeName.OnExpression).Identifier.ShouldEqual("i");
        ((SimpleNameNode)joinClauseWithTypeName.EqualsExpression).Identifier.ShouldEqual("p");

        var joinIntoClause = query.QueryBody.BodyClauses[5] as JoinIntoClauseNode;
        joinIntoClause.TypeName.IsEmpty.ShouldBeTrue();
        joinIntoClause.Identifier.ShouldEqual("n");
        ((SimpleNameNode)joinIntoClause.InExpression).Identifier.ShouldEqual("array");
        ((SimpleNameNode)joinIntoClause.OnExpression).Identifier.ShouldEqual("i");
        ((SimpleNameNode)joinIntoClause.EqualsExpression).Identifier.ShouldEqual("n");
        joinIntoClause.IntoIdentifier.ShouldEqual("o");

        var joinIntoClauseWithTypeName = query.QueryBody.BodyClauses[6] as JoinIntoClauseNode;
        joinIntoClauseWithTypeName.TypeName.IsEmpty.ShouldBeFalse();
        joinIntoClauseWithTypeName.TypeName.TypeTags[0].Identifier.ShouldEqual("int");
        joinIntoClauseWithTypeName.Identifier.ShouldEqual("q");
        ((SimpleNameNode)joinIntoClauseWithTypeName.InExpression).Identifier.ShouldEqual("array");
        ((SimpleNameNode)joinIntoClauseWithTypeName.OnExpression).Identifier.ShouldEqual("i");
        ((SimpleNameNode)joinIntoClauseWithTypeName.EqualsExpression).Identifier.ShouldEqual("q");
        joinIntoClauseWithTypeName.IntoIdentifier.ShouldEqual("r");

        var orderbyClause = query.QueryBody.BodyClauses[7] as OrderByClauseNode;
        orderbyClause.Orderings.Count.ShouldEqual(2);
        ((SimpleNameNode) orderbyClause.Orderings[0].Expression).Identifier.ShouldEqual("k");
        orderbyClause.Orderings[0].IsAscending.ShouldBeTrue();
        ((SimpleNameNode)orderbyClause.Orderings[1].Expression).Identifier.ShouldEqual("j");
        orderbyClause.Orderings[1].IsAscending.ShouldBeFalse();

        ((SimpleNameNode)query.QueryBody.SelectClause.Expression).Identifier.ShouldEqual("i");

        query.QueryBody.GroupClause.ShouldBeNull();

        query.QueryBody.QueryContinuation.Identifier.ShouldEqual("m");

        var embeddedQuery = query.QueryBody.QueryContinuation.QueryBody;
        embeddedQuery.BodyClauses.Count.ShouldEqual(0);
        ((SimpleNameNode) embeddedQuery.SelectClause.Expression).Identifier.ShouldEqual("m");
        embeddedQuery.GroupClause.ShouldBeNull();
        embeddedQuery.QueryContinuation.ShouldBeNull();
      }
    }
  }
}
