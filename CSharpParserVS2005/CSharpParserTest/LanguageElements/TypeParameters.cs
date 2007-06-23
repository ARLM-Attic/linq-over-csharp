using System.Collections.Generic;
using CSharpParser;
using CSharpParser.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class TypeParameters: ParserTestBed
  {
    [TestMethod]
    public void TypeParameterConstraintsAreOK()
    {
      CSharpProject parser = new CSharpProject(WorkingFolder);
      parser.AddFile(@"TypeParameters\TypeParamConstraintsAreOk.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].Namespaces[0].TypeDeclarations[0];
      TypeParameterConstraintCollection coList = td.ParameterConstraints;
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
    public void DuplicatedTypeParametersFail()
    {
      CSharpProject parser = new CSharpProject(WorkingFolder);
      parser.AddFile(@"TypeParameters\DuplicatedTypeParams.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0692");
      Assert.AreEqual(parser.Errors[1].Code, "CS0692");
      Assert.AreEqual(parser.Errors[2].Code, "CS0692");
    }

    [TestMethod]
    public void DuplicatedParameterConstraintsFail()
    {
      CSharpProject parser = new CSharpProject(WorkingFolder);
      parser.AddFile(@"TypeParameters\DuplicatedConstraints.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0409");
      Assert.AreEqual(parser.Errors[1].Code, "CS0409");
    }
  }
}
