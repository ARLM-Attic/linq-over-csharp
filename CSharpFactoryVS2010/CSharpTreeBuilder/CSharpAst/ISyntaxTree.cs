// ================================================================================================
// ISyntaxTree.cs
//
// Created: 2009.03.04, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This interface represents the syntax tree belonging to a project.
  /// </summary>
  // ================================================================================================
  public interface ISyntaxTree
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file nodes belonging to the syntax tree
    /// </summary>
    /// <value>The source file nodes.</value>
    // ----------------------------------------------------------------------------------------------
    SourceFileNodeCollection SourceFileNodes { get; }
  }
}