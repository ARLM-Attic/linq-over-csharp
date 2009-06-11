using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilderTest.Ast
{
  // ================================================================================================
  /// <summary>
  /// Tests the fluent interface of creating AST nodes
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class FluentInterfaceTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creating UsingNamespaceNode: using global::System.Text;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingNamespaceNode()
    {
      var usingNamespaceNode = new UsingNamespaceNode().Qualifier("global").TypeTag("System").TypeTag("Text");

      usingNamespaceNode.StartToken.Value.ShouldEqual(Token.Using.Value);
      usingNamespaceNode.TypeName.TypeTags[0].IdentifierToken.Value.ShouldEqual("global");
      usingNamespaceNode.TypeName.TypeTags[1].IdentifierToken.Value.ShouldEqual("System");
      usingNamespaceNode.TypeName.TypeTags[2].IdentifierToken.Value.ShouldEqual("Text");
      usingNamespaceNode.TerminatingToken.Value.ShouldEqual(Token.Semicolon.Value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creating UsingNamespaceNode: using global::System.Text;
    /// But qualifier is specified last.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingNamespaceNode_Qualifier_Last()
    {
      var usingNamespaceNode = new UsingNamespaceNode().TypeTag("System").TypeTag("Text").Qualifier("global");

      usingNamespaceNode.StartToken.Value.ShouldEqual(Token.Using.Value);
      usingNamespaceNode.TypeName.TypeTags[0].IdentifierToken.Value.ShouldEqual("global");
      usingNamespaceNode.TypeName.TypeTags[1].IdentifierToken.Value.ShouldEqual("System");
      usingNamespaceNode.TypeName.TypeTags[2].IdentifierToken.Value.ShouldEqual("Text");
      usingNamespaceNode.TerminatingToken.Value.ShouldEqual(Token.Semicolon.Value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creating UsingNamespaceNode: using global::System.Text;
    /// But qualifier is specified last.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingAliasNode()
    {
      var usingAliasNode = new UsingAliasNode();
      usingAliasNode.Alias("MyAlias").Qualifier("global").TypeTag("System").TypeTag("Text");

      usingAliasNode.StartToken.Value.ShouldEqual(Token.Using.Value);
      usingAliasNode.Alias.ShouldEqual("MyAlias");
      usingAliasNode.EqualToken.Value.ShouldEqual("=");
      usingAliasNode.TypeName.TypeTags[0].IdentifierToken.Value.ShouldEqual("global");
      usingAliasNode.TypeName.TypeTags[1].IdentifierToken.Value.ShouldEqual("System");
      usingAliasNode.TypeName.TypeTags[2].IdentifierToken.Value.ShouldEqual("Text");
      usingAliasNode.TerminatingToken.Value.ShouldEqual(Token.Semicolon.Value);
    }
  }
}
