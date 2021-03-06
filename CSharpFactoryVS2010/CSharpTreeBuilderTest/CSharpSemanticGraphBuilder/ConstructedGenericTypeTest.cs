﻿using System.Linq;
using System.Reflection;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the constructed generic types.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class ConstructedGenericTypeTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Simple generic type with 1 type parameter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Simple()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ConstructedGenericType\Simple.cs");
      InvokeParser(project).ShouldBeTrue();

      var tpmComparer = new TypeParameterMapEqualityComparer();

      // declaration of: public class A<T1> 
      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A", 1);
      classA.IsGeneric.ShouldBeTrue();
      classA.IsUnboundGeneric.ShouldBeTrue();
      classA.IsOpen.ShouldBeTrue();
      classA.IsGenericClone.ShouldBeFalse();
      classA.DirectGenericTemplate.ShouldBeNull();
      classA.OriginalGenericTemplate.ShouldBeNull();
      {
        var typeParameterMap = classA.TypeParameterMap;
        typeParameterMap.Count.ShouldEqual(1);
        var typeParameters = typeParameterMap.TypeParameters.ToList();
        typeParameters[0].ToString().ShouldEqual("global::A`1.T1");
        var typeArguments = typeParameterMap.TypeArguments.ToList();
        typeArguments[0].ShouldBeNull();
      }

      // public class B
      var classB = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("B", 0);
      classB.IsGeneric.ShouldBeFalse();
      classB.IsUnboundGeneric.ShouldBeFalse();
      classB.IsOpen.ShouldBeFalse();
      classB.IsGenericClone.ShouldBeFalse();
      classB.DirectGenericTemplate.ShouldBeNull();
      classB.OriginalGenericTemplate.ShouldBeNull();

      // public A<int> b;
      var fieldB = classB.GetOwnMember<FieldEntity>("b");
      fieldB.TypeReference.Target.ToString().ShouldEqual("global::A`1[global::System.Int32]");

      // Constructed type: A<int>
      var constructedA = fieldB.Type;
      constructedA.ShouldEqual(fieldB.TypeReference.Target);
      constructedA.IsGeneric.ShouldBeTrue();
      constructedA.IsUnboundGeneric.ShouldBeFalse();
      constructedA.IsOpen.ShouldBeFalse();
      constructedA.IsGenericClone.ShouldBeTrue();
      constructedA.DirectGenericTemplate.ShouldEqual(classA);
      constructedA.OriginalGenericTemplate.ShouldEqual(classA);
      {
        var typeParameterMap = constructedA.TypeParameterMap;
        typeParameterMap.Count.ShouldEqual(1);
        typeParameterMap.TypeParameters.ToList()[0].ToString().ShouldEqual("global::A`1.T1");
        typeParameterMap.TypeArguments.ToList()[0].ToString().ShouldEqual("global::System.Int32");
      }

      // A<int> --> public T1 a1;
      {
        var field = constructedA.GetOwnMember<FieldEntity>("a1");
        // The field inherits the TPM of the constructed class, but it's not the same TPM object.
        tpmComparer.Equals(field.TypeParameterMap, constructedA.TypeParameterMap).ShouldBeTrue();
        field.TypeParameterMap.ShouldNotEqual(constructedA.TypeParameterMap);

        // The type resolver object is a clone.
        field.TypeReference.IsGenericClone.ShouldBeTrue();
        // The template of the clone is resolved to "global::A`1.T1"
        (field.TypeReference.OriginalGenericTemplate as Resolver<TypeEntity>).Target.ToString().ShouldEqual("global::A`1.T1");
        // But this cloned type reference is mapped with the T1 -> int mapping.
        field.TypeReference.Target.ToString().ShouldEqual("global::System.Int32");
      }
      // A<int> --> public T1[] a2;
      {
        var field = constructedA.GetOwnMember<FieldEntity>("a2");
        var resolvedType = field.Type;
        resolvedType.ToString().ShouldEqual("global::System.Int32[]");
        resolvedType.IsGeneric.ShouldBeFalse();
        resolvedType.IsUnboundGeneric.ShouldBeFalse();
        resolvedType.IsOpen.ShouldBeFalse();
        resolvedType.IsGenericClone.ShouldBeFalse();
      }
      // A<int> --> public A<T1> a3;
      {
        var field = constructedA.GetOwnMember<FieldEntity>("a3");
        var resolvedType = field.Type;
        resolvedType.ToString().ShouldEqual("global::A`1[global::System.Int32]");
        resolvedType.IsGeneric.ShouldBeTrue();
        resolvedType.IsUnboundGeneric.ShouldBeFalse();
        resolvedType.IsOpen.ShouldBeFalse();
        resolvedType.IsGenericClone.ShouldBeTrue();
        resolvedType.DirectGenericTemplate.ShouldEqual(classA);
        resolvedType.OriginalGenericTemplate.ShouldEqual(classA);
      }
      // A<int> --> public A<T1[]> a4;
      {
        var field = constructedA.GetOwnMember<FieldEntity>("a4");
        field.Type.ToString().ShouldEqual("global::A`1[global::System.Int32[]]");
      }
      // A<int> --> public A<A<T1>> a5;
      {
        var field = constructedA.GetOwnMember<FieldEntity>("a5");
        field.Type.ToString().ShouldEqual("global::A`1[global::A`1[global::System.Int32]]");
        field.Type.IsOpen.ShouldBeFalse();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Embedded generic types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void EmbeddedType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ConstructedGenericType\EmbeddedType.cs");
      InvokeParser(project).ShouldBeTrue();

      var tpmComparer = new TypeParameterMapEqualityComparer();

      // declaration of: public class A
      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A", 0);
      classA.IsGeneric.ShouldBeFalse();
      classA.IsUnboundGeneric.ShouldBeFalse();
      classA.IsOpen.ShouldBeFalse();
      classA.IsGenericClone.ShouldBeFalse();
      classA.DirectGenericTemplate.ShouldBeNull();
      classA.OriginalGenericTemplate.ShouldBeNull();
      classA.TypeParameterMap.IsEmpty.ShouldBeTrue();

      // declaration of: public class B<T1>
      var classB = classA.GetSingleChildType<ClassEntity>("B", 1);
      classB.IsGeneric.ShouldBeTrue();
      classB.IsUnboundGeneric.ShouldBeTrue();
      classB.IsOpen.ShouldBeTrue();
      classB.IsGenericClone.ShouldBeFalse();
      {
        var typeParameterMap = classB.TypeParameterMap;
        typeParameterMap.Count.ShouldEqual(1);
        var typeParameters = typeParameterMap.TypeParameters.ToList();
        typeParameters[0].ToString().ShouldEqual("global::A+B`1.T1");
        var typeArguments = typeParameterMap.TypeArguments.ToList();
        typeArguments[0].ShouldBeNull();
      }

      // declaration of: public class C
      var classC = classB.GetSingleChildType<ClassEntity>("C", 0);
      classC.IsGeneric.ShouldBeTrue();
      classC.IsUnboundGeneric.ShouldBeTrue();
      classC.IsOpen.ShouldBeTrue();
      classC.IsGenericClone.ShouldBeFalse();
      {
        var typeParameterMap = classC.TypeParameterMap;
        typeParameterMap.Count.ShouldEqual(1);
        var typeParameters = typeParameterMap.TypeParameters.ToList();
        typeParameters[0].ToString().ShouldEqual("global::A+B`1.T1");
        var typeArguments = typeParameterMap.TypeArguments.ToList();
        typeArguments[0].ShouldBeNull();
      }

      // declaration of: public class D<T2>
      var classD = classC.GetSingleChildType<ClassEntity>("D", 1);
      classD.IsGeneric.ShouldBeTrue();
      classD.IsUnboundGeneric.ShouldBeTrue();
      classD.IsOpen.ShouldBeTrue();
      classD.IsGenericClone.ShouldBeFalse();
      {
        var typeParameterMap = classD.TypeParameterMap;
        typeParameterMap.Count.ShouldEqual(2);
        var typeParameters = typeParameterMap.TypeParameters.ToList();
        typeParameters[0].ToString().ShouldEqual("global::A+B`1.T1");
        typeParameters[1].ToString().ShouldEqual("global::A+B`1+C+D`1.T2");
        var typeArguments = typeParameterMap.TypeArguments.ToList();
        typeArguments[0].ShouldBeNull();
        typeArguments[1].ShouldBeNull();
      }

      // public class X<T3>
      var classX = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("X", 1);
      classX.IsGeneric.ShouldBeTrue();
      classX.IsUnboundGeneric.ShouldBeTrue();
      classX.IsOpen.ShouldBeTrue();
      classX.IsGenericClone.ShouldBeFalse();
      {
        var typeParameterMap = classX.TypeParameterMap;
        typeParameterMap.Count.ShouldEqual(1);
        var typeParameters = typeParameterMap.TypeParameters.ToList();
        typeParameters[0].ToString().ShouldEqual("global::X`1.T3");
        var typeArguments = typeParameterMap.TypeArguments.ToList();
        typeArguments[0].ShouldBeNull();
      }

      // public A.B<int>.C.D<long> x1;
      var fieldX1 = classX.GetOwnMember<FieldEntity>("x1");
      {
        var type = fieldX1.Type;
        type.ToString().ShouldEqual("global::A+B`1[global::System.Int32]+C+D`1[global::System.Int64]");
        type.IsGeneric.ShouldBeTrue();
        type.IsUnboundGeneric.ShouldBeFalse();
        type.IsOpen.ShouldBeFalse();
        type.IsGenericClone.ShouldBeTrue();
        type.OriginalGenericTemplate.ShouldEqual(classD);
        {
          var typeParameterMap = type.TypeParameterMap;
          typeParameterMap.Count.ShouldEqual(2);
          var typeParameters = typeParameterMap.TypeParameters.ToList();
          typeParameters[0].ToString().ShouldEqual("global::A+B`1.T1");
          typeParameters[1].ToString().ShouldEqual("global::A+B`1+C+D`1.T2");
          var typeArguments = typeParameterMap.TypeArguments.ToList();
          typeArguments[0].ToString().ShouldEqual("global::System.Int32");
          typeArguments[1].ToString().ShouldEqual("global::System.Int64");
        }
      }

      // public A.B<int>.C.D<T3> x2;
      var fieldX2 = classX.GetOwnMember<FieldEntity>("x2");
      {
        var type = fieldX2.Type;
        type.ToString().ShouldEqual("global::A+B`1[global::System.Int32]+C+D`1[global::X`1.T3]");
        type.IsGeneric.ShouldBeTrue();
        type.IsUnboundGeneric.ShouldBeFalse();
        type.IsOpen.ShouldBeTrue();
        type.IsGenericClone.ShouldBeTrue();
        type.OriginalGenericTemplate.ShouldEqual(classD);
        {
          var typeParameterMap = type.TypeParameterMap;
          typeParameterMap.Count.ShouldEqual(2);
          var typeParameters = typeParameterMap.TypeParameters.ToList();
          typeParameters[0].ToString().ShouldEqual("global::A+B`1.T1");
          typeParameters[1].ToString().ShouldEqual("global::A+B`1+C+D`1.T2");
          var typeArguments = typeParameterMap.TypeArguments.ToList();
          typeArguments[0].ToString().ShouldEqual("global::System.Int32");
          typeArguments[1].ToString().ShouldEqual("global::X`1.T3");
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test argument generic cloning.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Argument()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ConstructedGenericType\Argument.cs");
      InvokeParser(project).ShouldBeTrue();

      var class_A = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A", 2);
      var method_D = class_A.GetOwnMember<MethodEntity>("D");
      var statement = method_D.Body.Statements.ToList()[0] as ExpressionStatementEntity;
      var invocation = statement.Expression as InvocationExpressionEntity;
      var memberAccess_M = invocation.ChildExpression as PrimaryMemberAccessExpressionEntity;
      {
        var result = memberAccess_M.ExpressionResult as MethodGroupExpressionResult;
        var instanceExpResult = result.InstanceExpression as VariableExpressionResult;
        instanceExpResult.Variable.ToString().ShouldEqual("global::B`2[global::System.Int32,global::System.Int64]_a");
        instanceExpResult.Type.ToString().ShouldEqual("global::A`2[global::System.Int64,global::System.Int32]");
        var methods = result.MethodGroup.Methods.ToList();
        methods[0].ToString().ShouldEqual("global::A`2[global::System.Int64,global::System.Int32]_M(global::System.Int64, global::System.Int32)");
      }
      var memberAccess_a = memberAccess_M.ChildExpression as PrimaryMemberAccessExpressionEntity;
      {
        var result = memberAccess_a.ExpressionResult as VariableExpressionResult;
        result.Variable.ToString().ShouldEqual("global::B`2[global::System.Int32,global::System.Int64]_a");
        result.Type.ToString().ShouldEqual("global::A`2[global::System.Int64,global::System.Int32]");
      }
      var simpleName_b = memberAccess_a.ChildExpression as SimpleNameExpressionEntity;
      {
        var result = simpleName_b.ExpressionResult as VariableExpressionResult;
        result.Variable.ToString().ShouldEqual("global::A`2_b");
        result.Type.ToString().ShouldEqual("global::B`2[global::System.Int32,global::System.Int64]");
      }
    }
  }
}