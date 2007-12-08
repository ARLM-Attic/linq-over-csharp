using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class ParserTests: ParserTestBed
  {
    [TestMethod]
    public void GlobalAttributesAreOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("GlobalAttributes.cs");
      parser.AddAssemblyReference("System.Xml");
      Assert.IsTrue(InvokeParser(parser));
      SourceFile file = parser.Files[0];
      Assert.AreEqual(file.GlobalAttributes.Count, 12);
      foreach (AttributeDeclaration attr in file.GlobalAttributes)
      {
        Assert.AreEqual(attr.Scope, "assembly");
      }
      Assert.AreEqual(file.GlobalAttributes[0].Name, "AssemblyTitle");
      Assert.AreEqual(file.GlobalAttributes[10].Name, "AssemblyVersion");
      Assert.AreEqual(file.GlobalAttributes[11].Name, "AssemblyFileVersion");
    }

    [TestMethod]
    public void EnumParsingIsOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("EnumTest.cs");
      Assert.IsTrue(InvokeParser(parser));
      SourceFile file = parser.Files[0];
      EnumDeclaration et = file.NestedNamespaces[0].TypeDeclarations[0] as EnumDeclaration;
      Assert.IsNotNull(et);
      Assert.AreEqual(et.Name, "EnumTest1");
      Assert.AreEqual(et.BaseTypeName, "byte");
      Assert.AreEqual(et.Attributes.Count, 2);
      Assert.AreEqual(et.Attributes[0].Name, "Serializable");
      Assert.AreEqual(et.Attributes[1].Name, "Flags");
      Assert.AreEqual(et.Values.Count, 4);
      Assert.AreEqual(et.Values[0].Name, "Value1");
      Assert.AreEqual(et.Values[0].Attributes.Count, 0);
      Assert.AreEqual(et.Values[1].Name, "Value2");
      Assert.AreEqual(et.Values[1].Attributes.Count, 0);
      Assert.AreEqual(et.Values[2].Name, "Value3");
      Assert.AreEqual(et.Values[2].Attributes.Count, 0);
      Assert.AreEqual(et.Values[3].Name, "Value4");
      Assert.AreEqual(et.Values[3].Attributes.Count, 0);

      et = file.NestedNamespaces[0].TypeDeclarations[1] as EnumDeclaration;
      Assert.IsNotNull(et);
      Assert.AreEqual(et.Name, "EnumTest2");
      Assert.IsFalse(et.HasBaseType);
      Assert.AreEqual(et.Attributes.Count, 1);
      Assert.AreEqual(et.Attributes[0].Name, "Flags");
      Assert.AreEqual(et.Values.Count, 4);
      Assert.AreEqual(et.Values[0].Name, "Value1");
      Assert.AreEqual(et.Values[0].Attributes.Count, 1);
      Assert.AreEqual(et.Values[0].Attributes[0].Name, "Description");
      Assert.AreEqual(et.Values[1].Name, "Value2");
      Assert.AreEqual(et.Values[1].Attributes.Count, 0);
      Assert.AreEqual(et.Values[2].Name, "Value3");
      Assert.AreEqual(et.Values[2].Attributes.Count, 0);
      Assert.AreEqual(et.Values[3].Name, "Value4");
      Assert.AreEqual(et.Values[3].Attributes.Count, 0);
    }

    [TestMethod]
    public void ConstDeclarationsAreOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("ConstDeclarations.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[0];
      Assert.AreEqual(td.Members.Count, 14);

      ConstDeclaration cd = td.Members[0] as ConstDeclaration;
      Assert.IsNotNull(cd);
      Assert.AreEqual(cd.Name, "a");
      Assert.AreEqual(cd.DeclaredVisibility, Visibility.Private);
      Assert.AreEqual(cd.ResultingType.Name, "int");
      Assert.IsFalse(cd.IsNew);

      cd = td.Members[1] as ConstDeclaration;
      Assert.IsNotNull(cd);
      Assert.AreEqual(cd.Name, "b");
      Assert.AreEqual(cd.DeclaredVisibility, Visibility.Public);
      Assert.AreEqual(cd.ResultingType.Name, "string");
      Assert.IsFalse(cd.IsNew);

      cd = td.Members[2] as ConstDeclaration;
      Assert.IsNotNull(cd);
      Assert.AreEqual(cd.Name, "c");
      Assert.AreEqual(cd.DeclaredVisibility, Visibility.Internal);
      Assert.AreEqual(cd.ResultingType.Name, "char");
      Assert.IsTrue(cd.IsNew);

      cd = td.Members[3] as ConstDeclaration;
      Assert.IsNotNull(cd);
      Assert.AreEqual(cd.Name, "d");
      Assert.AreEqual(cd.DeclaredVisibility, Visibility.Protected);
      Assert.AreEqual(cd.ResultingType.FullName, "System.Double");
      Assert.IsFalse(cd.IsNew);
    }

    [TestMethod]
    public void DelegatesAndEventsAreOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("EventDeclarations.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[0];
      DelegateDeclaration dd = td as DelegateDeclaration;
      Assert.IsNotNull(dd);
      Assert.AreEqual(dd.Name, "NewDelegate");
      Assert.IsTrue(dd.ReturnType.IsVoid);

      td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[1];
      Assert.AreEqual(td.NestedTypes.Count, 3);

      dd = td.NestedTypes[0] as DelegateDeclaration;
      Assert.IsNotNull(dd);
      Assert.AreEqual(dd.Name, "MyEvent1`2");
      Assert.AreEqual(dd.DeclaredVisibility, Visibility.Private);
      Assert.IsTrue(dd.ReturnType.IsVoid);
      Assert.AreEqual(dd.TypeParameters.Count, 2);
      Assert.AreEqual(dd.TypeParameters[0].Name, "T");
      Assert.AreEqual(dd.TypeParameters[1].Name, "U");

      dd = td.NestedTypes[1] as DelegateDeclaration;
      Assert.IsNotNull(dd);
      Assert.AreEqual(dd.Name, "MyEvent2");
      Assert.AreEqual(dd.DeclaredVisibility, Visibility.Protected);
      Assert.IsTrue(dd.ReturnType.IsVoid);
      Assert.AreEqual(dd.TypeParameters.Count, 0);

      dd = td.NestedTypes[2] as DelegateDeclaration;
      Assert.IsNotNull(dd);
      Assert.AreEqual(dd.Name, "MyEvent3`1");
      Assert.AreEqual(dd.DeclaredVisibility, Visibility.Public);
      Assert.AreEqual(dd.ReturnType.Name, "T");
      Assert.AreEqual(dd.TypeParameters.Count, 1);
      Assert.AreEqual(dd.TypeParameters[0].Name, "T");

      Assert.AreEqual(td.Members.Count, 5);

      FieldDeclaration fd = td.Members[0] as FieldDeclaration;
      Assert.IsNotNull(fd);
      Assert.IsTrue(fd.IsEvent);
      Assert.AreEqual(fd.Name, "_Event1");
      Assert.AreEqual(fd.DeclaredVisibility, Visibility.Public);
      Assert.AreEqual(fd.ResultingType.Name, "NewDelegate");

      fd = td.Members[1] as FieldDeclaration;
      Assert.IsNotNull(fd);
      Assert.IsTrue(fd.IsEvent);
      Assert.AreEqual(fd.Name, "_Event2");
      Assert.AreEqual(fd.DeclaredVisibility, Visibility.Public);
      Assert.AreEqual(fd.ResultingType.Name, "NewDelegate");

      fd = td.Members[2] as FieldDeclaration;
      Assert.IsNotNull(fd);
      Assert.IsFalse(fd.IsEvent);
      Assert.AreEqual(fd.Name, "_IntField1");
      Assert.AreEqual(fd.DeclaredVisibility, Visibility.Protected);
      Assert.IsTrue(fd.IsVolatile);
      Assert.AreEqual(fd.ResultingType.Name, "int");

      fd = td.Members[3] as FieldDeclaration;
      Assert.IsNotNull(fd);
      Assert.IsFalse(fd.IsEvent);
      Assert.AreEqual(fd.Name, "_IntField2");
      Assert.AreEqual(fd.DeclaredVisibility, Visibility.Protected);
      Assert.IsTrue(fd.IsVolatile);
      Assert.AreEqual(fd.ResultingType.Name, "int");

      fd = td.Members[4] as FieldDeclaration;
      Assert.IsNotNull(fd);
      Assert.IsTrue(fd.IsEvent);
      Assert.AreEqual(fd.Name, "_Event3");
      Assert.AreEqual(fd.DeclaredVisibility, Visibility.Private);
      Assert.AreEqual(fd.ResultingType.ParametrizedName,
        "CSharpParserTest.TestFiles.MyEvent1<System.Collections.Generic.List<System.Int32>, System.Collections.Generic.List<System.String>>");
      Assert.AreEqual(fd.ResultingType.Arguments.Count, 2);
      Assert.AreEqual(fd.ResultingType.Arguments[0].ParametrizedName, "System.Collections.Generic.List<System.Int32>");
      Assert.AreEqual(fd.ResultingType.Arguments[1].ParametrizedName, "System.Collections.Generic.List<System.String>");
    }

    [TestMethod]
    public void PropertiesAreOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("PropertyDeclarations.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[1];
      Assert.IsNotNull(td);
      Assert.AreEqual(td.Members.Count, 9);

      PropertyDeclaration pd = td.Members[3] as PropertyDeclaration;
      Assert.IsNotNull(pd);
      Assert.AreEqual(pd.Name, "IntProp1");
      Assert.AreEqual(pd.ResultingType.Name, "int");
      Assert.IsTrue(pd.HasGetter);
      Assert.IsTrue(pd.HasSetter);
      Assert.AreEqual(pd.Getter.DeclaredVisibility, Visibility.Default);
      Assert.IsTrue(pd.Getter.HasBody);
      Assert.AreEqual(pd.Setter.DeclaredVisibility, Visibility.Protected);
      Assert.IsTrue(pd.Setter.HasBody);

      pd = td.Members[4] as PropertyDeclaration;
      Assert.IsNotNull(pd);
      Assert.AreEqual(pd.Name, "IntProp2");
      Assert.AreEqual(pd.ResultingType.Name, "int");
      Assert.IsTrue(pd.HasGetter);
      Assert.IsTrue(pd.HasSetter);
      Assert.AreEqual(pd.Getter.DeclaredVisibility, Visibility.Default);
      Assert.IsTrue(pd.Getter.HasBody);
      Assert.AreEqual(pd.Setter.DeclaredVisibility, Visibility.Private);
      Assert.IsTrue(pd.Setter.HasBody);

      pd = td.Members[5] as PropertyDeclaration;
      Assert.IsNotNull(pd);
      Assert.AreEqual(pd.Name, "StringProp3");
      Assert.AreEqual(pd.ResultingType.Name, "string");
      Assert.IsTrue(pd.HasGetter);
      Assert.IsFalse(pd.HasSetter);
      Assert.AreEqual(pd.Getter.DeclaredVisibility, Visibility.Default);
      Assert.IsTrue(pd.Getter.HasBody);

      IndexerDeclaration ind = td.Members[6] as IndexerDeclaration;
      Assert.IsNotNull(ind);
      Assert.AreEqual(ind.Name, "this");
      Assert.AreEqual(ind.ResultingType.Name, "string");
      Assert.AreEqual(ind.FormalParameters.Count, 2);
      Assert.IsTrue(pd.HasGetter);
      Assert.IsFalse(pd.HasSetter);
      Assert.AreEqual(pd.Getter.DeclaredVisibility, Visibility.Default);
      Assert.IsTrue(pd.Getter.HasBody);

      FormalParameter fp = ind.FormalParameters[0];
      Assert.AreEqual(fp.Name, "index1");
      Assert.AreEqual(fp.Type.TailName, "int");
      Assert.AreEqual(fp.Kind, FormalParameterKind.In);

      fp = ind.FormalParameters[1];
      Assert.AreEqual(fp.Name, "index2");
      Assert.AreEqual(fp.Type.TailName, "string");
      Assert.AreEqual(fp.Kind, FormalParameterKind.In);

      ind = td.Members[7] as IndexerDeclaration;
      Assert.IsNotNull(ind);
      Assert.AreEqual(ind.Name, "ExplicitIntf");
      Assert.AreEqual(ind.ResultingType.Name, "int");
      Assert.AreEqual(ind.FormalParameters.Count, 2);

      fp = ind.FormalParameters[0];
      Assert.AreEqual(fp.Name, "index1");
      Assert.AreEqual(fp.Type.TailName, "int");
      Assert.AreEqual(fp.Kind, FormalParameterKind.In);

      fp = ind.FormalParameters[1];
      Assert.AreEqual(fp.Name, "index2");
      Assert.AreEqual(fp.Type.TailName, "string");
      Assert.AreEqual(fp.Kind, FormalParameterKind.In);

      pd = td.Members[8] as PropertyDeclaration;
      Assert.IsNotNull(pd);
      Assert.AreEqual(pd.Name, "IntProp3");
      Assert.AreEqual(pd.ResultingType.Name, "int");
      Assert.IsTrue(pd.HasGetter);
      Assert.IsTrue(pd.HasSetter);
      Assert.AreEqual(pd.Getter.DeclaredVisibility, Visibility.Default);
      Assert.IsFalse(pd.Getter.HasBody);
      Assert.AreEqual(pd.Setter.DeclaredVisibility, Visibility.Default);
      Assert.IsFalse(pd.Setter.HasBody);
    }

    [TestMethod]
    public void MethodsAreOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("MethodDeclarations.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[0];
      Assert.IsNotNull(td);

      MethodDeclaration md = td.Members[0] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.IsTrue(md.ResultingType.IsVoid);
      Assert.AreEqual(md.Name, "SimpleMethod");
      Assert.AreEqual(md.DeclaredVisibility, Visibility.Public);
      Assert.IsTrue(md.IsStatic);
      Assert.AreEqual(md.FormalParameters.Count, 0);

      md = td.Members[1] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.ResultingType.TailName, "string");
      Assert.AreEqual(md.Name, "ReverseString");
      Assert.AreEqual(md.DeclaredVisibility, Visibility.Protected);
      Assert.AreEqual(md.FormalParameters.Count, 1);
      FormalParameter fp = md.FormalParameters[0];
      Assert.AreEqual(fp.Name, "source");
      Assert.AreEqual(fp.Type.TailName, "string");

      md = td.Members[2] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.ResultingType.TailName, "T");
      Assert.AreEqual(md.Name, "AbstractMethod");
      Assert.AreEqual(md.DeclaredVisibility, Visibility.Protected);
      Assert.IsTrue(md.IsAbstract);
      Assert.AreEqual(md.FormalParameters.Count, 2);
      fp = md.FormalParameters[0];
      Assert.AreEqual(fp.Name, "par1");
      Assert.AreEqual(fp.Type.TailName, "U");
      fp = md.FormalParameters[1];
      Assert.AreEqual(fp.Name, "par2");
      Assert.AreEqual(fp.Type.TailName, "V");
      Assert.AreEqual(md.TypeParameters.Count, 3);
      Assert.AreEqual(md.TypeParameters[0].Name, "T");
      Assert.AreEqual(md.TypeParameters[1].Name, "U");
      Assert.AreEqual(md.TypeParameters[2].Name, "V");

      md = td.Members[3] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.ResultingType.TailName, "IEnumerator");
      Assert.AreEqual(md.FullName, "IEnumerable.GetEnumerator");
      Assert.AreEqual(md.Name, "GetEnumerator");
      Assert.AreEqual(md.DeclaredVisibility, Visibility.Default);
      Assert.AreEqual(md.FormalParameters.Count, 0);
    }

    [TestMethod]
    public void CastOperatorsAreOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("CastOperatorDeclarations.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[0];
      Assert.IsNotNull(td);

      CastOperatorDeclaration md = td.Members[0] as CastOperatorDeclaration;
      Assert.IsNotNull(md);
      Assert.IsTrue(md.IsImplicit);
      Assert.AreEqual(md.ResultingType.TailName, "int");
      Assert.AreEqual(md.FormalParameters.Count, 1);
      FormalParameter fp = md.FormalParameters[0];
      Assert.AreEqual(fp.Name, "dec");
      Assert.AreEqual(fp.Type.Name, "CastOperatorDeclaration");

      md = td.Members[1] as CastOperatorDeclaration;
      Assert.IsNotNull(md);
      Assert.IsTrue(md.IsExplicit);
      Assert.AreEqual(md.ResultingType.TailName, "CastOperatorDeclaration");
      Assert.AreEqual(md.FormalParameters.Count, 1);
      fp = md.FormalParameters[0];
      Assert.AreEqual(fp.Name, "value");
      Assert.AreEqual(fp.Type.Name, "int");
    }

    [TestMethod]
    public void SimpleStatementsAreOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("SimpleStatements.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[0];
      Assert.IsNotNull(td);
      MethodDeclaration md = td.Members[0] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.Statements.Count, 4);
      Assert.IsTrue(md.Statements[0] is EmptyStatement);
      Assert.IsTrue(md.Statements[1] is CheckedBlock);
      Assert.IsTrue((md.Statements[1] as CheckedBlock).Statements[0] is EmptyStatement);
      Assert.AreSame(md.Statements[1], (md.Statements[1] as CheckedBlock).Statements[0].Parent);
      Assert.IsTrue(md.Statements[2] is UncheckedBlock);
      Assert.IsTrue((md.Statements[2] as UncheckedBlock).Statements[0] is EmptyStatement);
      Assert.AreSame(md.Statements[2], (md.Statements[2] as UncheckedBlock).Statements[0].Parent);
      Assert.IsTrue(md.Statements[3] is UnsafeBlock);
      Assert.IsTrue((md.Statements[3] as UnsafeBlock).Statements[0] is EmptyStatement);
      Assert.AreSame(md.Statements[3], (md.Statements[3] as UnsafeBlock).Statements[0].Parent);
    }

    [TestMethod]
    public void ComparisonOperatorIsOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("ComparisonOperator.cs");
      Assert.IsTrue(InvokeParser(parser));
    }

    [TestMethod]
    public void DuplicateModifiersDiscovered()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile("DuplicateModifiers.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 15);
      for (int i = 0; i < 14; i++)
      {
        Assert.AreEqual(parser.Errors[i].Code, "CS1004");
      }
      Assert.AreEqual(parser.Errors[14].Code, "CS0106");
    }
  }
}