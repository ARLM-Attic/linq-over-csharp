using CSharpParser.ProjectModel;
using CSharpParser.Semantics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class TypeResolutions : ParserTestBed
  {
    [TestMethod]
    public void PrimitiveTypesOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\PrimitiveTypes.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration cd = parser.Files[0].TypeDeclarations[0];
      Assert.AreEqual(cd.Fields[0].ResultingType.ResolvingType, NetBinaryType.Boolean);
      Assert.AreEqual(cd.Fields[1].ResultingType.ResolvingType, NetBinaryType.Byte);
      Assert.AreEqual(cd.Fields[2].ResultingType.ResolvingType, NetBinaryType.Char);
      Assert.AreEqual(cd.Fields[3].ResultingType.ResolvingType, NetBinaryType.Decimal);
      Assert.AreEqual(cd.Fields[4].ResultingType.ResolvingType, NetBinaryType.Double);
      Assert.AreEqual(cd.Fields[5].ResultingType.ResolvingType, NetBinaryType.Single);
      Assert.AreEqual(cd.Fields[6].ResultingType.ResolvingType, NetBinaryType.Int32);
      Assert.AreEqual(cd.Fields[7].ResultingType.ResolvingType, NetBinaryType.Int64);
      Assert.AreEqual(cd.Fields[8].ResultingType.ResolvingType, NetBinaryType.Object);
      Assert.AreEqual(cd.Fields[9].ResultingType.ResolvingType, NetBinaryType.SByte);
      Assert.AreEqual(cd.Fields[10].ResultingType.ResolvingType, NetBinaryType.Int16);
      Assert.AreEqual(cd.Fields[11].ResultingType.ResolvingType, NetBinaryType.String);
      Assert.AreEqual(cd.Fields[12].ResultingType.ResolvingType, NetBinaryType.UInt32);
      Assert.AreEqual(cd.Fields[13].ResultingType.ResolvingType, NetBinaryType.UInt64);
      Assert.AreEqual(cd.Fields[14].ResultingType.ResolvingType, NetBinaryType.UInt16);
      MethodDeclaration md = cd.Methods[0];
      Assert.AreEqual(md.FormalParameters[0].Type.ResolvingType, NetBinaryType.Boolean);
      Assert.AreEqual(md.FormalParameters[1].Type.ResolvingType, NetBinaryType.Byte);
      Assert.AreEqual(md.FormalParameters[2].Type.ResolvingType, NetBinaryType.Char);
      Assert.AreEqual(md.FormalParameters[3].Type.ResolvingType, NetBinaryType.Decimal);
      Assert.AreEqual(md.FormalParameters[4].Type.ResolvingType, NetBinaryType.Double);
      Assert.AreEqual(md.FormalParameters[5].Type.ResolvingType, NetBinaryType.Single);
      Assert.AreEqual(md.FormalParameters[6].Type.ResolvingType, NetBinaryType.Int32);
      Assert.AreEqual(md.FormalParameters[7].Type.ResolvingType, NetBinaryType.Int64);
      Assert.AreEqual(md.FormalParameters[8].Type.ResolvingType, NetBinaryType.Object);
      Assert.AreEqual(md.FormalParameters[9].Type.ResolvingType, NetBinaryType.SByte);
      Assert.AreEqual(md.FormalParameters[10].Type.ResolvingType, NetBinaryType.Int16);
      Assert.AreEqual(md.FormalParameters[11].Type.ResolvingType, NetBinaryType.String);
      Assert.AreEqual(md.FormalParameters[12].Type.ResolvingType, NetBinaryType.UInt32);
      Assert.AreEqual(md.FormalParameters[13].Type.ResolvingType, NetBinaryType.UInt64);
      Assert.AreEqual(md.FormalParameters[14].Type.ResolvingType, NetBinaryType.UInt16);
    }

    [TestMethod]
    public void ConflictingNamespaceFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\InvalidNamespace1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0101");
    }

    [TestMethod]
    public void ConflictingNamespaceFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\InvalidNamespace2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0101");
    }

    [TestMethod]
    public void ConflictingNamespaceFails3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\InvalidNamespace3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0101");
      Assert.AreEqual(parser.Errors[1].Code, "CS0101");
      Assert.AreEqual(parser.Errors[2].Code, "CS0101");
    }

    [TestMethod]
    public void ImportSystemIsOk()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\UsingNamespace1.cs");
      parser.AddAliasedReference("EA1", "System.Data");
      parser.AddAliasedReference("EA1", "System.Xml");
      parser.AddAliasedReference("EA2", "System.Xml");
      parser.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void MissingExternAliasDetected1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\UsingNamespace2.cs");
      parser.AddAssemblyReference("System.Data");
      parser.AddAssemblyReference("System.Xml");
      parser.AddAliasedReference("EA1", "System.Data");
      parser.AddAliasedReference("EA2", "System.Xml");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 6);
      Assert.AreEqual(parser.Errors[0].Code, "CS0430");
      Assert.AreEqual(parser.Errors[1].Code, "CS0430");
      Assert.AreEqual(parser.Errors[2].Code, "CS0430");
      Assert.AreEqual(parser.Errors[3].Code, "CS0430");
      Assert.AreEqual(parser.Errors[4].Code, "CS0430");
      Assert.AreEqual(parser.Errors[5].Code, "CS0430");
    }

    [TestMethod]
    public void MissingExternAliasDetected2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\UsingNamespace2.cs");
      parser.AddAssemblyReference("System.Data");
      parser.AddAssemblyReference("System.Xml");
      parser.AddAliasedReference("MISSING3", "System.Data");
      parser.AddAliasedReference("MISSING4", "System.Xml");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0430");
      Assert.AreEqual(parser.Errors[1].Code, "CS0430");
      Assert.AreEqual(parser.Errors[2].Code, "CS0430");
      Assert.AreEqual(parser.Errors[3].Code, "CS0430");
    }

    [TestMethod]
    public void MissingExternAliasDetected3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\UsingNamespace2.cs");
      parser.AddAssemblyReference("System.Data");
      parser.AddAssemblyReference("System.Xml");
      parser.AddAliasedReference("MISSING1", "System.Data");
      parser.AddAliasedReference("MISSING2", "System.Xml");
      parser.AddAliasedReference("MISSING3", "System.Data");
      parser.AddAliasedReference("MISSING4", "System.Xml");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void MissingNamespaceHierarchy1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\UsingNamespace3.cs");
      parser.AddAssemblyReference("System.Data");
      parser.AddAssemblyReference("System.Xml");
      parser.AddAliasedReference("EA1", "System.Data");
      parser.AddAliasedReference("EA2", "System.Xml");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 6);
    }

    [TestMethod]
    public void NestedUsingsOk1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\UsingNamespace4.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void NestedUsingsOk2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\UsingNamespace5.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void InvalidGlobalNameFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeResolution\UsingNamespace6.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0400");
      Assert.AreEqual(parser.Errors[1].Code, "CS0400");
    }
  }
}