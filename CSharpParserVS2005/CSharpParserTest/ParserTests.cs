using System;
using System.Collections.Generic;
using CSharpParser;
using CSharpParser.ParserFiles;
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
    public void BasicTestIsOK()
    {
      Assert.IsTrue(InvokeParser("Parser.cs"));
    }

    [TestMethod]
    public void GlobalAttributesAreOK()
    {
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile("GlobalAttributes.cs");
      Assert.IsTrue(InvokeParser(parser));
      ProjectFile file = parser.Files[0];
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
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile("EnumTest.cs");
      Assert.IsTrue(InvokeParser(parser));
      ProjectFile file = parser.Files[0];
      EnumDeclaration et = file.Namespaces[0].TypeDeclarations[0] as EnumDeclaration;
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

      et = file.Namespaces[0].TypeDeclarations[1] as EnumDeclaration;
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
    public void TypeParameterConstraintsAreOK()
    {
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile("TypeParamConstraintTest.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].Namespaces[0].TypeDeclarations[0];
      List<TypeParameterConstraint> coList = td.ParameterConstraints;
      Assert.AreEqual(coList.Count, 6);

      TypeParameterConstraint con = coList[0];
      Assert.AreEqual(con.Name, "A");
      Assert.AreEqual(con.ParameterType, ParameterConstraintType.Class);
      Assert.AreEqual(con.Constraints.Count, 1);
      Assert.AreEqual(con.Constraints[0].Name, "class");
      Assert.IsFalse(con.HasNew);

      con = coList[1];
      Assert.AreEqual(con.Name, "B");
      Assert.AreEqual(con.ParameterType, ParameterConstraintType.Struct);
      Assert.AreEqual(con.Constraints.Count, 1);
      Assert.AreEqual(con.Constraints[0].Name, "struct");
      Assert.IsFalse(con.HasNew);

      con = coList[2];
      Assert.AreEqual(con.Name, "C");
      Assert.AreEqual(con.ParameterType, ParameterConstraintType.Type);
      Assert.AreEqual(con.Constraints.Count, 0);
      Assert.IsTrue(con.HasNew);

      con = coList[3];
      Assert.AreEqual(con.Name, "D");
      Assert.AreEqual(con.ParameterType, ParameterConstraintType.Type);
      Assert.AreEqual(con.Constraints.Count, 2);
      Assert.AreEqual(con.Constraints[0].Name, "ArrayList");
      Assert.AreEqual(con.Constraints[1].Name, "IEnumerable");
      Assert.AreEqual(con.Constraints[1].Arguments.Count, 1);
      Assert.AreEqual(con.Constraints[1].Arguments[0].Name, "A");
      Assert.IsFalse(con.HasNew);

      con = coList[4];
      Assert.AreEqual(con.Name, "E");
      Assert.AreEqual(con.ParameterType, ParameterConstraintType.Type);
      Assert.AreEqual(con.Constraints.Count, 1);
      Assert.AreEqual(con.Constraints[0].Name, "D");
      Assert.IsTrue(con.HasNew);

      con = coList[5];
      Assert.AreEqual(con.Name, "F");
      Assert.AreEqual(con.ParameterType, ParameterConstraintType.Type);
      Assert.AreEqual(con.Constraints.Count, 3);
      Assert.AreEqual(con.Constraints[0].Name, "ArrayList");
      Assert.AreEqual(con.Constraints[1].Name, "IEnumerable");
      Assert.AreEqual(con.Constraints[2].Name, "IEquatable");
      Assert.AreEqual(con.Constraints[1].Arguments.Count, 1);
      Assert.AreEqual(con.Constraints[1].Arguments[0].Name, "B");
      Assert.AreEqual(con.Constraints[2].Arguments.Count, 1);
      Assert.AreEqual(con.Constraints[2].Arguments[0].Name, "D");
      Assert.IsTrue(con.HasNew);
    }

    [TestMethod]
    public void ConstDeclarationsAreOK()
    {
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile("ConstDeclarations.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].Namespaces[0].TypeDeclarations[0];
      Assert.AreEqual(td.Members.Count, 14);

      ConstDeclaration cd = td.Members[0] as ConstDeclaration;
      Assert.IsNotNull(cd);
      Assert.AreEqual(cd.Name, "a");
      Assert.AreEqual(cd.Visibility, Visibility.Private);
      Assert.AreEqual(cd.ResultingType.Name, "int");
      Assert.IsFalse(cd.IsNew);

      cd = td.Members[1] as ConstDeclaration;
      Assert.IsNotNull(cd);
      Assert.AreEqual(cd.Name, "b");
      Assert.AreEqual(cd.Visibility, Visibility.Public);
      Assert.AreEqual(cd.ResultingType.Name, "string");
      Assert.IsFalse(cd.IsNew);

      cd = td.Members[2] as ConstDeclaration;
      Assert.IsNotNull(cd);
      Assert.AreEqual(cd.Name, "c");
      Assert.AreEqual(cd.Visibility, Visibility.Internal);
      Assert.AreEqual(cd.ResultingType.Name, "char");
      Assert.IsTrue(cd.IsNew);

      cd = td.Members[3] as ConstDeclaration;
      Assert.IsNotNull(cd);
      Assert.AreEqual(cd.Name, "d");
      Assert.AreEqual(cd.Visibility, Visibility.Protected);
      Assert.AreEqual(cd.ResultingType.FullName, "System.Double");
      Assert.IsFalse(cd.IsNew);
    }

    [TestMethod]
    public void DelegatesAndEventsAreOK()
    {
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile("EventDeclarations.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].Namespaces[0].TypeDeclarations[0];
      DelegateDeclaration dd = td as DelegateDeclaration;
      Assert.IsNotNull(dd);
      Assert.AreEqual(dd.Name, "NewDelegate");
      Assert.AreEqual(dd.ReturnType.Kind, TypeKind.@void);

      td = parser.Files[0].Namespaces[0].TypeDeclarations[1];
      Assert.AreEqual(td.NestedTypes.Count, 3);

      dd = td.NestedTypes[0] as DelegateDeclaration;
      Assert.IsNotNull(dd);
      Assert.AreEqual(dd.Name, "MyEvent1");
      Assert.AreEqual(dd.Visibility, Visibility.Private);
      Assert.AreEqual(dd.ReturnType.Kind, TypeKind.@void);
      Assert.AreEqual(dd.TypeParameters.Count, 2);
      Assert.AreEqual(dd.TypeParameters[0].Name, "T");
      Assert.AreEqual(dd.TypeParameters[1].Name, "U");

      dd = td.NestedTypes[1] as DelegateDeclaration;
      Assert.IsNotNull(dd);
      Assert.AreEqual(dd.Name, "MyEvent2");
      Assert.AreEqual(dd.Visibility, Visibility.Protected);
      Assert.AreEqual(dd.ReturnType.Kind, TypeKind.@void);
      Assert.AreEqual(dd.TypeParameters.Count, 0);

      dd = td.NestedTypes[2] as DelegateDeclaration;
      Assert.IsNotNull(dd);
      Assert.AreEqual(dd.Name, "MyEvent3");
      Assert.AreEqual(dd.Visibility, Visibility.Public);
      Assert.AreEqual(dd.ReturnType.Name, "T");
      Assert.AreEqual(dd.TypeParameters.Count, 1);
      Assert.AreEqual(dd.TypeParameters[0].Name, "T");

      Assert.AreEqual(td.Members.Count, 5);

      FieldDeclaration fd = td.Members[0] as FieldDeclaration;
      Assert.IsNotNull(fd);
      Assert.IsTrue(fd.IsEvent);
      Assert.AreEqual(fd.Name, "_Event1");
      Assert.AreEqual(fd.Visibility, Visibility.Public);
      Assert.AreEqual(fd.ResultingType.Name, "NewDelegate");

      fd = td.Members[1] as FieldDeclaration;
      Assert.IsNotNull(fd);
      Assert.IsTrue(fd.IsEvent);
      Assert.AreEqual(fd.Name, "_Event2");
      Assert.AreEqual(fd.Visibility, Visibility.Public);
      Assert.AreEqual(fd.ResultingType.Name, "NewDelegate");

      fd = td.Members[2] as FieldDeclaration;
      Assert.IsNotNull(fd);
      Assert.IsFalse(fd.IsEvent);
      Assert.AreEqual(fd.Name, "_IntField1");
      Assert.AreEqual(fd.Visibility, Visibility.Protected);
      Assert.IsTrue(fd.IsVolatile);
      Assert.AreEqual(fd.ResultingType.Name, "int");

      fd = td.Members[3] as FieldDeclaration;
      Assert.IsNotNull(fd);
      Assert.IsFalse(fd.IsEvent);
      Assert.AreEqual(fd.Name, "_IntField2");
      Assert.AreEqual(fd.Visibility, Visibility.Protected);
      Assert.IsTrue(fd.IsVolatile);
      Assert.AreEqual(fd.ResultingType.Name, "int");

      fd = td.Members[4] as FieldDeclaration;
      Assert.IsNotNull(fd);
      Assert.IsTrue(fd.IsEvent);
      Assert.AreEqual(fd.Name, "_Event3");
      Assert.AreEqual(fd.Visibility, Visibility.Private);
      Assert.AreEqual(fd.ResultingType.Name, "MyEvent1");
      Assert.AreEqual(fd.ResultingType.Arguments.Count, 2);
      Assert.AreEqual(fd.ResultingType.Arguments[0].Name, "int");
      Assert.AreEqual(fd.ResultingType.Arguments[1].Name, "string");
    }

    [TestMethod]
    public void PropertiesAreOK()
    {
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile("PropertyDeclarations.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].Namespaces[0].TypeDeclarations[1];
      Assert.IsNotNull(td);
      Assert.AreEqual(td.Members.Count, 9);

      PropertyDeclaration pd = td.Members[3] as PropertyDeclaration;
      Assert.IsNotNull(pd);
      Assert.AreEqual(pd.Name, "IntProp1");
      Assert.AreEqual(pd.ResultingType.Name, "int");
      Assert.IsTrue(pd.HasGetter);
      Assert.IsTrue(pd.HasSetter);
      Assert.AreEqual(pd.Getter.Visibility, Visibility.Default);
      Assert.IsTrue(pd.Getter.HasBody);
      Assert.AreEqual(pd.Setter.Visibility, Visibility.Protected);
      Assert.IsTrue(pd.Setter.HasBody);

      pd = td.Members[4] as PropertyDeclaration;
      Assert.IsNotNull(pd);
      Assert.AreEqual(pd.Name, "IntProp2");
      Assert.AreEqual(pd.ResultingType.Name, "int");
      Assert.IsTrue(pd.HasGetter);
      Assert.IsTrue(pd.HasSetter);
      Assert.AreEqual(pd.Getter.Visibility, Visibility.Default);
      Assert.IsTrue(pd.Getter.HasBody);
      Assert.AreEqual(pd.Setter.Visibility, Visibility.Private);
      Assert.IsTrue(pd.Setter.HasBody);

      pd = td.Members[5] as PropertyDeclaration;
      Assert.IsNotNull(pd);
      Assert.AreEqual(pd.Name, "StringProp3");
      Assert.AreEqual(pd.ResultingType.Name, "string");
      Assert.IsTrue(pd.HasGetter);
      Assert.IsFalse(pd.HasSetter);
      Assert.AreEqual(pd.Getter.Visibility, Visibility.Default);
      Assert.IsTrue(pd.Getter.HasBody);

      IndexerDeclaration ind = td.Members[6] as IndexerDeclaration;
      Assert.IsNotNull(ind);
      Assert.AreEqual(ind.Name, "");
      Assert.AreEqual(ind.ResultingType.Name, "string");
      Assert.AreEqual(ind.FormalParameters.Count, 2);
      Assert.IsTrue(pd.HasGetter);
      Assert.IsFalse(pd.HasSetter);
      Assert.AreEqual(pd.Getter.Visibility, Visibility.Default);
      Assert.IsTrue(pd.Getter.HasBody);

      FormalParameter fp = ind.FormalParameters[0];
      Assert.AreEqual(fp.Name, "index1");
      Assert.AreEqual(fp.Type.RightmostName, "int");
      Assert.AreEqual(fp.Kind, FormalParameterKind.In);

      fp = ind.FormalParameters[1];
      Assert.AreEqual(fp.Name, "index2");
      Assert.AreEqual(fp.Type.RightmostName, "string");
      Assert.AreEqual(fp.Kind, FormalParameterKind.In);

      ind = td.Members[7] as IndexerDeclaration;
      Assert.IsNotNull(ind);
      Assert.AreEqual(ind.Name, "ExplicitIntf");
      Assert.AreEqual(ind.ResultingType.Name, "int");
      Assert.AreEqual(ind.FormalParameters.Count, 2);

      fp = ind.FormalParameters[0];
      Assert.AreEqual(fp.Name, "index1");
      Assert.AreEqual(fp.Type.RightmostName, "int");
      Assert.AreEqual(fp.Kind, FormalParameterKind.In);

      fp = ind.FormalParameters[1];
      Assert.AreEqual(fp.Name, "index2");
      Assert.AreEqual(fp.Type.RightmostName, "string");
      Assert.AreEqual(fp.Kind, FormalParameterKind.In);

      pd = td.Members[8] as PropertyDeclaration;
      Assert.IsNotNull(pd);
      Assert.AreEqual(pd.Name, "IntProp3");
      Assert.AreEqual(pd.ResultingType.Name, "int");
      Assert.IsTrue(pd.HasGetter);
      Assert.IsTrue(pd.HasSetter);
      Assert.AreEqual(pd.Getter.Visibility, Visibility.Default);
      Assert.IsFalse(pd.Getter.HasBody);
      Assert.AreEqual(pd.Setter.Visibility, Visibility.Default);
      Assert.IsFalse(pd.Setter.HasBody);
    }

    [TestMethod]
    public void MethodsAreOK()
    {
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile("MethodDeclarations.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].Namespaces[0].TypeDeclarations[0];
      Assert.IsNotNull(td);

      MethodDeclaration md = td.Members[0] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.ResultingType.Kind, TypeKind.@void);
      Assert.AreEqual(md.Name, "SimpleMethod");
      Assert.AreEqual(md.Visibility, Visibility.Public);
      Assert.IsTrue(md.IsStatic);
      Assert.AreEqual(md.FormalParameters.Count, 0);

      md = td.Members[1] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.ResultingType.RightmostName, "string");
      Assert.AreEqual(md.Name, "ReverseString");
      Assert.AreEqual(md.Visibility, Visibility.Protected);
      Assert.AreEqual(md.FormalParameters.Count, 1);
      FormalParameter fp = md.FormalParameters[0];
      Assert.AreEqual(fp.Name, "source");
      Assert.AreEqual(fp.Type.RightmostName, "string");

      md = td.Members[2] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.ResultingType.RightmostName, "T");
      Assert.AreEqual(md.Name, "AbstractMethod");
      Assert.AreEqual(md.Visibility, Visibility.Private);
      Assert.IsTrue(md.IsAbstract);
      Assert.AreEqual(md.FormalParameters.Count, 2);
      fp = md.FormalParameters[0];
      Assert.AreEqual(fp.Name, "par1");
      Assert.AreEqual(fp.Type.RightmostName, "U");
      fp = md.FormalParameters[1];
      Assert.AreEqual(fp.Name, "par2");
      Assert.AreEqual(fp.Type.RightmostName, "V");
      Assert.AreEqual(md.TypeParameters.Count, 3);
      Assert.AreEqual(md.TypeParameters[0].Name, "T");
      Assert.AreEqual(md.TypeParameters[1].Name, "U");
      Assert.AreEqual(md.TypeParameters[2].Name, "V");

      md = td.Members[3] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.ResultingType.RightmostName, "IEnumerator");
      Assert.AreEqual(md.ExplicitName.FullName, "IEnumerable.GetEnumerator");
      Assert.AreEqual(md.Name, "GetEnumerator");
      Assert.AreEqual(md.Visibility, Visibility.Default);
      Assert.AreEqual(md.FormalParameters.Count, 0);
    }

    [TestMethod]
    public void CastOperatorsAreOK()
    {
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile("CastOperatorDeclarations.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].Namespaces[0].TypeDeclarations[0];
      Assert.IsNotNull(td);

      CastOperatorDeclaration md = td.Members[0] as CastOperatorDeclaration;
      Assert.IsNotNull(md);
      Assert.IsTrue(md.IsImplicit);
      Assert.AreEqual(md.ResultingType.RightmostName, "int");
      Assert.AreEqual(md.FormalParameters.Count, 1);
      FormalParameter fp = md.FormalParameters[0];
      Assert.AreEqual(fp.Name, "dec");
      Assert.AreEqual(fp.Type.Name, "CastOperatorDeclaration");

      md = td.Members[1] as CastOperatorDeclaration;
      Assert.IsNotNull(md);
      Assert.IsTrue(md.IsExplicit);
      Assert.AreEqual(md.ResultingType.RightmostName, "CastOperatorDeclaration");
      Assert.AreEqual(md.FormalParameters.Count, 1);
      fp = md.FormalParameters[0];
      Assert.AreEqual(fp.Name, "value");
      Assert.AreEqual(fp.Type.Name, "int");
    }

    [TestMethod]
    public void SimpleStatementsAreOK()
    {
      ProjectParser parser = new ProjectParser(WorkingFolder);
      parser.AddFile("SimpleStatements.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].Namespaces[0].TypeDeclarations[0];
      Assert.IsNotNull(td);
      MethodDeclaration md = td.Members[0] as MethodDeclaration;
      Assert.IsNotNull(md);
      Assert.AreEqual(md.Statements.Count, 4);
      Assert.IsTrue(md.Statements[0] is EmptyStatement);
      Assert.IsTrue(md.Statements[1] is CheckedBlock);
      Assert.IsTrue((md.Statements[1] as CheckedBlock).Statements[0] is EmptyStatement);
      Assert.AreSame(md.Statements[1], (md.Statements[1] as CheckedBlock).Statements[0].ParentBlock);
      Assert.IsTrue(md.Statements[2] is UncheckedBlock);
      Assert.IsTrue((md.Statements[2] as UncheckedBlock).Statements[0] is EmptyStatement);
      Assert.AreSame(md.Statements[2], (md.Statements[2] as UncheckedBlock).Statements[0].ParentBlock);
      Assert.IsTrue(md.Statements[3] is UnsafeBlock);
      Assert.IsTrue((md.Statements[3] as UnsafeBlock).Statements[0] is EmptyStatement);
      Assert.AreSame(md.Statements[3], (md.Statements[3] as UnsafeBlock).Statements[0].ParentBlock);
    }
  }
}