// ================================================================================================
// IProjectContainer.cs
//
// Created: 2009.02.25, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;

namespace CSharpFactory.SolutionHierarchy
{
  // ================================================================================================
  /// <summary>
  /// This type represents a container that holds projects.
  /// </summary>
  // ================================================================================================
  public interface IProjectContainer
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of projects held in this container.
    /// </summary>
    /// <value>The collection of projects held in this container.</value>
    // ----------------------------------------------------------------------------------------------
    ImmutableCollection<IProject> Projects { get; }
  }
}