using CSharpFactory.ProjectModel;
using CSharpFactory.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpParserTest.SyntaxTreeWriter
{
  [TestClass]
  public class SyntaxTreeFileWriterTest: ParserTestBed
  {
    private const string csProjFolder =
      @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactoryTest\ProjectProvider\WinFormsAppTest";
    private const string csOutputFolder =
      @"C:\Work\LINQOverCSharp\CSharpFactoryVS2010\CSharpFactoryTest\ProjectProvider\WinFormsOutput";

    [TestMethod]
    public void OutputDirCreationOk()
    {
      var parser = new CompilationUnit(csProjFolder, true);
      parser.AddAssemblyReference("System.Core");
      parser.AddAssemblyReference("System.Data");
      parser.AddAssemblyReference("System.Drawing");
      parser.AddAssemblyReference("System.Xml.Linq");
      parser.AddAssemblyReference("System.Windows.Forms");
      Assert.IsTrue(InvokeParser(parser));
      var treeWriter = new SyntaxTreeTextWriter(parser.SyntaxTree, parser.ProjectProvider)
                         {WorkingFolder = csOutputFolder};
      treeWriter.WriteTree();
      parser = new CompilationUnit(csOutputFolder, true);
      parser.AddAssemblyReference("System.Core");
      parser.AddAssemblyReference("System.Data");
      parser.AddAssemblyReference("System.Drawing");
      parser.AddAssemblyReference("System.Xml.Linq");
      parser.AddAssemblyReference("System.Windows.Forms");
      Assert.IsTrue(InvokeParser(parser));
    }
  }
}
