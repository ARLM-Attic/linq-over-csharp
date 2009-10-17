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
    [Ignore]  // Under development
    public void TypeParameter()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberLookup\TypeParameter.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A", 1);
      var typeParameter = classA.GetOwnTypeParameterByName("T");
      var field = classA.GetMember<FieldEntity>("t");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of T (contextType) that are accessible in class A, named "GetType", with 0 type params.
      {
        var members = memberLookup.Lookup("GetType", 0, typeParameter, field, false).OrderBy(x => x.ToString()).ToList();
        members.Count.ShouldEqual(4);
        members[0].ToString().ShouldEqual("global::C_GetType");
        members[1].ToString().ShouldEqual("global::I1_GetType()");
        members[2].ToString().ShouldEqual("global::I1_GetType(global::System.Int32)");
        members[3].ToString().ShouldEqual("global::I2_GetType");
      }

      // Select only invocable members.
      {
        var members = memberLookup.Lookup("GetType", 0, typeParameter, field, true).OrderBy(x => x.ToString()).ToList();
        members.Count.ShouldEqual(3);
        members[0].ToString().ShouldEqual("global::I1_GetType()");
        members[1].ToString().ShouldEqual("global::I1_GetType(global::System.Int32)");
        members[2].ToString().ShouldEqual("global::System.Object_GetType()");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests member lookup when the context is a constructed generic type
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore] // Under development
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
      var members = memberLookup.Lookup("GetType", 0, contextType, field, false).OrderBy(x => x.ToString()).ToList();
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
      var classB = globalNamespace.GetSingleChildType<ClassEntity>("B");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of class B (contextType) that are accessible in class A, named "GetType", with 0 type params.
      {
        var members = memberLookup.Lookup("GetType", 0, classB, field, false).OrderBy(x => x.ToString()).ToList();
        members.Count.ShouldEqual(4);
        members[0].ToString().ShouldEqual("global::D_GetType()");
        members[1].ToString().ShouldEqual("global::D_GetType`1()");
        members[2].ToString().ShouldEqual("global::D_GetType`2()");
        members[3].ToString().ShouldEqual("global::System.Object_GetType()");
      }
      // Selecting members of class B (contextType) that are accessible in class A, named "GetType", with 1 type params.
      {
        var members = memberLookup.Lookup("GetType", 1, classB, field, false).OrderBy(x => x.ToString()).ToList();
        members.Count.ShouldEqual(1);
        members[0].ToString().ShouldEqual("global::D_GetType`1()");
      }
      // Selecting members of class B (contextType) that are accessible in class A, named "GetType", with 2 type params.
      {
        var members = memberLookup.Lookup("GetType", 2, classB, field, false).OrderBy(x => x.ToString()).ToList();
        members.Count.ShouldEqual(1);
        members[0].ToString().ShouldEqual("global::D_GetType`2()");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests member lookup when the member is a nested type
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NestedType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberLookup\NestedType.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A");
      var field = classA.GetMember<FieldEntity>("b");
      var classB = globalNamespace.GetSingleChildType<ClassEntity>("B");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);
      
      // Selecting members of class B (contextType) that are accessible in class A, named "C", with 0 type params.
      {
        var members = memberLookup.Lookup("C", 0, classB, field, false).ToList();
        members.Count.ShouldEqual(1);
        members[0].ToString().ShouldEqual("global::B+C");
      }
      // Selecting members of class B (contextType) that are accessible in class A, named "C", with 1 type params.
      {
        var members = memberLookup.Lookup("C", 1, classB, field, false).ToList();
        members.Count.ShouldEqual(1);
        members[0].ToString().ShouldEqual("global::B+C`1");
      }
      // Selecting invocable members only.
      {
        var members = memberLookup.Lookup("C", 1, classB, field, true).ToList();
        members.Count.ShouldEqual(0);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests that:
    /// If M is a constant, field, property, event, or enumeration member, 
    /// then all members declared in a base type of S are removed from the set.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void MemberHidesAllBaseMembers()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberLookup\MemberHidesAllBaseMembers.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classS = globalNamespace.GetSingleChildType<ClassEntity>("S");
      var field = classS.GetMember<ConstantMemberEntity>("test");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of class S (contextType) that are accessible in class S, named "M", with 0 type params.
      {
        var members = memberLookup.Lookup("M", 0, classS, field, false).ToList();
        members.Count.ShouldEqual(1);
        members[0].ToString().ShouldEqual("global::S_M");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests that:
    /// If M is a type declaration, then all non-types declared in a base type of S are removed from the set, 
    /// and all type declarations with the same number of type parameters as M 
    /// declared in a base type of S are removed from the set.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void NestedTypeHidesNonTypeMembers()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberLookup\NestedTypeHidesNonTypeMembers.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classS = globalNamespace.GetSingleChildType<ClassEntity>("S");
      var field = classS.GetMember<ConstantMemberEntity>("test");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of class S (contextType) that are accessible in class S, named "M", with 0 type params.
      {
        var members = memberLookup.Lookup("M", 0, classS, field, false).ToList();
        members.Count.ShouldEqual(1);
        members[0].ToString().ShouldEqual("global::S+M");
      }
      // Selecting members of class S (contextType) that are accessible in class S, named "M2", with 1 type params.
      {
        var members = memberLookup.Lookup("M2", 1, classS, field, false).ToList();
        members.Count.ShouldEqual(1);
        members[0].ToString().ShouldEqual("global::S+M2`1");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests that:
    /// If M is a method, then all non-method members declared in a base type of S are removed from the set.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void MethodHidesAllNonMethodMembers()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberLookup\MethodHidesAllNonMethodMembers.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classS = globalNamespace.GetSingleChildType<ClassEntity>("S");
      var field = classS.GetMember<ConstantMemberEntity>("test");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of class S (contextType) that are accessible in class S, named "M", with 0 type params.
      {
        var members = memberLookup.Lookup("M", 0, classS, field, false).OrderBy(x => x.ToString()).ToList();
        members.Count.ShouldEqual(2);
        members[0].ToString().ShouldEqual("global::Base0_M()");
        members[1].ToString().ShouldEqual("global::S_M()");
      }
    }
  }
}
