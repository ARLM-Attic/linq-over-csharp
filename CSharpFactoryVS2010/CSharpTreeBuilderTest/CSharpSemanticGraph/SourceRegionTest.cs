using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilderTest.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// Tests the SourceRegion class
  /// </summary>
  // ================================================================================================
  [TestClass]
  public class SourceRegionTest
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tests the Contains method
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    [TestMethod]
    public void Contains()
    {
      var compilationUnitNode = new CompilationUnitNode();
      var sourceRegion = new SourceRegion(new SourcePoint(compilationUnitNode, 2),
                                          new SourcePoint(compilationUnitNode, 4));

      sourceRegion.Contains(new SourcePoint(null, 3)).ShouldBeFalse();
      sourceRegion.Contains(new SourcePoint(compilationUnitNode, 1)).ShouldBeFalse();
      sourceRegion.Contains(new SourcePoint(compilationUnitNode, 5)).ShouldBeFalse();

      sourceRegion.Contains(new SourcePoint(compilationUnitNode, 2)).ShouldBeTrue();
      sourceRegion.Contains(new SourcePoint(compilationUnitNode, 3)).ShouldBeTrue();
      sourceRegion.Contains(new SourcePoint(compilationUnitNode, 4)).ShouldBeTrue();
    }
  }
}