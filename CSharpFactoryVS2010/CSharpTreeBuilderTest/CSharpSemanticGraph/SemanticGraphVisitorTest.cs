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
        var root = project.SemanticGraph.GlobalNamespace;
        sgVisitorMock.Visit(root as RootNamespaceEntity);
        sgVisitorMock.Visit(root as NamespaceEntity);
        sgVisitorMock.Visit(root as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(root as SemanticEntity);

        var ns1 = root.GetChildNamespace("Ns1");
        sgVisitorMock.Visit(ns1 as NamespaceEntity);
        sgVisitorMock.Visit(ns1 as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(ns1 as SemanticEntity);

        var ns2 = ns1.GetChildNamespace("Ns2");
        sgVisitorMock.Visit(ns2 as NamespaceEntity);
        sgVisitorMock.Visit(ns2 as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(ns2 as SemanticEntity);

        var class1 = ns1.GetSingleChildType<ClassEntity>("Class1");
        sgVisitorMock.Visit(class1 as ClassEntity);
        sgVisitorMock.Visit(class1 as ChildTypeCapableTypeEntity);
        sgVisitorMock.Visit(class1 as GenericCapableTypeEntity);
        sgVisitorMock.Visit(class1 as TypeEntity);
        sgVisitorMock.Visit(class1 as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(class1 as SemanticEntity);

        var class2 = root.GetSingleChildType<ClassEntity>("Class2");
        sgVisitorMock.Visit(class2 as ClassEntity);
        sgVisitorMock.Visit(class2 as ChildTypeCapableTypeEntity);
        sgVisitorMock.Visit(class2 as GenericCapableTypeEntity);
        sgVisitorMock.Visit(class2 as TypeEntity);
        sgVisitorMock.Visit(class2 as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(class2 as SemanticEntity);

        var class3 = class2.GetSingleChildType<ClassEntity>("Class3");
        sgVisitorMock.Visit(class3 as ClassEntity);
        sgVisitorMock.Visit(class3 as ChildTypeCapableTypeEntity);
        sgVisitorMock.Visit(class3 as GenericCapableTypeEntity);
        sgVisitorMock.Visit(class3 as TypeEntity);
        sgVisitorMock.Visit(class3 as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(class3 as SemanticEntity);
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
        var root = project.SemanticGraph.GlobalNamespace;
        sgVisitorMock.Visit(root as RootNamespaceEntity);
        sgVisitorMock.Visit(root as NamespaceEntity);
        sgVisitorMock.Visit(root as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(root as SemanticEntity);

        var usingNsA = root.UsingNamespaces.ToArray()[0];
        sgVisitorMock.Visit(usingNsA as UsingNamespaceEntity);
        sgVisitorMock.Visit(usingNsA as UsingEntity);
        sgVisitorMock.Visit(usingNsA as SemanticEntity);

        var usingAliasX = root.UsingAliases.ToArray()[0];
        sgVisitorMock.Visit(usingAliasX as UsingAliasEntity);
        sgVisitorMock.Visit(usingAliasX as UsingEntity);
        sgVisitorMock.Visit(usingAliasX as SemanticEntity);

        var nsA = root.GetChildNamespace("A");
        sgVisitorMock.Visit(nsA as NamespaceEntity);
        sgVisitorMock.Visit(nsA as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(nsA as SemanticEntity);

        var usingNsB = nsA.UsingNamespaces.ToArray()[0];
        sgVisitorMock.Visit(usingNsB as UsingNamespaceEntity);
        sgVisitorMock.Visit(usingNsB as UsingEntity);
        sgVisitorMock.Visit(usingNsB as SemanticEntity);

        var usingAliasY = nsA.UsingAliases.ToArray()[0];
        sgVisitorMock.Visit(usingAliasY as UsingAliasEntity);
        sgVisitorMock.Visit(usingAliasY as UsingEntity);
        sgVisitorMock.Visit(usingAliasY as SemanticEntity);

        var nsB = nsA.GetChildNamespace("B");
        sgVisitorMock.Visit(nsB as NamespaceEntity);
        sgVisitorMock.Visit(nsB as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(nsB as SemanticEntity);
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
        var root = project.SemanticGraph.GlobalNamespace;
        sgVisitorMock.Visit(root as RootNamespaceEntity);
        sgVisitorMock.Visit(root as NamespaceEntity);
        sgVisitorMock.Visit(root as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(root as SemanticEntity);

        var externAlias = root.ExternAliases.ToArray()[0];
        sgVisitorMock.Visit(externAlias as ExternAliasEntity);
        sgVisitorMock.Visit(externAlias as SemanticEntity);

        var nsA = root.GetChildNamespace("A");
        sgVisitorMock.Visit(nsA as NamespaceEntity);
        sgVisitorMock.Visit(nsA as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(nsA as SemanticEntity);

        var externAlias2 = nsA.ExternAliases.ToArray()[0];
        sgVisitorMock.Visit(externAlias2 as ExternAliasEntity);
        sgVisitorMock.Visit(externAlias2 as SemanticEntity);
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
        var root = project.SemanticGraph.GlobalNamespace;
        sgVisitorMock.Visit(root as RootNamespaceEntity);
        sgVisitorMock.Visit(root as NamespaceEntity);
        sgVisitorMock.Visit(root as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(root as SemanticEntity);

        var enumE = root.GetSingleChildType<EnumEntity>("E");
        sgVisitorMock.Visit(enumE as EnumEntity);
        sgVisitorMock.Visit(enumE as TypeEntity);
        sgVisitorMock.Visit(enumE as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(enumE as SemanticEntity);

        var e1 = enumE.GetMember<EnumMemberEntity>("E1");
        sgVisitorMock.Visit(e1 as EnumMemberEntity);
        sgVisitorMock.Visit(e1 as ConstantMemberEntity);
        sgVisitorMock.Visit(e1 as NonTypeMemberEntity);
        sgVisitorMock.Visit(e1 as SemanticEntity);

        var e2 = enumE.GetMember<EnumMemberEntity>("E2");
        sgVisitorMock.Visit(e2 as EnumMemberEntity);
        sgVisitorMock.Visit(e2 as ConstantMemberEntity);
        sgVisitorMock.Visit(e2 as NonTypeMemberEntity);
        sgVisitorMock.Visit(e2 as SemanticEntity);
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
        var root = project.SemanticGraph.GlobalNamespace;
        sgVisitorMock.Visit(root as RootNamespaceEntity);
        sgVisitorMock.Visit(root as NamespaceEntity);
        sgVisitorMock.Visit(root as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(root as SemanticEntity);

        var class1 = root.GetSingleChildType<ClassEntity>("Class1");
        sgVisitorMock.Visit(class1 as ClassEntity);
        sgVisitorMock.Visit(class1 as ChildTypeCapableTypeEntity);
        sgVisitorMock.Visit(class1 as GenericCapableTypeEntity);
        sgVisitorMock.Visit(class1 as TypeEntity);
        sgVisitorMock.Visit(class1 as NamespaceOrTypeEntity);
        sgVisitorMock.Visit(class1 as SemanticEntity);

        var a = class1.GetMember<ConstantMemberEntity>("a");
        sgVisitorMock.Visit(a as ConstantMemberEntity);
        sgVisitorMock.Visit(a as NonTypeMemberEntity);
        sgVisitorMock.Visit(a as SemanticEntity);

        var a_init = a.InitializerExpression;
        sgVisitorMock.Visit(a_init as TypedLiteralExpressionEntity);
        sgVisitorMock.Visit(a_init as LiteralExpressionEntity);
        sgVisitorMock.Visit(a_init as ExpressionEntity);
        sgVisitorMock.Visit(a_init as SemanticEntity);

        var b = class1.GetMember<FieldEntity>("b");
        sgVisitorMock.Visit(b as FieldEntity);
        sgVisitorMock.Visit(b as NonTypeMemberEntity);
        sgVisitorMock.Visit(b as SemanticEntity);

        var M = class1.GetMember<MethodEntity>("M");
        sgVisitorMock.Visit(M as MethodEntity);
        sgVisitorMock.Visit(M as FunctionMemberWithBodyEntity);
        sgVisitorMock.Visit(M as FunctionMemberEntity);
        sgVisitorMock.Visit(M as NonTypeMemberEntity);
        sgVisitorMock.Visit(M as SemanticEntity);

        var body = M.Body;
        sgVisitorMock.Visit(body as BlockStatementEntity);
        sgVisitorMock.Visit(body as DeclarationSpaceDefiningStatementEntity);
        sgVisitorMock.Visit(body as StatementEntity);
        sgVisitorMock.Visit(body as SemanticEntity);

        var p1 = M.Parameters.ToList()[0];
        sgVisitorMock.Visit(p1 as ParameterEntity);
        sgVisitorMock.Visit(p1 as NonFieldVariableEntity);
        sgVisitorMock.Visit(p1 as SemanticEntity);

        var p2 = M.Parameters.ToList()[1];
        sgVisitorMock.Visit(p2 as ParameterEntity);
        sgVisitorMock.Visit(p2 as NonFieldVariableEntity);
        sgVisitorMock.Visit(p2 as SemanticEntity);

      }
      mocks.ReplayAll();

      // We only travers the global namespace, not the whole semantic graph, because that would include the built-in types too
      project.SemanticGraph.GlobalNamespace.AcceptVisitor(sgVisitorMock);

      // Assert
      mocks.VerifyAll();
    }
  }
}