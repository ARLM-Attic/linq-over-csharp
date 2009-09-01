using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest
{
  // ================================================================================================
  /// <summary>
  /// Tests partial methods
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class PartialMethodDeclaration : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests partial method declarations and implementation structure
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void PartialMethods()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"PartialMethodDeclaration\PartialMethods.cs");
      InvokeParser(project, true, false).ShouldBeTrue();

      var source = project.SyntaxTree.CompilationUnitNodes[0];
      source.TypeDeclarations.Count.ShouldEqual(2);

      int classIndex = 0;

      {
        var classDecl = source.TypeDeclarations[classIndex] as ClassDeclarationNode;
        classDecl.ShouldNotBeNull();
        classDecl.IsPartial.ShouldBeTrue();

        var methodDecl = classDecl.MemberDeclarations[0] as MethodDeclarationNode;
        methodDecl.IsPartial.ShouldBeTrue();
        methodDecl.Body.ShouldBeNull();
      }

      classIndex++;

      {
        var classDecl = source.TypeDeclarations[classIndex] as ClassDeclarationNode;
        classDecl.ShouldNotBeNull();
        classDecl.IsPartial.ShouldBeTrue();

        var methodDecl = classDecl.MemberDeclarations[0] as MethodDeclarationNode;
        methodDecl.IsPartial.ShouldBeTrue();
        methodDecl.Body.ShouldNotBeNull();
      }

    }
  }
}