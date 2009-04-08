using CSharpFactory.ProjectModel;
using CSharpFactory.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class TypeDeclarations : ParserTestBed
  {
    [TestMethod]
    public void NewOnNotnestedClassFails()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1530");
    }

    [TestMethod]
    public void NewOnNotnestedStructFails()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"StructDeclaration\StructDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1530");
    }

    [TestMethod]
    public void NewOnNotnestedDelegateFails()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"DelegateDeclaration\DelegateDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1530");
    }

    [TestMethod]
    public void NewOnNotnestedEnumFails()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"EnumDeclaration\EnumDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1530");
    }

    [TestMethod]
    public void NewOnNotnestedInterfaceFails()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"InterfaceDeclaration\InterfaceDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS1530");
    }

    [TestMethod]
    public void SealedOrStaticAbstractFails()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0418");
      Assert.AreEqual(parser.Errors[1].Code, "CS0418");
      Assert.AreEqual(parser.Errors[2].Code, "CS0418");
      Assert.AreEqual(parser.Errors[3].Code, "CS0418");
    }

    [TestMethod]
    public void SealedAndStaticClassFails()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0441");
      Assert.AreEqual(parser.Errors[1].Code, "CS0441");
    }

    [TestMethod]
    public void InvalidClassModifiersFail()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0116");
      Assert.AreEqual(parser.Errors[2].Code, "CS0116");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
    }

    [TestMethod]
    public void InvalidStructModifiersFail()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"StructDeclaration\StructDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 7);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0116");
      Assert.AreEqual(parser.Errors[2].Code, "CS0116");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0106");
    }

    [TestMethod]
    public void InvalidInterfaceModifiersFail()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"InterfaceDeclaration\InterfaceDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 7);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0116");
      Assert.AreEqual(parser.Errors[2].Code, "CS0116");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0106");
    }

    [TestMethod]
    public void InvalidDelegateModifiersFail()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"DelegateDeclaration\DelegateDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 7);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0106");
      Assert.AreEqual(parser.Errors[2].Code, "CS0106");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0106");
    }

    [TestMethod]
    public void InvalidEnumModifiersFail()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"EnumDeclaration\EnumDeclaration1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 7);
      Assert.AreEqual(parser.Errors[0].Code, "CS0106");
      Assert.AreEqual(parser.Errors[1].Code, "CS0116");
      Assert.AreEqual(parser.Errors[2].Code, "CS0116");
      Assert.AreEqual(parser.Errors[3].Code, "CS0106");
      Assert.AreEqual(parser.Errors[4].Code, "CS0106");
      Assert.AreEqual(parser.Errors[5].Code, "CS0106");
      Assert.AreEqual(parser.Errors[6].Code, "CS0106");
    }

    [TestMethod]
    public void MemberWithEnclosingNameFails()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration5.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 9);
      Assert.AreEqual(parser.Errors[0].Code, "CS0542");
      Assert.AreEqual(parser.Errors[1].Code, "CS0542");
      Assert.AreEqual(parser.Errors[2].Code, "CS0542");
      Assert.AreEqual(parser.Errors[3].Code, "CS0542");
      Assert.AreEqual(parser.Errors[4].Code, "CS0101");
      Assert.AreEqual(parser.Errors[5].Code, "CS0101");
      Assert.AreEqual(parser.Errors[6].Code, "CS0101");
      Assert.AreEqual(parser.Errors[7].Code, "CS0542");
      Assert.AreEqual(parser.Errors[8].Code, "CS0542");
    }

    [TestMethod]
    public void TypeParameterWithInvalidNameFails()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration6.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0102");
      Assert.AreEqual(parser.Errors[1].Code, "CS0694");
      Assert.AreEqual(parser.Errors[2].Code, "CS0102");
    }

    [TestMethod]
    public void ClassDeclarationIsOk()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration7.cs");
      Assert.IsTrue(InvokeParser(parser));
      var source = parser.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(source.TypeDeclarations.Count, 1);
      var typeDecl = source.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "C");
      var nsDecl = source.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.TypeDeclarations.Count, 2);
      typeDecl = nsDecl.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "A");
      typeDecl = nsDecl.TypeDeclarations[1];
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "B");
    }

    [TestMethod]
    public void StructDeclarationIsOk()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"StructDeclaration\StructDeclaration3.cs");
      Assert.IsTrue(InvokeParser(parser));
      var source = parser.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(source.TypeDeclarations.Count, 1);
      var typeDecl = source.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(StructDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "C");
      var nsDecl = source.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.TypeDeclarations.Count, 2);
      typeDecl = nsDecl.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(StructDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "A");
      typeDecl = nsDecl.TypeDeclarations[1];
      Assert.IsInstanceOfType(typeDecl, typeof(StructDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "B");
    }

    [TestMethod]
    public void InterfaceDeclarationIsOk()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"InterfaceDeclaration\InterfaceDeclaration3.cs");
      Assert.IsTrue(InvokeParser(parser));
      var source = parser.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(source.TypeDeclarations.Count, 1);
      var typeDecl = source.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(InterfaceDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "C");
      var nsDecl = source.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.TypeDeclarations.Count, 2);
      typeDecl = nsDecl.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(InterfaceDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "A");
      typeDecl = nsDecl.TypeDeclarations[1];
      Assert.IsInstanceOfType(typeDecl, typeof(InterfaceDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "B");
    }

    [TestMethod]
    public void EnumDeclarationIsOk()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"EnumDeclaration\EnumDeclaration3.cs");
      Assert.IsTrue(InvokeParser(parser));
      var source = parser.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(source.TypeDeclarations.Count, 1);
      var typeDecl = source.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(EnumDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "C");
      var nsDecl = source.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.TypeDeclarations.Count, 2);
      typeDecl = nsDecl.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(EnumDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "A");
      typeDecl = nsDecl.TypeDeclarations[1];
      Assert.IsInstanceOfType(typeDecl, typeof(EnumDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "B");
    }

    [TestMethod]
    public void DelegateDeclarationIsOk()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"DelegateDeclaration\DelegateDeclaration3.cs");
      Assert.IsTrue(InvokeParser(parser));
      var source = parser.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(source.TypeDeclarations.Count, 1);
      var typeDecl = source.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(DelegateDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "C");
      var nsDecl = source.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.TypeDeclarations.Count, 2);
      typeDecl = nsDecl.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(DelegateDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "A");
      typeDecl = nsDecl.TypeDeclarations[1];
      Assert.IsInstanceOfType(typeDecl, typeof(DelegateDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "B");
    }

    [TestMethod]
    public void ClassDeclarationWithTypeParamsIsOk()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration8.cs");
      Assert.IsTrue(InvokeParser(parser));
      var source = parser.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(source.TypeDeclarations.Count, 1);
      var typeDecl = source.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "C");
      Assert.IsTrue(typeDecl.HasTypeParameters);
      Assert.AreEqual(typeDecl.TypeParameters.Count, 1);
      Assert.AreEqual(typeDecl.TypeParameters[0].Identifier, "Z");
      var nsDecl = source.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.TypeDeclarations.Count, 2);
      typeDecl = nsDecl.TypeDeclarations[0];
      Assert.IsFalse(typeDecl.HasTypeParameters);
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "A");
      typeDecl = nsDecl.TypeDeclarations[1];
      Assert.IsTrue(typeDecl.HasTypeParameters);
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "B");
      Assert.AreEqual(typeDecl.TypeParameters.Count, 2);
      Assert.AreEqual(typeDecl.TypeParameters[0].Identifier, "X");
      Assert.AreEqual(typeDecl.TypeParameters[1].Identifier, "Y");
    }

    [TestMethod]
    public void ClassDeclarationWithBaseListIsOk()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration8.cs");
      Assert.IsTrue(InvokeParser(parser));
      var source = parser.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(source.TypeDeclarations.Count, 1);
      var typeDecl = source.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "C");
      Assert.AreEqual(typeDecl.BaseTypes.Count, 1);
      Assert.AreEqual(typeDecl.BaseTypes[0].TypeTags.Count, 4);
      Assert.AreEqual(typeDecl.BaseTypes[0].TypeTags[0].Identifier, "System");
      Assert.AreEqual(typeDecl.BaseTypes[0].TypeTags[1].Identifier, "Collections");
      Assert.AreEqual(typeDecl.BaseTypes[0].TypeTags[2].Identifier, "Generic");
      Assert.AreEqual(typeDecl.BaseTypes[0].TypeTags[3].Identifier, "List");

      var nsDecl = source.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.TypeDeclarations.Count, 2);
      typeDecl = nsDecl.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "A");
      Assert.AreEqual(typeDecl.BaseTypes.Count, 2);
      Assert.AreEqual(typeDecl.BaseTypes[0].TypeTags.Count, 1);
      Assert.AreEqual(typeDecl.BaseTypes[0].TypeTags[0].Identifier, "List");
      Assert.AreEqual(typeDecl.BaseTypes[1].TypeTags.Count, 1);
      Assert.AreEqual(typeDecl.BaseTypes[1].TypeTags[0].Identifier, "IEnumerable");

      typeDecl = nsDecl.TypeDeclarations[1];
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "B");
      Assert.AreEqual(typeDecl.BaseTypes.Count, 1);
      Assert.AreEqual(typeDecl.BaseTypes[0].TypeTags.Count, 1);
      Assert.AreEqual(typeDecl.BaseTypes[0].TypeTags[0].Identifier, "Dictionary");
    }

    [TestMethod]
    public void ClassDeclarationWithTypeConstraintIsOk()
    {
      var parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"ClassDeclaration\ClassDeclaration8.cs");
      Assert.IsTrue(InvokeParser(parser));
      var source = parser.SyntaxTree.SourceFileNodes[0];
      Assert.AreEqual(source.TypeDeclarations.Count, 1);

      var typeDecl = source.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "C");
      Assert.AreEqual(typeDecl.TypeParameterConstraints.Count, 1);
      Assert.AreEqual(typeDecl.TypeParameterConstraints[0].ConstraintTags.Count, 2);
      Assert.IsTrue(typeDecl.TypeParameterConstraints[0].ConstraintTags[0].IsTypeName);
      Assert.AreEqual(typeDecl.TypeParameterConstraints[0].ConstraintTags[0].ConstraintToken.val, "Hashtable");
      Assert.IsTrue(typeDecl.TypeParameterConstraints[0].ConstraintTags[1].IsTypeName);
      Assert.AreEqual(typeDecl.TypeParameterConstraints[0].ConstraintTags[1].ConstraintToken.val, "IEnumerable");

      var nsDecl = source.NamespaceDeclarations[0];
      Assert.AreEqual(nsDecl.TypeDeclarations.Count, 2);
      typeDecl = nsDecl.TypeDeclarations[0];
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "A");
      Assert.AreEqual(typeDecl.TypeParameterConstraints.Count, 0);

      typeDecl = nsDecl.TypeDeclarations[1];
      Assert.IsInstanceOfType(typeDecl, typeof(ClassDeclarationNode));
      Assert.AreEqual(typeDecl.Name, "B");
      Assert.AreEqual(typeDecl.TypeParameterConstraints.Count, 2);
      Assert.AreEqual(typeDecl.TypeParameterConstraints[0].ConstraintTags.Count, 3);
      Assert.IsTrue(typeDecl.TypeParameterConstraints[0].ConstraintTags[0].IsClass);
      Assert.IsTrue(typeDecl.TypeParameterConstraints[0].ConstraintTags[1].IsTypeName);
      Assert.AreEqual(typeDecl.TypeParameterConstraints[0].ConstraintTags[1].ConstraintToken.val, "IEnumerable");
      Assert.IsTrue(typeDecl.TypeParameterConstraints[0].ConstraintTags[2].IsNew);
      Assert.AreEqual(typeDecl.TypeParameterConstraints[1].ConstraintTags.Count, 1);
      Assert.IsTrue(typeDecl.TypeParameterConstraints[1].ConstraintTags[0].IsStruct);
    }
  }
}