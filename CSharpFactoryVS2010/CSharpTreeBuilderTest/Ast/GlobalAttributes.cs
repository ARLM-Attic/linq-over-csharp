// ================================================================================================
// GlobalAttributes.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpTreeBuilderTest
{
  [TestClass]
  public class GlobalAttributes: ParserTestBed
  {
    [TestMethod]
    public void GlobalAttributesAreOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"GlobalAttributes\GlobalAttributesOK.cs");
      Assert.IsTrue(InvokeParser(project, true, false));
      var file = project.SyntaxTree.CompilationUnitNodes[0];

      // --- Check global attributes in the file
      Assert.AreEqual(file.GlobalAttributes.Count, 5);
      Assert.AreEqual(file.GlobalAttributes[0].Attributes.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[0].Attributes[0].Arguments.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[1].Attributes.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[1].Attributes[0].Arguments.Count, 0);
      Assert.AreEqual(file.GlobalAttributes[2].Attributes.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[2].Attributes[0].Arguments.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[3].Attributes.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[3].Attributes[0].Arguments.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[4].Attributes.Count, 2);
      Assert.AreEqual(file.GlobalAttributes[4].Attributes[0].Arguments.Count, 1);
      Assert.AreEqual(file.GlobalAttributes[4].Attributes[1].Arguments.Count, 1);
    }

    [TestMethod]
    public void GlobalAttributeBoundariesAreOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"GlobalAttributes\GlobalAttributesOK.cs");
      Assert.IsTrue(InvokeParser(project, true, false));
      var file = project.SyntaxTree.CompilationUnitNodes[0];

      // --- Check global attributes in the file
      Assert.AreEqual(file.GlobalAttributes.Count, 5);

      var attrNode = file.GlobalAttributes[0];
      Assert.AreEqual(attrNode.StartToken.Value, "[");
      Assert.AreEqual(attrNode.StartToken.Line, 4);
      Assert.AreEqual(attrNode.StartToken.Column, 1);
      Assert.AreEqual(attrNode.TerminatingToken.Value, "]");
      Assert.AreEqual(attrNode.TerminatingToken.Line, 4);
      Assert.AreEqual(attrNode.TerminatingToken.Column, 29);
      Assert.AreEqual(attrNode.Target, "assembly");
      var aNode = attrNode.Attributes[0];
      Assert.AreEqual(aNode.StartToken.Value, "ComVisible");
      Assert.AreEqual(aNode.StartToken.Line, 4);
      Assert.AreEqual(aNode.StartToken.Column, 12);
      Assert.AreEqual(aNode.SeparatorToken.Value, ":");
      Assert.AreEqual(aNode.SeparatorToken.Line, 4);
      Assert.AreEqual(aNode.SeparatorToken.Column, 10);
      Assert.AreEqual(aNode.TerminatingToken.Value, ")");
      Assert.AreEqual(aNode.TerminatingToken.Line, 4);
      Assert.AreEqual(aNode.TerminatingToken.Column, 28);
      var nameTag = aNode.TypeName.TypeTags[0];
      Assert.AreEqual(nameTag.Identifier, "ComVisible");
      Assert.AreEqual(nameTag.IdentifierToken.Line, 4);
      Assert.AreEqual(nameTag.IdentifierToken.Column, 12);
      Assert.AreEqual(aNode.Arguments.StartToken.Value, "(");
      Assert.AreEqual(aNode.Arguments.StartToken.Line, 4);
      Assert.AreEqual(aNode.Arguments.StartToken.Column, 22);
      Assert.AreEqual(aNode.Arguments.TerminatingToken.Value, ")");
      Assert.AreEqual(aNode.Arguments.TerminatingToken.Line, 4);
      Assert.AreEqual(aNode.Arguments.TerminatingToken.Column, 28);

      attrNode = file.GlobalAttributes[4];
      Assert.AreEqual(attrNode.StartToken.Value, "[");
      Assert.AreEqual(attrNode.StartToken.Line, 8);
      Assert.AreEqual(attrNode.StartToken.Column, 1);
      Assert.AreEqual(attrNode.TerminatingToken.Value, "]");
      Assert.AreEqual(attrNode.TerminatingToken.Line, 8);
      Assert.AreEqual(attrNode.TerminatingToken.Column, 83);
      Assert.AreEqual(attrNode.Target, "assembly");
      aNode = attrNode.Attributes[0];
      Assert.AreEqual(aNode.StartToken.Value, "AssemblyConfiguration");
      Assert.AreEqual(aNode.StartToken.Line, 8);
      Assert.AreEqual(aNode.StartToken.Column, 12);
      Assert.AreEqual(aNode.SeparatorToken.Value, ":");
      Assert.AreEqual(aNode.SeparatorToken.Line, 8);
      Assert.AreEqual(aNode.SeparatorToken.Column, 10);
      Assert.AreEqual(aNode.TerminatingToken.Value, ")");
      Assert.AreEqual(aNode.TerminatingToken.Line, 8);
      Assert.AreEqual(aNode.TerminatingToken.Column, 36);
      nameTag = aNode.TypeName.TypeTags[0];
      Assert.AreEqual(nameTag.Identifier, "AssemblyConfiguration");
      Assert.AreEqual(nameTag.IdentifierToken.Line, 8);
      Assert.AreEqual(nameTag.IdentifierToken.Column, 12);
      Assert.AreEqual(aNode.Arguments.StartToken.Value, "(");
      Assert.AreEqual(aNode.Arguments.StartToken.Line, 8);
      Assert.AreEqual(aNode.Arguments.StartToken.Column, 33);
      Assert.AreEqual(aNode.Arguments.TerminatingToken.Value, ")");
      Assert.AreEqual(aNode.Arguments.TerminatingToken.Line, 8);
      Assert.AreEqual(aNode.Arguments.TerminatingToken.Column, 36);

      aNode = attrNode.Attributes[1];
      Assert.AreEqual(aNode.StartToken.Value, "Guid");
      Assert.AreEqual(aNode.StartToken.Line, 8);
      Assert.AreEqual(aNode.StartToken.Column, 39);
      Assert.AreEqual(aNode.SeparatorToken.Value, ",");
      Assert.AreEqual(aNode.SeparatorToken.Line, 8);
      Assert.AreEqual(aNode.SeparatorToken.Column, 37);
      Assert.AreEqual(aNode.TerminatingToken.Value, ")");
      Assert.AreEqual(aNode.TerminatingToken.Line, 8);
      Assert.AreEqual(aNode.TerminatingToken.Column, 82);
      nameTag = aNode.TypeName.TypeTags[0];
      Assert.AreEqual(nameTag.Identifier, "Guid");
      Assert.AreEqual(nameTag.IdentifierToken.Line, 8);
      Assert.AreEqual(nameTag.IdentifierToken.Column, 39);
      Assert.AreEqual(aNode.Arguments.StartToken.Value, "(");
      Assert.AreEqual(aNode.Arguments.StartToken.Line, 8);
      Assert.AreEqual(aNode.Arguments.StartToken.Column, 43);
      Assert.AreEqual(aNode.Arguments.TerminatingToken.Value, ")");
      Assert.AreEqual(aNode.Arguments.TerminatingToken.Line, 8);
      Assert.AreEqual(aNode.Arguments.TerminatingToken.Column, 82);
    }
    [TestMethod]

    public void GlobalAttributeSyntaxTreeWriterOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"GlobalAttributes\GlobalAttributesOK.cs");
      Assert.IsTrue(InvokeParser(project, true, false));
      Assert.AreEqual(project.Errors.Count, 0);
      var treeWriter = new SyntaxTreeTextWriter(project.SyntaxTree, project.ProjectProvider) { WorkingFolder = TempOutputFolder };
      treeWriter.WriteTree();
      project = new CSharpProject(WorkingFolder);
      project.AddFile(@"GlobalAttributes\GlobalAttributesOK.cs");
      Assert.IsTrue(InvokeParser(project, true, false));
    }

    [TestMethod]
    public void GlobalAttributeParentsAreOk()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"GlobalAttributes\GlobalAttributesOK.cs");
      Assert.IsTrue(InvokeParser(project, true, false));
      var file = project.SyntaxTree.CompilationUnitNodes[0];
      foreach (var attrNode in file.GlobalAttributes)
      {
        Assert.AreEqual(attrNode.ParentNode, file);
        foreach (var aNode in attrNode.Attributes)
        {
          Assert.AreEqual(aNode.ParentNode, attrNode);
          Assert.AreEqual(aNode.TypeName.ParentNode, aNode);
          foreach (var aArg in aNode.Arguments)
          {
            Assert.AreEqual(aArg.ParentNode, aNode);
          }
        }
      }
    }
  }
}