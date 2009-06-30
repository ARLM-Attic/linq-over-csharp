// ================================================================================================
// ICSharpSyntaxTree.cs
//
// Created: 2009.03.04, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This interface represents the syntax tree belonging to a project.
  /// </summary>
  // ================================================================================================
  public interface ICSharpSyntaxTree
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file nodes belonging to the syntax tree
    /// </summary>
    /// <value>The source file nodes.</value>
    // ----------------------------------------------------------------------------------------------
    CompilationUnitNodeCollection CompilationUnitNodes { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resets the syntax tree so that it could be built fromthe beginning.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    void Reset();
  }
}