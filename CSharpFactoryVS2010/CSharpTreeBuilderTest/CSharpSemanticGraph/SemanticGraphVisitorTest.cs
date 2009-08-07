using System.Linq;
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
    public void VisitNamespaceOrTypeEntity()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SemanticGraphVisitor\VisitNamespaceOrTypeEntity.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph));

      // Arrange
      var mocks = new MockRepository();
      var sgVisitorMock = mocks.StrictMock<SemanticGraphVisitor>();
      using (mocks.Ordered())
      {
        sgVisitorMock.Visit(project.SemanticGraph.GlobalNamespace);
        sgVisitorMock.Visit(project.SemanticGraph.GlobalNamespace.ChildNamespaces[0]);
        sgVisitorMock.Visit(project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildNamespaces[0]);
        var class1 = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as ClassEntity;
        sgVisitorMock.Visit(class1);
        sgVisitorMock.Visit(class1.Members.ToArray()[0] as FieldEntity);
        sgVisitorMock.Visit((TypeEntity)project.SemanticGraph.GlobalNamespace.ChildTypes[0]);
        sgVisitorMock.Visit((TypeEntity)((ClassEntity)project.SemanticGraph.GlobalNamespace.ChildTypes[0]).ChildTypes[0]);
      }
      mocks.ReplayAll();

      // We only travers the global namespace, not the whole semantic graph, because that would include the built-in types too
      project.SemanticGraph.GlobalNamespace.AcceptVisitor(sgVisitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of using entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitUsingEntity()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SemanticGraphVisitor\VisitUsingEntity.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project, project.SemanticGraph));

      // Arrange
      var mocks = new MockRepository();
      var sgVisitorMock = mocks.StrictMock<SemanticGraphVisitor>();
      using (mocks.Ordered())
      {
        var global = project.SemanticGraph.GlobalNamespace;
        sgVisitorMock.Visit(global);
        sgVisitorMock.Visit(global.UsingNamespaces.ToArray()[0]);
        sgVisitorMock.Visit(global.UsingAliases.ToArray()[0]);
        sgVisitorMock.Visit(global.ChildNamespaces[0]);
        sgVisitorMock.Visit(global.ChildNamespaces[0].UsingNamespaces.ToArray()[0]);
        sgVisitorMock.Visit(global.ChildNamespaces[0].UsingAliases.ToArray()[0]);
        sgVisitorMock.Visit(global.ChildNamespaces[0].ChildNamespaces[0]);
      }
      mocks.ReplayAll();

      // We only travers the global namespace, not the whole semantic graph, because that would include the built-in types too
      project.SemanticGraph.GlobalNamespace.AcceptVisitor(sgVisitorMock);

      // Assert
      mocks.VerifyAll();
    }
  }
}