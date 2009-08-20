﻿using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the type resolution logic of the TypeDeclarationResolverSemanticGraphVisitor class
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class TypeDeclarationResolutionTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of non-generic base types
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NonGenericBaseTypes()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeDeclarationResolution\NonGenericBaseTypes.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph));
      project.SemanticGraph.AcceptVisitor(new TypeDeclarationResolverSemanticGraphVisitor(project, project.SemanticGraph));

      // class A1 : C0
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("C0");
      }
      // class A2 : C0.C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[1].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("C0.C1");
      }
      // class A3 : N1.N1C0
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[2].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N1.N1C0");
      }
      // class A4 : N1.N2.N2C0
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[3].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N1.N2.N2C0");
      }
      // class A5 : N1.N2.N2C0.N2C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[4].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N1.N2.N2C0.N2C1");
      }
      // class N3C1 : N3C2
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N3.N3C2");
      }
      // class N5C1 : N4C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N4C1");
      }
      // class N5C2 : N6.N6C1
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N6.N6C1");
      }
      // class N5C2C1 : N5C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N5.N5C1");
      }
      // class N5C2C2 : N4C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[1].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N4C1");
      }
      // class N5C2C3 : N5C2C1
      {
        var baseTypeRef = ((ClassEntity)project.SemanticGraph.GlobalNamespace.ChildNamespaces[1].ChildNamespaces[0].ChildTypes[1])
          .ChildTypes[2].BaseTypeReferences.ToArray()[0];
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("N4.N5.N5C2.N5C2C1");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of generic base types
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void GenericBaseTypes()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeDeclarationResolution\GenericBaseTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      // class A1<T1> : A3<T1, A2<T1>>
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("A3`2[A1`1.T1,A2`1[A1`1.T1]]");
      }
      // class A2<T2> : A3<int, long>
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("A3`2[int,long]");
      }
      // class A3<T3, T4>
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("System.Object");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of built-in base types
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void BuiltInBaseTypes()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeDeclarationResolution\BuiltInBaseTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      // class A1 : object
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[0].BaseTypeReferences.ToArray()[0];
        baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("object");
        var aliasedTypeRef = ((BuiltInTypeEntity) baseTypeRef.TargetEntity).AliasedTypeReference;
        aliasedTypeRef.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        aliasedTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("System.Object");
        ((BuiltInTypeEntity)baseTypeRef.TargetEntity).AliasedType.FullyQualifiedName.ShouldEqual("System.Object");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of reflected base types
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ReflectedBaseTypes()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeDeclarationResolution\ReflectedBaseTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      // class A1 : System.Object
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseTypeReferences.ToArray()[0];
        baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("System.Object");
      }
      // class A2 : System.Collections.Generic.Dictionary<int,long>
      {
        var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseTypeReferences.ToArray()[0];
        baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        baseTypeRef.TargetEntity.FullyQualifiedName.ShouldEqual("System.Collections.Generic.Dictionary`2[int,long]");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of implicit base types
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ImplicitBaseTypes()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeDeclarationResolution\ImplicitBaseTypes.cs");
      InvokeParser(project).ShouldBeTrue();

      int i = 0;

      // class A1 // implicitly : System.Object
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("System.Object");
      }
      // struct A2 // implicitly: System.ValueType
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("System.ValueType");
      }
      // enum A3 // implicitly: System.Enum
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("System.Enum");
      }
      // delegate void A4(); // implicitly: System.MulticastDelegate
      {
        project.SemanticGraph.GlobalNamespace.ChildTypes[i++].BaseType.FullyQualifiedName.ShouldEqual("System.MulticastDelegate");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of base types of types imported from mscorlib.dll
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ResolveMscorlibBaseTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      var factory = new MetadataImporterSemanticEntityFactory(project, project.SemanticGraph);
      factory.CreateEntitiesFromAssembly(Assembly.GetAssembly(typeof(int)).Location, "global");
      project.SemanticGraph.AcceptVisitor(new TypeDeclarationResolverSemanticGraphVisitor(project, project.SemanticGraph));

      project.Warnings.Count.ShouldEqual(0);
      project.Errors.Count.ShouldEqual(0);

      var collection = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("System")
        .GetChildNamespaceByName("Collections").GetChildNamespaceByName("ObjectModel")
        .GetChildTypeByDistinctiveName("Collection`1");
      var keyedCollection = project.SemanticGraph.GlobalNamespace.GetChildNamespaceByName("System")
        .GetChildNamespaceByName("Collections").GetChildNamespaceByName("ObjectModel")
        .GetChildTypeByDistinctiveName("KeyedCollection`2");
      keyedCollection.FullyQualifiedName.ShouldEqual("System.Collections.ObjectModel.KeyedCollection`2");
      ((ConstructedGenericTypeEntity)keyedCollection.BaseType).UnderlyingType.ShouldEqual(collection);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of builtin type aliases to types defined in mscorlib.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ResolveAliasesToMscorlibTypes()
    {
      var project = new CSharpProject(WorkingFolder);
      var factory = new MetadataImporterSemanticEntityFactory(project, project.SemanticGraph);
      factory.CreateEntitiesFromAssembly(Assembly.GetAssembly(typeof(int)).Location, "global");
      project.SemanticGraph.AcceptVisitor(new TypeDeclarationResolverSemanticGraphVisitor(project, project.SemanticGraph));

      project.Warnings.Count.ShouldEqual(0);
      project.Errors.Count.ShouldEqual(0);

      // Builtin type aliases must be resolved
      project.SemanticGraph.BuiltInTypes.All(
        builtInType => builtInType.AliasedTypeReference.ResolutionState == ResolutionState.Resolved).ShouldBeTrue();

      // Checking only one if the builtin types whether it resolved to the right system type.
      project.SemanticGraph.GetBuiltInTypeByName("int").AliasedType.FullyQualifiedName.ShouldEqual("System.Int32");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0118: 'A' is a 'namespace' but is used like a 'type'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0118_NamespaceIsUsedLikeAType()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeDeclarationResolution\CS0118_NamespaceIsUsedLikeAType.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph));
      project.SemanticGraph.AcceptVisitor(new TypeDeclarationResolverSemanticGraphVisitor(project, project.SemanticGraph));

      var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[0].BaseTypeReferences.ToArray()[0];
      baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Unresolvable);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0118");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0246: The type or namespace name 'A' could not be found (are you missing a using directive or an assembly reference?)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0246_TypeNameCouldNotBeFound()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeDeclarationResolution\CS0246_TypeNameCouldNotBeFound.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph));
      project.SemanticGraph.AcceptVisitor(new TypeDeclarationResolverSemanticGraphVisitor(project, project.SemanticGraph));

      var baseTypeRef = project.SemanticGraph.GlobalNamespace.ChildTypes[0].BaseTypeReferences.ToArray()[0];
      baseTypeRef.ResolutionState.ShouldEqual(ResolutionState.Unresolvable);

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0246");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Error CS0576: Namespace 'namespace' contains a definition conflicting with alias 'identifier'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore]
    public void CS0576_UsingAliasConflictsWithDeclaration()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeDeclarationResolution\CS0576_UsingAliasConflictsWithDeclaration.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph));
      project.SemanticGraph.AcceptVisitor(new TypeDeclarationResolverSemanticGraphVisitor(project, project.SemanticGraph));

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0576");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of namespace names in using namespace entities.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingNamespaceNames()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeDeclarationResolution\UsingNamespaceNames.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph));
      project.SemanticGraph.AcceptVisitor(new TypeDeclarationResolverSemanticGraphVisitor(project, project.SemanticGraph));

      project.Errors.Count.ShouldEqual(0);
      project.Warnings.Count.ShouldEqual(0);

      var global = project.SemanticGraph.GlobalNamespace;
      var namespaceA = global.ChildNamespaces[0];
      var namespaceB = namespaceA.ChildNamespaces[0];
      var namespaceC = namespaceB.ChildNamespaces[0];

      // using A;
      {
        var usingNamespace = global.UsingNamespaces.ToArray()[0];
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        usingNamespace.NamespaceReference.TargetEntity.ShouldEqual(namespaceA);
        usingNamespace.ImportedNamespace.ShouldEqual(namespaceA);
      }
      // using A.B;
      {
        var usingNamespace = global.UsingNamespaces.ToArray()[1];
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        usingNamespace.NamespaceReference.TargetEntity.ShouldEqual(namespaceB);
        usingNamespace.ImportedNamespace.ShouldEqual(namespaceB);
      }

      // using B;
      {
        var usingNamespace = namespaceA.UsingNamespaces.ToArray()[0];
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        usingNamespace.NamespaceReference.TargetEntity.ShouldEqual(namespaceB);
        usingNamespace.ImportedNamespace.ShouldEqual(namespaceB);
      }
      // using B.C;
      {
        var usingNamespace = namespaceA.UsingNamespaces.ToArray()[1];
        usingNamespace.NamespaceReference.ResolutionState.ShouldEqual(ResolutionState.Resolved);
        usingNamespace.NamespaceReference.TargetEntity.ShouldEqual(namespaceC);
        usingNamespace.ImportedNamespace.ShouldEqual(namespaceC);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0138: A using namespace directive can only be applied to namespaces; 'A' is a type not a namespace
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0138_UsingNamespaceWithTypeName()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeDeclarationResolution\CS0138_UsingNamespaceWithTypeName.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph));
      project.SemanticGraph.AcceptVisitor(new TypeDeclarationResolverSemanticGraphVisitor(project, project.SemanticGraph));

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0138");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0246: The type or namespace name 'A' could not be found (are you missing a using directive or an assembly reference?)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0246_NamespaceNameCouldNotBeFound()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeDeclarationResolution\CS0246_NamespaceNameCouldNotBeFound.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph));
      project.SemanticGraph.AcceptVisitor(new TypeDeclarationResolverSemanticGraphVisitor(project, project.SemanticGraph));

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0246");
      project.Warnings.Count.ShouldEqual(0);
    }
  }
}