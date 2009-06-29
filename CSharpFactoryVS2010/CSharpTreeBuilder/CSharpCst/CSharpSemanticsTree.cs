// ================================================================================================
// CSharpSemanticsTree.cs
//
// Created: 2009.05.22, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Cst
{
  // ================================================================================================
  /// <summary>
  /// This class represents the semantics tree of a compilation unit.
  /// </summary>
  // ================================================================================================
  public class CSharpSemanticsTree : ICSharpSemanticsTree
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resets the semantics tree so that it could be built from the beginning.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public void Reset()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds entity to the graph. 
    /// If it already exists then tries to merge the new object into the existing one.
    /// </summary>
    /// <param name="entity">An entity</param>
    // ----------------------------------------------------------------------------------------------
    public void AddEntity(CompilationEntity entity)
    {
      throw new System.NotImplementedException();
    }

  }
}