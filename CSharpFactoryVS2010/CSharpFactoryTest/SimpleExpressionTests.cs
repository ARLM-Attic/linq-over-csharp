using CSharpFactory.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest
{
  /// <summary>
  /// Summary description for SimpleExpressionTests
  /// </summary>
  [TestClass]
  public class SimpleExpressionTests : ParserTestBed
  {
    [TestMethod]
    public void Expression1IsOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("SimpleExpressions.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[0];
      Assert.IsNotNull(td);
      MethodDeclaration method1 = td.Members[0] as MethodDeclaration;
      Assert.IsNotNull(method1);
      LocalVariableDeclaration decl1 = method1.Statements[0] as LocalVariableDeclaration;

      // int x = 6;
      Assert.IsNotNull(decl1);
      Assert.AreEqual(decl1.Variable.ResultingType.TailName, "int");
      Assert.AreEqual(decl1.Name, "x");
      Assert.IsNotNull(decl1.Variable.Initializer);
      ExpressionInitializer sin = decl1.Variable.Initializer as ExpressionInitializer;
      Assert.IsNotNull(sin);
      Int32Constant con6 = sin.Expression as Int32Constant;
      Assert.AreEqual(con6.Value, 6);
    }

    [TestMethod]
    public void Expression2IsOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("SimpleExpressions.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[0];
      Assert.IsNotNull(td);
      MethodDeclaration method1 = td.Members[0] as MethodDeclaration;
      Assert.IsNotNull(method1);
      LocalVariableDeclaration decl1 = method1.Statements[1] as LocalVariableDeclaration;

      // int y = x = 8;
      Assert.IsNotNull(decl1);
      Assert.AreEqual(decl1.Variable.ResultingType.TailName, "int");
      Assert.AreEqual(decl1.Name, "y");
      Assert.IsNotNull(decl1.Variable.Initializer);
      ExpressionInitializer sin = decl1.Variable.Initializer as ExpressionInitializer;
      Assert.IsNotNull(sin);
      AssignmentOperator asg = sin.Expression as AssignmentOperator;
      Assert.IsNotNull(asg);
      NamedLiteral nl = asg.LeftOperand as NamedLiteral;
      Assert.IsNotNull(nl);
      Assert.AreEqual(nl.Name, "x");
      Int32Constant con6 = asg.RightOperand as Int32Constant;
      Assert.AreEqual(con6.Value, 8);
    }

    [TestMethod]
    public void Expression3IsOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("SimpleExpressions.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[0];
      Assert.IsNotNull(td);
      MethodDeclaration method1 = td.Members[0] as MethodDeclaration;
      Assert.IsNotNull(method1);
      LocalVariableDeclaration decl1 = method1.Statements[2] as LocalVariableDeclaration;

      // int a1 = 2*3*4;
      Assert.IsNotNull(decl1);
      Assert.AreEqual(decl1.Variable.ResultingType.TailName, "int");
      Assert.AreEqual(decl1.Name, "a1");
      Assert.IsNotNull(decl1.Variable.Initializer);
      ExpressionInitializer sin = decl1.Variable.Initializer as ExpressionInitializer;
      Assert.IsNotNull(sin);
      MultiplyOperator op1 = sin.Expression as MultiplyOperator;
      Assert.IsNotNull(op1);
      Int32Constant con4 = op1.RightOperand as Int32Constant;
      Assert.AreEqual(con4.Value, 4);
      MultiplyOperator op2 = op1.LeftOperand as MultiplyOperator;
      Assert.IsNotNull(op2);
      Int32Constant con3 = op2.RightOperand as Int32Constant;
      Assert.AreEqual(con3.Value, 3);
      Int32Constant con2 = op2.LeftOperand as Int32Constant;
      Assert.AreEqual(con2.Value, 2);
    }

    [TestMethod]
    public void Expression4IsOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("SimpleExpressions.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[0];
      Assert.IsNotNull(td);
      MethodDeclaration method1 = td.Members[0] as MethodDeclaration;
      Assert.IsNotNull(method1);
      LocalVariableDeclaration decl1 = method1.Statements[3] as LocalVariableDeclaration;

      // int a2 = 2 + 3 + 4;
      Assert.IsNotNull(decl1);
      Assert.AreEqual(decl1.Variable.ResultingType.TailName, "int");
      Assert.AreEqual(decl1.Name, "a2");
      Assert.IsNotNull(decl1.Variable.Initializer);
      ExpressionInitializer sin = decl1.Variable.Initializer as ExpressionInitializer;
      Assert.IsNotNull(sin);
      AddOperator op1 = sin.Expression as AddOperator;
      Assert.IsNotNull(op1);
      Int32Constant con4 = op1.RightOperand as Int32Constant;
      Assert.AreEqual(con4.Value, 4);
      AddOperator op2 = op1.LeftOperand as AddOperator;
      Assert.IsNotNull(op2);
      Int32Constant con3 = op2.RightOperand as Int32Constant;
      Assert.AreEqual(con3.Value, 3);
      Int32Constant con2 = op2.LeftOperand as Int32Constant;
      Assert.AreEqual(con2.Value, 2);
    }

    [TestMethod]
    public void Expression5IsOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("SimpleExpressions.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[0];
      Assert.IsNotNull(td);
      MethodDeclaration method1 = td.Members[0] as MethodDeclaration;
      Assert.IsNotNull(method1);
      LocalVariableDeclaration decl1 = method1.Statements[4] as LocalVariableDeclaration;

      // int a3 = 2 + 3 * 4 + 5;
      Assert.IsNotNull(decl1);
      Assert.AreEqual(decl1.Variable.ResultingType.TailName, "int");
      Assert.AreEqual(decl1.Name, "a3");
      Assert.IsNotNull(decl1.Variable.Initializer);
      ExpressionInitializer sin = decl1.Variable.Initializer as ExpressionInitializer;
      Assert.IsNotNull(sin);
      AddOperator op1 = sin.Expression as AddOperator;
      Assert.IsNotNull(op1);
      Int32Constant con5 = op1.RightOperand as Int32Constant;
      Assert.AreEqual(con5.Value, 5);
      AddOperator op2 = op1.LeftOperand as AddOperator;
      Assert.IsNotNull(op2);
      Int32Constant con2 = op2.LeftOperand as Int32Constant;
      Assert.AreEqual(con2.Value, 2);
      MultiplyOperator op3 = op2.RightOperand as MultiplyOperator;
      Assert.IsNotNull(op3);
      Int32Constant con3 = op3.LeftOperand as Int32Constant;
      Assert.AreEqual(con3.Value, 3);
      Int32Constant con4 = op3.RightOperand as Int32Constant;
      Assert.AreEqual(con4.Value, 4);
    }
  }
}
