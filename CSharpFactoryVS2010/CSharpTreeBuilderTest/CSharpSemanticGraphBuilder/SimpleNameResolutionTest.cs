using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.ProjectContent;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// Tests the simple name resolution logic.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class SimpleNameResolutionTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0103: The name 'x' does not exist in the current context
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0103_SimpleNameUndefined()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\CS0103_SimpleNameUndefined.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0103");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to a static member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void StaticMember()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\StaticMember.cs");
      InvokeParser(project).ShouldBeTrue();

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var fieldB = classA.GetMember<FieldEntity>("b");
      var fieldC = classA.GetMember<FieldEntity>("c");
      var simpleNameC = (fieldB.Initializer as ScalarInitializerEntity).Expression as SimpleNameExpressionEntity;
      simpleNameC.SimpleNameResult.SingleEntity.ShouldEqual(fieldC);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// error CS0120: An object reference is required for the non-static field, method, or property 'A.c'
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void CS0120_StaticMemberExpected()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\CS0120_StaticMemberExpected.cs");
      InvokeParser(project).ShouldBeFalse();

      project.Errors.Count.ShouldEqual(1);
      project.Errors[0].Code.ShouldEqual("CS0120");
      project.Warnings.Count.ShouldEqual(0);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to a type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore] // Continue after PrimaryExpressionMemberAccessExpressionEntity is done.
    public void ResolvedToType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\ResolvedToType.cs");
      InvokeParser(project).ShouldBeTrue();

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var fieldA = classA.GetMember<FieldEntity>("a");
      // TODO: continue
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to a method type-parameter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore]  // Completing this test requires method body and local variable semantic entity handling.
    public void MethodTypeParameter()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"SimpleNameResolution\MethodTypeParameter.cs");
      InvokeParser(project).ShouldBeTrue();

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A");
      var methodM = classA.GetMethod("M", 1, null);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to an instance member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore]  // Completing this test requires method body and local variable semantic entity handling.
    public void InstanceMember()
    {
      // TODO
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Test the resolution of a simple name to a method group.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore]  // Completing this test requires method body and invocation semantic entity handling.
    public void MethodGroup()
    {
      // TODO
    }
  }
}