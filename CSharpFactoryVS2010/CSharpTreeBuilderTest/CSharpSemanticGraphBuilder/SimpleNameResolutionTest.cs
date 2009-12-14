using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the simple name resolution logic.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class SimpleNameResolutionTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0103: The name 'x' does not exist in the current context
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0103_SimpleNameUndefined()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\CS0103_SimpleNameUndefined.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0103");
      project.Warnings.Count.ShouldEqual(0);
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to a namespace entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ResolvedToNamespace()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\ResolvedToNamespace.cs");
      InvokeParser(project).ShouldBeTrue();

      // private static int a1 = N1.C1.c1;
      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var field_a1 = class_A.GetOwnMember<FieldEntity>("a1");
      var memberAccess_c1 = (field_a1.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
      var memberAccess_C1 = memberAccess_c1.ChildExpression as PrimaryMemberAccessExpressionEntity;
      var simpleName_N1 = memberAccess_C1.ChildExpression as SimpleNameExpressionEntity;

      var namespaceEntity = (simpleName_N1.ExpressionResult as NamespaceExpressionResult).Namespace;
      namespaceEntity.ToString().ShouldEqual("global::N1");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to a static member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void StaticMember()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\StaticMember.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var field_b = class_A.GetOwnMember<FieldEntity>("b");
      var field_c = class_A.GetOwnMember<FieldEntity>("c");
      var simpleName_c = (field_b.Initializer as ScalarInitializerEntity).Expression as SimpleNameExpressionEntity;
      (simpleName_c.ExpressionResult as VariableExpressionResult).Variable.ShouldEqual(field_c);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0120: An object reference is required for the non-static field, method, or property 'A.c'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0120_StaticMemberExpected()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\CS0120_StaticMemberExpected.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0120");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to a type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ResolvedToType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\ResolvedToType.cs");
      InvokeParser(project).ShouldBeTrue();

      // static int a = A.b;
      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var field_A = class_A.GetOwnMember<FieldEntity>("a");
      var memberAccess_b = (field_A.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
      var simpleName_A = memberAccess_b.ChildExpression as SimpleNameExpressionEntity;
      (simpleName_A.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::A");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to a method type-parameter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore] // How to test this ???
    public void MethodTypeParameter()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\MethodTypeParameter.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var method_M = class_A.GetOwnMember<MethodEntity>("M");
      // TODO: cont
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to an instance member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void InstanceMember()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\InstanceMember.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var method_M = class_A.GetOwnMember<MethodEntity>("M");
      var statement = method_M.Body.Statements.ToList()[0] as ExpressionStatementEntity;
      var assignment = statement.Expression as AssignmentExpressionEntity;
      var simpleName_a = assignment.LeftExpression as SimpleNameExpressionEntity;
      (simpleName_a.ExpressionResult as VariableExpressionResult).Variable.ToString().ShouldEqual("global::A_a");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to a method group.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void MethodGroup()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\MethodGroup.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var method_M1 = class_A.GetOwnMember<MethodEntity>("M1");
      var statement = method_M1.Body.Statements.ToList()[0] as ExpressionStatementEntity;
      var invocation = statement.Expression as InvocationExpressionEntity;
      var simpleName_a = invocation.ChildExpression as SimpleNameExpressionEntity;
      var result = simpleName_a.ExpressionResult as MethodGroupExpressionResult;
      var methods = result.MethodGroup.Methods.ToList();
      methods.Count.ShouldEqual(1);
      methods[0].ToString().ShouldEqual("global::A_M2()");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to a local variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void LocalVariable()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\LocalVariable.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var method_M = class_A.GetMember<MethodEntity>("M");
      var variable_decl_statement = method_M.Body.Statements.ToList()[0] as LocalVariableDeclarationStatementEntity;
      var variable_a = variable_decl_statement.LocalVariables.ToList()[0];
      var assignment_statement = method_M.Body.Statements.ToList()[1] as ExpressionStatementEntity;
      var assignment = assignment_statement.Expression as AssignmentExpressionEntity;
      var simpleName_a = assignment.LeftExpression as SimpleNameExpressionEntity;
      var result = simpleName_a.ExpressionResult as VariableExpressionResult;
      result.Type.ToString().ShouldEqual("global::System.Int32");
      result.Variable.ShouldEqual(variable_a);
    }
  }
}