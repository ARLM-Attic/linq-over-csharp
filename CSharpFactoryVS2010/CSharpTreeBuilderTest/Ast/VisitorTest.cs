using CSharpTreeBuilder.CSharpAstBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilderTest.Ast
{
  // ================================================================================================
  /// <summary>
  /// Tests the visitor traversal of the AST nodes
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class VisitorTest 
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the visiting of SourceFileNode
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void VisitSourceFileNode()
    {
      // Set up a syntax tree
      var compilationUnitNode = new SourceFileNode();
      compilationUnitNode.UsingNamespace().TypeTag("System");
      compilationUnitNode.UsingAlias("myAlias").TypeTag("System");

      /*
            // Fluent stílusban?
            compilationUnitNode.AddChild(new UsingNamespaceNode().Qualifier("global").NameTag("System").NameTag("Text");
            compilationUnitNode.Using().GlobalQualifier().NameTag("System").NameTag("Text"));
            compilationUnitNode.Using().Qualifier("MyQual").NameTag("System").NameTag("Text");
            compilationUnitNode.Using("MyQual::System.Text");

            compilationUnitNode.Class().Modifier(ModifierType.Static);
            compilationUnitNode.InternalClass();
            var myClass = compilationUnitNode.SealedClass("MyClass").TypeParam("A").Constraint(ConstraintType.Class);

      */

      // Set up expectations on mock
      var visitorMock = new Mock<ISyntaxNodeVisitor>(MockBehavior.Strict);
      visitorMock.Setup(v => v.Visit(compilationUnitNode));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[0]));
      visitorMock.Setup(v => v.Visit(compilationUnitNode.UsingNodes[1]));

      // Act
      compilationUnitNode.AcceptVisitor(visitorMock.Object);

      // Verify that all set up expectation were met. Order is not verified, unfortunately.
      visitorMock.VerifyAll();
    }
  }
}
