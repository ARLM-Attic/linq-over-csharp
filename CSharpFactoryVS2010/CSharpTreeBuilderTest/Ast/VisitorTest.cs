using CSharpTreeBuilder.CSharpAstBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilderTest.Ast
{
  /// <summary>
  /// Tests the visitor traversal of the AST nodes
  /// </summary>
  [TestClass]
  public class VisitorTest 
  {
    [TestMethod]
    public void VisitSourceFileNode()
    {
      // Set up a syntax tree
      SourceFileNode sourceFileNode = new SourceFileNode("Test");
      Token systemNameToken = new Token("System");
      TypeOrNamespaceNode namespaceNode = new TypeOrNamespaceNode(systemNameToken);
      sourceFileNode.AddUsing(null, namespaceNode, null);


/*      
      // SourceFileNode-ot nevezzük át CompilationUnitNode-ra, jelezve hogy nem csak fáj lehet!
      var compilationUnitNode = new CompilationUnitNode("Test"); // A név ne legyen kötelező

      // Fluent stílusban?
      compilationUnitNode.AddChild(new UsingNamespaceNode().Qualifier("global").NameTag("System").NameTag("Text");
      compilationUnitNode.Using().GlobalQualifier().NameTag("System").NameTag("Text"));
      compilationUnitNode.Using().Qualifier("MyQual").NameTag("System").NameTag("Text");
      compilationUnitNode.Using("MyQual::System.Text");

      compilationUnitNode.Class().Modifier(ModifierType.Static);
      compilationUnitNode.InternalClass();
      var myClass = compilationUnitNode.SealedClass("MyClass").TypeParam("A").Constraint(ConstraintType.Class);

      // Hasonló a UsingAliasNode-ra
      compilationUnitNode.AddChild(new UsingAliasNode().Alias("text").Qualifier("global").NameTag("System").NameTag("Text"));

*/            

      // Set up expectations on mock
      var visitorMock = new Mock<ISyntaxNodeVisitor>(MockBehavior.Strict);
      visitorMock.Setup(v => v.Visit(sourceFileNode.UsingNodes[0]));

      // Act
      sourceFileNode.AcceptVisitor(visitorMock.Object);

      // Verify that all set up expectation were met. Order is not verified, unfortunately.
      visitorMock.VerifyAll();
    }
  }
}
