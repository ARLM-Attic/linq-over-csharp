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
    /// Creating UsingNamespaceNode: using System.Text;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingNamespaceNode()
    {
      var usingNamespaceNode = new UsingNamespaceNode().TypeTag("System").TypeTag("Text");

      usingNamespaceNode.StartToken.Value.ShouldEqual(Token.Using.Value);
      usingNamespaceNode.TypeName.HasQualifier.ShouldBeFalse();
      usingNamespaceNode.TypeName.QualifierToken.ShouldBeNull();
      usingNamespaceNode.TypeName.Qualifier.ShouldBeNull();
      usingNamespaceNode.TypeName.TypeTags.Count.ShouldEqual(2);
      usingNamespaceNode.TypeName.TypeTags[0].IdentifierToken.Value.ShouldEqual("System");
      usingNamespaceNode.TypeName.TypeTags[0].SeparatorToken.ShouldBeNull();
      usingNamespaceNode.TypeName.TypeTags[1].IdentifierToken.Value.ShouldEqual("Text");
      usingNamespaceNode.TypeName.TypeTags[1].SeparatorToken.Value.ShouldEqual(".");
      usingNamespaceNode.TerminatingToken.Value.ShouldEqual(Token.Semicolon.Value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creating UsingNamespaceNode: using global::System.Text;
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingNamespaceNode_WithQualifier()
    {
      var usingNamespaceNode = new UsingNamespaceNode().Qualifier("global").TypeTag("System").TypeTag("Text");

      usingNamespaceNode.StartToken.Value.ShouldEqual(Token.Using.Value);
      usingNamespaceNode.TypeName.HasQualifier.ShouldBeTrue();
      usingNamespaceNode.TypeName.Qualifier.ShouldEqual("global");
      usingNamespaceNode.TypeName.TypeTags.Count.ShouldEqual(2);
      usingNamespaceNode.TypeName.TypeTags[0].IdentifierToken.Value.ShouldEqual("System");
      usingNamespaceNode.TypeName.TypeTags[0].SeparatorToken.Value.ShouldEqual("::");
      usingNamespaceNode.TypeName.TypeTags[1].IdentifierToken.Value.ShouldEqual("Text");
      usingNamespaceNode.TypeName.TypeTags[1].SeparatorToken.Value.ShouldEqual(".");
      usingNamespaceNode.TerminatingToken.Value.ShouldEqual(Token.Semicolon.Value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creating UsingNamespaceNode: using global::System.Text;
    /// But qualifier is specified last.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingNamespaceNode_WithQualifierLast()
    {
      var usingNamespaceNode = new UsingNamespaceNode().TypeTag("System").TypeTag("Text").Qualifier("global");

      usingNamespaceNode.StartToken.Value.ShouldEqual(Token.Using.Value);
      usingNamespaceNode.TypeName.HasQualifier.ShouldBeTrue();
      usingNamespaceNode.TypeName.Qualifier.ShouldEqual("global");
      usingNamespaceNode.TypeName.TypeTags.Count.ShouldEqual(2);
      usingNamespaceNode.TypeName.TypeTags[0].IdentifierToken.Value.ShouldEqual("System");
      usingNamespaceNode.TypeName.TypeTags[0].SeparatorToken.Value.ShouldEqual("::");
      usingNamespaceNode.TypeName.TypeTags[1].IdentifierToken.Value.ShouldEqual("Text");
      usingNamespaceNode.TypeName.TypeTags[1].SeparatorToken.Value.ShouldEqual(".");
      usingNamespaceNode.TerminatingToken.Value.ShouldEqual(Token.Semicolon.Value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creating UsingNamespaceNode: if qualifier is specified twice, then the second overwrites the first.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingNamespaceNode_WithQualifierTwice()
    {
      var usingNamespaceNode = new UsingNamespaceNode().Qualifier("global1").Qualifier("global2").TypeTag("System");

      usingNamespaceNode.StartToken.Value.ShouldEqual(Token.Using.Value);
      usingNamespaceNode.TypeName.HasQualifier.ShouldBeTrue();
      usingNamespaceNode.TypeName.Qualifier.ShouldEqual("global2");
      usingNamespaceNode.TypeName.TypeTags.Count.ShouldEqual(1);
      usingNamespaceNode.TypeName.TypeTags[0].IdentifierToken.Value.ShouldEqual("System");
      usingNamespaceNode.TypeName.TypeTags[0].SeparatorToken.Value.ShouldEqual("::");
      usingNamespaceNode.TerminatingToken.Value.ShouldEqual(Token.Semicolon.Value);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creating UsingAliasNode: using MyAlias = global::System.Text;
    /// But qualifier is specified last.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingAliasNode()
    {
      var usingAliasNode = new UsingAliasNode("MyAlias").Qualifier("global").TypeTag("System").TypeTag("Text");

      usingAliasNode.StartToken.Value.ShouldEqual(Token.Using.Value);
      usingAliasNode.Alias.ShouldEqual("MyAlias");
      usingAliasNode.EqualToken.Value.ShouldEqual("=");
      usingAliasNode.TypeName.HasQualifier.ShouldBeTrue();
      usingAliasNode.TypeName.Qualifier.ShouldEqual("global");
      usingAliasNode.TypeName.TypeTags.Count.ShouldEqual(2);
      usingAliasNode.TypeName.TypeTags[0].IdentifierToken.Value.ShouldEqual("System");
      usingAliasNode.TypeName.TypeTags[0].SeparatorToken.Value.ShouldEqual("::");
      usingAliasNode.TypeName.TypeTags[1].IdentifierToken.Value.ShouldEqual("Text");
      usingAliasNode.TypeName.TypeTags[1].SeparatorToken.Value.ShouldEqual(".");
      usingAliasNode.TerminatingToken.Value.ShouldEqual(Token.Semicolon.Value);
    }
  }
}
