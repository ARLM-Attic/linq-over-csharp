using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Tests the EnumEntity class.
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class EnumEntityTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the AddType method
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddType()
    {
      var enumEntity = new EnumEntity();
      enumEntity.AddChildType(new EnumEntity());
    }
  }
}
