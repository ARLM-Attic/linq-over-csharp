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
      var field = classA.GetOwnMember<FieldEntity>("t");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of T (contextType) that are accessible in class A, named "GetType", with 0 type params.
      {
        var memberLookupResult = memberLookup.Lookup("GetType", 0, typeParameter, field, false);
        var methods = memberLookupResult.MethodGroup.Methods.OrderBy(x => x.ToString()).ToList();
        methods.Count.ShouldEqual(3);
        methods[0].ToString().ShouldEqual("global::C_GetType()");
        methods[1].ToString().ShouldEqual("global::I1_GetType(global::System.Int32)");
        methods[2].ToString().ShouldEqual("global::System.Object_GetType()");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests member lookup when the context is a constructed generic type
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void GenericType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberLookup\GenericType.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A");
      var field = classA.GetOwnMember<FieldEntity>("b");
      var contextType = field.Type;

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of class B<int> (contextType) that are accessible in class A, named "GetType", with 0 type params.
      {
        var memberLookupResult = memberLookup.Lookup("GetType", 0, contextType, field, false);
        var methods = memberLookupResult.MethodGroup.Methods.OrderBy(x => x.ToString()).ToList();
        methods.Count.ShouldEqual(2);
        methods[0].ToString().ShouldEqual("global::D`1[global::System.Int32]_GetType()");
        methods[1].ToString().ShouldEqual("global::System.Object_GetType()");
      }
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
      var field = classA.GetOwnMember<FieldEntity>("b");
      var classB = globalNamespace.GetSingleChildType<ClassEntity>("B");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of class B (contextType) that are accessible in class A, named "GetType", with 0 type params.
      {
        var memberLookupResult = memberLookup.Lookup("GetType", 0, classB, field, false);
        var methods = memberLookupResult.MethodGroup.Methods.OrderBy(x => x.ToString()).ToList();
        methods.Count.ShouldEqual(4);
        methods[0].ToString().ShouldEqual("global::D_GetType()");
        methods[1].ToString().ShouldEqual("global::D_GetType`1()");
        methods[2].ToString().ShouldEqual("global::D_GetType`2()");
        methods[3].ToString().ShouldEqual("global::System.Object_GetType()");
      }
      // Selecting members of class B (contextType) that are accessible in class A, named "GetType", with 1 type params.
      {
        var memberLookupResult = memberLookup.Lookup("GetType", 1, classB, field, false);
        var methods = memberLookupResult.MethodGroup.Methods.OrderBy(x => x.ToString()).ToList();
        methods.Count.ShouldEqual(1);
        methods[0].ToString().ShouldEqual("global::D_GetType`1()");
      }
      // Selecting members of class B (contextType) that are accessible in class A, named "GetType", with 2 type params.
      {
        var memberLookupResult = memberLookup.Lookup("GetType", 2, classB, field, false);
        var methods = memberLookupResult.MethodGroup.Methods.OrderBy(x => x.ToString()).ToList();
        methods.Count.ShouldEqual(1);
        methods[0].ToString().ShouldEqual("global::D_GetType`2()");
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
      var field = classA.GetOwnMember<FieldEntity>("b");
      var classB = globalNamespace.GetSingleChildType<ClassEntity>("B");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);
      
      // Selecting members of class B (contextType) that are accessible in class A, named "C", with 0 type params.
      {
        var memberLookupResult = memberLookup.Lookup("C", 0, classB, field, false);
        memberLookupResult.SingleMember.ToString().ShouldEqual("global::B+C");
      }
      // Selecting members of class B (contextType) that are accessible in class A, named "C", with 1 type params.
      {
        var memberLookupResult = memberLookup.Lookup("C", 1, classB, field, false);
        memberLookupResult.SingleMember.ToString().ShouldEqual("global::B+C`1");
      }
      // Selecting invocable members only.
      {
        var memberLookupResult = memberLookup.Lookup("C", 1, classB, field, true);
        memberLookupResult.IsEmpty.ShouldBeTrue();
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
      var field = classS.GetOwnMember<ConstantMemberEntity>("test");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of class S (contextType) that are accessible in class S, named "M", with 0 type params.
      {
        var memberLookupResult = memberLookup.Lookup("M", 0, classS, field, false);
        memberLookupResult.SingleMember.ToString().ShouldEqual("global::S_M");
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
      var field = classS.GetOwnMember<ConstantMemberEntity>("test");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of class S (contextType) that are accessible in class S, named "M", with 0 type params.
      {
        var memberLookupResult = memberLookup.Lookup("M", 0, classS, field, false);
        memberLookupResult.SingleMember.ToString().ShouldEqual("global::S+M");
      }
      // Selecting members of class S (contextType) that are accessible in class S, named "M2", with 1 type params.
      {
        var memberLookupResult = memberLookup.Lookup("M2", 1, classS, field, false);
        memberLookupResult.SingleMember.ToString().ShouldEqual("global::S+M2`1");
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
      var field = classS.GetOwnMember<ConstantMemberEntity>("test");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of class S (contextType) that are accessible in class S, named "M", with 0 type params.
      {
        var memberLookupResult = memberLookup.Lookup("M", 0, classS, field, false);
        var methods = memberLookupResult.MethodGroup.Methods.OrderBy(x => x.ToString()).ToList();
        methods.Count.ShouldEqual(2);
        methods[0].ToString().ShouldEqual("global::Base0_M()");
        methods[1].ToString().ShouldEqual("global::S_M()");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests that class members hide interface members
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void ClassMemberHidesInterfaceMembers()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberLookup\ClassMemberHidesInterfaceMembers.cs");
      InvokeParser(project).ShouldBeTrue();

      var globalNamespace = project.SemanticGraph.GlobalNamespace;
      var classA = globalNamespace.GetSingleChildType<ClassEntity>("A", 1);
      var typeParameter = classA.GetOwnTypeParameterByName("T");
      var field = classA.GetOwnMember<FieldEntity>("t");

      var memberLookup = new MemberLookup(project, project.SemanticGraph);

      // Selecting members of T (contextType) that are accessible in class A, named "M1", with 0 type params.
      {
        var memberLookupResult = memberLookup.Lookup("M1", 0, typeParameter, field, false);
        memberLookupResult.SingleMember.ToString().ShouldEqual("global::Base_M1");
      }

      // Selecting members of T (contextType) that are accessible in class A, named "M2", with 0 type params.
      {
        var memberLookupResult = memberLookup.Lookup("M2", 0, typeParameter, field, false);
        var methods = memberLookupResult.MethodGroup.Methods.OrderBy(x => x.ToString()).ToList();
        methods.Count.ShouldEqual(2);
        methods[0].ToString().ShouldEqual("global::Base_M2()");
        methods[1].ToString().ShouldEqual("global::I3_M2(global::System.Int32)");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests that different type of member with same name cause ambiguity in multiple inheritence.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [Ignore] // csc.exe signals warning but the spec demands an error.
    public void AmbiguityInMultipleInheritance()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"MemberLookup\AmbiguityInMultipleInheritance.cs");
      InvokeParser(project).ShouldBeTrue();

      project.Errors.Count.ShouldEqual(1);
    }
  }
}
