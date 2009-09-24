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

      CheckTestAssemblyImportResult(project.SemanticGraph.GlobalNamespace, "global");

      // check namespace A
      project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].IsDeclaredInSource.ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks if MetadataImportTestSubject.dll content exists in the semantic graph.
    /// </summary>
    /// <param name="rootNamespace">The root namespace that contains the entities to be checked.</param>
    // ----------------------------------------------------------------------------------------------
    private static void CheckTestAssemblyImportResult(RootNamespaceEntity rootNamespace, string rootName)
    {
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
          var member = entity.GetMember<ConstantMemberEntity>("a1");
          member.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Public);
          member.IsDeclaredInSource.ShouldBeFalse();
          member.IsNew.ShouldBeFalse();
          member.IsOverride.ShouldBeFalse();
          member.IsStatic.ShouldBeTrue();
          member.IsVirtual.ShouldBeFalse();
          member.Name.ShouldEqual("a1");
          member.Parent.ShouldEqual(entity);
          member.Program.TargetAssemblyName.Name.ShouldEqual(TestAssemblyName);
          member.ReflectedMetadata.Name.ShouldEqual("a1");
          member.SyntaxNodes.Count.ShouldEqual(0);
          member.Type.ShouldBeNull();
          member.TypeReference.ResolutionState.ShouldEqual(ResolutionState.NotYetResolved);
        }
        // internal static string a2;
        {
          var member = entity.GetMember<FieldEntity>("a2");
          member.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Assembly);
          member.IsStatic.ShouldBeTrue();
        }
        // protected string a3;
        {
          var member = entity.GetMember<FieldEntity>("a3");
          member.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Family);
          member.IsStatic.ShouldBeFalse();
        }
        // protected internal string a4;
        {
          var member = entity.GetMember<FieldEntity>("a4");
          member.DeclaredAccessibility.ShouldEqual(AccessibilityKind.FamilyOrAssembly);
        }
        // private string a5;
        {
          var member = entity.GetMember<FieldEntity>("a5");
          member.DeclaredAccessibility.ShouldEqual(AccessibilityKind.Private);
        }

        // public int P1 { get; set; }
        {
          var member = entity.GetMember<PropertyEntity>("P1");
          // member.AutoImplementedField.ShouldNotBeNull();
          member.DeclaredAccessibility.ShouldBeNull();
          member.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Private);
          // member.IsAutoImplemented.ShouldBeTrue();
          //member.GetAccessor.
          //member.SetAccessor.
        }
      }

      // public abstract class BaseClass
      {
        var entity = rootNamespace.GetSingleChildType<ClassEntity>("BaseClass");
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
        entity.OwnTypeParameters[0].ToString().ShouldEqual(rootName + "::A.B.Generic1`2'T1");
        entity.OwnTypeParameters[1].ToString().ShouldEqual(rootName + "::A.B.Generic1`2'T2");
      }

      // public class PublicClass
      {
        var classEntity = rootNamespace.GetSingleChildType<ClassEntity>("PublicClass");
        classEntity.FullyQualifiedName.ShouldEqual("PublicClass");
        classEntity.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
        classEntity.ReflectedMetadata.Name.ShouldEqual("PublicClass");

        // public class PublicNestedClass
        {
          var nestedClass = classEntity.GetSingleChildType<ClassEntity>("PublicNestedClass");
          nestedClass.FullyQualifiedName.ShouldEqual("PublicClass.PublicNestedClass");
          nestedClass.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Public);
          nestedClass.ReflectedMetadata.Name.ShouldEqual("PublicNestedClass");
        }
        // public class InternalNestedClass
        {
          var nestedClass = classEntity.GetSingleChildType<ClassEntity>("InternalNestedClass");
          nestedClass.FullyQualifiedName.ShouldEqual("PublicClass.InternalNestedClass");
          nestedClass.EffectiveAccessibility.ShouldEqual(AccessibilityKind.Assembly);
          nestedClass.ReflectedMetadata.Name.ShouldEqual("InternalNestedClass");
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
      CheckTestAssemblyImportResult(project.SemanticGraph.GlobalNamespace, "global");

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
      factory.ImportTypesIntoSemanticGraph(TestAssemblyPathAndFilename, "global");
      
      // Then the entity builder and resolver
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project));
      project.SemanticGraph.AcceptVisitor(new TypeResolverPass1SemanticGraphVisitor(project, project.SemanticGraph));

      // Check resulting semantic graph
      CheckTestAssemblyImportResult(project.SemanticGraph.GlobalNamespace, "global");

      // check namespace A
      project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].IsDeclaredInSource.ShouldBeTrue();
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
      var semanticGraph = new SemanticGraph();
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
      var semanticGraph = new SemanticGraph();
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
      project.SemanticGraph.NullableGenericTypeDefinition.AllTypeParameters.Count.ShouldEqual(1);
      project.SemanticGraph.NullableGenericTypeDefinition.AllTypeParameters[0].ToString().ShouldEqual("global::System.Nullable`1'T");
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

      CheckTestAssemblyImportResult(project.SemanticGraph.GetRootNamespaceByName("MyAlias"), "MyAlias");

      project.SemanticGraph.GlobalNamespace.ChildNamespaces.Count.ShouldEqual(0);
      project.SemanticGraph.GlobalNamespace.ChildTypes.Count.ShouldEqual(0);
    }
  }
}
