using System.Linq;
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
    /// Test the resolution of a primary member access to static field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_StaticField()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_StaticField.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var field_a1 = class_A.GetOwnMember<FieldEntity>("a1");
      var memberAccess_c1 = (field_a1.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
      var memberAccess_C2 = memberAccess_c1.ChildExpression as PrimaryMemberAccessExpressionEntity;
      var memberAccess_C1 = memberAccess_C2.ChildExpression as PrimaryMemberAccessExpressionEntity;
      var memberAccess_N2 = memberAccess_C1.ChildExpression as PrimaryMemberAccessExpressionEntity;
      var simpleName_N1 = memberAccess_N2.ChildExpression as SimpleNameExpressionEntity;

      (simpleName_N1.ExpressionResult as NamespaceExpressionResult).Namespace.ToString().ShouldEqual("global::N1");
      (memberAccess_N2.ExpressionResult as NamespaceExpressionResult).Namespace.ToString().ShouldEqual("global::N1.N2");
      (memberAccess_C1.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::N1.N2.C1");
      (memberAccess_C2.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::N1.N2.C1+C2`1[global::System.Int32]");
      (memberAccess_c1.ExpressionResult as VariableExpressionResult).Type.ToString().ShouldEqual("global::System.Int32");
      (memberAccess_c1.ExpressionResult as VariableExpressionResult).Variable.ToString().ShouldEqual("global::N1.N2.C1+C2`1[global::System.Int32]_c1");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a primary member access to a readonly static field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_StaticField_ReadOnly()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_StaticField_ReadOnly.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var field_a1 = class_A.GetOwnMember<FieldEntity>("a1");
      var field_a2 = class_A.GetOwnMember<FieldEntity>("a2");
      var memberAccess_a1 = (field_a2.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
      var simpleName_A = memberAccess_a1.ChildExpression as SimpleNameExpressionEntity;

      (simpleName_A.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::A");
      (memberAccess_a1.ExpressionResult as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.UInt32");
      (memberAccess_a1.ExpressionResult as ValueExpressionResult).Entity.ShouldEqual(field_a1);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a primary member access to a static property.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_StaticProperty()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_StaticProperty.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var field_a = class_A.GetOwnMember<FieldEntity>("a");
      var property_P = class_A.GetOwnMember<PropertyEntity>("P");
      var memberAccess_P = (field_a.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
      var simpleName_A = memberAccess_P.ChildExpression as SimpleNameExpressionEntity;

      (simpleName_A.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::A");
      (memberAccess_P.ExpressionResult as PropertyAccessExpressionResult).InstanceExpression.ShouldBeNull();
      (memberAccess_P.ExpressionResult as PropertyAccessExpressionResult).PropertyEntity.ShouldEqual(property_P);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a primary member access to a static method group.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_StaticMethod()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_StaticMethod.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var field_a = class_A.GetOwnMember<FieldEntity>("a");
      var method_M = class_A.GetMember<MethodEntity>("M");
      var invocation = (field_a.Initializer as ScalarInitializerEntity).Expression as InvocationExpressionEntity;
      var memberAccess_a = invocation.ChildExpression as PrimaryMemberAccessExpressionEntity;
      var simpleName_A = memberAccess_a.ChildExpression as SimpleNameExpressionEntity;

      (simpleName_A.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::A");
      var result = (memberAccess_a.ExpressionResult as MethodGroupExpressionResult);
      result.InstanceExpression.ShouldBeNull();
      var methodGroup = result.MethodGroup;
      methodGroup.Methods.Count().ShouldEqual(1);
      methodGroup.Methods.ToList()[0].ShouldEqual(method_M);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a primary member access to a constant member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_Constant()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_Constant.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var const_a = class_A.GetOwnMember<ConstantMemberEntity>("a");
      var field_b = class_A.GetOwnMember<FieldEntity>("b");
      var memberAccess_a = (field_b.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
      var simpleName_A = memberAccess_a.ChildExpression as SimpleNameExpressionEntity;

      (simpleName_A.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::A");
      (memberAccess_a.ExpressionResult as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.Int32");
      (memberAccess_a.ExpressionResult as ValueExpressionResult).Entity.ShouldEqual(const_a);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a primary member access to an enum member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_EnumMember()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_EnumMember.cs");
      InvokeParser(project).ShouldBeTrue();

      var enum_E = project.SemanticGraph.GlobalNamespace.GetSingleChildType<EnumEntity>("E");
      var enumMember_E1 = enum_E.GetMember<EnumMemberEntity>("E1");

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var field_a = class_A.GetOwnMember<FieldEntity>("a");
      var memberAccess_a = (field_a.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
      var simpleName_A = memberAccess_a.ChildExpression as SimpleNameExpressionEntity;

      (simpleName_A.ExpressionResult as TypeExpressionResult).Type.ToString().ShouldEqual("global::E");
      (memberAccess_a.ExpressionResult as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.Int64");
      (memberAccess_a.ExpressionResult as ValueExpressionResult).Entity.ShouldEqual(enumMember_E1);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a primary member access to an instance field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_InstanceField()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_InstanceField.cs");
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
    /// Test the resolution of a primary member access to a read-only instance field
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_InstanceField_ReadOnly()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_InstanceField_ReadOnly.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var method_M = class_A.GetOwnMember<MethodEntity>("M");
      var field_a1 = class_A.GetOwnMember<FieldEntity>("a1");
      var statement = method_M.Body.Statements.ToList()[0] as LocalVariableDeclarationStatementEntity;
      var variable_x = statement.LocalVariables.ToList()[0];
      var memberAccess_a1 = (variable_x.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
      var result_a1 = memberAccess_a1.ExpressionResult as ValueExpressionResult;
      result_a1.Type.ToString().ShouldEqual("global::System.Int32");
      result_a1.Entity.ShouldEqual(field_a1);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a primary member access to an instance method.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_InstanceMethod()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_InstanceMethod.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var method_M1 = class_A.GetOwnMember<MethodEntity>("M1");
      var method_M2 = class_A.GetOwnMember<MethodEntity>("M2");
      var statement = method_M2.Body.Statements.ToList()[0] as ExpressionStatementEntity;
      var invocation = statement.Expression as InvocationExpressionEntity;
      var memberAccess_M1 = invocation.ChildExpression as MemberAccessExpressionEntity;
      var result = memberAccess_M1.ExpressionResult as MethodGroupExpressionResult;
      (result.InstanceExpression as VariableExpressionResult).Type.ShouldEqual(class_A);
      (result.InstanceExpression as VariableExpressionResult).Variable.ShouldEqual(method_M2.Parameters.ToList()[0]);
      result.MethodGroup.Methods.Count().ShouldEqual(1);
      result.MethodGroup.Methods.ToList()[0].ShouldEqual(method_M1);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a primary member access to an instance property.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_InstanceProperty()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_InstanceProperty.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var method_M = class_A.GetOwnMember<MethodEntity>("M");
      var property_P = class_A.GetOwnMember<PropertyEntity>("P");
      var statement = method_M.Body.Statements.ToList()[0] as LocalVariableDeclarationStatementEntity;
      var variable_x = statement.LocalVariables.ToList()[0];
      var memberAccess_P = (variable_x.Initializer as ScalarInitializerEntity).Expression as MemberAccessExpressionEntity;
      var result = memberAccess_P.ExpressionResult as PropertyAccessExpressionResult;
      (result.InstanceExpression as VariableExpressionResult).Type.ShouldEqual(class_A);
      (result.InstanceExpression as VariableExpressionResult).Variable.ShouldEqual(method_M.Parameters.ToList()[0]);
      result.PropertyEntity.ShouldEqual(property_P);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a primary member access to an instance property via another instance property.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_InstancePropertyDereference()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_InstancePropertyDereference.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var method_M = class_A.GetOwnMember<MethodEntity>("M");
      var property_P1 = class_A.GetOwnMember<PropertyEntity>("P1");
      var property_P2 = class_A.GetOwnMember<PropertyEntity>("P2");
      var statement = method_M.Body.Statements.ToList()[0] as LocalVariableDeclarationStatementEntity;
      var variable_x = statement.LocalVariables.ToList()[0];
      var memberAccess_P2 = (variable_x.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
      var memberAccess_P1 = memberAccess_P2.ChildExpression as PrimaryMemberAccessExpressionEntity;
      var result_P1 = memberAccess_P1.ExpressionResult as PropertyAccessExpressionResult;
      (result_P1.InstanceExpression as VariableExpressionResult).Type.ShouldEqual(class_A);
      (result_P1.InstanceExpression as VariableExpressionResult).Variable.ShouldEqual(method_M.Parameters.ToList()[0]);
      result_P1.PropertyEntity.ShouldEqual(property_P1);
      var result_P2 = memberAccess_P2.ExpressionResult as PropertyAccessExpressionResult;
      (result_P2.InstanceExpression as ValueExpressionResult).Type.ShouldEqual(class_A);
      (result_P2.InstanceExpression as ValueExpressionResult).Entity.ShouldEqual(property_P1);
      result_P2.PropertyEntity.ShouldEqual(property_P2);
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
        var class_int = project.SemanticGraph.GlobalNamespace.GetChildNamespace("System").GetSingleChildType<StructEntity>("Int32");
        var constant_MaxValue = class_int.GetMember<ConstantMemberEntity>("MaxValue");

        var field_a = class_A.GetOwnMember<FieldEntity>("a");
        var memberAccess = (field_a.Initializer as ScalarInitializerEntity).Expression as PredefinedTypeMemberAccessExpressionEntity;

        memberAccess.PredefinedTypeEntity.ToString().ShouldEqual("global::System.Int32");
        (memberAccess.ExpressionResult as ValueExpressionResult).Type.ToString().ShouldEqual("global::System.Int32");
        (memberAccess.ExpressionResult as ValueExpressionResult).Entity.ShouldEqual(constant_MaxValue);
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name with a type parameter in it.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PrimaryMemberAccess_WithTypeParameter()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\PrimaryMemberAccess_WithTypeParameter.cs");
      InvokeParser(project).ShouldBeTrue();

      // public static T2 b = A<T2>.t;
      {
        var class_B = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("B", 1);
        var field_b = class_B.GetMember<FieldEntity>("b");
        field_b.Type.ToString().ShouldEqual("global::B`1.T2");
        var memberAccess_t = (field_b.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
        var result_t = memberAccess_t.ExpressionResult as VariableExpressionResult;
        result_t.Type.ToString().ShouldEqual("global::B`1.T2");
        result_t.Variable.ToString().ShouldEqual("global::A`1[global::B`1.T2]_t");
        var simpleName_A_T2 = memberAccess_t.ChildExpression as SimpleNameExpressionEntity;
        var result_A_T2 = simpleName_A_T2.ExpressionResult as TypeExpressionResult;
        result_A_T2.Type.ToString().ShouldEqual("global::A`1[global::B`1.T2]");
      }

      // class C: B<int>
      {
        var class_C = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("C");
        var field_b = class_C.GetMember<FieldEntity>("b");
        field_b.Type.ToString().ShouldEqual("global::System.Int32");
        var memberAccess_t = (field_b.Initializer as ScalarInitializerEntity).Expression as PrimaryMemberAccessExpressionEntity;
        var result_t = memberAccess_t.ExpressionResult as VariableExpressionResult;
        result_t.Type.ToString().ShouldEqual("global::System.Int32");
        result_t.Variable.ToString().ShouldEqual("global::A`1[global::System.Int32]_t");
        var simpleName_A_int = memberAccess_t.ChildExpression as SimpleNameExpressionEntity;
        var result_A_int = simpleName_A_int.ExpressionResult as TypeExpressionResult;
        result_A_int.Type.ToString().ShouldEqual("global::A`1[global::System.Int32]");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a qualified alias member access expresssionwith a type parameter in it.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void QualifiedAliasMemberAccess_WithTypeParameter()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberAccessResolution\QualifiedAliasMemberAccess_WithTypeParameter.cs");
      InvokeParser(project).ShouldBeTrue();

      // public static T2 b = global::A<T2>.t;
      {
        var class_B = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("B", 1);
        var field_b = class_B.GetMember<FieldEntity>("b");
        field_b.Type.ToString().ShouldEqual("global::B`1.T2");
        var memberAccess_t = (field_b.Initializer as ScalarInitializerEntity).Expression as QualifiedAliasMemberAccessExpressionEntity;
        var result_qualifiedAliasMember = memberAccess_t.QualifiedAliasMemberNodeResolver.Target as TypeExpressionResult;
        result_qualifiedAliasMember.Type.ToString().ShouldEqual("global::A`1[global::B`1.T2]");
        var result_t = memberAccess_t.ExpressionResult as VariableExpressionResult;
        result_t.Type.ToString().ShouldEqual("global::B`1.T2");
        result_t.Variable.ToString().ShouldEqual("global::A`1[global::B`1.T2]_t");
      }

      // class C: B<int>
      {
        var class_C = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("C");
        var field_b = class_C.GetMember<FieldEntity>("b");
        field_b.Type.ToString().ShouldEqual("global::System.Int32");
        var memberAccess_t = (field_b.Initializer as ScalarInitializerEntity).Expression as QualifiedAliasMemberAccessExpressionEntity;
        var result_qualifiedAliasMember = memberAccess_t.QualifiedAliasMemberNodeResolver.Target as TypeExpressionResult;
        result_qualifiedAliasMember.Type.ToString().ShouldEqual("global::A`1[global::System.Int32]");
        var result_t = memberAccess_t.ExpressionResult as VariableExpressionResult;
        result_t.Type.ToString().ShouldEqual("global::System.Int32");
        result_t.Variable.ToString().ShouldEqual("global::A`1[global::System.Int32]_t");
      }
    }
  }
}