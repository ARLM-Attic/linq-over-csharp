using CSharpParser.ProjectModel;
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
      Assert.AreEqual(cd.Fields[0].ResultingType.ResolutionInfo.Resolver, typeof(bool));
      Assert.AreEqual(cd.Fields[1].ResultingType.ResolutionInfo.Resolver, typeof(byte));
      Assert.AreEqual(cd.Fields[2].ResultingType.ResolutionInfo.Resolver, typeof(char));
      Assert.AreEqual(cd.Fields[3].ResultingType.ResolutionInfo.Resolver, typeof(decimal));
      Assert.AreEqual(cd.Fields[4].ResultingType.ResolutionInfo.Resolver, typeof(double));
      Assert.AreEqual(cd.Fields[5].ResultingType.ResolutionInfo.Resolver, typeof(float));
      Assert.AreEqual(cd.Fields[6].ResultingType.ResolutionInfo.Resolver, typeof(int));
      Assert.AreEqual(cd.Fields[7].ResultingType.ResolutionInfo.Resolver, typeof(long));
      Assert.AreEqual(cd.Fields[8].ResultingType.ResolutionInfo.Resolver, typeof(object));
      Assert.AreEqual(cd.Fields[9].ResultingType.ResolutionInfo.Resolver, typeof(sbyte));
      Assert.AreEqual(cd.Fields[10].ResultingType.ResolutionInfo.Resolver, typeof(short));
      Assert.AreEqual(cd.Fields[11].ResultingType.ResolutionInfo.Resolver, typeof(string));
      Assert.AreEqual(cd.Fields[12].ResultingType.ResolutionInfo.Resolver, typeof(uint));
      Assert.AreEqual(cd.Fields[13].ResultingType.ResolutionInfo.Resolver, typeof(ulong));
      Assert.AreEqual(cd.Fields[14].ResultingType.ResolutionInfo.Resolver, typeof(ushort));
      MethodDeclaration md = cd.Methods[0];
      Assert.AreEqual(md.FormalParameters[0].Type.ResolutionInfo.Resolver, typeof(bool));
      Assert.AreEqual(md.FormalParameters[1].Type.ResolutionInfo.Resolver, typeof(byte));
      Assert.AreEqual(md.FormalParameters[2].Type.ResolutionInfo.Resolver, typeof(char));
      Assert.AreEqual(md.FormalParameters[3].Type.ResolutionInfo.Resolver, typeof(decimal));
      Assert.AreEqual(md.FormalParameters[4].Type.ResolutionInfo.Resolver, typeof(double));
      Assert.AreEqual(md.FormalParameters[5].Type.ResolutionInfo.Resolver, typeof(float));
      Assert.AreEqual(md.FormalParameters[6].Type.ResolutionInfo.Resolver, typeof(int));
      Assert.AreEqual(md.FormalParameters[7].Type.ResolutionInfo.Resolver, typeof(long));
      Assert.AreEqual(md.FormalParameters[8].Type.ResolutionInfo.Resolver, typeof(object));
      Assert.AreEqual(md.FormalParameters[9].Type.ResolutionInfo.Resolver, typeof(sbyte));
      Assert.AreEqual(md.FormalParameters[10].Type.ResolutionInfo.Resolver, typeof(short));
      Assert.AreEqual(md.FormalParameters[11].Type.ResolutionInfo.Resolver, typeof(string));
      Assert.AreEqual(md.FormalParameters[12].Type.ResolutionInfo.Resolver, typeof(uint));
      Assert.AreEqual(md.FormalParameters[13].Type.ResolutionInfo.Resolver, typeof(ulong));
      Assert.AreEqual(md.FormalParameters[14].Type.ResolutionInfo.Resolver, typeof(ushort));
    }
  }
}