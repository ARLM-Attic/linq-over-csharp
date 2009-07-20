using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Tests declaration space
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class DeclarationSpaceTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the properties of an empty declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Empty()
    {
      var declarationSpace = new DeclarationSpace();
      declarationSpace.NameCount.ShouldEqual(0);
      declarationSpace.IsNameDefined("A").ShouldBeFalse();
      declarationSpace["A"].ShouldBeNull();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the properties of a definitely defined name in a declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Definite()
    {
      var declarationSpace = new DeclarationSpace();
      declarationSpace.DefineName("A", new RootNamespaceEntity("aRoot"));

      declarationSpace.NameCount.ShouldEqual(1);
      declarationSpace.IsNameDefined("A").ShouldBeTrue();
      var nameTableEntry = declarationSpace["A"];
      nameTableEntry.ShouldNotBeNull();
      nameTableEntry.State.ShouldEqual(NameTableEntryState.Definite);
      nameTableEntry.Name.ShouldEqual("A");
      ((RootNamespaceEntity) nameTableEntry.Entity).FqnWithRoot.ShouldEqual("aRoot");
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the properties of an ambigous name in a declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Ambigous()
    {
      var declarationSpace = new DeclarationSpace();
      declarationSpace.DefineName("A", new RootNamespaceEntity("aRoot1"));
      declarationSpace.DefineName("A", new RootNamespaceEntity("aRoot2"));

      declarationSpace.NameCount.ShouldEqual(1);
      declarationSpace.IsNameDefined("A").ShouldBeTrue();
      var nameTableEntry = declarationSpace["A"];
      nameTableEntry.ShouldNotBeNull();
      nameTableEntry.State.ShouldEqual(NameTableEntryState.Ambigous);
      nameTableEntry.Name.ShouldEqual("A");

      int i = 0;
      foreach (var entity in nameTableEntry.Entities)
      {
        if (i == 0) ((RootNamespaceEntity) entity).FqnWithRoot.ShouldEqual("aRoot1");
        if (i == 1) ((RootNamespaceEntity) entity).FqnWithRoot.ShouldEqual("aRoot2");
        i++;
      }

      try
      {
        var x = nameTableEntry.Entity;
      }
      catch (ApplicationException e)
      {
        e.Message.Contains("not definite").ShouldBeTrue();
      }
    }
  }
}
