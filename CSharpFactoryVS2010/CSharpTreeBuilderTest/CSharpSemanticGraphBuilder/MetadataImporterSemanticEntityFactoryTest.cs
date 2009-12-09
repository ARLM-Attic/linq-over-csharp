using System;
using System.Linq;
using System.Reflection;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the class that creates semantic entities from assembly metadata.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class MetadataImporterSemanticEntityFactoryTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the metadata import to an empty semantic graph.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ImportToEmptySemanticGraph()
    {
      var project = new CSharpProject(WorkingFolder);
      var factory = new MetadataImporterSemanticEntityFactory(project, project.SemanticGraph);
      factory.ImportTypesIntoSemanticGraph(TestAssemblyPathAndFilename, "global");

      CheckTestAssemblyImportResult(project.SemanticGraph, "global");

      // check namespace A
      project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].IsDeclaredInSource.ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks if MetadataImportTestSubject.dll content exists in the semantic graph.
    /// </summary>
    /// <param name="semanticGraph">The semantic graph to be checked.</param>
    /// <param name="rootName">The name of the root namespace that contains the entities to be checked.</param>
    // ----------------------------------------------------------------------------------------------
    private static void CheckTestAssemblyImportResult(SemanticGraph semanticGraph, string rootName)
    {
      var rootNamespace = semanticGraph.GetRootNamespaceByName(rootName);

      rootNamespace.ShouldNotBeNull();

      // public class Class0 : BaseClass
      {
        var entity = rootNamespace.GetSingleChildType<ClassEntity>("Class0");
        entity.FullyQualifiedName.ShouldEqual("Class0");
        entity.ToString().ShouldEqual(rootName + "::Class0");
        entity.Parent.ShouldEqual(rootNamespace);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
        entity.Program.TargetAssemblyName.Name.ShouldEqual(TestAssemblyName);

        // public const string a1 = "a1";
        {
          var member = entity.GetOwnMember<ConstantMemberEntity>("a1");
          member.FullyQualifiedName.ShouldEqual("Class0.a1");
          member.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Public);
          member.IsDeclaredInSource.ShouldBeFalse();
          member.IsNew.ShouldBeFalse();
          member.IsStatic.ShouldBeTrue();
          member.Name.ShouldEqual("a1");
          member.Parent.ShouldEqual(entity);
          member.Program.TargetAssemblyName.Name.ShouldEqual(TestAssemblyName);
          (member.ReflectedMetadata as FieldInfo).Name.ShouldEqual("a1");
          member.SyntaxNodes.Count.ShouldEqual(0);
          member.Type.ShouldBeNull();
          member.TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        }
        // internal static string a2;
        {
          var member = entity.GetOwnMember<FieldEntity>("a2");
          member.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Assembly);
          member.IsStatic.ShouldBeTrue();
        }
        // protected string a3;
        {
          var member = entity.GetOwnMember<FieldEntity>("a3");
          member.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Family);
          member.IsStatic.ShouldBeFalse();
        }
        // protected internal string a4;
        {
          var member = entity.GetOwnMember<FieldEntity>("a4");
          member.DeclaredAccessibility.ShouldEqual(AccessibilityKind.FamilyOrAssembly);
        }
        // private string a5;
        {
          var member = entity.GetOwnMember<FieldEntity>("a5");
          member.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Private);
        }

        // public int P1 { get; set; }
        {
          var member = entity.GetOwnMember<PropertyEntity>("P1");
          member.FullyQualifiedName.ShouldEqual("Class0.P1");
          member.AutoImplementedField.ShouldBeNull();
          member.DeclaredAccessibility.ShouldBeNull();
          member.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Private);
          member.IsAutoImplemented.ShouldBeFalse();
          member.IsNew.ShouldBeFalse();
          member.IsStatic.ShouldBeFalse();
          member.IsOverride.ShouldBeFalse();
          member.IsVirtual.ShouldBeFalse();
          member.IsSealed.ShouldBeFalse();

          var getAccessor = member.GetAccessor;
          getAccessor.Name.ShouldEqual("get_P1");
          getAccessor.FullyQualifiedName.ShouldEqual("Class0.get_P1");
          getAccessor.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Public);
          getAccessor.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
          getAccessor.IsAbstract.ShouldBeFalse();

          var setAccessor = member.SetAccessor;
          setAccessor.Name.ShouldEqual("set_P1");
          setAccessor.FullyQualifiedName.ShouldEqual("Class0.set_P1");
          setAccessor.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Public);
          setAccessor.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
          setAccessor.IsAbstract.ShouldBeFalse();
        }

        // TODO: test explicitly implemented interface members (not yet handled)
        // int Interface0<Class0>.P1 { get; set; }

        // static int P2 { get; set; }
        {
          var member = entity.GetOwnMember<PropertyEntity>("P2");
          member.IsStatic.ShouldBeTrue();
          member.IsOverride.ShouldBeFalse();
          member.IsVirtual.ShouldBeFalse();
          member.IsSealed.ShouldBeFalse();
        }
        // public virtual int P3 { get; private set; }
        {
          var member = entity.GetOwnMember<PropertyEntity>("P3");
          member.IsStatic.ShouldBeFalse();
          member.IsOverride.ShouldBeFalse();
          member.IsVirtual.ShouldBeTrue();
          member.IsSealed.ShouldBeFalse();

          var setAccessor = member.SetAccessor;
          setAccessor.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Private);
          setAccessor.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Private);
          setAccessor.IsAbstract.ShouldBeFalse();
        }
        // public override int P4 { get { return 0; } }
        {
          var member = entity.GetOwnMember<PropertyEntity>("P4");
          member.IsStatic.ShouldBeFalse();
          member.IsOverride.ShouldBeTrue();
          member.IsVirtual.ShouldBeFalse();
          member.IsSealed.ShouldBeFalse();

          member.GetAccessor.ShouldNotBeNull();
          member.SetAccessor.ShouldBeNull();
        }
        // public override sealed int P5 { set { } }
        {
          var member = entity.GetOwnMember<PropertyEntity>("P5");
          member.IsStatic.ShouldBeFalse();
          member.IsOverride.ShouldBeTrue();
          member.IsVirtual.ShouldBeFalse();
          member.IsSealed.ShouldBeTrue();

          member.GetAccessor.ShouldBeNull();
          member.SetAccessor.ShouldNotBeNull();
        }

        // public void M1<T1, T2, T3>(int a, ref int b, out int c)
        //   where T1 : PublicClass, A.B.IInterface1, new()
        //   where T2 : class
        //   where T3 : struct
        {
          var member = (from m in entity.OwnMembers
                        where m.Name == "M1"
                        && m is MethodEntity
                        && (m as MethodEntity).OwnTypeParameterCount == 3
                        select m).Cast<MethodEntity>().FirstOrDefault();
          member.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Public);
          member.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
          member.FullyQualifiedName.ShouldEqual("Class0.M1");
          member.IsAbstract.ShouldBeFalse();
          member.IsStatic.ShouldBeFalse();
          member.IsOverride.ShouldBeFalse();
          member.IsVirtual.ShouldBeFalse();
          member.IsSealed.ShouldBeFalse();
          member.IsDeclaredInSource.ShouldBeFalse();
          member.IsExplicitlyImplemented.ShouldBeFalse();
          member.IsGeneric.ShouldBeTrue();
          member.IsNew.ShouldBeFalse();
          member.IsPartial.ShouldBeFalse();
          member.Name.ShouldEqual("M1");
          member.OwnTypeParameterCount.ShouldEqual(3);
          (member.ReflectedMetadata as MethodInfo).Name.ShouldEqual("M1");
          member.ReturnTypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);

          var typeParameters = member.OwnTypeParameters.ToList();
          {
            var typeParam = typeParameters.Where(x => x.Name == "T1").FirstOrDefault();
            typeParam.TypeReferenceConstraints.Count().ShouldEqual(2);
            typeParam.HasDefaultConstructorConstraint.ShouldBeTrue();
            typeParam.HasNonNullableValueTypeConstraint.ShouldBeFalse();
            typeParam.HasReferenceTypeConstraint.ShouldBeFalse();
            typeParam.DeclaredAccessibility.ShouldBeNull();
            typeParam.EffectiveAccessibility.ShouldBeNull();
            typeParam.FullyQualifiedName.ShouldEqual("T1");
            typeParam.ToString().ShouldEqual("T1");
            (typeParam.ReflectedMetadata as Type).Name .ShouldEqual("T1");
          }
          {
            var typeParam = typeParameters.Where(x => x.Name == "T2").FirstOrDefault();
            typeParam.TypeReferenceConstraints.Count().ShouldEqual(0);
            typeParam.HasDefaultConstructorConstraint.ShouldBeFalse();
            typeParam.HasNonNullableValueTypeConstraint.ShouldBeFalse();
            typeParam.HasReferenceTypeConstraint.ShouldBeTrue();
          }
          {
            var typeParam = typeParameters.Where(x => x.Name == "T3").FirstOrDefault();
            typeParam.TypeReferenceConstraints.Count().ShouldEqual(0);
            typeParam.HasDefaultConstructorConstraint.ShouldBeTrue();
            typeParam.HasNonNullableValueTypeConstraint.ShouldBeTrue();
            typeParam.HasReferenceTypeConstraint.ShouldBeFalse();
          }

          var parameters = member.Parameters.ToList();
          {
            var param = parameters.Where(x => x.Name == "a").FirstOrDefault();
            param.Name.ShouldEqual("a");
            param.FullyQualifiedName.ShouldEqual("a");
            param.Kind.ShouldEqual(ParameterKind.Value);
            (param.ReflectedMetadata as ParameterInfo).Name.ShouldEqual("a");
            param.TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
          }
          {
            var param = parameters.Where(x => x.Name == "b").FirstOrDefault();
            param.Kind.ShouldEqual(ParameterKind.Reference);
          }
          {
            var param = parameters.Where(x => x.Name == "c").FirstOrDefault();
            param.Kind.ShouldEqual(ParameterKind.Output);
          }
        }

        // public override void M2() { }
        {
          var member = entity.GetOwnMethod("M2", 0, null);
          member.IsAbstract.ShouldBeFalse();
          member.IsStatic.ShouldBeFalse();
          member.IsOverride.ShouldBeTrue();
          member.IsVirtual.ShouldBeFalse();
          member.IsSealed.ShouldBeFalse();
        }
        // public override sealed void M3() { }
        {
          var member = entity.GetOwnMethod("M3", 0, null);
          member.IsAbstract.ShouldBeFalse();
          member.IsStatic.ShouldBeFalse();
          member.IsOverride.ShouldBeTrue();
          member.IsVirtual.ShouldBeFalse();
          member.IsSealed.ShouldBeTrue();
        }
        // public static void M4() { }
        {
          var member = entity.GetOwnMethod("M4", 0, null);
          member.IsAbstract.ShouldBeFalse();
          member.IsStatic.ShouldBeTrue();
          member.IsOverride.ShouldBeFalse();
          member.IsVirtual.ShouldBeFalse();
          member.IsSealed.ShouldBeFalse();
        }

      }

      // public abstract class BaseClass
      {
        var entity = rootNamespace.GetSingleChildType<ClassEntity>("BaseClass");

        // public abstract int P6 { get; set; }
        {
          var member = entity.GetOwnMember<PropertyEntity>("P6");
          member.GetAccessor.IsAbstract.ShouldBeTrue();
          member.SetAccessor.IsAbstract.ShouldBeTrue();
        }
        // public virtual void M2() { }
        {
          var member = entity.GetOwnMethod("M2", 0, null);
          member.IsAbstract.ShouldBeFalse();
          member.IsStatic.ShouldBeFalse();
          member.IsOverride.ShouldBeFalse();
          member.IsVirtual.ShouldBeTrue();
          member.IsSealed.ShouldBeFalse();
        }
        // public abstract void M3();
        {
          var member = entity.GetOwnMethod("M3", 0, null);
          member.IsAbstract.ShouldBeTrue();
          member.IsStatic.ShouldBeFalse();
          member.IsOverride.ShouldBeFalse();
          member.IsVirtual.ShouldBeTrue();
          member.IsSealed.ShouldBeFalse();
        }
      }

      // namespace A
      {
        var entity = rootNamespace.GetChildNamespace("A");
        entity.Name.ShouldEqual("A");
        entity.FullyQualifiedName.ShouldEqual("A");
        entity.ToString().ShouldEqual(rootName + "::A");
      }
      // namespace B
      {
        var entity = rootNamespace.GetChildNamespace("A").GetChildNamespace("B");
        entity.Name.ShouldEqual("B");
        entity.FullyQualifiedName.ShouldEqual("A.B");
        entity.ToString().ShouldEqual(rootName + "::A.B");
        entity.IsDeclaredInSource.ShouldBeFalse();
      }

      var namespaceAB = rootNamespace.GetChildNamespace("A").GetChildNamespace("B");

      // class Class1
      {
        var entity = namespaceAB.GetSingleChildType<ClassEntity>("Class1");
        entity.Name.ShouldEqual("Class1");
        entity.FullyQualifiedName.ShouldEqual("A.B.Class1");
        entity.ToString().ShouldEqual(rootName + "::A.B.Class1");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
      }
      // class Class1.SubClass1
      {
        var entity = namespaceAB.GetSingleChildType<ClassEntity>("Class1").GetSingleChildType<ClassEntity>("SubClass1");
        entity.Name.ShouldEqual("SubClass1");
        entity.FullyQualifiedName.ShouldEqual("A.B.Class1.SubClass1");
        entity.ToString().ShouldEqual(rootName + "::A.B.Class1+SubClass1");
        entity.Parent.ShouldEqual(namespaceAB.GetSingleChildType<ClassEntity>("Class1"));
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
      }
      // enum Enum1
      {
        var entity = namespaceAB.GetSingleChildType<EnumEntity>("Enum1");
        entity.Name.ShouldEqual("Enum1");
        entity.FullyQualifiedName.ShouldEqual("A.B.Enum1");
        entity.ToString().ShouldEqual(rootName + "::A.B.Enum1");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);

        // EnumValue1,
        {
          var member = entity.GetOwnMember<EnumMemberEntity>("EnumValue1");
          member.DeclaredAccessibility.ShouldBeNull();
          member.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
          member.IsDeclaredInSource.ShouldBeFalse();
          member.IsNew.ShouldBeFalse();
          member.IsStatic.ShouldBeTrue();
          member.Name.ShouldEqual("EnumValue1");
          member.Parent.ShouldEqual(entity);
          member.Program.TargetAssemblyName.Name.ShouldEqual(TestAssemblyName);
          (member.ReflectedMetadata as FieldInfo).Name.ShouldEqual("EnumValue1");
          member.SyntaxNodes.Count.ShouldEqual(0);
          member.Type.ShouldBeNull();
          member.TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        }

      }
      // struct Struct1
      {
        var entity = namespaceAB.GetSingleChildType<StructEntity>("Struct1");
        entity.Name.ShouldEqual("Struct1");
        entity.FullyQualifiedName.ShouldEqual("A.B.Struct1");
        entity.ToString().ShouldEqual(rootName + "::A.B.Struct1");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
      }
      // interface IInterface1
      {
        var entity = namespaceAB.GetSingleChildType<InterfaceEntity>("IInterface1");
        entity.Name.ShouldEqual("IInterface1");
        entity.FullyQualifiedName.ShouldEqual("A.B.IInterface1");
        entity.ToString().ShouldEqual(rootName + "::A.B.IInterface1");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
      }
      // delegate void Delegate1();
      {
        var entity = namespaceAB.GetSingleChildType<DelegateEntity>("Delegate1");
        entity.Name.ShouldEqual("Delegate1");
        entity.FullyQualifiedName.ShouldEqual("A.B.Delegate1");
        entity.ToString().ShouldEqual(rootName + "::A.B.Delegate1");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
      }
      // class Generic1<T1,T2>
      {
        var entity = namespaceAB.GetSingleChildType<ClassEntity>("Generic1", 2);
        entity.Name.ShouldEqual("Generic1");
        entity.FullyQualifiedName.ShouldEqual("A.B.Generic1");
        entity.ToString().ShouldEqual(rootName + "::A.B.Generic1`2");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeTrue();
        entity.GetOwnTypeParameterByName("T1").ToString().ShouldEqual(rootName + "::A.B.Generic1`2.T1");
        entity.GetOwnTypeParameterByName("T2").ToString().ShouldEqual(rootName + "::A.B.Generic1`2.T2");
      }

      // public class PublicClass
      {
        var classEntity = rootNamespace.GetSingleChildType<ClassEntity>("PublicClass");
        classEntity.FullyQualifiedName.ShouldEqual("PublicClass");
        classEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
        (classEntity.ReflectedMetadata as Type).Name.ShouldEqual("PublicClass");

        // public class PublicNestedClass
        {
          var nestedClass = classEntity.GetSingleChildType<ClassEntity>("PublicNestedClass");
          nestedClass.FullyQualifiedName.ShouldEqual("PublicClass.PublicNestedClass");
          nestedClass.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
          (nestedClass.ReflectedMetadata as Type).Name.ShouldEqual("PublicNestedClass");
        }
        // public class InternalNestedClass
        {
          var nestedClass = classEntity.GetSingleChildType<ClassEntity>("InternalNestedClass");
          nestedClass.FullyQualifiedName.ShouldEqual("PublicClass.InternalNestedClass");
          nestedClass.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Assembly);
          (nestedClass.ReflectedMetadata as Type).Name.ShouldEqual("InternalNestedClass");
        }
        // public class ProtectedNestedClass
        {
          var nestedClass = classEntity.GetSingleChildType<ClassEntity>("ProtectedNestedClass");
          nestedClass.FullyQualifiedName.ShouldEqual("PublicClass.ProtectedNestedClass");
          nestedClass.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Family);
        }
        // public class ProtectedInternalNestedClass
        {
          var nestedClass = classEntity.GetSingleChildType<ClassEntity>("ProtectedInternalNestedClass");
          nestedClass.FullyQualifiedName.ShouldEqual("PublicClass.ProtectedInternalNestedClass");
          nestedClass.EffectiveAccessibility.ShouldEqual(AccessibilityKind.FamilyOrAssembly);
        }
        // public class PrivateNestedClass
        {
          var nestedClass = classEntity.GetSingleChildType<ClassEntity>("PrivateNestedClass");
          nestedClass.FullyQualifiedName.ShouldEqual("PublicClass.PrivateNestedClass");
          nestedClass.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Private);
        }
      }

      // internal class InternalClass
      {
        var classEntity = rootNamespace.GetSingleChildType<ClassEntity>("InternalClass");
        classEntity.FullyQualifiedName.ShouldEqual("InternalClass");
        classEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Assembly);
      }

      // static class StaticClass
      {
        var classEntity = rootNamespace.GetSingleChildType<ClassEntity>("StaticClass");
        classEntity.FullyQualifiedName.ShouldEqual("StaticClass");
        classEntity.IsStatic.ShouldBeTrue();
      }

      // abstract class AbstractClass
      {
        var classEntity = rootNamespace.GetSingleChildType<ClassEntity>("AbstractClass");
        classEntity.FullyQualifiedName.ShouldEqual("AbstractClass");
        classEntity.IsAbstract.ShouldBeTrue();
      }

      // sealed class SealedClass
      {
        var classEntity = rootNamespace.GetSingleChildType<ClassEntity>("SealedClass");
        classEntity.FullyQualifiedName.ShouldEqual("SealedClass");
        classEntity.IsSealed.ShouldBeTrue();
      }

    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// EntityBuilder already created a namespace that metadata importer also wants to create.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ImportToExistingNamespace()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MetadataImporterSemanticEntityFactory\ImportToExistingNamespace.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      
      // First the entity builder and resolver
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project));
      project.SemanticGraph.AcceptVisitor(new TypeResolverPass1SemanticGraphVisitor(project, project.SemanticGraph));

      // Then the importer
      var factory = new MetadataImporterSemanticEntityFactory(project, project.SemanticGraph);
      factory.ImportTypesIntoSemanticGraph(TestAssemblyPathAndFilename, "global");

      // Check resulting semantic graph
      CheckTestAssemblyImportResult(project.SemanticGraph, "global");

      // check namespace A
      project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].IsDeclaredInSource.ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Metadata importer already created a namespace that entity builder also wants to create.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuildingAnExistingNamespace()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MetadataImporterSemanticEntityFactory\ImportToExistingNamespace.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      
      // First the import
      var factory = new MetadataImporterSemanticEntityFactory(project, project.SemanticGraph);
      factory.ImportTypesIntoSemanticGraph(typeof(int).Assembly.Location, "global");
      factory.ImportTypesIntoSemanticGraph(TestAssemblyPathAndFilename, "global");
      
      // Then the entity builder and resolver
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project));
      project.SemanticGraph.AcceptVisitor(new TypeResolverPass1SemanticGraphVisitor(project, project.SemanticGraph));

      // Check resulting semantic graph
      CheckTestAssemblyImportResult(project.SemanticGraph, "global");

      // check namespace A
      project.SemanticGraph.GlobalNamespace.GetChildNamespace("A").IsDeclaredInSource.ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests error: CS0006_MetadataFileNotFound
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0006_MetadataFileNotFound()
    {
      var project = new CSharpProject(WorkingFolder);
      var semanticGraph = new SemanticGraph(null);
      var factory = new MetadataImporterSemanticEntityFactory(project, semanticGraph);
      factory.ImportTypesIntoSemanticGraph(@"c:\nosuchfile.dll", "global");

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0006");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests error: CS0009_IncorrectFormat
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0009_IncorrectFormat()
    {
      var project = new CSharpProject(WorkingFolder);
      var semanticGraph = new SemanticGraph(null);
      var factory = new MetadataImporterSemanticEntityFactory(project, semanticGraph);
      factory.ImportTypesIntoSemanticGraph(Path.Combine(Environment.SystemDirectory, @"ntdll.dll"), "global");

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0009");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Imports mscorlib.dll
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ImportMscorlib()
    {
      var project = new CSharpProject(WorkingFolder);
      var factory = new MetadataImporterSemanticEntityFactory(project, project.SemanticGraph);
      factory.ImportTypesIntoSemanticGraph(Assembly.GetAssembly(typeof (int)).Location, "global");

      project.Warnings.Count.ShouldEqual(0);
      project.Errors.Count.ShouldEqual(0);

      // check System.object
      var objectEntity = project.SemanticGraph.GetEntityByMetadataObject(typeof(System.Object)) as ClassEntity;
      objectEntity.Name.ShouldEqual("Object");
      objectEntity.FullyQualifiedName.ShouldEqual("System.Object");
      objectEntity.ReflectedMetadata.ShouldEqual(typeof (object));

      // check System.Nullable`1
      // It's not a reference to be resolved, but a lookup in metadata map, so it is already populated
      project.SemanticGraph.NullableGenericTypeDefinition.ToString().ShouldEqual("global::System.Nullable`1");
      project.SemanticGraph.NullableGenericTypeDefinition.AllTypeParameterCount.ShouldEqual(1);
      project.SemanticGraph.NullableGenericTypeDefinition.GetOwnTypeParameterByName("T").ToString().ShouldEqual("global::System.Nullable`1.T");
      project.SemanticGraph.NullableGenericTypeDefinition.ReflectedMetadata.ShouldEqual(typeof (System.Nullable<>));

      // check System.Array
      // It's not a reference to be resolved, but a lookup in metadata map, so it is already populated
      project.SemanticGraph.SystemArray.ToString().ShouldEqual("global::System.Array");
      project.SemanticGraph.SystemArray.ReflectedMetadata.ShouldEqual(typeof (System.Array));

      // check BuiltIn types
      project.SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Int).ToString().ShouldEqual("global::System.Int32");
      project.SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Int).ReflectedMetadata.ShouldEqual(typeof (int));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the metadata import with an alias
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ImportWithAlias()
    {
      var project = new CSharpProject(WorkingFolder);
      var factory = new MetadataImporterSemanticEntityFactory(project, project.SemanticGraph);
      factory.ImportTypesIntoSemanticGraph(TestAssemblyPathAndFilename, "MyAlias");

      CheckTestAssemblyImportResult(project.SemanticGraph, "MyAlias");

      project.SemanticGraph.GlobalNamespace.ChildNamespaces.Count.ShouldEqual(0);
      project.SemanticGraph.GlobalNamespace.ChildTypes.Count().ShouldEqual(0);
    }
  }
}
