// ================================================================================================
// IProject.cs
//
// Created: 2009.02.25, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Semantics;
using CSharpFactory.Syntax;

namespace CSharpFactory.SolutionHierarchy
{
  // ================================================================================================
  /// <summary>
  /// This interface represents a project in a solution.
  /// </summary>
  // ================================================================================================
  public interface IProject: IProjectContainer
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent solution of this project.
    /// </summary>
    /// <value>The parent solution.</value>
    // ----------------------------------------------------------------------------------------------
    ISolution ParentSolution { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Syntax tree representing the project's grammar
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ISyntaxTree SyntaxTree { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Semantic tree representing the project
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ISemanticsTree SemanticTree { get; }
  }
}