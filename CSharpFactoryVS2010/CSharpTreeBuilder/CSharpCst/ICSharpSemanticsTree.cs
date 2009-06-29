// ================================================================================================
// ICSharpSemanticsTree.cs
//
// Created: 2009.04.05, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Cst
{
  // ================================================================================================
  /// <summary>
  /// This interface represents the C# semantics tree.
  /// </summary>
  // ================================================================================================
  public interface ICSharpSemanticsTree
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resets the semantics tree so that it could be built from the beginning.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    void Reset();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds entity to the graph. 
    /// If it already exists then tries to merge the new object into the existing one.
    /// </summary>
    /// <param name="entity">An entity</param>
    // ----------------------------------------------------------------------------------------------
    void AddEntity(CompilationEntity entity);
  }
}