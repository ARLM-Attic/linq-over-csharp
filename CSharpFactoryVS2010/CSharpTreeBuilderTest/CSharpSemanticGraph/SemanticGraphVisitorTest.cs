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
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project));

      // Arrange
      var mocks = new MockRepository();
      var sgVisitorMock = mocks.StrictMock<SemanticGraphVisitor>();
      using (mocks.Ordered())
      {
        Expect.Call(sgVisitorMock.Visit(project.SemanticGraph.GlobalNamespace)).Return(true);
        Expect.Call(sgVisitorMock.Visit(project.SemanticGraph.GlobalNamespace.ChildNamespaces[0])).Return(true);
        Expect.Call(sgVisitorMock.Visit(project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildNamespaces[0])).Return(true);
        var class1 = project.SemanticGraph.GlobalNamespace.ChildNamespaces[0].ChildTypes[0] as ClassEntity;
        Expect.Call(sgVisitorMock.Visit(class1)).Return(true);
        Expect.Call(sgVisitorMock.Visit(project.SemanticGraph.GlobalNamespace.ChildTypes[0] as ClassEntity)).Return(true);
        Expect.Call(sgVisitorMock.Visit(((ClassEntity)project.SemanticGraph.GlobalNamespace.ChildTypes[0]).ChildTypes[0] as ClassEntity)).Return(true);
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
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project));

      // Arrange
      var mocks = new MockRepository();
      var sgVisitorMock = mocks.StrictMock<SemanticGraphVisitor>();
      using (mocks.Ordered())
      {
        var global = project.SemanticGraph.GlobalNamespace;
        Expect.Call(sgVisitorMock.Visit(global)).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.UsingNamespaces.ToArray()[0])).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.UsingAliases.ToArray()[0])).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.ChildNamespaces[0])).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.ChildNamespaces[0].UsingNamespaces.ToArray()[0])).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.ChildNamespaces[0].UsingAliases.ToArray()[0])).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.ChildNamespaces[0].ChildNamespaces[0])).Return(true);
      }
      mocks.ReplayAll();

      // We only travers the global namespace, not the whole semantic graph, because that would include the built-in types too
      project.SemanticGraph.GlobalNamespace.AcceptVisitor(sgVisitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of extern alias entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitExternAliasEntity()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SemanticGraphVisitor\VisitExternAliasEntity.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project));

      // Arrange
      var mocks = new MockRepository();
      var sgVisitorMock = mocks.StrictMock<SemanticGraphVisitor>();
      using (mocks.Ordered())
      {
        var global = project.SemanticGraph.GlobalNamespace;
        Expect.Call(sgVisitorMock.Visit(global)).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.ExternAliases.ToArray()[0])).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.ChildNamespaces[0])).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.ChildNamespaces[0].ExternAliases.ToArray()[0])).Return(true);
      }
      mocks.ReplayAll();

      // We only travers the global namespace, not the whole semantic graph, because that would include the built-in types too
      project.SemanticGraph.GlobalNamespace.AcceptVisitor(sgVisitorMock);

      // Assert
      mocks.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of extern alias entities
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitEnumEntity()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SemanticGraphVisitor\VisitEnumEntity.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project));

      // Arrange
      var mocks = new MockRepository();
      var sgVisitorMock = mocks.StrictMock<SemanticGraphVisitor>();
      using (mocks.Ordered())
      {
        var global = project.SemanticGraph.GlobalNamespace;
        Expect.Call(sgVisitorMock.Visit(global)).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.ChildTypes[0] as EnumEntity)).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.ChildTypes[0].Members.ToList()[0] as EnumMemberEntity)).Return(true);
        Expect.Call(sgVisitorMock.Visit(global.ChildTypes[0].Members.ToList()[1] as EnumMemberEntity)).Return(true);
      }
      mocks.ReplayAll();

      // We only travers the global namespace, not the whole semantic graph, because that would include the built-in types too
      project.SemanticGraph.GlobalNamespace.AcceptVisitor(sgVisitorMock);

      // Assert
      mocks.VerifyAll();
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of class members
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitClassMembers()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SemanticGraphVisitor\VisitClassMembers.cs");
      InvokeParser(project, true, false).ShouldBeTrue();
      project.SyntaxTree.AcceptVisitor(new EntityBuilderSyntaxNodeVisitor(project));

      // Arrange
      var mocks = new MockRepository();
      var sgVisitorMock = mocks.StrictMock<SemanticGraphVisitor>();
      using (mocks.Ordered())
      {
        var global = project.SemanticGraph.GlobalNamespace;
        Expect.Call(sgVisitorMock.Visit(global)).Return(true);
        var class1 = global.ChildTypes[0] as ClassEntity;
        Expect.Call(sgVisitorMock.Visit(class1)).Return(true);
        var members = class1.Members.ToList();
        var constMember = members[0] as ConstantMemberEntity;
        Expect.Call(sgVisitorMock.Visit(constMember)).Return(true);
        Expect.Call(sgVisitorMock.Visit(constMember.InitializerExpression as TypedLiteralExpressionEntity)).Return(true);
        var fieldMember = members[1] as FieldEntity;
        Expect.Call(sgVisitorMock.Visit(fieldMember)).Return(true);
        var methodMember = members[2] as MethodEntity;
        Expect.Call(sgVisitorMock.Visit(methodMember)).Return(true);
      }
      mocks.ReplayAll();

      // We only travers the global namespace, not the whole semantic graph, because that would include the built-in types too
      project.SemanticGraph.GlobalNamespace.AcceptVisitor(sgVisitorMock);

      // Assert
      mocks.VerifyAll();
    }
  }
}