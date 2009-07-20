using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the type resolution logic of the TypeResolverSemanticGraphVisitor class
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class TypeResolutionTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the resolution of base types
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void TypesDeclaredInCode()
    {
      // Set up SyntaxTree and SemanticGraph
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeResolution\TypesDeclaredInCode.cs");
      InvokeParser(project).ShouldBeTrue();
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph));
      semanticGraph.AcceptVisitor(new TypeResolverSemanticGraphVisitor());

      // class B : A
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypesForUnitTests[1].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FqnWithRoot.ShouldEqual("global::A");
      }
      // class C : B
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypesForUnitTests[1].ChildTypesForUnitTests[0].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FqnWithRoot.ShouldEqual("global::B");
      }
      // class D : B.C
      {
        var baseTypeRef = semanticGraph.GlobalNamespace.ChildTypesForUnitTests[1].ChildTypesForUnitTests[0]
          .ChildTypesForUnitTests[0].BaseTypes[0];
        baseTypeRef.ResolvedEntity.FqnWithRoot.ShouldEqual("global::B.C");
      }
    }
  }
}