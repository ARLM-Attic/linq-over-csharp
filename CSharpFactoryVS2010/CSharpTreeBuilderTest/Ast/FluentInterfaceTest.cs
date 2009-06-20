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
    /// Creating a TypeTagNode with TypeTageNode(string) constructor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void TypeTagNode_StringCtor()
    {
      var node = new TypeTagNode("System");

      node.HasIdentifier.ShouldBeTrue();
      node.HasTypeArguments.ShouldBeFalse();
      node.Identifier.ShouldEqual("System");

      node.StartToken.Value.ShouldEqual("System");
      node.SeparatorToken.ShouldBeNull();
      node.TerminatingToken.Value.ShouldEqual("System");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creating a TypeTagNode with TypeTageNode(string) constructor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void TypeTagNode_WithTypeArg()
    {
      var node = new TypeTagNode("IDictionary").TypeArg("string","int");

      node.HasIdentifier.ShouldBeTrue();
      node.HasTypeArguments.ShouldBeTrue();
      node.Identifier.ShouldEqual("IDictionary");
      node.Arguments[0].TypeTags[0].Identifier.ShouldEqual("string");
      node.Arguments[1].TypeTags[0].Identifier.ShouldEqual("int");

      node.StartToken.Value.ShouldEqual("IDictionary");
      node.SeparatorToken.ShouldBeNull();
      node.TerminatingToken.Value.ShouldEqual(">");
    }

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
      var usingNamespaceNode = new UsingNamespaceNode().Qualifier("global").TypeTag("System","Text");

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
      var usingNamespaceNode = new UsingNamespaceNode().TypeTag("System","Text").Qualifier("global");

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
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingAliasNode()
    {
      var usingAliasNode = new UsingAliasNode("MyAlias").Qualifier("global").TypeTag("System","Text");

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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creating UsingAliasNode: using StringList = System.Collections.Generic.IDictionary{int,string};
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void UsingAliasNode_WithTypeArgs()
    {
      var usingAliasNode = new UsingAliasNode("StringList")
        .TypeTag("System","Collections","Generic")
        .TypeTag(new TypeTagNode("IDictionary").TypeArg("int","string"));

      usingAliasNode.StartToken.Value.ShouldEqual(Token.Using.Value);
      usingAliasNode.Alias.ShouldEqual("StringList");
      usingAliasNode.EqualToken.Value.ShouldEqual("=");
      usingAliasNode.TypeName.HasQualifier.ShouldBeFalse();
      usingAliasNode.TypeName.TypeTags.Count.ShouldEqual(4);
      usingAliasNode.TypeName.TypeTags[0].SeparatorToken.ShouldBeNull();
      usingAliasNode.TypeName.TypeTags[0].IdentifierToken.Value.ShouldEqual("System");
      usingAliasNode.TypeName.TypeTags[1].SeparatorToken.Value.ShouldEqual(".");
      usingAliasNode.TypeName.TypeTags[1].IdentifierToken.Value.ShouldEqual("Collections");
      usingAliasNode.TypeName.TypeTags[2].SeparatorToken.Value.ShouldEqual(".");
      usingAliasNode.TypeName.TypeTags[2].IdentifierToken.Value.ShouldEqual("Generic");
      usingAliasNode.TypeName.TypeTags[3].SeparatorToken.Value.ShouldEqual(".");
      usingAliasNode.TypeName.TypeTags[3].IdentifierToken.Value.ShouldEqual("IDictionary");
      usingAliasNode.TypeName.TypeTags[3].Arguments.StartToken.Value.ShouldEqual("<");
      usingAliasNode.TypeName.TypeTags[3].Arguments[0].TypeTags[0].SeparatorToken.ShouldBeNull();
      usingAliasNode.TypeName.TypeTags[3].Arguments[0].TypeTags[0].Identifier.ShouldEqual("int");
      usingAliasNode.TypeName.TypeTags[3].Arguments[1].SeparatorToken.Value.ShouldEqual(",");
      usingAliasNode.TypeName.TypeTags[3].Arguments[1].TypeTags[0].Identifier.ShouldEqual("string");
      usingAliasNode.TypeName.TypeTags[3].TerminatingToken.Value.ShouldEqual(">");
      usingAliasNode.TerminatingToken.Value.ShouldEqual(Token.Semicolon.Value);
    }
  }
}
