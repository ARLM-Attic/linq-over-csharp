using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using System.IO;

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
      factory.CreateEntitiesFromAssembly(TestAssemblyPathAndFilename, "global");

      CheckTestAssemblyImportResult(project.SemanticGraph.GlobalNamespace);

      // check namespace A
      project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].IsExplicit.ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks if MetadataImportTestSubject.dll content exists in the semantic graph.
    /// </summary>
    /// <param name="rootNamespace">The root namespace that contains the entities to be checked.</param>
    // ----------------------------------------------------------------------------------------------
    private static void CheckTestAssemblyImportResult(RootNamespaceEntity rootNamespace)
    {
      rootNamespace.ShouldNotBeNull();

      // class Class0
      {
        var entity = rootNamespace.ChildTypes[0] as ClassEntity;
        entity.Name.ShouldEqual("Class0");
        entity.DistinctiveName.ShouldEqual("Class0");
        entity.FullyQualifiedName.ShouldEqual("Class0");
        entity.Parent.ShouldEqual(rootNamespace);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
      }
      // namespace A
      {
        var entity = rootNamespace.ChildNamespaces[0];
        entity.Name.ShouldEqual("A");
        entity.DistinctiveName.ShouldEqual("A");
        entity.FullyQualifiedName.ShouldEqual("A");
      }
      // namespace B
      {
        var entity = rootNamespace.ChildNamespaces[0].ChildNamespaces[0];
        entity.Name.ShouldEqual("B");
        entity.DistinctiveName.ShouldEqual("B");
        entity.FullyQualifiedName.ShouldEqual("A.B");
        entity.IsExplicit.ShouldBeFalse();
      }

      var namespaceAB = rootNamespace.ChildNamespaces[0].ChildNamespaces[0];

      // class Class1
      {
        var entity = namespaceAB.ChildTypes[0] as ClassEntity;
        entity.Name.ShouldEqual("Class1");
        entity.DistinctiveName.ShouldEqual("Class1");
        entity.FullyQualifiedName.ShouldEqual("A.B.Class1");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
      }
      // class Class1.SubClass1
      {
        var entity = ((ClassEntity) namespaceAB.ChildTypes[0]).ChildTypes[0] as ClassEntity;
        entity.Name.ShouldEqual("SubClass1");
        entity.DistinctiveName.ShouldEqual("SubClass1");
        entity.FullyQualifiedName.ShouldEqual("A.B.Class1.SubClass1");
        entity.Parent.ShouldEqual(namespaceAB.ChildTypes[0]);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
      }
      // enum Enum1
      {
        var entity = namespaceAB.ChildTypes[1] as EnumEntity;
        entity.Name.ShouldEqual("Enum1");
        entity.DistinctiveName.ShouldEqual("Enum1");
        entity.FullyQualifiedName.ShouldEqual("A.B.Enum1");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);
      }
      // struct Struct1
      {
        var entity = namespaceAB.ChildTypes[2] as StructEntity;
        entity.Name.ShouldEqual("Struct1");
        entity.DistinctiveName.ShouldEqual("Struct1");
        entity.FullyQualifiedName.ShouldEqual("A.B.Struct1");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
      }
      // interface IInterface1
      {
        var entity = namespaceAB.ChildTypes[3] as InterfaceEntity;
        entity.Name.ShouldEqual("IInterface1");
        entity.DistinctiveName.ShouldEqual("IInterface1");
        entity.FullyQualifiedName.ShouldEqual("A.B.IInterface1");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
      }
      // delegate void Delegate1();
      {
        var entity = namespaceAB.ChildTypes[4] as DelegateEntity;
        entity.Name.ShouldEqual("Delegate1");
        entity.DistinctiveName.ShouldEqual("Delegate1");
        entity.FullyQualifiedName.ShouldEqual("A.B.Delegate1");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeFalse();
      }
      // class Generic1<T1,T2>
      {
        var entity = namespaceAB.ChildTypes[5] as ClassEntity;
        entity.Name.ShouldEqual("Generic1");
        entity.DistinctiveName.ShouldEqual("Generic1`2");
        entity.FullyQualifiedName.ShouldEqual("A.B.Generic1`2");
        entity.Parent.ShouldEqual(namespaceAB);
        entity.SyntaxNodes.Count.ShouldEqual(0);
        entity.IsGeneric.ShouldBeTrue();
        entity.OwnTypeParameters[0].FullyQualifiedName.ShouldEqual("A.B.Generic1`2.T1");
        entity.OwnTypeParameters[1].FullyQualifiedName.ShouldEqual("A.B.Generic1`2.T2");
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
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph));
      project.SemanticGraph.AcceptVisitor(new TypeDeclarationResolverSemanticGraphVisitor(project, project.SemanticGraph));

      // Then the importer
      var factory = new MetadataImporterSemanticEntityFactory(project, project.SemanticGraph);
      factory.CreateEntitiesFromAssembly(TestAssemblyPathAndFilename, "global");

      // Check resulting semantic graph
      CheckTestAssemblyImportResult(project.SemanticGraph.GlobalNamespace);

      // check namespace A
      project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].IsExplicit.ShouldBeTrue();
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
      factory.CreateEntitiesFromAssembly(TestAssemblyPathAndFilename, "global");
      
      // Then the entity builder and resolver
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph));
      project.SemanticGraph.AcceptVisitor(new TypeDeclarationResolverSemanticGraphVisitor(project, project.SemanticGraph));

      // Check resulting semantic graph
      CheckTestAssemblyImportResult(project.SemanticGraph.GlobalNamespace);

      // check namespace A
      project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].IsExplicit.ShouldBeTrue();
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
      factory.CreateEntitiesFromAssembly(@"c:\nosuchfile.dll", "global");

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
      factory.CreateEntitiesFromAssembly(Path.Combine(Environment.SystemDirectory, @"ntdll.dll"), "global");

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
      factory.CreateEntitiesFromAssembly(Assembly.GetAssembly(typeof (int)).Location, "global");

      project.Warnings.Count.ShouldEqual(0);
      project.Errors.Count.ShouldEqual(0);

      // check System.object
      var objectEntity = project.SemanticGraph.GetEntityByMetadataObject(typeof(System.Object)) as ClassEntity;
      objectEntity.Name.ShouldEqual("Object");
      objectEntity.FullyQualifiedName.ShouldEqual("System.Object");

      // check System.Nullable`1
      project.SemanticGraph.NullableGenericTypeDefinition.FullyQualifiedName.ShouldEqual("System.Nullable`1");
      project.SemanticGraph.NullableGenericTypeDefinition.AllTypeParameters.Count.ShouldEqual(1);
      project.SemanticGraph.NullableGenericTypeDefinition.AllTypeParameters[0].FullyQualifiedName.ShouldEqual("System.Nullable`1.T");
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
      factory.CreateEntitiesFromAssembly(TestAssemblyPathAndFilename, "MyAlias");

      CheckTestAssemblyImportResult(project.SemanticGraph.GetRootNamespaceByName("MyAlias"));

      project.SemanticGraph.GlobalNamespace.ChildNamespaces.Count.ShouldEqual(0);
      project.SemanticGraph.GlobalNamespace.ChildTypes.Count.ShouldEqual(0);
    }
  }
}
