﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the member access resolution logic.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class MemberAccessResolutionTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a primary member access in the context of a static field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_Static()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_Static.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var field_a1 = class_A.GetOwnMember<FieldEntity>("a1");
      var memberAccess_c1 = (field_a1.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
      var memberAccess_C1 = memberAccess_c1.ChildExpression as PrimaryMemberAccessExpressionEntity;
      var memberAccess_N2 = memberAccess_C1.ChildExpression as PrimaryMemberAccessExpressionEntity;
      var simpleName_N1 = memberAccess_N2.ChildExpression as SimpleNameExpressionEntity;

      (simpleName_N1.ExpressionResult as NamespaceExpressionResult).Namespace.ToString().ShouldEqual("global::N1");
      (memberAccess_N2.ExpressionResult as NamespaceExpressionResult).Namespace.ToString().ShouldEqual("global::N1.N2");
      (memberAccess_C1.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::N1.N2.C1");
      (memberAccess_c1.ExpressionResult as VariableExpressionResult).Type.ToString().ShouldEqual("global::System.Int32");
      (memberAccess_c1.ExpressionResult as VariableExpressionResult).Variable.ToString().ShouldEqual("global::N1.N2.C1_c1");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a primary member access in the context of an instance field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_Instance()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_Instance.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var method_M = class_A.GetOwnMember<MethodEntity>("M");
      var statement = method_M.Body.Statements.ToList()[0] as ExpressionStatementEntity;
      var assignment = statement.Expression as AssignmentExpressionEntity;
      var memberAccess_c1 = assignment.RightExpression as MemberAccessExpressionEntity;
      (memberAccess_c1.ExpressionResult as VariableExpressionResult).Variable.ToString().ShouldEqual("global::N1.N2.C1_c1");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a qualified alias member access.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void QualifiedAliasMemberAccess()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\QualifiedAliasMemberAccess.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      //  private static int a1 = global::N1.N2.C1.c1;
      {
        var field_a1 = class_A.GetOwnMember<FieldEntity>("a1");
        var memberAccess_c1 = (field_a1.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
        var memberAccess_C1 = memberAccess_c1.ChildExpression as PrimaryMemberAccessExpressionEntity;
        var memberAccess_N2 = memberAccess_C1.ChildExpression as QualifiedAliasMemberAccessExpressionEntity;

        (memberAccess_N2.ExpressionResult as NamespaceExpressionResult).Namespace.ToString().ShouldEqual("global::N1.N2");
        (memberAccess_C1.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::N1.N2.C1");
        (memberAccess_c1.ExpressionResult as VariableExpressionResult).Type.ToString().ShouldEqual("global::System.Int32");
        (memberAccess_c1.ExpressionResult as VariableExpressionResult).Variable.ToString().ShouldEqual("global::N1.N2.C1_c1");
      }
      //  private static int a2 = N1_Alias::N2.N3.D1.d1;
      {
        var field_a2 = class_A.GetOwnMember<FieldEntity>("a2");
        var memberAccess_d1 = (field_a2.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
        var memberAccess_D1 = memberAccess_d1.ChildExpression as PrimaryMemberAccessExpressionEntity;
        var memberAccess_N3 = memberAccess_D1.ChildExpression as QualifiedAliasMemberAccessExpressionEntity;

        (memberAccess_N3.ExpressionResult as NamespaceExpressionResult).Namespace.ToString().ShouldEqual("global::N1.N2.N3");
        (memberAccess_D1.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::N1.N2.N3.D1");
        (memberAccess_d1.ExpressionResult as VariableExpressionResult).Type.ToString().ShouldEqual("global::System.Int32");
        (memberAccess_d1.ExpressionResult as VariableExpressionResult).Variable.ToString().ShouldEqual("global::N1.N2.N3.D1_d1");
      }
      //  private static int a3 = N1_Alias::N2.C1.c1;
      {
        var field_a3 = class_A.GetOwnMember<FieldEntity>("a3");
        var memberAccess_c1 = (field_a3.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
        var memberAccess_C1 = memberAccess_c1.ChildExpression as QualifiedAliasMemberAccessExpressionEntity;

        (memberAccess_C1.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::N1.N2.C1");
        (memberAccess_c1.ExpressionResult as VariableExpressionResult).Type.ToString().ShouldEqual("global::System.Int32");
        (memberAccess_c1.ExpressionResult as VariableExpressionResult).Variable.ToString().ShouldEqual("global::N1.N2.C1_c1");
      }
      //  private static int a4 = N2_Alias::C1.c1;
      {
        var field_a4 = class_A.GetOwnMember<FieldEntity>("a4");
        var memberAccess_c1 = (field_a4.Initializer as ScalarInitializerEntity).Expression as QualifiedAliasMemberAccessExpressionEntity;

        (memberAccess_c1.ExpressionResult as VariableExpressionResult).Type.ToString().ShouldEqual("global::System.Int32");
        (memberAccess_c1.ExpressionResult as VariableExpressionResult).Variable.ToString().ShouldEqual("global::N1.N2.C1_c1");
      }
      //  private static int a5 = global::E<int, long>.e;
      {
        var field_a5 = class_A.GetOwnMember<FieldEntity>("a5");
        var memberAccess_e = (field_a5.Initializer as ScalarInitializerEntity).Expression as QualifiedAliasMemberAccessExpressionEntity;

        (memberAccess_e.ExpressionResult as VariableExpressionResult).Type.ToString().ShouldEqual("global::System.Int32");
        (memberAccess_e.ExpressionResult as VariableExpressionResult).Variable.ToString().ShouldEqual("global::E`2[global::System.Int32,global::System.Int64]_e");
      }
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a predefined type member access.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PredefinedTypeMemberAccess()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PredefinedTypeMemberAccess.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");

      //  static int a = int.MaxValue;
      {
        var field_a = class_A.GetOwnMember<FieldEntity>("a");
        var memberAccess = (field_a.Initializer as ScalarInitializerEntity).Expression as PredefinedTypeMemberAccessExpressionEntity;

        memberAccess.PredefinedTypeEntity.ToString().ShouldEqual("global::System.Int32");
        (memberAccess.ExpressionResult as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.Int32");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0120: An object reference is required for the non-static field, method, or property 'string.Length.get'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void InvalidMemberReference_CS0120_ObjectReferenceRequired()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\InvalidMemberReference_CS0120_ObjectReferenceRequired.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("InvalidMemberReference");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0122: 'B.b' is inaccessible due to its protection level
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void InvalidMemberReference_CS0122_InaccessibleField()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\InvalidMemberReference_CS0122_InaccessibleField.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("InvalidMemberReference");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0122: inaccessible class via qualified alias member
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void InvalidMemberReference_CS0122_InaccessibleClass()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\InvalidMemberReference_CS0122_InaccessibleClass.cs");
      project.AddAssemblyReference(TestAssemblyPathAndFilename);
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("InvalidMemberReference");
    }
  }
}