using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

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
      var members = memberLookup.Lookup("GetType", 0, typeParameter, field).ToList();
      members.Count.ShouldEqual(4);
    }
  }
}
