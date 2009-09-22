using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the PartialTypeMergingSemanticGraphVisitor class
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class PartialTypeMergingSemanticGraphVisitorTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the merging of partial classes
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PartialClass()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"PartialTypeMergingSemanticGraphVisitor\PartialClass.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // There are 2 partial classes named A
      project.SemanticGraph.GlobalNamespace.GetChildTypes<ClassEntity>("A",1).Count().ShouldEqual(2);

      // Merge the partial types
      var typeMerger = new PartialTypeMergingSemanticGraphVisitor();
      project.SemanticGraph.AcceptVisitor(typeMerger);

      // There should be only 1 class, named A
      project.SemanticGraph.GlobalNamespace.GetChildTypes<ClassEntity>("A",1).Count().ShouldEqual(1);
      var classEntity = project.SemanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity;
      classEntity.ToString().ShouldEqual("global::A`1");

      // Class A should have two syntax nodes
      classEntity.SyntaxNodes.Count.ShouldEqual(2);
      classEntity.SyntaxNodes[0].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[0]);
      classEntity.SyntaxNodes[1].ShouldEqual(project.SyntaxTree.CompilationUnitNodes[0].TypeDeclarations[1]);

      // Both syntax nodes should point to class A
      classEntity.SyntaxNodes[0].SemanticEntities.Count.ShouldEqual(1);
      classEntity.SyntaxNodes[0].SemanticEntities[0].ShouldEqual(classEntity);
      classEntity.SyntaxNodes[1].SemanticEntities.Count.ShouldEqual(1);
      classEntity.SyntaxNodes[1].SemanticEntities[0].ShouldEqual(classEntity);

      // Base type references should be merged
      var baseTypeRefs = classEntity.BaseTypeReferences.ToList();
      baseTypeRefs.Count.ShouldEqual(5);
      ((TypeNodeBasedTypeEntityReference)baseTypeRefs[0]).SyntaxNode.TypeName.ToString().ShouldEqual("B");
      ((TypeNodeBasedTypeEntityReference)baseTypeRefs[1]).SyntaxNode.TypeName.ToString().ShouldEqual("I1");
      ((TypeNodeBasedTypeEntityReference)baseTypeRefs[2]).SyntaxNode.TypeName.ToString().ShouldEqual("I2");
      ((TypeNodeBasedTypeEntityReference)baseTypeRefs[3]).SyntaxNode.TypeName.ToString().ShouldEqual("I2");
      ((TypeNodeBasedTypeEntityReference)baseTypeRefs[4]).SyntaxNode.TypeName.ToString().ShouldEqual("I3");

      // Members should be merged
      var members = classEntity.Members.ToList();
      members[0].Name.ShouldEqual("a1");
      members[1].Name.ShouldEqual("a2");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the merging of partial structs
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PartialStruct()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"PartialTypeMergingSemanticGraphVisitor\PartialStruct.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // There are 2 partial structs named A
      project.SemanticGraph.GlobalNamespace.GetChildTypes<StructEntity>("A", 1).Count().ShouldEqual(2);

      // Merge the partial types
      var typeMerger = new PartialTypeMergingSemanticGraphVisitor();
      project.SemanticGraph.AcceptVisitor(typeMerger);

      // There should be only 1 struct, named A
      project.SemanticGraph.GlobalNamespace.GetChildTypes<StructEntity>("A", 1).Count().ShouldEqual(1);
      project.SemanticGraph.GlobalNamespace.ChildTypes[0].ToString().ShouldEqual("global::A`1");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the merging of partial interfaces
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PartialInterface()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"PartialTypeMergingSemanticGraphVisitor\PartialInterface.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      var visitor = new EntityBuilderSyntaxNodeVisitor(project);
      project.SyntaxTree.AcceptVisitor(visitor);

      // There are 2 partial interfaces named A
      project.SemanticGraph.GlobalNamespace.GetChildTypes<InterfaceEntity>("A", 1).Count().ShouldEqual(2);

      // Merge the partial types
      var typeMerger = new PartialTypeMergingSemanticGraphVisitor();
      project.SemanticGraph.AcceptVisitor(typeMerger);

      // There should be only 1 interface, named A
      project.SemanticGraph.GlobalNamespace.GetChildTypes<InterfaceEntity>("A", 1).Count().ShouldEqual(1);
      project.SemanticGraph.GlobalNamespace.ChildTypes[0].ToString().ShouldEqual("global::A`1");
    }
  }
}
