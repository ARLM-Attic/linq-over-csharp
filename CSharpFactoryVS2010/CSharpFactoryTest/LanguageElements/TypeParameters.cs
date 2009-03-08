using System.Collections.Generic;
using CSharpFactory.ProjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.LanguageElements
{
  [TestClass]
  public class TypeParameters: ParserTestBed
  {
    [TestMethod]
    public void TypeParameterConstraintsAreOK()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\TypeParamConstraintsAreOk.cs");
      Assert.IsTrue(InvokeParser(parser));
      TypeDeclaration td = parser.Files[0].NestedNamespaces[0].TypeDeclarations[0];
      List <TypeParameterConstraint> coList = td.ParameterConstraints;
      Assert.AreEqual(coList.Count, 6);

      TypeParameterConstraint con = coList[0];
      Assert.AreEqual(con.Name, "A");
      Assert.AreEqual(con.Constraints.Count, 1);
      Assert.AreEqual(con.Primary.Classification, ConstraintClassification.Class);
      Assert.IsFalse(con.HasNew);

      con = coList[1];
      Assert.AreEqual(con.Name, "B");
      Assert.AreEqual(con.Constraints.Count, 1);
      Assert.AreEqual(con.Primary.Classification, ConstraintClassification.Struct);
      Assert.IsFalse(con.HasNew);

      con = coList[2];
      Assert.AreEqual(con.Name, "C");
      Assert.AreEqual(con.Constraints.Count, 1);
      Assert.IsTrue(con.HasNew);

      con = coList[3];
      Assert.AreEqual(con.Name, "D");
      Assert.AreEqual(con.Constraints.Count, 2);
      Assert.AreEqual(con.Constraints[0].Type.Name, "ArrayList");
      Assert.AreEqual(con.Constraints[1].Type.ParametrizedName, "System.Collections.Generic.IEnumerable<A>");
      Assert.AreEqual(con.Constraints[1].Type.Arguments.Count, 1);
      Assert.AreEqual(con.Constraints[1].Type.Arguments[0].Name, "A");
      Assert.IsFalse(con.HasNew);

      con = coList[4];
      Assert.AreEqual(con.Name, "E");
      Assert.AreEqual(con.Constraints.Count, 2);
      Assert.AreEqual(con.Constraints[0].Type.Name, "D");
      Assert.IsTrue(con.HasNew);

      con = coList[5];
      Assert.AreEqual(con.Name, "F");
      Assert.AreEqual(con.Constraints.Count, 4);
      Assert.AreEqual(con.Constraints[0].Type.Name, "ArrayList");
      Assert.AreEqual(con.Constraints[1].Type.ParametrizedName, "System.Collections.Generic.IEnumerable<B>");
      Assert.AreEqual(con.Constraints[2].Type.ParametrizedName, "System.IEquatable<D>");
      Assert.AreEqual(con.Constraints[1].Type.Arguments.Count, 1);
      Assert.AreEqual(con.Constraints[1].Type.Arguments[0].Name, "B");
      Assert.AreEqual(con.Constraints[2].Type.Arguments.Count, 1);
      Assert.AreEqual(con.Constraints[2].Type.Arguments[0].Name, "D");
      Assert.IsTrue(con.HasNew);
    }

    [TestMethod]
    public void DuplicatedTypeParametersFail()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
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
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\DuplicatedConstraints.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0409");
      Assert.AreEqual(parser.Errors[1].Code, "CS0409");
    }

    [TestMethod]
    public void MissingTypeParametersFail()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\MissingTypeParameter.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0699");
      Assert.AreEqual(parser.Errors[1].Code, "CS0699");
      Assert.AreEqual(parser.Errors[2].Code, "CS0699");
    }

    [TestMethod]
    public void ConstraintFails1()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\ConstraintFails1.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0449");
      Assert.AreEqual(parser.Errors[1].Code, "CS0449");
    }

    [TestMethod]
    public void ConstraintFails2()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\ConstraintFails2.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 9);
      Assert.AreEqual(parser.Errors[0].Code, "CS0401");
      Assert.AreEqual(parser.Errors[1].Code, "CS0401");
      Assert.AreEqual(parser.Errors[2].Code, "CS0451");
      Assert.AreEqual(parser.Errors[3].Code, "CS0451");
      Assert.AreEqual(parser.Errors[4].Code, "CS0401");
      Assert.AreEqual(parser.Errors[5].Code, "CS0451");
      Assert.AreEqual(parser.Errors[6].Code, "CS0401");
      Assert.AreEqual(parser.Errors[7].Code, "CS0401");
      Assert.AreEqual(parser.Errors[8].Code, "CS0401");
    }

    [TestMethod]
    public void ConstraintFails3()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\ConstraintFails3.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 5);
      Assert.AreEqual(parser.Errors[0].Code, "CS0702");
      Assert.AreEqual(parser.Errors[1].Code, "CS0702");
      Assert.AreEqual(parser.Errors[2].Code, "CS0702");
      Assert.AreEqual(parser.Errors[3].Code, "CS0702");
      Assert.AreEqual(parser.Errors[4].Code, "CS0702");
    }

    [TestMethod]
    public void ConstraintFails4()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\ConstraintFails4.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 6);
      Assert.AreEqual(parser.Errors[0].Code, "CS0406");
      Assert.AreEqual(parser.Errors[1].Code, "CS0406");
      Assert.AreEqual(parser.Errors[2].Code, "CS0406");
      Assert.AreEqual(parser.Errors[3].Code, "CS0406");
      Assert.AreEqual(parser.Errors[4].Code, "CS0406");
      Assert.AreEqual(parser.Errors[5].Code, "CS0406");
    }

    [TestMethod]
    public void ConstraintFails5()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\ConstraintFails5.cs");
      parser.AddAssemblyReference("System.Drawing");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 4);
      Assert.AreEqual(parser.Errors[0].Code, "CS0701");
      Assert.AreEqual(parser.Errors[1].Code, "CS0701");
      Assert.AreEqual(parser.Errors[2].Code, "CS0701");
      Assert.AreEqual(parser.Errors[3].Code, "CS0701");
    }

    [TestMethod]
    public void ConstraintFails6()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\ConstraintFails6.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 6);
      Assert.AreEqual(parser.Errors[0].Code, "CS0405");
      Assert.AreEqual(parser.Errors[1].Code, "CS0405");
      Assert.AreEqual(parser.Errors[2].Code, "CS0405");
      Assert.AreEqual(parser.Errors[3].Code, "CS0405");
      Assert.AreEqual(parser.Errors[4].Code, "CS0405");
      Assert.AreEqual(parser.Errors[5].Code, "CS0405");
    }

    [TestMethod]
    public void ConstraintFails7()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\ConstraintFails7.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0701");
      Assert.AreEqual(parser.Errors[1].Code, "CS0701");
    }

    [TestMethod]
    public void ConstraintFails8()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\ConstraintFails8.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 2);
      Assert.AreEqual(parser.Errors[0].Code, "CS0454");
      Assert.AreEqual(parser.Errors[1].Code, "CS0454");
    }

    [TestMethod]
    public void ConstraintFails9()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\ConstraintFails9.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 3);
      Assert.AreEqual(parser.Errors[0].Code, "CS0456");
      Assert.AreEqual(parser.Errors[1].Code, "CS0456");
      Assert.AreEqual(parser.Errors[2].Code, "CS0456");
    }

    [TestMethod]
    public void ConstraintFails10()
    {
      CompilationUnit parser = new CompilationUnit(WorkingFolder);
      parser.AddFile(@"TypeParameters\ConstraintFails10.cs");
      Assert.IsFalse(InvokeParser(parser));
      Assert.AreEqual(parser.Errors.Count, 1);
      Assert.AreEqual(parser.Errors[0].Code, "CS0455");
    }
  }
}
