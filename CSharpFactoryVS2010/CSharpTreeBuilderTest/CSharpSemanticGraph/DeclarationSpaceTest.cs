using System;
using System.Linq;
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
      declarationSpace.Define(new RootNamespaceEntity("A"));

      declarationSpace.NameCount.ShouldEqual(1);
      declarationSpace.IsNameDefined("A").ShouldBeTrue();
      var nameTableEntry = declarationSpace["A"];
      nameTableEntry.ShouldNotBeNull();
      nameTableEntry.State.ShouldEqual(NameTableEntryState.Definite);
      nameTableEntry.Name.ShouldEqual("A");
      ((RootNamespaceEntity) nameTableEntry.Entity).FullyQualifiedName.ShouldEqual("A");
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
      var entity1 = new RootNamespaceEntity("A");
      var entity2 = new ClassEntity("A");
      declarationSpace.Define(entity1);
      declarationSpace.Define(entity2);

      declarationSpace.NameCount.ShouldEqual(1);
      declarationSpace.IsNameDefined("A").ShouldBeTrue();
      var nameTableEntry = declarationSpace["A"];
      nameTableEntry.ShouldNotBeNull();
      nameTableEntry.State.ShouldEqual(NameTableEntryState.Ambigous);
      nameTableEntry.Name.ShouldEqual("A");

      var entities = nameTableEntry.Entities.ToArray();
      entities[0].ShouldEqual(entity1 as RootNamespaceEntity);
      entities[1].ShouldEqual(entity2 as ClassEntity);

      try
      {
        var x = nameTableEntry.Entity;
      }
      catch (ApplicationException e)
      {
        e.Message.Contains("not definite").ShouldBeTrue();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the redefinition of a name
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Redefine()
    {
      var declarationSpace = new DeclarationSpace();
      var entity1 = new RootNamespaceEntity("A");
      declarationSpace.Define(entity1);

      var entity2 = new ClassEntity("A");
      declarationSpace.Redefine(entity2);

      declarationSpace.NameCount.ShouldEqual(1);
      declarationSpace.IsNameDefined("A").ShouldBeTrue();
      var nameTableEntry = declarationSpace["A"];
      nameTableEntry.ShouldNotBeNull();
      nameTableEntry.State.ShouldEqual(NameTableEntryState.Definite);
      nameTableEntry.Name.ShouldEqual("A");
      ((ClassEntity)nameTableEntry.Entity).FullyQualifiedName.ShouldEqual("A");
    }
  }
}
