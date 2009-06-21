using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest.Ast
{
  // ================================================================================================
  /// <summary>
  /// Tests the visitor traversal of the AST nodes
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class VisitorTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of a CompilationUnitNode
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitCompilationUnitNode()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Visitor\CompilationUnitNodeVisitorTest.cs");
      InvokeParser(project).ShouldBeTrue();
      var compilationUnitNode = project.SyntaxTree.SourceFileNodes[0];

      // Set up expectations on mock
      var visitorMock = new Mock<ISyntaxNodeVisitor>(MockBehavior.Strict);
      visitorMock.Setup(v => v.Visit(compilationUnitNode));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.ExternAliasNodes[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[0].TypeName));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit((UsingAliasNode) compilationUnitNode.UsingNodes[1]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[1].TypeName));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[1].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].TypeName));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].TypeName.TypeTags[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].Arguments[0]));
      //visitorMock.Setup(v => v.Visit(compilationUnitNode.GlobalAttributes[0].Attributes[0].Arguments[0].Expression));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.NamespaceDeclarations[0]));
      visitorMock.Setup(v => v.Visit((ClassDeclarationNode) compilationUnitNode.TypeDeclarations[0]));
      visitorMock.Setup(v => v.Visit((StructDeclarationNode) compilationUnitNode.TypeDeclarations[1]));
      visitorMock.Setup(v => v.Visit((InterfaceDeclarationNode) compilationUnitNode.TypeDeclarations[2]));
      visitorMock.Setup(v => v.Visit((EnumDeclarationNode) compilationUnitNode.TypeDeclarations[3]));
      visitorMock.Setup(v => v.Visit((DelegateDeclarationNode) compilationUnitNode.TypeDeclarations[4]));

      // Act
      compilationUnitNode.AcceptVisitor(visitorMock.Object);

      // Verify that all set up expectation were met. Order is not verified, unfortunately.
      visitorMock.VerifyAll();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the visiting of a TypeOrNamespaceNode
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitTypeOrNamespaceNode()
    {
      // Set up a syntax tree
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Visitor\TypeOrNamespaceNodeVisitorTest.cs");
      InvokeParser(project).ShouldBeTrue();
      var typeOrNamespaceNode = project.SyntaxTree.SourceFileNodes[0].UsingNodes[0].TypeName;

      // Set up expectations on mock
      var visitorMock = new Mock<ISyntaxNodeVisitor>(MockBehavior.Strict);
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode));
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[0]));                                                   // System
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[1]));                                                   // Collections
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[2]));                                                   // Generic
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3]));                                                   // IDictionary
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0]));                                        // System.Nullable<int>**[][,]
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[0]));                              // System
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[1]));                              // Nullable
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[1].Arguments[0]));                   // int
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeTags[1].Arguments[0].TypeTags[0]));         // int
      visitorMock.Setup(v => v.Visit((PointerModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeModifiers[0]));   // *
      visitorMock.Setup(v => v.Visit((PointerModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeModifiers[1]));   // *
      //visitorMock.Setup(v => v.Visit((ArrayModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeModifiers[2]));     // []
      //visitorMock.Setup(v => v.Visit((ArrayModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[0].TypeModifiers[3]));     // [,]
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1]));                                        // string**[][,]
      visitorMock.Setup(v => v.Visit(typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeTags[0]));                              // string
      visitorMock.Setup(v => v.Visit((PointerModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeModifiers[0]));   // *
      visitorMock.Setup(v => v.Visit((PointerModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeModifiers[1]));   // *
      //visitorMock.Setup(v => v.Visit((ArrayModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeModifiers[2]));     // []
      //visitorMock.Setup(v => v.Visit((ArrayModifierNode) typeOrNamespaceNode.TypeTags[3].Arguments[1].TypeModifiers[3]));     // [,]

      // Act
      typeOrNamespaceNode.AcceptVisitor(visitorMock.Object);

      // Verify that all set up expectation were met. Order is not verified, unfortunately.
      visitorMock.VerifyAll();
    }

  }
}
