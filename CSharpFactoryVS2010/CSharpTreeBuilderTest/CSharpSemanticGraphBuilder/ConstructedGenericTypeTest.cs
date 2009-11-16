using System.Linq;
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
      classA.HasGenericTemplate.ShouldBeFalse();
      classA.DirectGenericTemplate.ShouldBeNull();
      classA.UnboundGenericTemplate.ShouldBeNull();
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
      classB.HasGenericTemplate.ShouldBeFalse();
      classB.DirectGenericTemplate.ShouldBeNull();
      classB.UnboundGenericTemplate.ShouldBeNull();

      // public A<int> b;
      var fieldB = classB.GetMember<FieldEntity>("b");
      fieldB.TypeReference.TargetEntity.ToString().ShouldEqual("global::A`1[global::System.Int32]");

      // Constructed type: A<int>
      var constructedA = fieldB.Type;
      constructedA.ShouldEqual(fieldB.TypeReference.TargetEntity);
      constructedA.IsGeneric.ShouldBeTrue();
      constructedA.IsUnboundGeneric.ShouldBeFalse();
      constructedA.IsOpen.ShouldBeFalse();
      constructedA.HasGenericTemplate.ShouldBeTrue();
      constructedA.DirectGenericTemplate.ShouldEqual(classA);
      constructedA.UnboundGenericTemplate.ShouldEqual(classA);
      {
        var typeParameterMap = constructedA.TypeParameterMap;
        typeParameterMap.Count.ShouldEqual(1);
        typeParameterMap.TypeParameters.ToList()[0].ToString().ShouldEqual("global::A`1.T1");
        typeParameterMap.TypeArguments.ToList()[0].ToString().ShouldEqual("global::System.Int32");
      }

      // A<int> --> public T1 a1;
      {
        var field = constructedA.GetMember<FieldEntity>("a1");
        // The field inherits the TPM of the constructed class, but it's not the same TPM object.
        tpmComparer.Equals(field.TypeParameterMap, constructedA.TypeParameterMap).ShouldBeTrue();
        field.TypeParameterMap.ShouldNotEqual(constructedA.TypeParameterMap);
        // The type of the field is resolved to T1 ...
        field.TypeReference.TargetEntity.ToString().ShouldEqual("global::A`1.T1");
        // ... but it returns the mapped type parameter: T1 -> int
        field.Type.ToString().ShouldEqual("global::System.Int32");
      }
      // A<int> --> public T1[] a2;
      {
        var field = constructedA.GetMember<FieldEntity>("a2");
        var templateType = field.TypeReference.TargetEntity;
        templateType.ToString().ShouldEqual("global::A`1.T1[]");
        templateType.IsGeneric.ShouldBeFalse();
        templateType.IsUnboundGeneric.ShouldBeFalse();
        templateType.IsOpen.ShouldBeTrue();
        templateType.HasGenericTemplate.ShouldBeFalse();
        var resolvedType = field.Type;
        resolvedType.ToString().ShouldEqual("global::System.Int32[]");
        resolvedType.IsGeneric.ShouldBeFalse();
        resolvedType.IsUnboundGeneric.ShouldBeFalse();
        resolvedType.IsOpen.ShouldBeFalse();
        resolvedType.HasGenericTemplate.ShouldBeFalse();
      }
      // A<int> --> public A<T1> a3;
      {
        var field = constructedA.GetMember<FieldEntity>("a3");
        var templateType = field.TypeReference.TargetEntity;
        templateType.ToString().ShouldEqual("global::A`1[global::A`1.T1]");
        templateType.IsGeneric.ShouldBeTrue();
        templateType.IsUnboundGeneric.ShouldBeFalse();
        templateType.IsOpen.ShouldBeTrue();
        templateType.HasGenericTemplate.ShouldBeTrue();
        templateType.DirectGenericTemplate.ShouldEqual(classA);
        templateType.UnboundGenericTemplate.ShouldEqual(classA);
        var resolvedType = field.Type;
        resolvedType.ToString().ShouldEqual("global::A`1[global::System.Int32]");
        resolvedType.IsGeneric.ShouldBeTrue();
        resolvedType.IsUnboundGeneric.ShouldBeFalse();
        resolvedType.IsOpen.ShouldBeFalse();
        resolvedType.HasGenericTemplate.ShouldBeTrue();
        resolvedType.DirectGenericTemplate.ShouldEqual(classA);
        resolvedType.UnboundGenericTemplate.ShouldEqual(classA);
      }
      // A<int> --> public A<T1[]> a4;
      {
        var field = constructedA.GetMember<FieldEntity>("a4");
        field.TypeReference.TargetEntity.ToString().ShouldEqual("global::A`1[global::A`1.T1[]]");
        field.Type.ToString().ShouldEqual("global::A`1[global::System.Int32[]]");
      }
      // A<int> --> public A<A<T1>> a5;
      {
        var field = constructedA.GetMember<FieldEntity>("a5");
        field.TypeReference.TargetEntity.ToString().ShouldEqual("global::A`1[global::A`1[global::A`1.T1]]");
        field.TypeReference.TargetEntity.IsOpen.ShouldBeTrue();
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
      classA.HasGenericTemplate.ShouldBeFalse();
      classA.DirectGenericTemplate.ShouldBeNull();
      classA.UnboundGenericTemplate.ShouldBeNull();
      classA.TypeParameterMap.IsEmpty.ShouldBeTrue();

      // declaration of: public class B<T1>
      var classB = classA.GetSingleChildType<ClassEntity>("B", 1);
      classB.IsGeneric.ShouldBeTrue();
      classB.IsUnboundGeneric.ShouldBeTrue();
      classB.IsOpen.ShouldBeTrue();
      classB.HasGenericTemplate.ShouldBeFalse();
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
      classC.HasGenericTemplate.ShouldBeFalse();
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
      classD.HasGenericTemplate.ShouldBeFalse();
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
      classX.HasGenericTemplate.ShouldBeFalse();
      {
        var typeParameterMap = classX.TypeParameterMap;
        typeParameterMap.Count.ShouldEqual(1);
        var typeParameters = typeParameterMap.TypeParameters.ToList();
        typeParameters[0].ToString().ShouldEqual("global::X`1.T3");
        var typeArguments = typeParameterMap.TypeArguments.ToList();
        typeArguments[0].ShouldBeNull();
      }

      // public A.B<int>.C.D<long> x1;
      var fieldX1 = classX.GetMember<FieldEntity>("x1");
      {
        var type = fieldX1.Type;
        type.ToString().ShouldEqual("global::A+B`1[global::System.Int32]+C+D`1[global::System.Int64]");
        type.IsGeneric.ShouldBeTrue();
        type.IsUnboundGeneric.ShouldBeFalse();
        type.IsOpen.ShouldBeFalse();
        type.HasGenericTemplate.ShouldBeTrue();
        type.UnboundGenericTemplate.ShouldEqual(classD);
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
      var fieldX2 = classX.GetMember<FieldEntity>("x2");
      {
        var type = fieldX2.Type;
        type.ToString().ShouldEqual("global::A+B`1[global::System.Int32]+C+D`1[global::X`1.T3]");
        type.IsGeneric.ShouldBeTrue();
        type.IsUnboundGeneric.ShouldBeFalse();
        type.IsOpen.ShouldBeTrue();
        type.HasGenericTemplate.ShouldBeTrue();
        type.UnboundGenericTemplate.ShouldEqual(classD);
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
  }
}