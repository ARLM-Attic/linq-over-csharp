using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilderTest.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Tests the MemberLookup class.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class MemberLookupTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests member lookup when the context is a type parameter
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void TypeParameter()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberLookup\TypeParameter.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A", 1);
      var typeParameter = classA.GetOwnTypeParameterByName("T");
      var field = classA.GetMember<FieldEntity>("a");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);
      var members = memberLookup.Lookup("GetType", 0, typeParameter, field).OrderBy(x => x.ToString()).ToList();
      members.Count.ShouldEqual(5);
      members[0].ToString().ShouldEqual("global::C_GetType");
      members[1].ToString().ShouldEqual("global::I1_GetType()");
      members[2].ToString().ShouldEqual("global::I1_GetType(global::System.Int32)");
      members[3].ToString().ShouldEqual("global::I2_GetType");
      members[4].ToString().ShouldEqual("global::System.Object_GetType()");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests member lookup when the context is a constructed generic type
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore] // Handling of generic classes and methods postponed
    public void GenericType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberLookup\GenericType.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A");
      var field = classA.GetMember<FieldEntity>("b");
      var contextType = field.Type;

      var memberLookup = new MemberLookup(project, project.SemanticGraph);
      var members = memberLookup.Lookup("GetType", 0, contextType, field).OrderBy(x => x.ToString()).ToList();
      members.Count.ShouldEqual(2);
      // TODO: check the found members
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests member lookup when the context is a non-generic type
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NonGenericType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberLookup\NonGenericType.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A");
      var field = classA.GetMember<FieldEntity>("b");
      var contextType = field.Type;

      var memberLookup = new MemberLookup(project, project.SemanticGraph);
      var members = memberLookup.Lookup("GetType", 0, contextType, field).OrderBy(x => x.ToString()).ToList();
      members.Count.ShouldEqual(2);
      members[0].ToString().ShouldEqual("global::D_GetType()");
      members[1].ToString().ShouldEqual("global::System.Object_GetType()");
    }
  }
}
