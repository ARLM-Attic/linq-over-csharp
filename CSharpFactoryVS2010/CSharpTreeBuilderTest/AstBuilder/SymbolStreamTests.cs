// ================================================================================================
// SymbolStreamTests.cs
//
// Created: 2009.06.26, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest.AstBuilder
{
  [TestClass]
  public class SymbolStreamTests: ParserTestBed
  {
    [TestMethod]
    public void ReadAllSymbolIsOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"ClassDeclaration\ClassDeclaration1.cs");
      InvokeParser(project).ShouldBeTrue();
      var st = project.SyntaxTree.SourceFileNodes[0].SymbolStream;
      st.ShouldNotBeNull();
      // --- "public class A"
      var symbol = st.ReadSymbol(0); symbol.Kind.ShouldEqual(CSharpParser._public);
      symbol.Value.ShouldEqual("public");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._class);
      symbol.Value.ShouldEqual("class");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.IsIdentifier.ShouldBeTrue();
      symbol.Value.ShouldEqual("A");
      symbol = st.ReadSymbol(); symbol.IsEndOfLine.ShouldBeTrue();
      // --- "{"
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._lbrace);
      symbol.Value.ShouldEqual("{");
      symbol = st.ReadSymbol(); symbol.IsEndOfLine.ShouldBeTrue();
      // --- "  public new class C {}"
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._public);
      symbol.Value.ShouldEqual("public");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._new);
      symbol.Value.ShouldEqual("new");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._class);
      symbol.Value.ShouldEqual("class");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.IsIdentifier.ShouldBeTrue();
      symbol.Value.ShouldEqual("C");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._lbrace);
      symbol.Value.ShouldEqual("{");
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._rbrace);
      symbol.Value.ShouldEqual("}");
      symbol = st.ReadSymbol(); symbol.IsEndOfLine.ShouldBeTrue();
      // --- "}"
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._rbrace);
      symbol.Value.ShouldEqual("}");
      symbol = st.ReadSymbol(); symbol.IsEndOfLine.ShouldBeTrue();
      // --- "<NewLine>"
      symbol = st.ReadSymbol(); symbol.IsEndOfLine.ShouldBeTrue();
      // --- "public class B : A"
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._public);
      symbol.Value.ShouldEqual("public");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._class);
      symbol.Value.ShouldEqual("class");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.IsIdentifier.ShouldBeTrue();
      symbol.Value.ShouldEqual("B");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._colon);
      symbol.Value.ShouldEqual(":");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.IsIdentifier.ShouldBeTrue();
      symbol.Value.ShouldEqual("A");
      symbol = st.ReadSymbol(); symbol.IsEndOfLine.ShouldBeTrue();
      // --- "{"
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._lbrace);
      symbol.Value.ShouldEqual("{");
      symbol = st.ReadSymbol(); symbol.IsEndOfLine.ShouldBeTrue();
      // --- "  public new class C {}"
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._public);
      symbol.Value.ShouldEqual("public");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._new);
      symbol.Value.ShouldEqual("new");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._class);
      symbol.Value.ShouldEqual("class");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.IsIdentifier.ShouldBeTrue();
      symbol.Value.ShouldEqual("C");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._lbrace);
      symbol.Value.ShouldEqual("{");
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._rbrace);
      symbol.Value.ShouldEqual("}");
      symbol = st.ReadSymbol(); symbol.IsEndOfLine.ShouldBeTrue();
      // --- "}"
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._rbrace);
      symbol.Value.ShouldEqual("}");
      symbol = st.ReadSymbol(); symbol.IsEndOfLine.ShouldBeTrue();
      // --- "<NewLine>"
      symbol = st.ReadSymbol(); symbol.IsEndOfLine.ShouldBeTrue();
      // --- "public new class C {}"
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._public);
      symbol.Value.ShouldEqual("public");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._new);
      symbol.Value.ShouldEqual("new");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._class);
      symbol.Value.ShouldEqual("class");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.IsIdentifier.ShouldBeTrue();
      symbol.Value.ShouldEqual("C");
      symbol = st.ReadSymbol(); symbol.IsWhitespace.ShouldBeTrue();
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._lbrace);
      symbol.Value.ShouldEqual("{");
      symbol = st.ReadSymbol(); symbol.Kind.ShouldEqual(CSharpParser._rbrace);
      symbol.Value.ShouldEqual("}");
      symbol = st.ReadSymbol(); symbol.IsEndOfLine.ShouldBeTrue();
    }
  }
}
