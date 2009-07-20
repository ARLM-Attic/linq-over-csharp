using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilderTest.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Tests the traversal logic of the semantic graph visitors
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class SemanticGraphVisitorTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of namespace and type entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NamespaceOrTypeEntityVisitor()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SemanticGraphVisitor\NamespaceOrTypeEntityVisitor.cs");
      InvokeParser(project).ShouldBeTrue();
      // Create semantic graph
      var semanticGraph = new SemanticGraph();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(semanticGraph));

      // Arrange
      var mocks = new MockRepository();
      var sgVisitorMock = mocks.StrictMock<SemanticGraphVisitor>();
      using (mocks.Ordered())
      {
        sgVisitorMock.Visit(semanticGraph.GlobalNamespace.ChildNamespacesForUnitTests[0].ChildTypesForUnitTests[0]);
        sgVisitorMock.Visit(semanticGraph.GlobalNamespace.ChildTypesForUnitTests[0]);
        sgVisitorMock.Visit(semanticGraph.GlobalNamespace.ChildTypesForUnitTests[0].ChildTypesForUnitTests[0]);
      }
      mocks.ReplayAll();

      // Act
      semanticGraph.AcceptVisitor(sgVisitorMock);

      // Assert
      mocks.VerifyAll();
    }
  }
}