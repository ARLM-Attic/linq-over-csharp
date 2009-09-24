using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Tests the accessibility of types and members
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class AccessibilityTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests which classes are accessible at the compilation unit level.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void AccessibleAtCompilationUnitLevel()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Accessibility\AccessibleAtCompilationUnitLevel.cs");
      InvokeParser(project).ShouldBeTrue();

      var compilationUnitNode = project.SyntaxTree.CompilationUnitNodes[0];
      var usingX1 = project.SemanticGraph.GlobalNamespace.GetUsingAliasByNameAndSourcePoint(
        "X1", new SourcePoint(compilationUnitNode, 3));
      usingX1.ShouldNotBeNull();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;

      var publicClass = globalNamespace.GetSingleChildType<ClassEntity>("PublicClass"); 
      publicClass.IsAccessibleBy(usingX1).ShouldBeTrue();
      publicClass.GetSingleChildType<ClassEntity>("PublicNestedClass").IsAccessibleBy(usingX1).ShouldBeTrue();
      publicClass.GetSingleChildType<ClassEntity>("InternalNestedClass").IsAccessibleBy(usingX1).ShouldBeTrue();
      publicClass.GetSingleChildType<ClassEntity>("ProtectedInternalNestedClass").IsAccessibleBy(usingX1).ShouldBeTrue();
      publicClass.GetSingleChildType<ClassEntity>("ProtectedNestedClass").IsAccessibleBy(usingX1).ShouldBeFalse();
      publicClass.GetSingleChildType<ClassEntity>("PrivateNestedClass").IsAccessibleBy(usingX1).ShouldBeFalse();

      var internalClass = globalNamespace.GetSingleChildType<ClassEntity>("InternalClass");
      internalClass.IsAccessibleBy(usingX1).ShouldBeTrue();
      internalClass.GetSingleChildType<ClassEntity>("PublicNestedInInternalClass").IsAccessibleBy(usingX1).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests public accessibility.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Public()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Accessibility\Public.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A");
      var a1 = classA.GetMember<FieldEntity>("a1");
      a1.ShouldNotBeNull();
      var classB = globalNamespace.GetSingleChildType<ClassEntity>("B");
      var b1 = classB.GetMember<FieldEntity>("b1");
      b1.ShouldNotBeNull();

      var publicNestedClass = classA.GetSingleChildType<ClassEntity>("PublicNestedClass");
      publicNestedClass.IsAccessibleBy(a1).ShouldBeTrue();
      publicNestedClass.IsAccessibleBy(b1).ShouldBeTrue();

      var publicMember = classA.GetMember<FieldEntity>("PublicMember");
      publicMember.IsAccessibleBy(a1).ShouldBeTrue();
      publicMember.IsAccessibleBy(b1).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests private accessibility.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Private()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Accessibility\Private.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A");
      var a1 = classA.GetMember<FieldEntity>("a1");
      a1.ShouldNotBeNull();
      var classB = globalNamespace.GetSingleChildType<ClassEntity>("B");
      var b1 = classB.GetMember<FieldEntity>("b1");
      b1.ShouldNotBeNull();

      var privateNestedClass = classA.GetSingleChildType<ClassEntity>("PrivateNestedClass");
      privateNestedClass.IsAccessibleBy(a1).ShouldBeTrue();
      privateNestedClass.IsAccessibleBy(b1).ShouldBeFalse();

      var privateMember = classA.GetMember<FieldEntity>("PrivateMember");
      privateMember.IsAccessibleBy(a1).ShouldBeTrue();
      privateMember.IsAccessibleBy(b1).ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests protected accessibility.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Protected()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Accessibility\Protected.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A");
      var a1 = classA.GetMember<FieldEntity>("a1");
      a1.ShouldNotBeNull();
      var classB = globalNamespace.GetSingleChildType<ClassEntity>("B");
      var b1 = classB.GetMember<FieldEntity>("b1");
      b1.ShouldNotBeNull();
      var classC = globalNamespace.GetSingleChildType<ClassEntity>("C");
      var c1 = classC.GetMember<FieldEntity>("c1");
      c1.ShouldNotBeNull();
      var classD = globalNamespace.GetSingleChildType<ClassEntity>("D");
      var d1 = classD.GetMember<FieldEntity>("d1");
      d1.ShouldNotBeNull();

      var protectedMember = classA.GetMember<FieldEntity>("ProtectedMember");
      protectedMember.IsAccessibleBy(a1).ShouldBeTrue();
      protectedMember.IsAccessibleBy(b1).ShouldBeTrue();
      protectedMember.IsAccessibleBy(c1).ShouldBeTrue();
      protectedMember.IsAccessibleBy(d1).ShouldBeFalse();
      
      var protectedNestedClass = classA.GetSingleChildType<ClassEntity>("ProtectedNestedClass");
      protectedNestedClass.IsAccessibleBy(a1).ShouldBeTrue();
      protectedNestedClass.IsAccessibleBy(b1).ShouldBeTrue();
      protectedNestedClass.IsAccessibleBy(c1).ShouldBeTrue();
      protectedNestedClass.IsAccessibleBy(d1).ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests internal accessibility.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Internal()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Accessibility\Internal.cs");
      project.AddAssemblyReference(TestAssemblyPathAndFilename);
      InvokeParser(project).ShouldBeTrue();

      var compilationUnitNode = project.SyntaxTree.CompilationUnitNodes[0];
      var usingX1 = project.SemanticGraph.GlobalNamespace.GetUsingAliasByNameAndSourcePoint(
        "X1", new SourcePoint(compilationUnitNode, 3));
      usingX1.ShouldNotBeNull();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;

      var publicClass = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("PublicClass");
      publicClass.IsAccessibleBy(usingX1).ShouldBeTrue();
      publicClass.GetSingleChildType<ClassEntity>("PublicNestedClass").IsAccessibleBy(usingX1).ShouldBeTrue();
      publicClass.GetSingleChildType<ClassEntity>("InternalNestedClass").IsAccessibleBy(usingX1).ShouldBeFalse();

      var internalClass = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("InternalClass");
      internalClass.IsAccessibleBy(usingX1).ShouldBeFalse();
      internalClass.GetSingleChildType<ClassEntity>("PublicNestedInInternalClass").IsAccessibleBy(usingX1).ShouldBeFalse();

      var sameProgramInternalClass = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("SameProgramInternalClass");
      sameProgramInternalClass.IsAccessibleBy(usingX1).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests protected internal accessibility in the same program (internal is effective).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ProtectedInternalInSameProgram()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Accessibility\ProtectedInternalInSameProgram.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A");
      var classB = globalNamespace.GetSingleChildType<ClassEntity>("B");
      var b1 = classB.GetMember<FieldEntity>("b1");
      b1.ShouldNotBeNull();

      var protectedInternalMember = classA.GetMember<FieldEntity>("ProtectedInternalMember");
      protectedInternalMember.IsAccessibleBy(b1).ShouldBeTrue();

      var protectedInternalNestedClass = classA.GetSingleChildType<ClassEntity>("ProtectedInternalNestedClass");
      protectedInternalNestedClass.IsAccessibleBy(b1).ShouldBeTrue();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests protected internal accessibility in imported program.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ProtectedInternalInImportedProgram()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Accessibility\ProtectedInternalInImportedProgram.cs");
      project.AddAssemblyReference(TestAssemblyPathAndFilename);
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var publicClass = globalNamespace.GetSingleChildType<ClassEntity>("PublicClass");
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A");
      var a1 = classA.GetMember<FieldEntity>("a1");
      a1.ShouldNotBeNull();
      var classB = globalNamespace.GetSingleChildType<ClassEntity>("B");
      var b1 = classB.GetMember<FieldEntity>("b1");
      b1.ShouldNotBeNull();

      var protectedInternalMember = publicClass.GetMember<FieldEntity>("protectedInternalNestedMember");
      protectedInternalMember.IsAccessibleBy(a1).ShouldBeFalse();
      protectedInternalMember.IsAccessibleBy(b1).ShouldBeTrue();

      var protectedInternalNestedClass = publicClass.GetSingleChildType<ClassEntity>("ProtectedInternalNestedClass");
      protectedInternalNestedClass.IsAccessibleBy(a1).ShouldBeFalse();
      protectedInternalNestedClass.IsAccessibleBy(b1).ShouldBeTrue();

      var internalMember = publicClass.GetMember<FieldEntity>("internalNestedMember");
      internalMember.IsAccessibleBy(a1).ShouldBeFalse();
      internalMember.IsAccessibleBy(b1).ShouldBeFalse();

      var internalNestedClass = publicClass.GetSingleChildType<ClassEntity>("InternalNestedClass");
      internalNestedClass.IsAccessibleBy(a1).ShouldBeFalse();
      internalNestedClass.IsAccessibleBy(b1).ShouldBeFalse();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests protected access for instance members (see spec 3.5.3).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore]
    public void ProtectedAccessForInstanceMembers()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"Accessibility\ProtectedAccessForInstanceMembers.cs");
      InvokeParser(project).ShouldBeTrue();

      //TODO: implement checking
    }
  }
}
