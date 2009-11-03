using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;

namespace CSharpTreeBuilderTest.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Tests logic in TypeParameterEntity class.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public sealed class TypeParameterEntityTest : ParserTestBed
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the logic behind the BaseClass property getter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void EffectiveBaseClass()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeParameterEntity\EffectiveBaseClass.cs");
      InvokeParser(project).ShouldBeTrue();

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("P", 2)
        .GetSingleChildType<ClassEntity>("A", 8);

      int i = 0;

      // T1: If T has no primary constraints or type parameter constraints, its effective base class is object.
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        typeParameter.BaseClass.ToString().ShouldEqual("global::System.Object");
      }
      // T2: If T has the value type constraint, its effective base class is System.ValueType.
      //  where T2 : struct
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        typeParameter.BaseClass.ToString().ShouldEqual("global::System.ValueType");
      }
      // T3: If T has a class-type constraint C but no type-parameter constraints, its effective base class is C.
      //  where T3 : C
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        typeParameter.BaseClass.ToString().ShouldEqual("global::C");
      }
      // T41, T42: If T has no class-type constraint but has one (T41) or more (T42) type-parameter constraints, 
      // its effective base class is the most encompassed type (§6.4.2) 
      // in the set of effective base classes of its type-parameter constraints. 
      //  where T41 : TP1
      //  where T42 : TP1, TP2
      //  where TP1 : B
      //  where TP2 : D
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        typeParameter.BaseClass.ToString().ShouldEqual("global::B");
      }
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        typeParameter.BaseClass.ToString().ShouldEqual("global::D");
      }
      // T51, T52: If T has both a class-type constraint and one or more type-parameter constraints, 
      // its effective base class is the most encompassed type (§6.4.2) 
      // in the set consisting of the class-type constraint of T and the effective base classes of its type-parameter constraints.
      //  where T51 : C, TP1
      //  where T52 : C, TP1, TP2
      //  where TP1 : B
      //  where TP2 : D
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        typeParameter.BaseClass.ToString().ShouldEqual("global::C");
      }
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        typeParameter.BaseClass.ToString().ShouldEqual("global::D");
      }
      // T6: If T has the reference type constraint but no class-type constraints, its effective base class is object.
      //  where T6 : class
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        typeParameter.BaseClass.ToString().ShouldEqual("global::System.Object");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the logic behind the BaseInterfaces property getter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void EffectiveInterfaceSet()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeParameterEntity\EffectiveInterfaceSet.cs");
      InvokeParser(project).ShouldBeTrue();

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("P", 2)
        .GetSingleChildType<ClassEntity>("A", 7);

      int i = 0;

      // T1: If T has no secondary-constraints, its effective interface set is empty.
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        var orderedInterfaceSet = typeParameter.BaseInterfaces.OrderBy(x => x.ToString()).ToList();
        orderedInterfaceSet.Count.ShouldEqual(0);
      }
      // T2: If T has interface-type constraints but no type-parameter constraints,
      // its effective interface set is its set of interface-type constraints.
      //where T21 : I1
      //where T22 : I1, I2
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        var orderedInterfaceSet = typeParameter.BaseInterfaces.OrderBy(x => x.ToString()).ToList();
        orderedInterfaceSet.Count.ShouldEqual(1);
        orderedInterfaceSet[0].ToString().ShouldEqual("global::I1");
      }
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        var orderedInterfaceSet = typeParameter.BaseInterfaces.OrderBy(x => x.ToString()).ToList();
        orderedInterfaceSet.Count.ShouldEqual(2);
        orderedInterfaceSet[0].ToString().ShouldEqual("global::I1");
        orderedInterfaceSet[1].ToString().ShouldEqual("global::I2");
      }
      // T31, T32: If T has no interface-type constraints but has type-parameter constraints, 
      // its effective interface set is the union of the effective interface sets of its type-parameter constraints.
      //where T31 : TP1
      //where T32 : TP1, TP2
      //where TP1 : I3
      //where TP2 : I4
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        var orderedInterfaceSet = typeParameter.BaseInterfaces.OrderBy(x => x.ToString()).ToList();
        orderedInterfaceSet.Count.ShouldEqual(1);
        orderedInterfaceSet[0].ToString().ShouldEqual("global::I3");
      }
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        var orderedInterfaceSet = typeParameter.BaseInterfaces.OrderBy(x => x.ToString()).ToList();
        orderedInterfaceSet.Count.ShouldEqual(2);
        orderedInterfaceSet[0].ToString().ShouldEqual("global::I3");
        orderedInterfaceSet[1].ToString().ShouldEqual("global::I4");
      }
      // T41, T42: If T has both interface-type constraints and type-parameter constraints, 
      // its effective interface set is the union of its set of interface-type constraints 
      // and the effective interface sets of its type-parameter constraints.
      //where T41 : I1, TP1
      //where T42 : I1, I2, TP1, TP2
      //where TP1 : I3
      //where TP2 : I4
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        var orderedInterfaceSet = typeParameter.BaseInterfaces.OrderBy(x => x.ToString()).ToList();
        orderedInterfaceSet.Count.ShouldEqual(2);
        orderedInterfaceSet[0].ToString().ShouldEqual("global::I1");
        orderedInterfaceSet[1].ToString().ShouldEqual("global::I3");
      }
      {
        var typeParameter = classA.OwnTypeParameters.ToList()[i++];
        var orderedInterfaceSet = typeParameter.BaseInterfaces.OrderBy(x => x.ToString()).ToList();
        orderedInterfaceSet.Count.ShouldEqual(4);
        orderedInterfaceSet[0].ToString().ShouldEqual("global::I1");
        orderedInterfaceSet[1].ToString().ShouldEqual("global::I2");
        orderedInterfaceSet[2].ToString().ShouldEqual("global::I3");
        orderedInterfaceSet[3].ToString().ShouldEqual("global::I4");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the logic behind the IsKnownToBeAReferenceType property getter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void KnownToBeAReferenceType()
    {
      var project = new CSharpProject(WorkingFolder);
      project.AddFile(@"TypeParameterEntity\KnownToBeAReferenceType.cs");
      InvokeParser(project).ShouldBeTrue();

      var classA = project.SemanticGraph.GlobalNamespace.GetSingleChildType<ClassEntity>("A", 3);

      var typeParameters = classA.OwnTypeParameters.ToList();
      //where T1: class   // true
      typeParameters[0].IsKnownToBeAReferenceType.ShouldBeTrue();
      //where T2: struct  // false
      typeParameters[1].IsKnownToBeAReferenceType.ShouldBeFalse();
      //where T3: B       // true
      typeParameters[2].IsKnownToBeAReferenceType.ShouldBeTrue();
    }
  }
}
